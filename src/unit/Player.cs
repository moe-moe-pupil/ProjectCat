using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Newtonsoft;
using GodotUtilities;
/// <summary lang='zh-CN'>
///     玩家单位
/// </summary>
public partial class Player : Unit
{
    private AnimatedSprite2D _animatedSprite;
    [Node]
    private Label _name;
    private bool _isJump = false;
    private Camera2D _camera;
    private Timer _timer,_timer2,_timer3; //_timer = EdgeJump;_timer2 = HoldJump;_timer3 = BufferingJump.
    public bool BodyDirection = false;
    public float time_sec;
    // Called when the node enters the scene tree for the first time.
    /// <inheritdoc/>
    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite2D>("Sprite");
        _timer = GetNode<Timer>("EdgeJump");
        _timer2 = GetNode<Timer>("HoldJump");
        _timer3 = GetNode<Timer>("BufferingJump");

        this.WireNodes();
        _name.Text = Name;

        if (Name == Global.Session.Username)
        {
            _camera = GetNode<Camera2D>("Camera2D");
            _camera.Current = true;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    /// <inheritdoc/>
    public override void _PhysicsProcess(double delta)
    {
      //   if (Name == Global.Session.Username)
        // {
            Vector2 velocity = Velocity;
            if (IsOnFloor() && !Input.IsActionPressed("ui_accept"))
            {
                _timer.Start(time_sec = -1);
                _timer.Paused = true;

                _timer2.Start(time_sec = -1);
                _timer2.Paused = true;

                _timer3.Start(time_sec = -1);
                _timer3.Paused = true;


            }

            if (Input.IsActionJustReleased("ui_accept"))
            {
                _timer3.Start(time_sec = -1);
                _timer3.Paused = true;
            }

            if (IsOnFloor())
            {
                _isJump = false;
            }


            if (!IsOnFloor())
            {
                _timer.Paused = false;
                velocity.y += Gravity * (float)delta * 130;
            }

            if (Input.IsActionJustPressed("ui_accept") && (IsOnFloor() || !_timer.IsStopped()))
            {
                _timer2.Paused = false;
                _isJump = true;
            }

            if (Input.IsActionJustPressed("ui_accept"))
            {
                _timer3.Paused = false;
            }

            if (Input.IsActionPressed("ui_accept") && IsOnFloor() && !_timer3.IsStopped())
            {
                _isJump = true;
                //_timer2.Start(time_sec = -1);
               

            }

            if (Input.IsActionPressed("ui_accept") && (IsOnFloor() || !_timer2.IsStopped() ) && _isJump)
            {
                velocity.y -= 14 * JumpVelocity;
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

            Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
            if (direction != Vector2.Zero)
            {
                velocity.x = direction.x * MoveSpeed * 100;
            }
            else if(IsOnFloor())
            {
                velocity.x = Mathf.MoveToward(Velocity.x, 0, MoveSpeed * 20);
            }
            else
            {
                velocity.x = Mathf.MoveToward(Velocity.x, 0, MoveSpeed * 3);
            }

            if (velocity.x == 0)
            {
                _animatedSprite.Play("Idle");
            }
            else
            {
                _animatedSprite.Play("Run");
            }
            Velocity = velocity;
            MoveAndSlide();
            var basicState = new PlayerState.BasicState();
            basicState.setValues(Position, _animatedSprite.Animation, _animatedSprite.FlipH);
            Global.Socket.SendMatchStateAsync(Global.Match.Id, 1, Newtonsoft.Json.JsonConvert.SerializeObject(basicState));
      //  }
    }
}