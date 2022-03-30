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
		Server.BindIp = "172.16.112.39";
		Error e = Server.Listen(PORT, null, false);
		if (e != Error.Ok)
		{
			print("Unable to start server: "+e);
			print("Potential IP addresses: "+IP.GetLocalAddresses().ToString());
		   
		}
		else
		{
			print("Server up! Listening for connections at " + Server.BindIp+"...");
		}

	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		
		Server.Poll();
		 
	}
	
	public void print(String toPrint)
	{
		GD.Print(toPrint);
		TextEdit output = null;
		foreach(Node child in this.GetChildren())
		{
			if(child.Name == "Output")
			{
				output = (TextEdit)child;
			}
		}
		if(output==null)
		{
			GD.Print("Couldn't get console output");
		}
		
		output.Text += toPrint+"\n";
		
	}

	public void Connected(int id, String proto)
	{
		print("Client " + id + " has Connected!");
	}

	public void Disconnected(int id, bool wasClean=false )
	{
		print("Client " + id + " Disconnected");

	}

	public void OnData(int id)
	{
		String packet = Server.GetPeer(id).GetPacket().ToString();

		print("Client " + id + " Sent: " + packet);

	}


   
}
