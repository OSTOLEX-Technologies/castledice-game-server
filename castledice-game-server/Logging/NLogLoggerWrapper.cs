using NLog;

namespace castledice_game_server.Logging;

public class NLogLoggerWrapper : ILogger
{
    private readonly NLog.Logger _logger;

    public NLogLoggerWrapper(Logger logger)
    {
        _logger = logger;
    }

    public void Debug(string message)
    {
        _logger.Debug(message);
    }

    public void Info(string message)
    {
        _logger.Info(message);
    }

    public void Warn(string message)
    {
        _logger.Warn(message);
    }

    public void Error(string message)
    {
        _logger.Error(message);
    }
}