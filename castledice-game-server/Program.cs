using System.Diagnostics;
using castledice_game_logic.GameConfiguration;
using castledice_game_logic.GameObjects;
using castledice_game_logic.TurnsLogic.TurnSwitchConditions;
using castledice_game_server.Auth;
using castledice_game_server.Configuration;
using castledice_game_server.GameController;
using castledice_game_server.GameController.ActionPoints;
using castledice_game_server.GameController.GameInitialization;
using castledice_game_server.GameController.GameInitialization.GameCreation;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.CellsGeneratorsCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.CastlesSpawning;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.TreesSpawning;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlaceablesConfigCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators.PlayerCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators.PlayerCreators.PlayersDecksCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators.PlayerCreators.PlayerTimerCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.TscConfigCreators;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators.BoardDataCreators;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators.BoardDataCreators.ContentDataCreators;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators.PlaceablesConfigDataCreators;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators.PlayerDataCreators;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators.TscConfigDataCreators;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Providers;
using castledice_game_server.GameController.GameOver;
using castledice_game_server.GameController.General;
using castledice_game_server.GameController.Moves;
using castledice_game_server.GameController.PlayerInitialization;
using castledice_game_server.GameController.PlayersDisconnectHandling;
using castledice_game_server.GameController.PlayersReadiness;
using castledice_game_server.GameController.Timers;
using castledice_game_server.GameRepository;
using castledice_game_server.GameService;
using castledice_game_server.HttpUtilities;
using castledice_game_server.Logging;
using castledice_game_server.NetworkManager;
using castledice_game_server.NetworkManager.MessageHandlers;
using castledice_game_server.NetworkManager.PlayerDisconnection;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_game_server.NetworkManager.RiptideWrappers;
using castledice_game_server.NetworkManager.Senders;
using castledice_game_server.Stubs;
using Microsoft.Extensions.Configuration;
using Riptide;
using Riptide.Transports.Tcp;
using Riptide.Utils;

