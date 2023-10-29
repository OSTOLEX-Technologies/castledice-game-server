using castledice_game_logic.GameObjects.Factories;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.TreesSpawning;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.BoardConfigProvidersTests.ContentSpawnersProivdersTests;

public class TreesSpawnerProviderTests
{
    private readonly Mock<ITreesFactoryProvider> _treesFactoryProviderMock = new();
    private readonly Mock<ITreesGenerationConfigProvider> _treesGenerationConfigProviderMock = new();
    
    [Fact]
    public void ReturnedTreesSpawner_ShouldHaveFactory_FromGivenProvider()
    {
        var expectedFactory = new Mock<ITreesFactory>().Object;
        _treesFactoryProviderMock.Setup(provider => provider.GetTreesFactory())
            .Returns(expectedFactory);
        var treesSpawnerProvider = new TreesSpawnerProvider(_treesGenerationConfigProviderMock.Object, _treesFactoryProviderMock.Object);
        
        var treesSpawner = treesSpawnerProvider.GetTreesSpawner();
        
        Assert.Same(expectedFactory, treesSpawner.Factory);
    }

    [Theory]
    [InlineData(2, 1, 3)]
    [InlineData(20, 10, 30)]
    [InlineData(15, 12, 13)]
    public void ReturnedTreesSpawner_ShouldHavePropertiesValues_EqualToConfigFromGivenGenerationConfigProvider(
        int maxTreesCount, int minTreesCount, int minDistanceBetweenTrees)
    {
        var config = new TreesGenerationConfig(maxTreesCount, maxTreesCount, minDistanceBetweenTrees);
        _treesGenerationConfigProviderMock.Setup(provider => provider.GetTreesGenerationConfig())
            .Returns(config);
        var treesSpawnerProvider = new TreesSpawnerProvider(_treesGenerationConfigProviderMock.Object, _treesFactoryProviderMock.Object);
        
        var treesSpawner = treesSpawnerProvider.GetTreesSpawner();
        
        Assert.Equal(maxTreesCount, treesSpawner.MaxTreesCount);
        Assert.Equal(minTreesCount, treesSpawner.MinTreesCount);
        Assert.Equal(minDistanceBetweenTrees, treesSpawner.MinDistanceBetweenTrees);
    }

}