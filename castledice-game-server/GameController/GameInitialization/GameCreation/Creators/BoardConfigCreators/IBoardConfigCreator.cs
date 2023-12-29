using castledice_game_logic;
using castledice_game_logic.GameConfiguration;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators;

public interface IBoardConfigCreator
{
    BoardConfig GetBoardConfig(List<Player> players);
}