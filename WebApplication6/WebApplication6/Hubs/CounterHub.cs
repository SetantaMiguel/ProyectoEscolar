using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace WebApplication6.Hubs
{
    public class CounterHub : Hub
    {

        public static int contador = 0;
        public override Task OnConnected()
        {
            contador++;
            Clients.All.mostrar(contador);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            contador--;
            Clients.All.mostrar(contador);
            return base.OnDisconnected(stopCalled);
        }
    }
}