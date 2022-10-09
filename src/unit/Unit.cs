using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

/// <summary>
///   游戏中的主要单位，可以是角色、敌人，甚至是陷阱机关
/// </summary>
public partial class Unit : CharacterBody3D
{
    /// <summary>
    ///   单位的基础生命值
    /// </summary>
    public double RedHeart;

    /// <summary>
    ///   单位的额外生命值
    /// </summary>
    public double SoulHeart;

    /// <summary>
    ///   单位对物理攻击的抗性
    /// </summary>
    public int PhysicalDefense;

    /// <summary>
    ///   单位对魔法攻击的抗性
    /// </summary>
    public int MagicDefense;

    /// <summary>
    ///   单位的移动速度
    /// </summary>
    public int MoveSpeed = 5;

    /// <summary>
    ///   单位的眩晕抗性
    /// </summary>
    private int StunResistance;

    /// <summary>
    ///   单位的击退抗性
    /// </summary>
    private int BounceResistance;

    /// <summary>
    ///   单位目前装备的卡牌的编号
    /// </summary>
    public List<int> EquipmentBar = new List<int>();
    private float JumpVelocity = 5;
    public float Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    private AnimatedSprite3D _animatedSprite;
    private string _animName = "Idle";
    private Label3D _name;
    public bool BodyDirection = false;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Name = GetMultiplayerAuthority().ToString();
        _animatedSprite = GetNode<AnimatedSprite3D>("Sprite");
        _name = GetNode<Label3D>("Name");
        _name.Text = GetMultiplayerAuthority().ToString();
        if(IsMultiplayerAuthority())
        {
            Camera3D camera = GetNode<Camera3D>("Camera3D");
            camera.Current = true;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        if(IsMultiplayerAuthority())
        {
            Vector3 velocity = Velocity;
            if (!IsOnFloor())
            {
                velocity.y -= Gravity * (float)delta;
            }

            if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
            {
                velocity.y = JumpVelocity;
            }


            Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

            if (inputDir.x > 0)
            {
                _animatedSprite.FlipH = false;
                BodyDirection = false;
            }
            else if (inputDir.x < 0)
            {
                _animatedSprite.FlipH = true;
                BodyDirection = true;
            }
            else
            {
                _animatedSprite.FlipH = BodyDirection;
            }

            Vector3 direction = (Transform.basis * new Vector3(inputDir.x, inputDir.y, 0)).Normalized();
            if (direction != Vector3.Zero)
            {
                velocity.x = direction.x * MoveSpeed;
            }
            else
            {
                velocity.x = Mathf.MoveToward(Velocity.x, 0, MoveSpeed);
            }

            if (velocity.x == 0)
            {
                _animatedSprite.Play("Idle");
                _animName = "Idle";
            }
            else
            {
                _animatedSprite.Play("Run");
                _animName = "Run";
            }
            Velocity = velocity;
            MoveAndSlide();
            Rpc("RemoteSetStatus", GlobalPosition, _animName, BodyDirection);
        }
    }

    [RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
    public void RemoteSetStatus(Vector3 authP, string anim, bool flipH)
    {
        GlobalPosition = authP;
        if(_animatedSprite.Animation.ToString() != anim)
        {
            _animatedSprite.Play(anim);
        }
        if(_animatedSprite.FlipH != flipH)
        {
            _animatedSprite.FlipH = flipH;
        }
        
    }
}