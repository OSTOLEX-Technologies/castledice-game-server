using castledice_game_logic.BoardGeneration.CellsGeneration;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.CellsGeneratorsCreators;

public interface ICellsGeneratorCreator
{
    ICellsGenerator GetCellsGenerator();
}