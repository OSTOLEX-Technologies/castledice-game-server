﻿using castledice_game_logic.GameConfiguration;
using castledice_game_logic.TurnsLogic.TurnSwitchConditions;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators.TscConfigDataCreators;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameStartDataCreationTests.CreatorsTests.TscConfigDataCreatorsTests;

public class TscConfigDataCreatorTests
{
    [Theory]
    [MemberData(nameof(GetTscTypes))]
    public void GetTscConfigData_ShouldReturnTscConfigData_WithSameTscTypesToUse(List<TscType> tscTypes)
    {
        var tscConfigData = new TscConfigDataCreator();
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