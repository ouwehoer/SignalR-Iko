using Microsoft.AspNetCore.SignalR;

namespace SignalR.Hubs;

public class ChatHub
    : Hub
{
    public ChatHub(ILogger<ChatHub> logger)
    {
        Logger = logger;
    }

    public ILogger<ChatHub> Logger { get; set; }

    public async Task SendMessage(
        string chatName,
        string username,
        string message)
    {
        await Clients
            .Group(chatName)
            .SendAsync("ReceiveChat",username, message);
    }

    public async Task AddToChat(
        string chatName,
        string userName)
    {
        // function below creates group if not existant
        // if ConnectionID already in group, function does nothing
        await Groups.AddToGroupAsync(Context.ConnectionId, chatName);

        await SendMessage(chatName, userName, $"is connected");
    }

    public async Task RemoveFromChat(
        string chatName)
    {
        // function below creates group if not existant
        // if ConnectionID already in group, function does nothing
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatName);
    }

    public async override Task OnConnectedAsync()
    {
        Logger.LogInformation($"Connected Client {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if (exception != null)
        {
            Logger.LogError($"{nameof(ChatHub)}.{nameof(OnDisconnectedAsync)} got an Exception: {exception.Message}");
        }
        else
        {
            Logger.LogInformation($"Disconnected Client {Context.ConnectionId}");
        }
        return base.OnDisconnectedAsync(exception);
    }
}