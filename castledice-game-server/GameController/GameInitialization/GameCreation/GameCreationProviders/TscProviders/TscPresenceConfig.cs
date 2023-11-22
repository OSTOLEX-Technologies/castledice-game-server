﻿using castledice_game_data_logic.TurnSwitchConditions;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders;

public class TscPresenceConfig
{
    public IReadOnlyCollection<TscType> PresentConditions => _presentConditions;
    private readonly HashSet<TscType> _presentConditions;

    public TscPresenceConfig(HashSet<TscType> presentConditions)
    {
        _presentConditions = presentConditions;
    }
}