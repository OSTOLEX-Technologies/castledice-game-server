using castledice_game_logic;
using castledice_game_logic.GameObjects;
using castledice_game_logic.GameObjects.Decks;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersDecksListsProviders;

public class PlayersDecksListProvider : IPlayersDecksProvider
{
    private readonly IPlayerDeckProvider _playerDeckProvider;

    public PlayersDecksListProvider(IPlayerDeckProvider playerDeckProvider)
    {
        _playerDeckProvider = playerDeckProvider;
    }

    public IDecksList GetPlayersDecksList(List<int> players)
    {
        var idsToDecks = new Dictionary<int, List<PlacementType>>();
        foreach (var player in players)
        {
            idsToDecks.Add(player, _playerDeckProvider.GetDeckForPlayer(player));
        }
        
        return new IndividualDecksList(idsToDecks);
    }
}