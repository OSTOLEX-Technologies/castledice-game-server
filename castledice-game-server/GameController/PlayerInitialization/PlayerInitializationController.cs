using castledice_events_logic.ServerToClient;
using castledice_game_server.Auth;
using castledice_game_server.Logging;
using castledice_game_server.NetworkManager.PlayerDisconnection;
using castledice_game_server.NetworkManager.PlayersTracking;

namespace castledice_game_server.GameController.PlayerInitialization;

public class PlayerInitializationController : IPlayerInitializationController
{
    private readonly IIdRetriever _idRetriever;
    private readonly IPlayerToClientIdsMap _idsMap;
    private readonly IPlayerDisconnecter _playerDisconnecter;
    private readonly IPlayerInitializationResultDTOSender _playerInitializationResultDtoSender;
    private readonly ILogger _logger;

    public PlayerInitializationController(IIdRetriever idRetriever, 
        IPlayerToClientIdsMap idsMap, 
        IPlayerDisconnecter playerDisconnecter,
        IPlayerInitializationResultDTOSender playerInitializationResultDtoSender,
        ILogger logger)
    {
        _idRetriever = idRetriever;
        _idsMap = idsMap;
        _playerDisconnecter = playerDisconnecter;
        _playerInitializationResultDtoSender = playerInitializationResultDtoSender;
        _logger = logger;
    }

    public async Task InitializePlayerAsync(string playerToken, ushort clientId)
    {
        try
        {
            var playerId = await _idRetriever.RetrievePlayerIdAsync(playerToken);
            if (_idsMap.PlayerHasClient(playerId))
            {
                _playerDisconnecter.DisconnectPlayerWithId(playerId);
            }
            _idsMap.SaveClientToPlayer(playerId, clientId);
            _playerInitializationResultDtoSender.SendInitializationResult(clientId, new PlayerInitializationResultDTO(true));
        }
        catch (Exception e)
        {
            _logger.Error(e);
            _playerInitializationResultDtoSender.SendInitializationResult(clientId, new PlayerInitializationResultDTO(false));
        }
    }
}