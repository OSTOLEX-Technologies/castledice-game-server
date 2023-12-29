using castledice_game_logic;
using castledice_game_logic.BoardGeneration.ContentGeneration;
using castledice_game_logic.GameObjects.Factories;
using castledice_game_logic.Math;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.CastlesSpawning;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.TreesSpawning;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.BoardConfigCreatorsTests.ContentSpawnersCreatorsTests;

public class ContentSpawnerListCreatorTests
{
    private readonly Mock<ICastlesSpawnerCreator> _castlesSpawnerCreatorMock = new();
    private readonly Mock<ITreesSpawnerCreator> _treesSpawnerCreatorMock = new();
    
    [Fact]
    public void ListFromGetContentSpawners_ShouldContainSpawner_FromCastlesSpawnerCreator()
    {
        var expectedSpawner = new CastlesSpawner(new Dictionary<Player, Vector2Int>(), new Mock<ICastlesFactory>().Object);
        _castlesSpawnerCreatorMock.Setup(creator => creator.GetCastlesSpawner(It.IsAny<List<Player>>()))
            .Returns(expectedSpawner);
        var contentSpawnersListCreator = new ContentSpawnersListCreator(_castlesSpawnerCreatorMock.Object, _treesSpawnerCreatorMock.Object);
        
        var contentSpawners = contentSpawnersListCreator.GetContentSpawners(It.IsAny<List<Player>>());
        
        Assert.Contains(expectedSpawner, contentSpawners);
    }
    
    [Fact]
    public void ListFromGetContentSpawners_ShouldContainSpawner_FromTreesSpawnerCreator()
    {
        var expectedSpawner = new RandomTreesSpawner(1, 2, 3, new Mock<ITreesFactory>().Object);
        _treesSpawnerCreatorMock.Setup(creator => creator.GetTreesSpawner())
            .Returns(expectedSpawner);
        var contentSpawnersListCreator = new ContentSpawnersListCreator(_castlesSpawnerCreatorMock.Object, _treesSpawnerCreatorMock.Object);
        
        var contentSpawners = contentSpawnersListCreator.GetContentSpawners(It.IsAny<List<Player>>());
        
        Assert.Contains(expectedSpawner, contentSpawners);
    }

    [Fact]
    public void GetContentSpawners_ShouldPassGivenPlayersList_ToGivenCastelsSpawnerCreator()
    {
        var expectedList = new List<Player>();
        _castlesSpawnerCreatorMock.Setup(creator => creator.GetCastlesSpawner(It.IsAny<List<Player>>()))
            .Returns(new CastlesSpawner(new Dictionary<Player, Vector2Int>(), new Mock<ICastlesFactory>().Object));
        var contentSpawnersListCreator = new ContentSpawnersListCreator(_castlesSpawnerCreatorMock.Object, _treesSpawnerCreatorMock.Object);
        
        contentSpawnersListCreator.GetContentSpawners(expectedList);
        
        _castlesSpawnerCreatorMock.Verify(creator => creator.GetCastlesSpawner(expectedList), Times.Once);
    }

    [Fact]
    public void GetContentSpawners_ShouldCallGetCastleSpawnerOnlyOnce_OnGivenCreator()
    {
        var contentSpawnersListCreator = new ContentSpawnersListCreator(_castlesSpawnerCreatorMock.Object, _treesSpawnerCreatorMock.Object);
        
        contentSpawnersListCreator.GetContentSpawners(It.IsAny<List<Player>>());
        
        _castlesSpawnerCreatorMock.Verify(creator => creator.GetCastlesSpawner(It.IsAny<List<Player>>()), Times.Once);
    }
}