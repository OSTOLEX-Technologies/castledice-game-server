using castledice_game_logic;
using castledice_game_logic.GameConfiguration;
using castledice_game_logic.GameObjects;

namespace castledice_game_server.GameController.GameInitialization.GameCreation;

public interface IGameConstructorWrapper
{
    Game ConstructGame(List<Player> players, BoardConfig boardConfig, PlaceablesConfig placeablesConfig,
        IDecksList decksList);
}