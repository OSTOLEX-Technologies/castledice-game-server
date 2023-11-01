using castledice_game_server.GameController.Moves;
using castledice_game_server.NetworkManager.PlayersTracking;

namespace castledice_game_server.NetworkManager;

public class MoveStatusSender : IMoveStatusSender
{
    private readonly IMessageSenderById _messageSender;
    private readonly IPlayerClientIdProvider _playerClientIdProvider;

    public MoveStatusSender(IMessageSenderById messageSender, IPlayerClientIdProvider playerClientIdProvider)
    {
        _messageSender = messageSender;
        _playerClientIdProvider = playerClientIdProvider;
    }

    public void SendMoveStatusToPlayer(bool isApproved, int playerId)
    {
        throw new NotImplementedException();
    }
}