using casltedice_events_logic.ServerToClient;
using castledice_game_server.GameController;
using castledice_game_server.GameController.GameInitialization;
using castledice_game_server.NetworkManager.DTOAccepters;

namespace castledice_game_server.NetworkManager;

public class GameInitializer : IMatchFoundDTOAccepter
{
    private readonly IGameInitializationController _gameInitializationController;

    public GameInitializer(IGameInitializationController gameInitializationController)
    {
        _gameInitializationController = gameInitializationController;
    }

    public void AcceptMatchFoundDTO(MatchFoundDTO matchFoundDTO)
    {
        Task.Run(() => _gameInitializationController.InitializeGameAsync(matchFoundDTO.PlayerIds));
    }
}