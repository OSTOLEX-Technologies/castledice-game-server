using castledice_game_logic;
using castledice_game_logic.BoardGeneration.ContentGeneration;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.TreesSpawning;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders;

public class ContentSpawnersListProvider : IContentSpawnersListProvider
{
    private readonly ICastlesSpawnerProvider _castlesSpawnerProvider;
    private readonly ITreesSpawnerProvider _treesSpawnerProvider;

    public ContentSpawnersListProvider(ICastlesSpawnerProvider castlesSpawnerProvider, ITreesSpawnerProvider treesSpawnerProvider)
    {
        _castlesSpawnerProvider = castlesSpawnerProvider;
        _treesSpawnerProvider = treesSpawnerProvider;
    }

    public List<IContentSpawner> GetContentSpawners(List<Player> players)
    {
        var contentSpawners = new List<IContentSpawner>();
        var castlesSpawner = _castlesSpawnerProvider.GetCastlesSpawner(players);
        var treesSpawner = _treesSpawnerProvider.GetTreesSpawner();
        contentSpawners.Add(castlesSpawner);
        contentSpawners.Add(treesSpawner);
        return contentSpawners;
    }
}