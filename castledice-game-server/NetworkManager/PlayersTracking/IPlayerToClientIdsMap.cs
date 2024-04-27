namespace castledice_game_server.NetworkManager.PlayersTracking;

public interface IPlayerToClientIdsMap
{
    public void SaveClientToPlayer(int playerId, ushort clientId);
    public int GetPlayerByClient(ushort clientId);
    public ushort GetClientByPlayer(int playerId);
    public bool PlayerHasClient(int playerId);
    public bool ClientHasPlayer(ushort clientId);
    public bool RemovePairByClient(ushort clientId);
    public bool RemovePairByPlayer(int playerId);
}