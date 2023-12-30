using castledice_game_logic.GameObjects;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators.PlayerCreators.PlayersDecksCreators;

public class DefaultDeckCreator : IPlayerDeckCreator
{
    private readonly List<PlacementType> _defaultDeck;

    public DefaultDeckCreator(List<PlacementType> defaultDeck)
    {
        _defaultDeck = defaultDeck;
    }

    public List<PlacementType> GetDeckForPlayer(int playerId)
    {
        return _defaultDeck;
    }
}