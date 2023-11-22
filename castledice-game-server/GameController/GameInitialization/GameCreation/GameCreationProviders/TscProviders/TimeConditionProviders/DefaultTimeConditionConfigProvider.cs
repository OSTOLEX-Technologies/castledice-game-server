namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders.TimeConditionProviders;

public class DefaultTimeConditionConfigProvider : ITimeConditionConfigProvider
{
    private readonly int _turnDuration;

    public DefaultTimeConditionConfigProvider(int turnDuration)
    {
        _turnDuration = turnDuration;
    }

    public TimeConditionConfig GetTimeConditionConfig()
    {
        return new TimeConditionConfig(_turnDuration);
    }
}