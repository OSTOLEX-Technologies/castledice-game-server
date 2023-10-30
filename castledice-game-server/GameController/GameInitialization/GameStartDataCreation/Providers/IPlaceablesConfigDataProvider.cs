﻿using castledice_game_data_logic.ConfigsData;
using castledice_game_logic.GameConfiguration;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Providers;

public interface IPlaceablesConfigDataProvider
{
    PlaceablesConfigData GetPlaceablesConfigData(PlaceablesConfig config);
}