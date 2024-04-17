using castledice_game_data_logic;
using castledice_game_logic;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators.PlayerDataCreators;

public class PlayerDataCreator : IPlayerDataCreator
{
    public PlayerData GetPlayerData(Player player)
    {
        var deck = player.Deck.ToList();
        var timeSpan = player.Timer.GetTimeLeft();
        return new PlayerData(player.Id, deck, timeSpan);
    }
}