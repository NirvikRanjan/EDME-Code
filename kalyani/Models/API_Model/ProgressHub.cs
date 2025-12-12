using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AutoSherpa_project.Models.API_Model
{
    public class ProgressHub: Hub
    {
        private static List<userConnectionDetails> CurrentConnections = new List<userConnectionDetails>();
    
        //public override Task OnConnected()
        //{
        //    userConnectionDetails logedinUsers = new userConnectionDetails();
        //    logedinUsers.ConnectionID = Context.ConnectionId;
        //    logedinUsers.Usrname = Context.QueryString["Name"];

        //    if(CurrentConnections.Count(m=>m.Usrname==logedinUsers.Usrname)==1)
        //    {
        //        Clients.Client(logedinUsers.ConnectionID).logoutExists("Login");
        //    }
        //    else
        //    {
        //        CurrentConnections.Add(logedinUsers);
        //    }

        //    return base.OnConnected();
        //}

        //public override Task OnDisconnected(bool stopCalled)
        //{
        //    var connection = CurrentConnections.FirstOrDefault(x => x.ConnectionID == Context.ConnectionId);

        //    if (connection != null)
        //    {
        //        CurrentConnections.Remove(connection);
        //    }

        //    return base.OnDisconnected(stopCalled);
        //}
    }
    class userConnectionDetails
    {
        public string Usrname { set; get; }
        public string ConnectionID { set; get; }
    }
}