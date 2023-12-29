using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.PlayersListsCreatorsTests;

public class DefaultPlayerTimeSpanCreatorTests
{
    [Fact]
    public void GetTimeSpan_ShouldReturnTimeSpanFromConstructor()
    {
        var expectedTimeSpan = GetRandomTimeSpan();
        var creator = new DefaultPlayerTimeSpanCreator(expectedTimeSpan);
        
        var timeSpan = creator.GetTimeSpan();
        
        Assert.Equal(expectedTimeSpan, timeSpan);
    }
}