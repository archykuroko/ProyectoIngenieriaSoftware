﻿using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ProyectoISNuevo.Hubs
{
    public class ChatHub : Hub
    {
        public async Task EnviarMensaje(string usuario, string mensaje)
        {
            await Clients.All.SendAsync("RecibirMensaje", usuario, mensaje);
        }
    }
}
