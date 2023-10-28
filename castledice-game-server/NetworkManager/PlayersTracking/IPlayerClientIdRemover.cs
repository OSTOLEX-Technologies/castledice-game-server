namespace castledice_game_server.NetworkManager.PlayersTracking;

public interface IPlayerClientIdRemover
{
    void RemoveClientIdForPlayer(int playerId);
}