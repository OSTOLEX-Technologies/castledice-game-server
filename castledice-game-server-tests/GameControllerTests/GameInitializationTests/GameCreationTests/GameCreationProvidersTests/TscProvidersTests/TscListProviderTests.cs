using castledice_game_data_logic.TurnSwitchConditions;
using castledice_game_logic.TurnsLogic;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.TscProvidersTests;

public class TscListProviderTests
{
    [Theory]
    [MemberData(nameof(GetTurnSwitchConditionsTestCases))]
    public void GetTurnSwitchConditions_ShouldReturnListOfConditions_AccordingToPresenceConfig(TscPresenceConfig config,
        Dictionary<TscType, ITurnSwitchCondition> typesToExpectedConditions)
    {
        var tscProvider = new Mock<ITscProvider>();
        var expectedConditions = typesToExpectedConditions.Values;
        foreach (var typeToCondition in typesToExpectedConditions)
        {
            tscProvider.Setup(provider => provider.GetTurnSwitchCondition(typeToCondition.Key))
                .Returns(typeToCondition.Value);
        }
        var configProvider = new Mock<ITscPresenceConfigProvider>();
        configProvider.Setup(provider => provider.GetTscPresenceConfig()).Returns(config);
        var tscListProvider = new TscListProvider(configProvider.Object, tscProvider.Object);
        
        var actualTscList = tscListProvider.GetTurnSwitchConditions();
        
        Assert.Equal(expectedConditions.Count, actualTscList.Count);
        foreach (var condition in actualTscList)
        {
            Assert.Contains(condition, expectedConditions);
        }
    }

    public static IEnumerable<object[]> GetTurnSwitchConditionsTestCases()
    {
        yield return new object[]
        {
            new TscPresenceConfig(new HashSet<TscType> { TscType.Time }),
            new Dictionary<TscType, ITurnSwitchCondition>
            {
                { TscType.Time, new Mock<ITurnSwitchCondition>().Object }
            }
        };
        yield return new object[]
        {
            new TscPresenceConfig(new HashSet<TscType> { TscType.Time, TscType.ActionPoints }),
            new Dictionary<TscType, ITurnSwitchCondition>
            {
                { TscType.Time, new Mock<ITurnSwitchCondition>().Object },
                { TscType.ActionPoints, new Mock<ITurnSwitchCondition>().Object }
            }
        };
    }
}