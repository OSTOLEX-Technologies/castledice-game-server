using castledice_events_logic.ServerToClient;
using castledice_game_data_logic;
using castledice_game_server.NetworkManager.PlayersTracking;
using castledice_game_server.NetworkManager.RiptideWrappers;
using castledice_riptide_dto_adapters.Extensions;
using Riptide;

namespace castledice_game_server.NetworkManager.Senders;

public class GameStartDataSender : IGameStartDataSender
{
    private readonly IMessageSenderById _messageSender;
    private readonly IPlayerToClientIdsMap _idsMap;

    public GameStartDataSender(IMessageSenderById messageSender, IPlayerToClientIdsMap idsMap)
    {
        _messageSender = messageSender;
        _idsMap = idsMap;
    }

    public void SendGameStartData(GameStartData data)
    {
        var clientsIds = GetClientIds(data.PlayersData.Select(playerData => playerData.PlayerId).ToList());
        var createGameDTO = new CreateGameDTO(data);
        SendDTOToClients(createGameDTO, clientsIds);
    }

    private List<ushort> GetClientIds(List<int> playerIds)
    {
        return playerIds.Select(id => _idsMap.GetClientByPlayer(id)).ToList();
    }

    private void SendDTOToClients(CreateGameDTO dto, List<ushort> clientsIds)
    {
        foreach (var id in clientsIds)
        {
            var message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientMessageType.CreateGame);
            message.AddCreateGameDTO(dto);
            _messageSender.Send(message, id);
        }
    }
}