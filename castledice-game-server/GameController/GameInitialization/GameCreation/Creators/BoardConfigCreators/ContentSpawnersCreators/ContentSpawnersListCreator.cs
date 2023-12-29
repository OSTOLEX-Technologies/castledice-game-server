using castledice_game_logic;
using castledice_game_logic.BoardGeneration.ContentGeneration;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.CastlesSpawning;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.TreesSpawning;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators;

public class ContentSpawnersListCreator : IContentSpawnersListCreator
{
    private readonly ICastlesSpawnerCreator _castlesSpawnerCreator;
    private readonly ITreesSpawnerCreator _treesSpawnerCreator;

    public ContentSpawnersListCreator(ICastlesSpawnerCreator castlesSpawnerCreator, ITreesSpawnerCreator treesSpawnerCreator)
    {
        _castlesSpawnerCreator = castlesSpawnerCreator;
        _treesSpawnerCreator = treesSpawnerCreator;
    }

    public List<IContentSpawner> GetContentSpawners(List<Player> players)
    {
        var contentSpawners = new List<IContentSpawner>();
        var castlesSpawner = _castlesSpawnerCreator.GetCastlesSpawner(players);
        var treesSpawner = _treesSpawnerCreator.GetTreesSpawner();
        contentSpawners.Add(castlesSpawner);
        contentSpawners.Add(treesSpawner);
        return contentSpawners;
    }
}