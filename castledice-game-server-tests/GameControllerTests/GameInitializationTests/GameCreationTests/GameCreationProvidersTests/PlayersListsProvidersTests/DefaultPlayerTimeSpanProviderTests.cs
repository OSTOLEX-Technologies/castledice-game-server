using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersListProviders;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.PlayersListsProvidersTests;

public class DefaultPlayerTimeSpanProviderTests
{
    [Fact]
    public void GetTimeSpan_ShouldReturnTimeSpanFromConstructor()
    {
        var expectedTimeSpan = GetRandomTimeSpan();
        var provider = new DefaultPlayerTimeSpanProvider(expectedTimeSpan);
        
        var timeSpan = provider.GetTimeSpan();
        
        Assert.Equal(expectedTimeSpan, timeSpan);
    }
}