internal class Program
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    
    //Game creation providers
    private static readonly ICastleConfigCreator CastleConfigCreator = new DefaultCastleConfigCreator(3, 1, 1);
    private static readonly IDuelCastlesPositionsCreator DuelCastlesPositionsCreator = new DefaultDuelCastlePositionsCreator((0, 0), (9, 9));
    private static readonly ITreeConfigCreator TreeConfigCreator = new DefaultTreeConfigCreator(1, false);
    private static readonly ITreesGenerationConfigCreator TreesGenerationConfigCreator = new DefaultTreesGenerationConfigCreator(0, 3, 3);
    private static readonly IRectGenerationConfigCreator RectGenerationConfigCreator =
        new DefaultRectGenerationConfigCreator(10, 10);
    private static readonly IKnightConfigCreator KnightConfigCreator = new DefaultKnightConfigCreator(1, 2);

    private static readonly IPlayerDeckCreator DeckCreator = new DefaultDeckCreator(new List<PlacementType>
    {
        PlacementType.Knight
    });
    private static readonly ITscConfigCreator TscConfigCreator = new DefaultTscConfigCreator(new List<TscType>
    {
        TscType.SwitchByActionPoints
    });
    private static readonly IPlayerTimeSpanCreator PlayerTimeSpanCreator = new DefaultPlayerTimeSpanCreator(TimeSpan.FromMinutes(100));

    private static readonly string GameStartDataVersion = "1.0.0";

    private static readonly RandomConfig RandomConfig = new (1, 7, 100);
    
    
    public static void Main(string[] args)
    {
        //Setting up logging
        RiptideLogger.Initialize(Logger.Debug, Logger.Info, Logger.Warn, Logger.Error, false);
        var loggerWrapper = new NLogLoggerWrapper(Logger);
        
        //Getting configs
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
        var matchMakerConnectionConfig = config.GetRequiredSection("MatchMakerConnectionOptions").Get<MatchMakerConnectionOptions>();
        var gameServerStartConfig = config.GetRequiredSection("GameServerStartOptions").Get<GameServerStartOptions>();
        var authBackendConnectionConfig = config.GetRequiredSection("AuthBackendConnectionOptions").Get<AuthBackendConnectionOptions>();
        var storageBackendConnectionConfig = config.GetRequiredSection("StorageBackendConnectionOptions").Get<StorageBackendConnectionOptions>();
        Debug.Assert(matchMakerConnectionConfig != null, nameof(matchMakerConnectionConfig) + " != null");
        Debug.Assert(gameServerStartConfig != null, nameof(gameServerStartConfig) + " != null");
        Debug.Assert(authBackendConnectionConfig != null, nameof(authBackendConnectionConfig) + " != null");
        Debug.Assert(storageBackendConnectionConfig != null, nameof(storageBackendConnectionConfig) + " != null");

        var httpClientWrapper = new HttpClientWrapper(new HttpClient());
        
        //Starting the server
        var gameServer = new Server(new TcpServer());
        gameServer.Start(gameServerStartConfig.Port, gameServerStartConfig.MaxClientCount);
        var serverWrapper = new ServerWrapper(gameServer);
        var matchMakerClient = new Client(new TcpClient());
        matchMakerClient.Connect($"{matchMakerConnectionConfig.Ip}:{matchMakerConnectionConfig.Port}");
        var clientWrapper = new ClientWrapper(matchMakerClient);
        
        //Setting up common objects
        var playersDictionary = new PlayerToClientIdsMap();
        var errorSender = new ErrorSender(serverWrapper, playersDictionary);
        var httpIdRetriever = new HttpIdRetriever(authBackendConnectionConfig.Url, httpClientWrapper);
        var idRetriever = new CachingIdRetrieverDecorator(httpIdRetriever);
        var playersDisconnecter = new PlayerDisconnecter(serverWrapper, playersDictionary, playersDictionary);
        var httpGameDataRepository = new HttpGameDataRepository(storageBackendConnectionConfig.Url, httpClientWrapper);
        var currentTimeProvider = new CurrentTimeProvider();
        var gameStartDataJsonConverter = new NewtonsoftGameStartDataJsonConverter();
        var localGameDataRepository = new LocalGameDataRepository();
        //TODO: Should be replaced with real implementation when game saving service is ready
        var gameSavingService = new GameSavingServiceStub();
        var activeGamesCollection = new ActiveGamesCollection();
        
        //Setting up matchmaker retranslation
        var requestGameRetranslator = new RequestGameRetranslator(clientWrapper);
        var cancelGameRetranslator = new CancelGameRetranslator(clientWrapper);
        var cancelGameResultRetranslator = new CancelGameResultRetranslator(serverWrapper, playersDictionary);
        RequestGameMessageHandler.SetAccepter(requestGameRetranslator);
        CancelGameMessageHandler.SetAccepter(cancelGameRetranslator);
        CancelGameResultMessageHandler.SetAccepter(cancelGameResultRetranslator);
        
        //Setting up players initialization
        var playerInitializationResultDtoSender = new PlayerInitializationResultDTOSender(serverWrapper);
        var playerInitializationController = new PlayerInitializationController(idRetriever, playersDictionary, playersDisconnecter, playerInitializationResultDtoSender, loggerWrapper);
        var playerInitializer = new PlayerInitializer(playerInitializationController);
        InitializePlayerMessageHandler.SetDTOAccepter(playerInitializer);
        
        //Setting up player disconnect handling
        var playerDisconnectedEventEmitter = new PlayerDisconnectedEventEmitter(serverWrapper, playersDictionary);
        var playerDisconnectController = new PlayerDisconnectController(playerDisconnectedEventEmitter, playersDictionary, activeGamesCollection, 
            new GameForPlayerProvider(activeGamesCollection), new PlayerDisconnectedSender(serverWrapper, playersDictionary));
        
        //Setting up game initialization
        var castlesFactoryProvider = new CastlesFactoryCreator(CastleConfigCreator);
        var duelCastlesSpawnerProvider =
            new DuelCastlesSpawnerCreator(DuelCastlesPositionsCreator, castlesFactoryProvider);
        var treesFactoryProvider = new TreesFactoryCreator(TreeConfigCreator);
        var treesSpawnerProvider = new TreesSpawnerCreator(TreesGenerationConfigCreator, treesFactoryProvider);
        var contentSpawnersListProvider =
            new ContentSpawnersListCreator(duelCastlesSpawnerProvider, treesSpawnerProvider);
        var rectCellsGeneratorProvider = new RectCellsGeneratorCreator(RectGenerationConfigCreator);
        var boardConfigProvider = new BoardConfigCreator(rectCellsGeneratorProvider, contentSpawnersListProvider);
        var placeablesConfigProvider = new PlaceablesConfigCreator(KnightConfigCreator);
        var timerCreator = new StopwatchPlayerTimerCreator(PlayerTimeSpanCreator);
        var playerCreator = new PlayerCreator(DeckCreator, timerCreator);
        var playersListProvider = new PlayersListCreator(playerCreator);
        var gameConstructorWrapper = new GameConstructorWrapper();
        var gameCreator = new GameCreator(playersListProvider, boardConfigProvider, placeablesConfigProvider, TscConfigCreator, gameConstructorWrapper);
        var cellsPresenceMatrixProvider = new CellsPresenceMatrixCreator();
        var contentDataProvider = new ContentDataCreator();
        var contentDataListProvider = new ContentDataListCreator(contentDataProvider);
        var boardDataProvider = new BoardDataCreator(cellsPresenceMatrixProvider, contentDataListProvider);
        var gameStartDataVersionProvider = new DefaultGameStartDataVersionProvider(GameStartDataVersion);
        var placeablesConfigDataProvider = new PlaceablesConfigDataCreator();
        var tsConfigDataProvider = new TscConfigDataCreator();
        var playerDataListCreator = new PlayersDataListCreator(new PlayerDataCreator());
        var gameStartDataCreator = new GameStartDataCreator(gameStartDataVersionProvider, boardDataProvider,
            placeablesConfigDataProvider, tsConfigDataProvider, playerDataListCreator);
        var gameStartDataSender = new GameStartDataSender(serverWrapper, playersDictionary);
        var gameInitializationController = new GameInitializationController(gameSavingService, activeGamesCollection,
            gameStartDataSender, gameCreator, gameStartDataCreator, errorSender, loggerWrapper);
        var gameInitializer = new GameInitializer(gameInitializationController);
        MatchFoundMessageHandler.SetDTOAccepter(gameInitializer);
        
        //Setting up moves controller
        var gameForPlayerProvider = new GameForPlayerProvider(activeGamesCollection);
        var dataToMoveConverterProvider = new DataToMoveConverterProvider();
        var moveDataSender = new MoveDataSender(serverWrapper, playersDictionary);
        var moveStatusSender = new MoveStatusSender(serverWrapper, playersDictionary);
        var movesController = new MovesController(gameForPlayerProvider, dataToMoveConverterProvider, moveDataSender,
            moveStatusSender, loggerWrapper);
       var moveAccepter = new MoveAccepter(movesController);
       MoveFromClientMessageHandler.SetAccepter(moveAccepter);
       
       //Setting up players readiness controller
       var playersReadinessTracker = new PlayersReadinessTracker();
       var gamePlayersReadinessNotifier = new GamePlayersReadinessNotifier();
       var playerReadinessController = new PlayerReadinessController(idRetriever, gameForPlayerProvider,
           playersReadinessTracker, gamePlayersReadinessNotifier, loggerWrapper);
       var playerReadinessAccepter = new PlayerReadinessAccepter(playerReadinessController);
       PlayerReadyMessageHandler.SetAccepter(playerReadinessAccepter);
        
       
       //Setting up action points controller
       var negentropyGeneratorsFactory = new NegentropyGeneratorsFactory(RandomConfig);
       var generatorsCollection = new GeneratorsCollection(negentropyGeneratorsFactory);
       var actionPointsSender = new ActionPointsSender(serverWrapper, playersDictionary);
       var actionPointsController = new ActionPointsController(activeGamesCollection, generatorsCollection,
           actionPointsSender, gamePlayersReadinessNotifier, loggerWrapper);
       
       //Setting up game over controller
       var historyProvider = new HistoryProviderStub();
       var gameOverController =
           new GameOverController(activeGamesCollection, gameSavingService, historyProvider, loggerWrapper);
       
       //Setting up timers controller
       var timerSwitchSender = new TimerSwitchSender(serverWrapper, playersDictionary);
       var timersController = new TimersController(gamePlayersReadinessNotifier, activeGamesCollection, timerSwitchSender, loggerWrapper);
       
       //Running the server and client
       while (true)
       {
           try
           {
                gameServer.Update();
                matchMakerClient.Update();
           }
           catch (Exception e)
           {
                Logger.Error(e);
           }
       }
    }
}