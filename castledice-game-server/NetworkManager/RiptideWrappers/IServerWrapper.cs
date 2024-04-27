using Riptide;

namespace castledice_game_server.NetworkManager.RiptideWrappers;

public interface IServerWrapper : IMessageSenderById, IClientDisconnecter
{
    public Server Server { get; }
    public event EventHandler<ServerDisconnectedEventArgs> ClientDisconnected;
}