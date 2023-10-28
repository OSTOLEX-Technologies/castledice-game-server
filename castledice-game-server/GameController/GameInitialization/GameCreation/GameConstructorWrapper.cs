using castledice_game_logic;
using castledice_game_logic.GameConfiguration;
using castledice_game_logic.GameObjects;

namespace castledice_game_server.GameController.GameInitialization.GameCreation;

public class GameConstructorWrapper : IGameConstructorWrapper
{
    public Game ConstructGame(List<Player> players, BoardConfig boardConfig, PlaceablesConfig placeablesConfig, IDecksList decksList)
    {
        return new Game(players, boardConfig, placeablesConfig, decksList);
    }
}