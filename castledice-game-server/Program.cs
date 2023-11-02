using System.Diagnostics;
using castledice_game_server.Configuration;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.CastlesSpawning;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.TreesSpawning;
using castledice_game_server.Logging;
using castledice_game_server.NetworkManager;
using castledice_game_server.NetworkManager.RiptideWrappers;
using Microsoft.Extensions.Configuration;
using Riptide;
using Riptide.Utils;

internal class Program
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    
    //Game creation providers
    private readonly ICastleConfigProvider CastleConfigProvider = new DefaultCastleConfigProvider(3, 1, 1);
    private readonly IDuelCastlesPositionsProvider DuelCastlesPositionsProvider = new DefaultDuelCastlePositionsProvider((0, 0), (9, 9));
    private readonly ITreeConfigProvider TreeConfigProvider = new DefaultTreeConfigProvider(1, false);
    private readonly ITreesGenerationConfigProvider TreesGenerationConfigProvider;
    
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
        var gameServer = new Server();
        gameServer.Start(gameServerStartConfig.Port, gameServerStartConfig.MaxClientCount);
        var serverWrapper = new ServerWrapper(gameServer);
        var matchMakerClient = new Client();
        matchMakerClient.Connect($"{matchMakerConnectionConfig.Ip}:{matchMakerConnectionConfig.Port}");
        var clientWrapper = new ClientWrapper(matchMakerClient);
        
        
    }
}