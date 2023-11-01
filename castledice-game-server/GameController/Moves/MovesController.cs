using castledice_game_data_logic.Moves;
using castledice_game_logic;
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
        try
        {
            var game = _gameProvider.GetGame(moveData.PlayerId);
            var converter = _converterProvider.GetDataToMoveConverter(game);
            var player = game.GetPlayer(moveData.PlayerId);
            var move = converter.ConvertToMove(moveData, player);
            var isMoveApplied = game.TryMakeMove(move);
            _moveStatusSender.SendMoveStatusToPlayer(isMoveApplied, moveData.PlayerId);

            if (isMoveApplied)
            {
                SendMoveDataToOtherPlayersOfGame(moveData, game);
            }
        }
        catch (Exception e)
        {
            _logger.Error(e.Message);
        }
    }
    
    private void SendMoveDataToOtherPlayersOfGame(MoveData moveData, Game game)
    {
        var otherPlayersIds = game.GetAllPlayersIds().Where(p => p != moveData.PlayerId);
        foreach (var id in otherPlayersIds)
        {
            _moveDataSender.SendDataToPlayer(moveData, id);
        }
    }
}