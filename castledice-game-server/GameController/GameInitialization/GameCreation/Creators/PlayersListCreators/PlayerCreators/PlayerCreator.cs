using castledice_game_logic;
using castledice_game_logic.ActionPointsLogic;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators.PlayerCreators.PlayersDecksCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators.PlayerCreators.PlayerTimerCreators;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators.PlayerCreators;

public class PlayerCreator : IPlayerCreator
{
    private readonly IPlayerDeckCreator _deckCreator;
    private readonly IPlayerTimerCreator _playerTimerCreator;

    public PlayerCreator(IPlayerDeckCreator deckCreator, IPlayerTimerCreator playerTimerCreator)
    {
        _deckCreator = deckCreator;
        _playerTimerCreator = playerTimerCreator;
    }

    public Player GetPlayer(int playerId)
    {
        var actionPoints = new PlayerActionPoints();
        var playerTimer = _playerTimerCreator.GetPlayerTimer();
        var deck = _deckCreator.GetDeckForPlayer(playerId);
        return new Player(actionPoints, playerTimer, deck, playerId);
    }
}