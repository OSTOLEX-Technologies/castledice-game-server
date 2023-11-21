namespace castledice_game_server.NetworkManager.PlayersTracking;

public interface IPlayerClientIdProvider
{
    ushort GetClientIdForPlayer(int playerId);
    bool PlayerHasClientId(int playerId);
}