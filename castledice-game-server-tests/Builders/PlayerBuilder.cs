using castledice_game_logic;
using castledice_game_logic.ActionPointsLogic;
using castledice_game_logic.GameObjects;
using castledice_game_logic.Time;
using Moq;

namespace castledice_game_server_tests.Builders;

public class PlayerBuilder
{
    public PlayerActionPoints ActionPoints { get; set; } = new();
    public IPlayerTimer Timer { get; set; } = new Mock<IPlayerTimer>().Object;
    public List<PlacementType> Deck { get; set; } = new();
    public int Id { get; set; } = 0;
    
    public Player Build()
    {
        return new Player(ActionPoints, Timer, Deck, Id);
    }
}