using castledice_game_server.Exceptions;
using castledice_game_server.GameRepository;
using static castledice_game_server_tests.ObjectCreationUtility;

namespace castledice_game_server_tests.GameRepositoryTests;

public class LocalGameDataRepositoryTests
{
    [Fact]
    public void GetGameData_ShouldReturnGameData_IfItWasAdded()
    {
        var gameData = GetGameData();
        var repository = new LocalGameDataRepository();
        
        repository.AddGameData(gameData);
        var result = repository.GetGameData(gameData.Id);
        
        Assert.Equal(gameData, result);
    }
    
    [Fact]
    public void GetGameData_ShouldThrowGameDataNotFoundException_IfNoGameDataInstanceFound()
    {
        var repository = new LocalGameDataRepository();
        
        Assert.Throws<GameDataNotFoundException>(() => repository.GetGameData(1));
    }
    
    [Fact]
    public void RemoveGameData_ShouldReturnTrue_IfGameDataWasRemoved()
    {
        var gameData = GetGameData();
        var repository = new LocalGameDataRepository();
        
        repository.AddGameData(gameData);
        var result = repository.RemoveGameData(gameData.Id);
        
        Assert.True(result);
    }
    
    [Fact]
    public void RemoveGameData_ShouldReturnFalse_IfNoGameDataInstanceFound()
    {
        var repository = new LocalGameDataRepository();
        
        var result = repository.RemoveGameData(1);
        
        Assert.False(result);
    }
    
    [Fact]
    public void RemoveGameData_ShouldRemoveGameDataInstance_IfItWasFound()
    {
        var gameData = GetGameData();
        var repository = new LocalGameDataRepository();
        
        repository.AddGameData(gameData);
        repository.RemoveGameData(gameData.Id);
        Assert.Throws<GameDataNotFoundException>(() => repository.GetGameData(gameData.Id));
    }
}