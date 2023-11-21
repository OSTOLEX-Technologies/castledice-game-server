using castledice_game_data_logic;
using castledice_game_logic.GameObjects;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Providers;

public class DecksDataProvider : IDecksDataProvider
{
    public List<PlayerDeckData> GetPlayersDecksData(IDecksList decksList, List<int> playersIds)
    {
        var playersDecksData = new List<PlayerDeckData>();
        foreach (var id in playersIds)
        {
            var deck = decksList.GetDeck(id);
            var deckData = new PlayerDeckData(id, deck);
            playersDecksData.Add(deckData);
        }
        
        return playersDecksData;
    }
}