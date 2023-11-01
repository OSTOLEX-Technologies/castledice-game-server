﻿namespace castledice_game_server.Exceptions;

public class GameNotSavedException : Exception
{
    public GameNotSavedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}