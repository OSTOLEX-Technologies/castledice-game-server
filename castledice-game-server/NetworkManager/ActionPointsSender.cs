using castledice_game_server.GameController.ActionPoints;
using castledice_game_server.NetworkManager.PlayersTracking;

namespace castledice_game_server.NetworkManager;

public class ActionPointsSender : IActionPointsSender
{
    private readonly IMessageSenderById _messageSender;
    private readonly IPlayerClientIdProvider _playerClientIdProvider;

    public ActionPointsSender(IMessageSenderById messageSender, IPlayerClientIdProvider playerClientIdProvider)
    {
        _messageSender = messageSender;
        _playerClientIdProvider = playerClientIdProvider;
    }

    public void SendActionPoints(int amount, int actionPointsAccepterId, int messageAccepterId)
    {
        throw new NotImplementedException();
    }
}