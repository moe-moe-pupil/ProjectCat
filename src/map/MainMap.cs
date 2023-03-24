using Godot;
using GodotUtilities;
using System;

public partial class MainMap : Node2D
{
    [Node("/root/GlobalScene")]
    private GlobalScene _global;
    [Node]
    private TileMap _terrian;

    public override void _Ready()
    {
        this.WireNodes();
        _global.GameBegin += () =>
        {
            _terrian.Visible = true;
        };
    }

    public override void _Process(double delta)
    {
    }

    public void OnGameBegin()
    {

    }
}
