using Godot;
using Nakama;

public partial class GlobalScene : Node
{
    static readonly string Address = "124.220.70.235", pcAddress = pcAddress = "res://src/unit/player.tscn";
    public Client NakamaClient = new("http", Address, 7350, "duckIsCat");
    public ISession Session;
    public ISocket Socket;
    public IMatch Match;
    // Called when the node enters the scene tree for the first time.
    /// <inheritdoc/>
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    /// <inheritdoc/>
    public override void _Process(double delta)
    {
    }

    public void AddPlayer(string name)
    {
        CharacterBody2D pc = GD.Load<PackedScene>(pcAddress).Instantiate() as CharacterBody2D;
        pc.Name = name;
        this.AddChild(pc);
    }

    public void RemovePlayer(string name)
    {
        CharacterBody2D pc = this.GetNode<CharacterBody2D>(name);
        this.RemoveChild(pc);
    }
}
