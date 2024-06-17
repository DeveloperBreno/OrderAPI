using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{

    [AllowAnonymous]
    public async Task SendMessage(string user, string message)
    {
        // Enviar a mensagem para todos os clientes conectados
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
