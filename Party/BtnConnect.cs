using Godot;
using System;

public class BtnConnect : Button
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
  

	WebSocketClient client = new WebSocketClient();
	public override void _Ready()
	{
		GD.Print("Hello!");

		client.Connect("connection_error", this, "connectionError");
		client.Connect("connection_established", this, "ConnectionSuccess");

	   
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{

		client.Poll();
	  //  client.GetPeer(1).PutPacket("Please work".ToUTF8());
	}

	public void connectionError()
	{
		// oof.
		// what was the error? who knows!
		GD.Print("Failed to Connect.");
	}

	public void ConnectionSuccess(String protocol)
	{
		// send something to the server and print a message to the user.
		GD.Print("Successfully connected!");
		client.GetPeer(1).PutPacket("Please work".ToUTF8());
	}

	public void _on_button_up()
	{
		GD.Print("clicked!");
		// wow, that was a lot
		String IP = ((LineEdit)(GetTree().Root.GetNode("Node2D").GetNode<Node>("LineEdit"))).Text;

		Error e = client.ConnectToUrl("ws://" + IP + ":9081");
	   // Error e = client.ConnectToUrl("wss://demo.piesocket.com/v3/channel_1?api_key=oCdCMcMPQpbvNjUIzqtvF1d2X2okWpDQj4AwARJuAgtjhzKxVEjQU6IdCjwm&notify_self");
		if (e == Error.Ok)
			return;
		GD.PrintErr(e);

		// assuming e is good

	   

	}
}
