using castledice_events_logic.ServerToClient;

namespace castledice_game_server_tests.Builders;

public class SwitchTimerDTOBuilder
{
    public TimeSpan TimeLeft { get; set; } = TimeSpan.Zero;
    public int PlayerId { get; set; } = 1;
    public bool Switch { get; set; } = true;
    
    public SwitchTimerDTO Build()
    {
        return new SwitchTimerDTO(TimeLeft, PlayerId, Switch);
    }
}