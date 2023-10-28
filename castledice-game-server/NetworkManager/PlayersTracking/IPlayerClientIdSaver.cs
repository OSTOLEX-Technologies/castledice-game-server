namespace castledice_game_server.NetworkManager.PlayersTracking;

public interface IPlayerClientIdSaver
{
    void SaveClientIdForPlayer(int playerId, ushort clientId);
}