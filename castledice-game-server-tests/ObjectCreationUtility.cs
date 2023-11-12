using castledice_game_data_logic;
using castledice_game_data_logic.ConfigsData;
using castledice_game_data_logic.Content;
using castledice_game_logic;
using castledice_game_logic.ActionPointsLogic;
using castledice_game_logic.BoardGeneration.CellsGeneration;
using castledice_game_logic.BoardGeneration.ContentGeneration;
using castledice_game_logic.GameConfiguration;
using castledice_game_logic.GameObjects;
using castledice_game_logic.GameObjects.Configs;
using castledice_game_logic.GameObjects.Factories.Castles;
using castledice_game_logic.Math;
using castledice_game_server.GameController.Moves;
using Moq;
using CastleGO = castledice_game_logic.GameObjects.Castle;

namespace castledice_game_server_tests;

public class ObjectCreationUtility
{
    public static Mock<IGameForPlayerProvider> GetGameForPlayerProviderMock()
    {
        var mock = new Mock<IGameForPlayerProvider>();
        var gameMock = GetGameMock();
        mock.Setup(x => x.GetGame(It.IsAny<int>())).Returns(gameMock.Object);
        return mock;
    }
    
    public static Mock<Game> GetGameMock()
    {
        var player = GetPlayer(1);
        var secondPlayer = GetPlayer(2);
        var playersList = new List<Player> { player, secondPlayer };
        var gameMock = new Mock<Game>(playersList, GetBoardConfig(new Dictionary<Player, Vector2Int>
        {
            {player, (0, 0)},
            {secondPlayer, (9, 9)}
        }), GetPlaceablesConfig(), new Mock<IDecksList>().Object);
        gameMock.Setup(x => x.GetPlayer(It.IsAny<int>())).Returns(player);
        gameMock.Setup(x => x.GetAllPlayers()).Returns(playersList);
        gameMock.Setup(x => x.GetAllPlayersIds()).Returns(new List<int> { 1, 2 });
        gameMock.Setup(g => g.GetCurrentPlayer()).Returns(GetPlayer(1));
        return gameMock;
    }
    
    public static GameStartData GetGameStartData()
    {
        return GetGameStartData(1, 2);
    }

    public static Player GetPlayer(int id)
    {
        return new Player(new PlayerActionPoints(), id);
    }
    
    public static GameStartData GetGameStartData(params int[] playerIds)
    {
        var version = "1.0.0";
        var boardData = GetBoardData();
        var playersIdsList = new List<int>(playerIds);
        var placeablesConfigs = new PlaceablesConfigData(new KnightConfigData(1, 2));
        var playerDecks = new List<PlayerDeckData>()
        {
            new(playerIds[0], new List<PlacementType> { PlacementType.Knight }),
            new (playerIds[1], new List<PlacementType> { PlacementType.Knight })
        };
        var data = new GameStartData(version, boardData, placeablesConfigs, playersIdsList, playerDecks);
        return data;
    }

    public static BoardData GetBoardData(CellType cellType = CellType.Square)
    {
        var cellsPresence = GetValuesMatrix(10, 10, true);
        return GetBoardData(cellsPresence, cellType);
    }

    public static GameData GetGameData(int id = 1, string config = "someconfig", DateTime startTime = default)
    {
        return GetGameData(new List<int> { 1, 2 }, id, config, startTime);
    }
    
    public static GameData GetGameData(List<int> playersIds, int id = 1, string config = "someconfig", DateTime startTime = default)
    {
        return new GameData(1, "someconfig", DateTime.Now, new List<int>{1, 2});
    }
        
    public static BoardData GetBoardData(bool[,] cellsPresence, CellType cellType = CellType.Square)
    {
        var boardLength = 10;
        var boardWidth = 10;
        var firstCastle = new CastleData((0, 0), 1, 1, 3, 3, 1);
        var secondCastle = new CastleData((9, 9), 1, 1, 3, 3, 2);
        var generatedContent = new List<ContentData>
        {
            firstCastle, 
            secondCastle
        };
        return new BoardData(boardLength, boardWidth, cellType, cellsPresence, generatedContent);
    }
    
    public static T[,] GetValuesMatrix<T>(int length, int width, T value)
    {
        var matrix = new T[length, width];
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                matrix[i, j] = value;
            }
        }

        return matrix;
    }

    public static Game GetGame()
    {
        var firstPlayer = new Player(new PlayerActionPoints(), 1);
        var secondPlayer = new Player(new PlayerActionPoints(), 2);
        return GetGame(firstPlayer, secondPlayer);
    }
    
    public static Game GetGame(Player firstPlayer, Player secondPlayer)
    {
        var players = new List<Player>()
        {
            firstPlayer,
            secondPlayer
        };

        var playersToCastlesPositions = new Dictionary<Player, Vector2Int>()
        {
            { firstPlayer, (0, 0) },
            { secondPlayer, (9, 9) }
        };
        var castleConfig = new CastleConfig(3, 1, 1);
        var castlesFactory = new CastlesFactory(castleConfig);
        var casltesSpawner = new CastlesSpawner(playersToCastlesPositions, castlesFactory);

        var contentSpanwers = new List<IContentSpawner>()
        {
            casltesSpawner
        };

        var cellsGenerator = new RectCellsGenerator(10, 10);
            
        var boardConfig = new BoardConfig(contentSpanwers, cellsGenerator, CellType.Square);

        var unitsConfig = new PlaceablesConfig(new KnightConfig(1, 2));

        var placementListProvider = new CommonDecksList(new List<PlacementType>
        {
            PlacementType.Knight
        });
            
        var game = new Game(players, boardConfig, unitsConfig, placementListProvider);
        
        return game;
    }

    public static BoardConfig GetBoardConfig()
    {
        var playersToCastlesPositions = new Dictionary<Player, Vector2Int>()
        {
            { new Player(new PlayerActionPoints(), 1), (0, 0) },
            { new Player(new PlayerActionPoints(), 2), (9, 9) }
        };
        return GetBoardConfig(playersToCastlesPositions);
    }
    
    public static Board GetFullBoard(int length, int width)
    {
        var board = new Board(CellType.Square);
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                board.AddCell(i, j);
            }
        }
        return board;
    }
    
    public static BoardConfig GetBoardConfig(Dictionary<Player, Vector2Int> playersToCastlesPositions)
    {
        var castleConfig = new CastleConfig(3, 1, 1);
        var castlesFactory = new CastlesFactory(castleConfig);
        var castlesSpawner = new CastlesSpawner(playersToCastlesPositions, castlesFactory);

        var contentSpawners = new List<IContentSpawner>
        {
            castlesSpawner
        };

        var cellsGenerator = new RectCellsGenerator(10, 10);
            
        var boardConfig = new BoardConfig(contentSpawners, cellsGenerator, CellType.Square);

        return boardConfig;
    }
    
    public static PlaceablesConfig GetPlaceablesConfig()
    {
        var unitsConfig = new PlaceablesConfig(new KnightConfig(1, 2));
        return unitsConfig;
    }
    
    public static CastleGO GetCastle(Player player, int durability = 3, int maxDurability = 3, int maxFreeDurability = 1, int captureHitCost = 1)
    {
        return new CastleGO(player, durability, maxDurability, maxFreeDurability, captureHitCost);
    }

    public static Knight GetKnight(Player player, int health = 3, int placementCost = 1)
    {
        return new Knight(player, placementCost, health);
    }

    public static Tree GetTree(int removeCost = 1, bool canBeRemoved = false)
    {
        return new Tree(removeCost, canBeRemoved);
    }
}