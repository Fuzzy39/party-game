using Godot;
using System;

public class BtnConnect : Button
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.

	private bool connected = false;
	WebSocketClient client = new WebSocketClient();
	public override void _Ready()
	{


		client.Connect("connection_error", this, "connectionError");
		client.Connect("connection_established", this, "ConnectionSuccess");

	   
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{

		client.Poll();
		//client.GetPeer(1).PutPacket("Please work".ToUTF8());
	}

	public void connectionError()
	{
		// oof.
		// what was the error? who knows!
		print("Failed to Connect.");
	}

	public void ConnectionSuccess(String protocol)
	{
		// send something to the server and print a message to the user
		//this.Disabled = true;
		connected = true;
		this.Text = "Disconnect";
		print("\nSuccessfully connected!");
		send("Example message");	
	}

	public void _on_button_up()
	{
		if (connected == false)
		{
			print("Trying to connect...");
			// wow, that was a lot
			String IP = ((LineEdit)(GetTree().Root.GetNode("Node2D").GetNode<Node>("LineEdit"))).Text;

			Error e = client.ConnectToUrl("ws://" + IP + ":9081");
			// Error e = client.ConnectToUrl("wss://demo.piesocket.com/v3/channel_1?api_key=oCdCMcMPQpbvNjUIzqtvF1d2X2okWpDQj4AwARJuAgtjhzKxVEjQU6IdCjwm&notify_self");
			if (e == Error.Ok)
				return;

			GD.PrintErr(e);
			print("Bad IP, couldn't connect.");
		}
		else
		{
			// disconnect
			client.DisconnectFromHost(reason: "requested by user");
			print("Disconnected!");
			connected = false;
			this.Text = "Connect";
		}

		// assuming e is good

	   

	}

	public void print(String toPrint)
	{
		GD.Print(toPrint);
		TextEdit output = null;
		foreach (Node child in this.GetTree().Root.GetChild(0).GetChildren())
		{
			if (child.Name == "Output")
			{
				output = (TextEdit)child;
			}
		}
		if (output == null)
		{
			GD.Print("Couldn't get console output");
			return;
		}

		output.Text += toPrint + "\n";

	}

	public void send(String message)
	{
		client.GetPeer(1).PutPacket(message.ToUTF8());
		print("Sent: " + message);
	}
}
