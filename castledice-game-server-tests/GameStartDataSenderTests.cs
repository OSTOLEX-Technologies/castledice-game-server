using casltedice_events_logic.ServerToClient;
using castledice_game_server_tests.TestImplementations;
using castledice_game_server.NetworkManager;
using castledice_riptide_dto_adapters.Extensions;
using Moq;
using Riptide;
using static castledice_game_server_tests.ObjectCreationUtility;

namespace castledice_game_server_tests;

public class GameStartDataSenderTests
{
    [Fact]
    public void SendGameStartData_ShouldThrowInvalidOperationException_IfPlayersIdsAreAbsentInPlayersDictionary()
    {
        var messageSender = new Mock<IMessageSenderById>().Object;
        var gameStartDataSender = new GameStartDataSender(messageSender);
        var gameStartData = GetGameStartData();
        
        Assert.Throws<InvalidOperationException>(() => gameStartDataSender.SendGameStartData(gameStartData));
    }

    [Theory]
    [MemberData(nameof(PlayersToClientsIds))]
    public void SendGameStartData_ShouldSendMessageWithCreateGameDTO_ToPlayersWithIdsFromGameStartData(Dictionary<int, ushort> playersToClientsIds)
    {
        SetUpPlayersDictionary(playersToClientsIds);
        var messageSenderMock = new Mock<IMessageSenderById>();
        var gameStartData = GetGameStartData(playersToClientsIds.Keys.ToArray());
        var gameStartDataSender = new GameStartDataSender(messageSenderMock.Object);
        
        gameStartDataSender.SendGameStartData(gameStartData);

        foreach (var clientId in playersToClientsIds.Values)
        {
            messageSenderMock.Verify(s => s.Send(It.IsAny<Message>(), clientId), Times.Once);
        }
        PlayersDictionary.Dictionary.Clear();
    }

    public static IEnumerable<object[]> PlayersToClientsIds()
    {
        yield return new[]
        {
            new Dictionary<int, ushort>
            {
                { 1, 3 },
                { 2, 4 }
            }
        };
        yield return new[]
        {
            new Dictionary<int, ushort>
            {
                { 1, 3 },
                { 2, 4 },
                { 3, 5 }
            }
        };
    }

    [Fact]
    public void SendGameStartData_ShouldSendMessage_WithAppropriateCreateGameDTO()
    {
        var firstPlayerId = 1;
        var secondPlayerId = 2;
        SetUpPlayersDictionary(new Dictionary<int, ushort>
        {
            { firstPlayerId, 3 },
            { secondPlayerId, 4 }
        });
        var gameStartData = GetGameStartData(firstPlayerId, secondPlayerId);
        var messageSender = new TestMessageSenderById();
        var gameStartDataSender = new GameStartDataSender(messageSender);
        var expectedDTO = new CreateGameDTO(gameStartData);
        
        gameStartDataSender.SendGameStartData(gameStartData);
        var sentMessage = messageSender.SentMessage;
        sentMessage.GetByte();
        sentMessage.GetByte();
        var sentDTO = sentMessage.GetCreateGameDTO();

        Assert.Equal(expectedDTO, sentDTO);
        PlayersDictionary.Dictionary.Clear();
    }

    private static void SetUpPlayersDictionary(Dictionary<int, ushort> playersIdsToClientsIds)
    {
        foreach (var keyValue in playersIdsToClientsIds)
        {
            PlayersDictionary.Dictionary.Add(keyValue.Key, keyValue.Value);
        }
    }
}