using System.Net;
using castledice_game_server.Auth;
using castledice_game_server.HttpUtilities;
using Moq;

namespace castledice_game_server_tests.AuthTests;

public class HttpIdRetrieverTests
{
    [Theory]
    [InlineData("sometoken", "https://auth-service.com/api/players/me")]
    [InlineData("someothertoken", "https://some-service.com/api/players/me")]
    public async void RetrievePlayerIdAsync_ShouldSendAppropriateMessage(string token, string url)
    {
        var mockMessageSender = new Mock<IHttpMessageSender>();
        SetUpMockMessageSender(mockMessageSender, """{"id": 1}""");
        var retriever = new HttpIdRetriever(url, mockMessageSender.Object);
        Predicate<HttpRequestMessage> messageIsAppropriate = message =>
        {
            var bearer = message.Headers.Authorization.Scheme;
            var actualToken = message.Headers.Authorization.Parameter;
            var uri = message.RequestUri.ToString();
            return bearer == "Bearer" && actualToken == token && uri == url;
        };
        
        await retriever.RetrievePlayerIdAsync(token);
        
        mockMessageSender.Verify(sender => sender.SendAsync(It.Is<HttpRequestMessage>(m => messageIsAppropriate(m))));
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async void RetrievePlayerIdAsync_ShouldReturnId_FromReturnedJson(int expectedId)
    {
        var mockMessageSender = new Mock<IHttpMessageSender>();
        SetUpMockMessageSender(mockMessageSender, $"{{\"id\": {expectedId}}}");
        var retriever = new HttpIdRetriever("https://auth-service.com/api/players/me", mockMessageSender.Object);
        
        var actualId = await retriever.RetrievePlayerIdAsync("sometoken");
        
        Assert.Equal(expectedId, actualId);
    }
    
    [Fact]
    public async void RetrievePlayerIdAsync_ShouldThrowArgumentException_IfResponseDoesNotContainIdField()
    {
        var mockMessageSender = new Mock<IHttpMessageSender>();
        SetUpMockMessageSender(mockMessageSender, """{}""");
        var retriever = new HttpIdRetriever("https://auth-service.com/api/players/me", mockMessageSender.Object);
        
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await retriever.RetrievePlayerIdAsync("sometoken"));
    }
    
    private static void SetUpMockMessageSender(Mock<IHttpMessageSender> mockMessageSender, string responseStr)
    {
        var responseString = new StringContent(responseStr);
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = responseString
        };
        mockMessageSender.Setup(sender => sender.SendAsync(It.IsAny<HttpRequestMessage>()))
            .ReturnsAsync(response);
    }
}