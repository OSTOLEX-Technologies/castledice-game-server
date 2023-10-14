using casltedice_events_logic.ClientToServer;
using castledice_game_server.Auth;

namespace castledice_game_server.NetworkManager;

public class PlayerInitializer
{
    private IIdRetriever _idRetriever;

    public PlayerInitializer(IIdRetriever idRetriever)
    {
        _idRetriever = idRetriever;
    }

    public void InitializePlayer(InitializePlayerDTO dto, ushort clientId)
    {
        var playerId = _idRetriever.RetrievePlayerId(dto.VerificationKey);
        PlayersDictionary.Dictionary.Remove(playerId); //TODO: Disconnect player with this id and send dto to old client.
        PlayersDictionary.Dictionary.Add(playerId, clientId);
    }
}