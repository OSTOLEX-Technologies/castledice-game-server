using System.Net;
using castledice_game_server.Auth;
using castledice_game_server.HttpUtilities;
using Moq;

namespace castledice_game_server_tests.AuthTests;

public class HttpIdRetrieverTests
{
    [Theory]
    [InlineData("sometoken", "https://auth-service.com/api/players")]
    [InlineData("someothertoken", "https://some-service.com/api/players")]
    public async void RetrievePlayerIdAsync_ShouldSendAppropriateMessage(string token, string url)
    {
        var mockMessageSender = new Mock<IHttpMessageSender>();
        HttpRequestMessage actualMessage = null;
        var responseString = new StringContent("""{"id": 1}""");
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = responseString };
        //Here we make our mock return the response we want and also capture the message it was called with
        mockMessageSender.Setup(sender => sender.SendAsync(It.IsAny<HttpRequestMessage>()))
            .Callback<HttpRequestMessage>(message => actualMessage = message)
            .ReturnsAsync(response);
        var retriever = new HttpIdRetriever(url, mockMessageSender.Object);
        
        await retriever.RetrievePlayerIdAsync(token);
        
        Assert.Equal(HttpMethod.Get, actualMessage.Method);
        Assert.Equal(url + "/me", actualMessage.RequestUri.ToString());
        Assert.Equal("Bearer", actualMessage.Headers.Authorization.Scheme);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async void RetrievePlayerIdAsync_ShouldReturnId_FromReturnedJson(int expectedId)
    {
        var mockMessageSender = new Mock<IHttpMessageSender>();
        var responseString = new StringContent($"{{\"id\": {expectedId}}}");
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = responseString };
        mockMessageSender.Setup(sender => sender.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(response);
        var retriever = new HttpIdRetriever("https://auth-service.com/api/players/me", mockMessageSender.Object);
        
        var actualId = await retriever.RetrievePlayerIdAsync("sometoken");
        
        Assert.Equal(expectedId, actualId);
    }
    
    [Fact]
    public async void RetrievePlayerIdAsync_ShouldThrowArgumentException_IfResponseDoesNotContainIdField()
    {
        var mockMessageSender = new Mock<IHttpMessageSender>();
        var responseString = new StringContent("""{}""");
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = responseString };
        mockMessageSender.Setup(sender => sender.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(response);
        var retriever = new HttpIdRetriever("https://auth-service.com/api/players/me", mockMessageSender.Object);
        
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await retriever.RetrievePlayerIdAsync("sometoken"));
    }
    
}