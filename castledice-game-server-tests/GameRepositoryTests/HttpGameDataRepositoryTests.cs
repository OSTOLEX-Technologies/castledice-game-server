using System.Net;
using castledice_game_data_logic;
using castledice_game_server.GameRepository;
using castledice_game_server.HttpUtilities;
using Moq;
using Newtonsoft.Json;

namespace castledice_game_server_tests.GameRepositoryTests;
using static ObjectCreationUtility;


public class HttpGameDataRepositoryTests
{
    private class TestMessageSender : IHttpMessageSender
    {
        public string ContentString { get; private set; }
        public HttpResponseMessage ResponseMessage { get; set; } = new HttpResponseMessage();
        
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            ContentString = await request.Content.ReadAsStringAsync();
            return ResponseMessage;
        }
    }
    
    [Theory]
    [MemberData(nameof(GameDataCases))]
    public async void PostGameDataAsync_ShouldSendMessage_WithJsonSerializedGameDataAsContent(GameData gameData)
    {
        var messageSender = new TestMessageSender();
        var jsonToReturn = "{\"status\": \"created\", \"game\": " + JsonConvert.SerializeObject(gameData) + "}";
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(jsonToReturn)
        };
        messageSender.ResponseMessage = response;
        var httpGameDataRepository = new HttpGameDataRepository("http://localhost:5000", messageSender);

        await httpGameDataRepository.PostGameDataAsync(gameData);
        
        Assert.True(messageSender.ContentString == JsonConvert.SerializeObject(gameData));
    }
    

    [Theory]
    [InlineData("http://localhost:5000")]
    [InlineData("http://some_site/")]
    [InlineData("http://some_site/with/some/path")]
    public async void PostGameDataAsync_ShouldSendMessage_WithAppropriateUrl(string url)
    {
        var httpMessageSenderMock = new Mock<IHttpMessageSender>();
        SetUpPostResponse(httpMessageSenderMock, GetGameData());
        var httpGameDataRepository = new HttpGameDataRepository(url, httpMessageSenderMock.Object);
        Predicate<HttpRequestMessage> requestHasProperUrl = request =>
            request.RequestUri.ToString() == url + "/game";
        
        await httpGameDataRepository.PostGameDataAsync(GetGameData());
        
        httpMessageSenderMock.Verify(sender => sender.SendAsync(It.Is<HttpRequestMessage>(m => requestHasProperUrl(m))),
            Times.Once);
    }

    [Fact]
    public async void PostGameDataAsync_ShouldSendMessage_WithAppropriateMediaType()
    {
        var expectedMediaType = "application/json";
        var httpMessageSenderMock = new Mock<IHttpMessageSender>();
        SetUpPostResponse(httpMessageSenderMock, GetGameData());
        var httpGameDataRepository = new HttpGameDataRepository("http://localhost:5000", httpMessageSenderMock.Object);
        Predicate<HttpRequestMessage> requestHasProperMediaType = request =>
            request.Content.Headers.ContentType.MediaType == expectedMediaType;
        
        await httpGameDataRepository.PostGameDataAsync(GetGameData());
        
        httpMessageSenderMock.Verify(sender => sender.SendAsync(It.Is<HttpRequestMessage>(m => requestHasProperMediaType(m))),
            Times.Once);
    }
    
    [Fact]
    public async void PostGameDataAsync_ShouldSendMessage_WithPostMethod()
    {
        var httpMessageSenderMock = new Mock<IHttpMessageSender>();
        SetUpPostResponse(httpMessageSenderMock, GetGameData());
        var httpGameDataRepository = new HttpGameDataRepository("http://localhost:5000", httpMessageSenderMock.Object);
        Predicate<HttpRequestMessage> requestPredicate = request =>
            request.Method == HttpMethod.Post;
        
        await httpGameDataRepository.PostGameDataAsync(GetGameData());
        
        httpMessageSenderMock.Verify(sender => sender.SendAsync(It.Is<HttpRequestMessage>(m => requestPredicate(m))),
            Times.Once);
    }
    
    [Theory]
    [MemberData(nameof(GameDataCases))]
    public async void PostGameDataAsync_ShouldReturnGameData_ParsedFromResponse(GameData expectedGameData)
    {
        var httpMessageSenderMock = new Mock<IHttpMessageSender>();
        var httpGameDataRepository = new HttpGameDataRepository("http://localhost:5000", httpMessageSenderMock.Object);
        var jsonToReturn = "{\"status\": \"created\", \"game\": " + JsonConvert.SerializeObject(expectedGameData) + "}";
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(jsonToReturn)
        };
        httpMessageSenderMock.Setup(sender => sender.SendAsync(It.IsAny<HttpRequestMessage>()))
            .ReturnsAsync(response);
        
        var actualGameData = await httpGameDataRepository.PostGameDataAsync(GetGameData());
        
        Assert.Equal(expectedGameData, actualGameData);
    }
    
    private static void SetUpPostResponse(Mock<IHttpMessageSender> mockMessageSender, GameData expectedGameData)
    {
        var jsonToReturn = "{\"status\": \"created\", \"game\": " + JsonConvert.SerializeObject(expectedGameData) + "}";
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(jsonToReturn)
        };
        mockMessageSender.Setup(sender => sender.SendAsync(It.IsAny<HttpRequestMessage>()))
            .ReturnsAsync(response);
    }

public static IEnumerable<object[]> GameDataCases()
    {
        yield return new[]
        {
            GetGameData()
        };
        yield return new[]
        {
            GetGameData(playersIds: new List<int> { 1, 2, 3, 4 })
        };
        yield return new[]
        {
            GetGameData(playersIds: new List<int> { 1, 2, 3, 4 }, config: "anotherconfig")
        };
        yield return new[]
        {
            GetGameData(playersIds: new List<int> { 1, 2, 3, 4 }, config: "anotherconfig", id: 12)
        };
    }

}