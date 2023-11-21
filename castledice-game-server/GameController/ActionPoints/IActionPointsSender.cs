namespace castledice_game_server.GameController.ActionPoints;

public interface IActionPointsSender
{
    void SendActionPoints(int amount, int actionPointsAccepterId, int messageAccepterId);
}