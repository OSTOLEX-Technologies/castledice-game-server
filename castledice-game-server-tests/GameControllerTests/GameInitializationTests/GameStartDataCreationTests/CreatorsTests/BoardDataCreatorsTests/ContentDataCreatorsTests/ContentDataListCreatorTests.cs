﻿using castledice_game_data_logic.Content;
using castledice_game_logic;
using castledice_game_logic.GameObjects;
using castledice_game_logic.Math;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators.BoardDataCreators.ContentDataCreators;
using Moq;
using CastleGO = castledice_game_logic.GameObjects.Castle;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameStartDataCreationTests.CreatorsTests.BoardDataCreatorsTests.ContentDataCreatorsTests;

public class ContentDataListCreatorTests
{
    [Theory]
    [MemberData(nameof(BoardWithContentTestCases))]
    public void GetContentDataList_ShouldPassAllContentOnBoard_ToGivenContentDataCreator(Board board,
        List<Content> boardContent)
    {
        var dataCreatorMock = new Mock<IContentDataCreator>();
        var contentDataListCreator = new ContentDataListCreator(dataCreatorMock.Object);
        
        contentDataListCreator.GetContentDataList(board);
        
        foreach (var content in boardContent)
        {
            dataCreatorMock.Verify(creator => creator.GetContentData(content, It.IsAny<Vector2Int>()), Times.Once);
        }
    }

    public static IEnumerable<object[]> BoardWithContentTestCases()
    {
        yield return TwoCastlesCase();
        yield return CastlesAndTreesCase();
    }

    private static object[] TwoCastlesCase()
    {
        var board = GetFullBoard(10, 10);
        var firstCastle = GetCastle(GetPlayer(1));
        var secondCastle = GetCastle(GetPlayer(2));
        board[0, 0].AddContent(firstCastle);
        board[9, 9].AddContent(secondCastle);
        return new object[]
        {
            board,
            new List<Content>
            {
                firstCastle,
                secondCastle
            }
        };
    }

    private static object[] CastlesAndTreesCase()
    {
        var board = GetFullBoard(10, 10);
        var firstCastle = GetCastle(GetPlayer(1));
        var secondCastle = GetCastle(GetPlayer(2));
        var firstTree = GetTree();
        var secondTree = GetTree();
        board[1, 2].AddContent(firstCastle);
        board[5, 4].AddContent(secondCastle);
        board[8, 7].AddContent(firstTree);
        board[3, 6].AddContent(secondTree);
        return new object[]
        {
            board,
            new List<Content>
            {
                firstCastle,
                secondCastle,
                firstTree,
                secondTree
            }
        };
    }

    [Theory]
    [MemberData(nameof(ContentDataCreatorTestCases))]
    public void GetContentDataList_ShouldReturnListOfAppropriateContentData(Board board, IContentDataCreator creator, List<ContentData> expectedList)
    {
        var contentDataListCreator = new ContentDataListCreator(creator);
        
        var actualList = contentDataListCreator.GetContentDataList(board);
        
        Assert.Equal(expectedList.Count, actualList.Count);
        foreach (var contentData in expectedList)
        {
            Assert.Contains(contentData, actualList);
        }
    }

    public static IEnumerable<object[]> ContentDataCreatorTestCases()
    {
        yield return CastlesTestCase();
        yield return TreeAndKnightTestCase();
    }

    private static object[] CastlesTestCase()
    {
        var board = GetFullBoard(5, 5);
        var firstCastle = new CastleGO(GetPlayer(1), 3, 3, 1, 1);
        var secondCastle = new CastleGO(GetPlayer(2), 3, 3, 1, 1);
        board[0, 0].AddContent(firstCastle);
        board[4, 4].AddContent(secondCastle);
        var firstCastleData = new CastleData(new Vector2Int(0, 0), 1, 1, 3, 3, 1);
        var secondCastleData = new CastleData(new Vector2Int(4, 4), 1, 1, 3, 3, 2);
        var creatorMock = new Mock<IContentDataCreator>();
        creatorMock.Setup(creator => creator.GetContentData(firstCastle, new Vector2Int(0, 0))).Returns(firstCastleData);
        creatorMock.Setup(creator => creator.GetContentData(secondCastle, new Vector2Int(4, 4))).Returns(secondCastleData);
        return new object[]
        {
            board,
            creatorMock.Object,
            new List<ContentData>
            {
                firstCastleData,
                secondCastleData
            }
        };
    }

    private static object[] TreeAndKnightTestCase()
    {
        var board = GetFullBoard(5, 5);
        var tree = new Tree(3, true);
        var knight = new Knight(GetPlayer(1), 3, 3);
        board[0, 0].AddContent(tree);
        board[4, 4].AddContent(knight);
        var treeData = new TreeData(new Vector2Int(0, 0), 3, true);
        var knightData = new KnightData(new Vector2Int(4, 4), 3, 3, 1);
        var creatorMock = new Mock<IContentDataCreator>();
        creatorMock.Setup(creator => creator.GetContentData(tree, new Vector2Int(0, 0))).Returns(treeData);
        creatorMock.Setup(creator => creator.GetContentData(knight, new Vector2Int(4, 4))).Returns(knightData);
        return new object[]
        {
            board,
            creatorMock.Object,
            new List<ContentData>
            {
                treeData,
                knightData
            }
        };
    }
}