using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators.PlayerCreators.PlayerTimerCreators;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.PlayersListsCreatorsTests.PlayerCreatorsTests.PlayerTimerCreatorsTests;

// This is a shortened version of class name. Full name is DefaultPlayerTimeSpanCreatorTests.
public class DefPlayerTimeSpanCreatorTests
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