using castledice_game_logic.GameObjects.Factories;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.TreesSpawning;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.BoardConfigCreatorsTests.ContentSpawnersCreatorsTests;

public class TreesSpawnerCreatorTests
{
    [Fact]
    public void ReturnedTreesSpawner_ShouldHaveFactory_FromGivenCreator()
    {
        var expectedFactory = new Mock<ITreesFactory>().Object;
        var factoryCreatorMock = GetTreesFactoryCreatorMock();
        factoryCreatorMock.Setup(provider => provider.GetTreesFactory())
            .Returns(expectedFactory);
        var treesSpawnerCreator = new TreesSpawnerCreator(GetTreesGenerationConfigCreatorMock().Object, factoryCreatorMock.Object);
        
        var treesSpawner = treesSpawnerCreator.GetTreesSpawner();
        
        Assert.Same(expectedFactory, treesSpawner.Factory);
    }

    [Theory]
    [InlineData(2, 1, 3)]
    [InlineData(20, 10, 30)]
    [InlineData(15, 12, 13)]
    public void ReturnedTreesSpawner_ShouldHavePropertiesValues_EqualToConfigFromGivenGenerationConfigCreator(
        int maxTreesCount, int minTreesCount, int minDistanceBetweenTrees)
    {
        var config = new TreesGenerationConfig(maxTreesCount, minTreesCount, minDistanceBetweenTrees);
        var configCreatorMock = GetTreesGenerationConfigCreatorMock();
        configCreatorMock.Setup(provider => provider.GetTreesGenerationConfig())
            .Returns(config);
        var treesSpawnerCreator = new TreesSpawnerCreator(configCreatorMock.Object, GetTreesFactoryCreatorMock().Object);
        
        var treesSpawner = treesSpawnerCreator.GetTreesSpawner();
        
        Assert.Equal(maxTreesCount, treesSpawner.MaxTreesCount);
        Assert.Equal(minTreesCount, treesSpawner.MinTreesCount);
        Assert.Equal(minDistanceBetweenTrees, treesSpawner.MinDistanceBetweenTrees);
    }

    private static Mock<ITreesFactoryCreator> GetTreesFactoryCreatorMock()
    {
        var mock = new Mock<ITreesFactoryCreator>();
        mock.Setup(provider => provider.GetTreesFactory())
            .Returns(new Mock<ITreesFactory>().Object);
        return mock;
    }
    
    private static Mock<ITreesGenerationConfigCreator> GetTreesGenerationConfigCreatorMock()
    {
        var mock = new Mock<ITreesGenerationConfigCreator>();
        mock.Setup(provider => provider.GetTreesGenerationConfig())
            .Returns(new TreesGenerationConfig(2, 1, 3));
        return mock;
    }
}