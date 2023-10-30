using castledice_game_server.Auth;
using castledice_game_server.NetworkManager.PlayerDisconnection;
using castledice_game_server.NetworkManager.PlayersTracking;

namespace castledice_game_server.GameController.PlayerInitialization;

public class PlayerInitializationController : IPlayerInitializationController
{
    private readonly IIdRetriever _idRetriever;
    private readonly IPlayerClientIdSaver _playerClientIdSaver;
    private readonly IPlayerClientIdProvider _playerClientIdProvider;
    private readonly IPlayerDisconnecter _playerDisconnecter;

    public PlayerInitializationController(IIdRetriever idRetriever, IPlayerClientIdSaver playerClientIdSaver, IPlayerClientIdProvider playerClientIdProvider, IPlayerDisconnecter playerDisconnecter)
    {
        _idRetriever = idRetriever;
        _playerClientIdSaver = playerClientIdSaver;
        _playerClientIdProvider = playerClientIdProvider;
        _playerDisconnecter = playerDisconnecter;
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
        }
        catch (Exception e)
        {
            //TODO: Add exception logging here
        }
    }
}