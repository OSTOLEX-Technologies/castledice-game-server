using castledice_game_logic;
using castledice_game_logic.ActionPointsLogic;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersListProviders;

public class PlayersListProvider : IPlayersListProvider
{
    private readonly IPlayerTimerProvider _playerTimerProvider;
    
    public PlayersListProvider(IPlayerTimerProvider playerTimerProvider)
    {
        _playerTimerProvider = playerTimerProvider;
    }
    
    public List<Player> GetPlayersList(List<int> playersIds)
    {
        return playersIds.Select(GetPlayer).ToList();
    }
    
    private Player GetPlayer(int playerId)
    {
        var timer = _playerTimerProvider.GetPlayerTimer();
        return new Player(new PlayerActionPoints(), timer, playerId);
    }
}