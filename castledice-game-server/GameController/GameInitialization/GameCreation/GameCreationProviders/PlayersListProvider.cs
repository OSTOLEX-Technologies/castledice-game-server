using castledice_game_logic;
using castledice_game_logic.ActionPointsLogic;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders;

public class PlayersListProvider : IPlayersListProvider
{
    public List<Player> GetPlayersList(List<int> playersIds)
    {
        return playersIds.Select(GetPlayer).ToList();
    }
    
    private Player GetPlayer(int playerId)
    {
        return new Player(new PlayerActionPoints(), playerId);
    }
}