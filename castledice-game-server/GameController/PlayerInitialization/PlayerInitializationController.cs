using castledice_game_server.Auth;
using castledice_game_server.Logging;
using castledice_game_server.NetworkManager.PlayerDisconnection;
using castledice_game_server.NetworkManager.PlayersTracking;

namespace castledice_game_server.GameController.PlayerInitialization;

public class PlayerInitializationController : IPlayerInitializationController
{
    private readonly IIdRetriever _idRetriever;
    private readonly IPlayerClientIdSaver _playerClientIdSaver;
    private readonly IPlayerClientIdProvider _playerClientIdProvider;
    private readonly IPlayerDisconnecter _playerDisconnecter;
    private readonly IInitializationResultSender _initializationResultSender;
    private readonly ILogger _logger;

    public PlayerInitializationController(IIdRetriever idRetriever, 
        IPlayerClientIdSaver playerClientIdSaver, 
        IPlayerClientIdProvider playerClientIdProvider, 
        IPlayerDisconnecter playerDisconnecter,
        IInitializationResultSender initializationResultSender,
        ILogger logger)
    {
        _idRetriever = idRetriever;
        _playerClientIdSaver = playerClientIdSaver;
        _playerClientIdProvider = playerClientIdProvider;
        _playerDisconnecter = playerDisconnecter;
        _initializationResultSender = initializationResultSender;
        _logger = logger;
    }

    public async Task InitializePlayerAsync(string playerToken, ushort clientId)
    {
        try
        {
            var playerId = await _idRetriever.RetrievePlayerIdAsync(playerToken);
            if (_playerClientIdProvider.PlayerHasClientId(playerId))
            {
                _playerDisconnecter.DisconnectPlayerWithId(playerId);
            }
            _playerClientIdSaver.SaveClientIdForPlayer(playerId, clientId);
            _initializationResultSender.SendInitializationResult(clientId, true);
        }
        catch (Exception e)
        {
            _logger.Error(e);
            _initializationResultSender.SendInitializationResult(clientId, false);
        }
    }
}