using castledice_game_logic;
using castledice_game_logic.BoardGeneration.ContentGeneration;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders;

public interface IContentSpawnersListProvider
{
    List<IContentSpawner> GetContentSpawners(List<Player> players);
}