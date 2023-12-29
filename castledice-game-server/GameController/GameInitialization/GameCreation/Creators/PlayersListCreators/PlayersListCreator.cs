using castledice_game_logic;
using castledice_game_logic.ActionPointsLogic;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators.PlayerCreators;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators;

public class PlayersListCreator : IPlayersListCreator
{
    private readonly IPlayerCreator _playerCreator;

    public PlayersListCreator(IPlayerCreator playerCreator)
    {
        _playerCreator = playerCreator;
    }

    public List<Player> GetPlayersList(List<int> playersIds)
    {
        var players = new List<Player>();
        foreach (var playerId in playersIds)
        {
            var player = _playerCreator.GetPlayer(playerId);
            players.Add(player);
        }
        return players;
    }
    
}