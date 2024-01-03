namespace castledice_game_server.GameController.Timers;

public interface ITimerSwitchSender
{
    /// <summary>
    /// </summary>
    /// <param name="playerToSwitchId"></param>
    /// <param name="timeLeft"></param>
    /// <param name="accepterPlayerId"></param>
    /// <param name="switchTo">
    /// If swithTo is true, then the player with id playerToSwitchId will get message to turn on his timer.
    /// If swithTo is false, then player`s timer will be turned off.
    /// </param>
    void SendTimerSwitch(int playerToSwitchId, TimeSpan timeLeft, int accepterPlayerId, bool switchTo);
}