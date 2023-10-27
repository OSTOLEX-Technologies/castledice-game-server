using casltedice_events_logic.ServerToClient;
using castledice_game_data_logic;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager;

public class GameStartDataSender : IGameStartDataSender
{
    private IMessageSenderById _messageSender;

    public GameStartDataSender(IMessageSenderById messageSender)
    {
        _messageSender = messageSender;
    }

    public void SendGameStartData(GameStartData data)
    {
        var playersIds = data.PlayersIds;
        var clientsIds = new List<ushort>();
        foreach (var id in playersIds)
        {
            if (PlayersDictionary.Dictionary.TryGetValue(id, out var value))
            {
                clientsIds.Add(value);
            }
            else
            {
                throw new InvalidOperationException("No client id for player with id: " + id);
            }
        }

        var createGameDTO = new CreateGameDTO(data);
        foreach (var id in clientsIds)
        {
            var message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientMessageType.CreateGame);
            message.AddCreateGameDTO(createGameDTO);
            _messageSender.Send(message, id);
        }
    }
}