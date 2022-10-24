using Godot;
using System;
using Nakama;

public partial class GlobalScene : Node
{
	static readonly string Address = "moemoepupil.com";
	public Client NakamaClient = new("http", Address, 7350, "duckIsCat");
	public ISession Session;
	public ISocket Socket;
	public IMatch Match;
	public struct BasicState
    {
		public string Anim;
		public Vector3 pos;
		public bool Flip;
    }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
