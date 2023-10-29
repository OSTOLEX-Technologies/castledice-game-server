using castledice_game_logic.GameObjects;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersDecksListsProviders;

public class DefaultDeckProvider : IPlayerDeckProvider
{
    private readonly List<PlacementType> _defaultDeck;

    public DefaultDeckProvider(List<PlacementType> defaultDeck)
    {
        _defaultDeck = defaultDeck;
    }

    public List<PlacementType> GetDeckForPlayer(int playerId)
    {
        throw new NotImplementedException();
    }
}