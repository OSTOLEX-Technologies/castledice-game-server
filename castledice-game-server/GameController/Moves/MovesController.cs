using castledice_game_data_logic.Moves;
using castledice_game_server.Logging;

namespace castledice_game_server.GameController.Moves;

public class MovesController : IMovesController
{
    private readonly IGameForPlayerProvider _gameProvider;
    private readonly IDataToMoveConverterProvider _converterProvider;
    private readonly IMoveDataSender _moveDataSender;
    private readonly IMoveStatusSender _moveStatusSender;
    private readonly ILogger _logger;

    public MovesController(IGameForPlayerProvider gameProvider, IDataToMoveConverterProvider converterProvider, IMoveDataSender moveDataSender, IMoveStatusSender moveStatusSender, ILogger logger)
    {
        _gameProvider = gameProvider;
        _converterProvider = converterProvider;
        _moveDataSender = moveDataSender;
        _moveStatusSender = moveStatusSender;
        _logger = logger;
    }

    public void MakeMove(MoveData moveData)
    {
        throw new NotImplementedException();
    }
}