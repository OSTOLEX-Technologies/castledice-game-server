using castledice_game_logic.Math;
using castledice_game_server.GameController.ActionPoints;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.ActionPointsTests;

public class GeneratorsCollectionTests
{
    [Fact]
    public void AddGeneratorForPlayer_ShouldAddGenerator_FromGivenFactory()
    {
        var factoryMock = new Mock<INumberGeneratorsFactory>();
        var expectedGenerator = new Mock<IRandomNumberGenerator>().Object;
        factoryMock.Setup(f => f.GetGenerator()).Returns(expectedGenerator);
        var generatorsCollection = new GeneratorsCollection(factoryMock.Object);
        
        generatorsCollection.AddGeneratorForPlayer(1);
        var actualGenerator = generatorsCollection.GetGeneratorForPlayer(1);
        
        Assert.Same(expectedGenerator, actualGenerator);
    }
    
    [Fact]
    public void AddGeneratorForPlayer_ShouldThrowArgumentException_IfGeneratorForGivenPlayerAlreadyExists()
    {
        var factoryMock = new Mock<INumberGeneratorsFactory>();
        var generatorsCollection = new GeneratorsCollection(factoryMock.Object);
        
        generatorsCollection.AddGeneratorForPlayer(1);
        
        Assert.Throws<ArgumentException>(() => generatorsCollection.AddGeneratorForPlayer(1));
    }
    
    [Fact]
    public void GetGeneratorForPlayer_ShouldThrowArgumentException_IfGeneratorForGivenPlayerDoesNotExist()
    {
        var factoryMock = new Mock<INumberGeneratorsFactory>();
        var generatorsCollection = new GeneratorsCollection(factoryMock.Object);
        
        Assert.Throws<ArgumentException>(() => generatorsCollection.GetGeneratorForPlayer(1));
    }
    
    [Fact]
    public void RemoveGeneratorForPlayer_ShouldReturnTrue_IfGeneratorForGivenPlayerExists()
    {
        var factoryMock = new Mock<INumberGeneratorsFactory>();
        var generatorsCollection = new GeneratorsCollection(factoryMock.Object);
        
        generatorsCollection.AddGeneratorForPlayer(1);
        var result = generatorsCollection.RemoveGeneratorForPlayer(1);
        
        Assert.True(result);
    }
    
    [Fact]
    public void RemoveGeneratorForPlayer_ShouldReturnFalse_IfGeneratorForGivenPlayerDoesNotExist()
    {
        var factoryMock = new Mock<INumberGeneratorsFactory>();
        var generatorsCollection = new GeneratorsCollection(factoryMock.Object);
        
        var result = generatorsCollection.RemoveGeneratorForPlayer(1);
        
        Assert.False(result);
    }
    
    [Fact]
    public void RemoveGeneratorForPlayer_ShouldRemoveGenerator_IfGeneratorForGivenPlayerExists()
    {
        var factoryMock = new Mock<INumberGeneratorsFactory>();
        var generatorsCollection = new GeneratorsCollection(factoryMock.Object);
        
        generatorsCollection.AddGeneratorForPlayer(1);
        generatorsCollection.RemoveGeneratorForPlayer(1);
        
        Assert.Throws<ArgumentException>(() => generatorsCollection.GetGeneratorForPlayer(1));
    }
}