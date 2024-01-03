﻿using castledice_game_data_logic.ConfigsData;
using castledice_game_logic.GameConfiguration;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators.PlaceablesConfigDataCreators;

public interface IPlaceablesConfigDataCreator
{
    PlaceablesConfigData GetPlaceablesConfigData(PlaceablesConfig config);
}