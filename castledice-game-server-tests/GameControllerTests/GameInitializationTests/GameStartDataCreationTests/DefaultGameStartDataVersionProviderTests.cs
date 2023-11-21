using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Providers;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameStartDataCreationTests;

public class DefaultGameStartDataVersionProviderTests
{
    [Theory]
    [InlineData("1.0.0")]
    [InlineData("1.0.1")]
    [InlineData("1.1.0")]
    public void GetGameStartDataVersion_ShouldReturnString_GivenInConstructor(string version)
    {
        var provider = new DefaultGameStartDataVersionProvider(version);
        
        var result = provider.GetGameStartDataVersion();
        
        Assert.Equal(version, result);
    }
}