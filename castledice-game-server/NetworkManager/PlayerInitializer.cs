using casltedice_events_logic.ClientToServer;
using castledice_game_server.Auth;
using castledice_game_server.NetworkManager.PlayerDisconnection;
using castledice_game_server.NetworkManager.PlayersTracking;

namespace castledice_game_server.NetworkManager;

public class PlayerInitializer
{
    private readonly IIdRetriever _idRetriever;
    private readonly IPlayerClientIdSaver _playerClientIdSaver;
    private readonly IPlayerClientIdProvider _playerClientIdProvider;
    private readonly IPlayerDisconnecter _playerDisconnecter;

    public PlayerInitializer(IIdRetriever idRetriever, IPlayerClientIdSaver playerClientIdSaver, IPlayerClientIdProvider playerClientIdProvider, IPlayerDisconnecter playerDisconnecter)
    {
        _idRetriever = idRetriever;
        _playerClientIdSaver = playerClientIdSaver;
        _playerClientIdProvider = playerClientIdProvider;
        _playerDisconnecter = playerDisconnecter;
    }

    public void InitializePlayer(InitializePlayerDTO dto, ushort clientId)
    {
        var playerId = _idRetriever.RetrievePlayerId(dto.VerificationKey);
        if (_playerClientIdProvider.PlayerHasClientId(playerId))
        {
            _playerDisconnecter.DisconnectPlayerWithId(playerId);
        }
        _playerClientIdSaver.SaveClientIdForPlayer(playerId, clientId);
    }
}