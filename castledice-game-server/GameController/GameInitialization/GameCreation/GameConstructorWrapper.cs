using castledice_game_logic;
using castledice_game_logic.GameConfiguration;

namespace castledice_game_server.GameController.GameInitialization.GameCreation;

public class GameConstructorWrapper : IGameConstructorWrapper
{
    public Game ConstructGame(List<Player> players, BoardConfig boardConfig, PlaceablesConfig placeablesConfig, TurnSwitchConditionsConfig turnSwitchConditionsConfig)
    {
        return new Game(players, boardConfig, placeablesConfig, turnSwitchConditionsConfig);
    }
}