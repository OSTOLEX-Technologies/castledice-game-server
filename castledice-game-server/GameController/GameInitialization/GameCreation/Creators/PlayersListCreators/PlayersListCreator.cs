using castledice_game_logic;
using castledice_game_logic.ActionPointsLogic;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators;

public class PlayersListCreator : IPlayersListCreator
{
    private readonly IPlayerTimerCreator _playerTimerCreator;
    
    public PlayersListCreator(IPlayerTimerCreator playerTimerCreator)
    {
        _playerTimerCreator = playerTimerCreator;
    }
    
    public List<Player> GetPlayersList(List<int> playersIds)
    {
        return playersIds.Select(GetPlayer).ToList();
    }
    
    private Player GetPlayer(int playerId)
    {
        var timer = _playerTimerCreator.GetPlayerTimer();
        return new Player(new PlayerActionPoints(), timer, null, playerId);
    }
}