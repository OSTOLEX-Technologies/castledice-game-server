using castledice_game_data_logic.TurnSwitchConditions;
using castledice_game_logic;
using castledice_game_logic.TurnsLogic;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders.ActionPointsConditionProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders.TimeConditionProviders;
using Moq;
using ITimer = castledice_game_logic.Time.ITimer;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.TscProvidersTests;

public class TscProviderTests
{
   [Fact]
   public void GetTurnSwitchCondition_ShouldReturnTimeCondition_WhenGivenTimeType()
   {
      var expectedCondition = new TimeCondition(new Mock<ITimer>().Object, 1, new PlayerTurnsSwitcher(new PlayersList(new List<Player>{GetPlayer(1)})));
      var timeConditionProviderMock = new Mock<ITimeConditionProvider>();
      timeConditionProviderMock.Setup(provider => provider.GetTimeCondition()).Returns(expectedCondition);
      var actionPointsConditionProviderMock = new Mock<IActionPointsConditionProvider>();
      var tscProvider = new TscProvider(actionPointsConditionProviderMock.Object, timeConditionProviderMock.Object);
      
      var actualCondition = tscProvider.GetTurnSwitchCondition(TscType.Time);
      
      Assert.Same(expectedCondition, actualCondition);
   }

   [Fact]
   public void GetTurnSwitchCondition_ShouldReturnActionPointsCondition_WhenGivenActionPointsType()
   {
      var expectedCondition = new ActionPointsCondition(new Mock<ICurrentPlayerProvider>().Object);
      var timeConditionProviderMock = new Mock<ITimeConditionProvider>();
      var actionPointsConditionProviderMock = new Mock<IActionPointsConditionProvider>();
      actionPointsConditionProviderMock.Setup(provider => provider.GetActionPointsCondition()).Returns(expectedCondition);
      var tscProvider = new TscProvider(actionPointsConditionProviderMock.Object, timeConditionProviderMock.Object);
      
      var actualCondition = tscProvider.GetTurnSwitchCondition(TscType.ActionPoints);
      
      Assert.Same(expectedCondition, actualCondition);
   }
}