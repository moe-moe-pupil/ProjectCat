using Godot;
using System;
using Nakama;

public partial class GlobalScene : Node
{
	static readonly string Address = "moemoepupil.com";
	public Client NakamaClient = new("http", Address, 7350, "duckIsCat");
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
