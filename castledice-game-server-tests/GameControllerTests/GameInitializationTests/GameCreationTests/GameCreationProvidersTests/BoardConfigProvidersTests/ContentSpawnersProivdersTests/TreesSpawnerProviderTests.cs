using castledice_game_logic.GameObjects.Factories;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.TreesSpawning;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.BoardConfigProvidersTests.ContentSpawnersProivdersTests;

public class TreesSpawnerProviderTests
{
    [Fact]
    public void ReturnedTreesSpawner_ShouldHaveFactory_FromGivenProvider()
    {
        var expectedFactory = new Mock<ITreesFactory>().Object;
        var factoryProviderMock = GetTreesFactoryProviderMock();
        factoryProviderMock.Setup(provider => provider.GetTreesFactory())
            .Returns(expectedFactory);
        var treesSpawnerProvider = new TreesSpawnerProvider(GetTreesGenerationConfigProviderMock().Object, factoryProviderMock.Object);
        
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
        var config = new TreesGenerationConfig(maxTreesCount, minTreesCount, minDistanceBetweenTrees);
        var configProviderMock = GetTreesGenerationConfigProviderMock();
        configProviderMock.Setup(provider => provider.GetTreesGenerationConfig())
            .Returns(config);
        var treesSpawnerProvider = new TreesSpawnerProvider(configProviderMock.Object, GetTreesFactoryProviderMock().Object);
        
        var treesSpawner = treesSpawnerProvider.GetTreesSpawner();
        
        Assert.Equal(maxTreesCount, treesSpawner.MaxTreesCount);
        Assert.Equal(minTreesCount, treesSpawner.MinTreesCount);
        Assert.Equal(minDistanceBetweenTrees, treesSpawner.MinDistanceBetweenTrees);
    }

    private static Mock<ITreesFactoryProvider> GetTreesFactoryProviderMock()
    {
        var mock = new Mock<ITreesFactoryProvider>();
        mock.Setup(provider => provider.GetTreesFactory())
            .Returns(new Mock<ITreesFactory>().Object);
        return mock;
    }
    
    private static Mock<ITreesGenerationConfigProvider> GetTreesGenerationConfigProviderMock()
    {
        var mock = new Mock<ITreesGenerationConfigProvider>();
        mock.Setup(provider => provider.GetTreesGenerationConfig())
            .Returns(new TreesGenerationConfig(2, 1, 3));
        return mock;
    }
}