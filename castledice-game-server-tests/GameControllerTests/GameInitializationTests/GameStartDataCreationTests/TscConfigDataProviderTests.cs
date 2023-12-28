using castledice_game_logic.GameConfiguration;
using castledice_game_logic.TurnsLogic.TurnSwitchConditions;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Providers;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameStartDataCreationTests;

public class TscConfigDataProviderTests
{
    [Theory]
    [MemberData(nameof(GetTscTypes))]
    public void GetTscConfigData_ShouldReturnTscConfigData_WithSameTscTypesToUse(List<TscType> tscTypes)
    {
        var tscConfigData = new TscConfigDataProvider();
        var turnSwitchConditionsConfig = new TurnSwitchConditionsConfig(tscTypes);
        
        var result = tscConfigData.GetTscConfigData(turnSwitchConditionsConfig);
        
        Assert.Equal(tscTypes, result.TscTypes);
    }

    public static IEnumerable<object[]> GetTscTypes()
    {
        var values = Enum.GetValues(typeof(TscType));
        var tscTypes = new List<TscType>();
        foreach (var value in values)
        {
            tscTypes.Add((TscType) value);
        }
        yield return new object[] { tscTypes };
    }
}