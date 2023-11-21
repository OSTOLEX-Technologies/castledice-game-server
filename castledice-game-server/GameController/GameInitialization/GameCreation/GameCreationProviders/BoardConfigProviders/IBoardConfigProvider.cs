using castledice_game_logic;
using castledice_game_logic.GameConfiguration;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders;

public interface IBoardConfigProvider
{
    BoardConfig GetBoardConfig(List<Player> players);
}