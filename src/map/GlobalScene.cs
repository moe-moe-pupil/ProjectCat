using Godot;
using Nakama;

public partial class GlobalScene : Node
{
    public static readonly string Address = "124.220.70.235";
    public static readonly string PcAddress = "res://src/unit/player.tscn";

    [Signal]
    public delegate void GameBeginEventHandler();

    public Client NakamaClient { get; init; } = new("http", Address, 7350, "duckIsCat");

    public ISession Session { get; set; }

    public ISocket Socket { get; set; }

    public IMatch Match { get; set; }

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
    }

    public void GameStart()
    {
        GameBegin += () =>
        {
            GD.Print("Game Begin");
        };
        EmitSignal(SignalName.GameBegin);

    }

    public void AddPlayer(string name)
    {
        CharacterBody2D pc = GD.Load<PackedScene>(PcAddress).Instantiate() as CharacterBody2D;
        pc.Name = name;
        AddChild(pc);
    }

    public void RemovePlayer(string name)
    {
        CharacterBody2D pc = GetNode<CharacterBody2D>(name);
        RemoveChild(pc);
    }
}
