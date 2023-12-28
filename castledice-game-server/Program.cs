using System.Diagnostics;
using castledice_game_logic.GameConfiguration;
using castledice_game_logic.GameObjects;
using castledice_game_logic.TurnsLogic.TurnSwitchConditions;
using castledice_game_server.Configuration;
using castledice_game_server.GameController;
using castledice_game_server.GameController.ActionPoints;
using castledice_game_server.GameController.GameInitialization;
using castledice_game_server.GameController.GameInitialization.GameCreation;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.CellsGeneratorsProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.CastlesSpawning;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.TreesSpawning;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlaceablesConfigProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersDecksListsProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersListProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscConfigProviders;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Providers;
using castledice_game_server.GameController.GameOver;
using castledice_game_server.GameController.Moves;
using castledice_game_server.GameController.PlayerInitialization;
using castledice_game_server.GameController.PlayersReadiness;
using castledice_game_server.Logging;
using castledice_game_server.NetworkManager;
using castledice_game_server.NetworkManager.MessageHandlers;
using castledice_game_server.NetworkManager.PlayerDisconnection;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_game_server.NetworkManager.RiptideWrappers;
using castledice_game_server.Stubs;
using Microsoft.Extensions.Configuration;
using Riptide;
using Riptide.Transports.Tcp;
using Riptide.Utils;

internal class Program
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    
    //Game creation providers
    private static readonly ICastleConfigProvider CastleConfigProvider = new DefaultCastleConfigProvider(3, 1, 1);
    private static readonly IDuelCastlesPositionsProvider DuelCastlesPositionsProvider = new DefaultDuelCastlePositionsProvider((0, 0), (9, 9));
    private static readonly ITreeConfigProvider TreeConfigProvider = new DefaultTreeConfigProvider(1, false);
    private static readonly ITreesGenerationConfigProvider TreesGenerationConfigProvider = new DefaultTreesGenerationConfigProvider(0, 3, 3);
    private static readonly IRectGenerationConfigProvider RectGenerationConfigProvider =
        new DefaultRectGenerationConfigProvider(10, 10);
    private static readonly IKnightConfigProvider KnightConfigProvider = new DefaultKnightConfigProvider(1, 2);

    private static readonly IPlayerDeckProvider DeckProvider = new DefaultDeckProvider(new List<PlacementType>
    {
        PlacementType.Knight
    });
    private static readonly ITscConfigProvider TscConfigProvider = new DefaultTscConfigProvider(new List<TscType>
    {
        TscType.SwitchByActionPoints
    });

    private static readonly string GameStartDataVersion = "1.0.0";

    private static readonly RandomConfig RandomConfig = new RandomConfig(1, 7, 100);
    
    
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
        Debug.Assert(matchMakerConnectionConfig != null, nameof(matchMakerConnectionConfig) + " != null");
        Debug.Assert(gameServerStartConfig != null, nameof(gameServerStartConfig) + " != null");

        //Starting the server
        var gameServer = new Server(new TcpServer());
        gameServer.Start(gameServerStartConfig.Port, gameServerStartConfig.MaxClientCount);
        var serverWrapper = new ServerWrapper(gameServer);
        var matchMakerClient = new Client(new TcpClient());
        matchMakerClient.Connect($"{matchMakerConnectionConfig.Ip}:{matchMakerConnectionConfig.Port}");
        var clientWrapper = new ClientWrapper(matchMakerClient);
        
        //Setting up common objects
        var playersDictionary = new PlayerToClientDictionary();
        var errorSender = new ErrorSender(serverWrapper, playersDictionary);
        var idRetriever = new StringIdRetrieverStub();//TODO: Replace with actual id retriever
        var playersDisconnecter = new PlayerDisconnecter(serverWrapper, playersDictionary, playersDictionary);
        var gameSavingService = new GameSavingServiceStub();//TODO: Replace with actual game saving service
        //var gameSavingService = new GameSavingServiceWithErrorStub() { ThrowErrorDelay = 1000};
        var activeGamesCollection = new ActiveGamesCollection();
        
        //Setting up matchmaker retranslation
        var requestGameRetranslator = new RequestGameRetranslator(clientWrapper);
        var cancelGameRetranslator = new CancelGameRetranslator(clientWrapper);
        var cancelGameResultRetranslator = new CancelGameResultRetranslator(serverWrapper, playersDictionary);
        RequestGameMessageHandler.SetAccepter(requestGameRetranslator);
        CancelGameMessageHandler.SetAccepter(cancelGameRetranslator);
        CancelGameResultMessageHandler.SetAccepter(cancelGameResultRetranslator);
        
        //Setting up players initialization
        var playerInitializationController = new PlayerInitializationController(idRetriever, playersDictionary, playersDictionary, playersDisconnecter, loggerWrapper);
        var playerInitializer = new PlayerInitializer(playerInitializationController);
        InitializePlayerMessageHandler.SetDTOAccepter(playerInitializer);
        
        //Setting up game initialization
        var castlesFactoryProvider = new CastlesFactoryProvider(CastleConfigProvider);
        var duelCastlesSpawnerProvider =
            new DuelCastlesSpawnerProvider(DuelCastlesPositionsProvider, castlesFactoryProvider);
        var treesFactoryProvider = new TreesFactoryProvider(TreeConfigProvider);
        var treesSpawnerProvider = new TreesSpawnerProvider(TreesGenerationConfigProvider, treesFactoryProvider);
        var contentSpawnersListProvider =
            new ContentSpawnersListProvider(duelCastlesSpawnerProvider, treesSpawnerProvider);
        var rectCellsGeneratorProvider = new RectCellsGeneratorProvider(RectGenerationConfigProvider);
        var boardConfigProvider = new BoardConfigProvider(rectCellsGeneratorProvider, contentSpawnersListProvider);
        var placeablesConfigProvider = new PlaceablesConfigProvider(KnightConfigProvider);
        var decksListProvider = new PlayersDecksListProvider(DeckProvider);
        var playersListProvider = new PlayersListProvider(null);
        var gameConstructorWrapper = new GameConstructorWrapper();
        var gameCreator = new GameCreator(playersListProvider, boardConfigProvider, placeablesConfigProvider, decksListProvider, TscConfigProvider, gameConstructorWrapper);
        var cellsPresenceMatrixProvider = new CellsPresenceMatrixProvider();
        var contentDataProvider = new ContentDataProvider();
        var contentDataListProvider = new ContentDataListProvider(contentDataProvider);
        var boardDataProvider = new BoardDataProvider(cellsPresenceMatrixProvider, contentDataListProvider);
        var gameStartDataVersionProvider = new DefaultGameStartDataVersionProvider(GameStartDataVersion);
        var placeablesConfigDataProvider = new PlaceablesConfigDataProvider();
        var decksDataProvider = new DecksDataProvider();
        var tsConfigDataProvider = new TscConfigDataProvider();
        var gameStartDataCreator = new GameStartDataCreator(gameStartDataVersionProvider, boardDataProvider,
            placeablesConfigDataProvider, tsConfigDataProvider, decksDataProvider);
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