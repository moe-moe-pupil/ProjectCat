using Godot;

/// <summary lang='zh-CN'>
///     游戏中的主要单位，可以是角色、敌人，甚至是陷阱机关
/// </summary>
public partial class Unit : CharacterBody2D
{
    public RDKitTools.Unit.Unit Status { get; private set; }

    public float JumpVelocity { get; private set; } = 30;

    public float Gravity { get; private set; } = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public GlobalScene Global { get; private set; }

    public override void _Ready()
    {
        Global = GetNode<GlobalScene>("/root/GlobalScene");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Name == Global.Session.Username)
        {
            Vector2 velocity = Velocity;
            if (!IsOnFloor())
            {
                velocity.Y -= Gravity * (float)delta;
            }

            if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
            {
                velocity.Y = JumpVelocity;
            }

            Velocity = velocity;
            MoveAndSlide();
        }
    }
}