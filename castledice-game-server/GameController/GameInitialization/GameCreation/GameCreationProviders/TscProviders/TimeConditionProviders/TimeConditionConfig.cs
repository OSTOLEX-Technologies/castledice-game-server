namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders.TimeConditionProviders;

public class TimeConditionConfig
{
    public int TurnDuration { get; }

    public TimeConditionConfig(int turnDuration)
    {
        TurnDuration = turnDuration;
    }
}