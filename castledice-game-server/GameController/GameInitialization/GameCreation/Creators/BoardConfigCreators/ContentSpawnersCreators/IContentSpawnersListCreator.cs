using castledice_game_logic;
using castledice_game_logic.BoardGeneration.ContentGeneration;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators;

public interface IContentSpawnersListCreator
{
    List<IContentSpawner> GetContentSpawners(List<Player> players);
}