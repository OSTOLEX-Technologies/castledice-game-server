using castledice_game_logic;
using castledice_game_logic.BoardGeneration.ContentGeneration;
using castledice_game_logic.GameObjects.Factories;
using castledice_game_logic.Math;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.BoardConfigProvidersTests.ContentSpawnersProivdersTests;

public class ContentSpawnerListProviderTests
{
    private readonly Mock<ICastlesSpawnerProvider> _castlesSpawnerProviderMock = new();
    private readonly Mock<ITreesSpawnerProvider> _treesSpawnerProviderMock = new();
    
    [Fact]
    public void ListFromGetContentSpawners_ShouldContainSpawner_FromCastlesSpawnerProvider()
    {
        var expectedSpawner = new CastlesSpawner(new Dictionary<Player, Vector2Int>(), new Mock<ICastlesFactory>().Object);
        _castlesSpawnerProviderMock.Setup(provider => provider.GetCastlesSpawner(It.IsAny<List<Player>>()))
            .Returns(expectedSpawner);
        var contentSpawnersListProvider = new ContentSpawnersListProvider(_castlesSpawnerProviderMock.Object, _treesSpawnerProviderMock.Object);
        
        var contentSpawners = contentSpawnersListProvider.GetContentSpawners(It.IsAny<List<Player>>());
        
        Assert.Contains(expectedSpawner, contentSpawners);
    }

    [Fact]
    public void GetContentSpawners_ShouldPassGivenPlayersList_ToGivenCastelsSpawnerProvider()
    {
        var expectedList = new List<Player>();
        _castlesSpawnerProviderMock.Setup(provider => provider.GetCastlesSpawner(It.IsAny<List<Player>>()))
            .Returns(new CastlesSpawner(new Dictionary<Player, Vector2Int>(), new Mock<ICastlesFactory>().Object));
        var contentSpawnersListProvider = new ContentSpawnersListProvider(_castlesSpawnerProviderMock.Object, _treesSpawnerProviderMock.Object);
        
        contentSpawnersListProvider.GetContentSpawners(expectedList);
        
        _castlesSpawnerProviderMock.Verify(provider => provider.GetCastlesSpawner(expectedList), Times.Once);
    }

    [Fact]
    public void GetContentSpawners_ShouldCallGetCastleSpawnerOnlyOnce_OnGivenProvider()
    {
        var contentSpawnersListProvider = new ContentSpawnersListProvider(_castlesSpawnerProviderMock.Object, _treesSpawnerProviderMock.Object);
        
        contentSpawnersListProvider.GetContentSpawners(It.IsAny<List<Player>>());
        
        _castlesSpawnerProviderMock.Verify(provider => provider.GetCastlesSpawner(It.IsAny<List<Player>>()), Times.Once);
    }
    
    [Fact]
    public void ListFromGetContentSpawners_ShouldContainSpawner_FromTreesSpawnerProvider()
    {
        var expectedSpawner = new RandomTreesSpawner(1, 2, 3, new Mock<ITreesFactory>().Object);
        _treesSpawnerProviderMock.Setup(provider => provider.GetTreesSpawner())
            .Returns(expectedSpawner);
        var contentSpawnersListProvider = new ContentSpawnersListProvider(_castlesSpawnerProviderMock.Object, _treesSpawnerProviderMock.Object);
        
        var contentSpawners = contentSpawnersListProvider.GetContentSpawners(It.IsAny<List<Player>>());
        
        Assert.Contains(expectedSpawner, contentSpawners);
    }
}