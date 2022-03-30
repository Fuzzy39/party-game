using Godot;
using System;

public class PartyServer : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    
    const int PORT = 9081;

    WebSocketServer Server = new WebSocketServer();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // text
        Server.Connect("client_connected", this, "Connected");
        Server.Connect("client_disconnected", this, "Disconnected");
        Server.Connect("client_close_request", this, "CloseRequest"); // we can ignore
        Server.Connect("data_received", this, "OnData");

        // try to listen, I guess.
        Server.BindIp = "192.168.1.9";
        Error e = Server.Listen(PORT, null, false);
        if (e != Error.Ok)
        {
            GD.Print("Unable to start server: "+e);
           
        }
        else
        {
            GD.Print("Server up! Listening for connections at " + IP.GetLocalAddresses().ToString()+"...");
        }

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        
        Server.Poll();
         
    }

    public void Connected(int id, String proto)
    {
        GD.Print("Client " + id + " has Connected!");
    }

    public void Disconnected(int id, bool wasClean=false )
    {
        GD.Print("Client " + id + " Disconnected");

    }

    public void OnData(int id)
    {
        String packet = Server.GetPeer(id).GetPacket().ToString();

        GD.Print("Client " + id + " Sent: " + packet);

    }


   
}
