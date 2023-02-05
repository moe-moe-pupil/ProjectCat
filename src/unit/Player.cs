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
    [Node]
    private AnimatedSprite2D _animatedSprite;
    [Node]
    private Label _name;
    private bool _isJump = false;
    private Camera2D _camera;
    [Node]
    private Timer _edgeJump,_holdJump,_bufferingJump;
    public bool BodyDirection = false;
    public float time_sec;

    public override void _Ready()
    {

        this.WireNodes();
        _name.Text = Name;

        if (Name == Global.Session.Username)
        {
            _camera = GetNode<Camera2D>("Camera2D");
            _camera.Current = true;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
      //   if (Name == Global.Session.Username)
        // {
            Vector2 velocity = Velocity;
            if (IsOnFloor() && !Input.IsActionPressed("ui_accept"))
            {
                _edgeJump.Start(time_sec = -1);
                _edgeJump.Paused = true;

                _holdJump.Start(time_sec = -1);
                _holdJump.Paused = true;

                _bufferingJump.Start(time_sec = -1);
                _bufferingJump.Paused = true;

            }

            if (Input.IsActionJustReleased("ui_accept"))
            {
                _bufferingJump.Start(time_sec = -1);
                _bufferingJump.Paused = true;
            }

            if (IsOnFloor())
            {
                _isJump = false;
            }
            else
            {
                _edgeJump.Paused = false;
                velocity.y += Gravity * (float)delta * 130;
            }

            //处理边缘跳跃和长按跳跃 成功触发跳跃
            if (Input.IsActionJustPressed("ui_accept") && (IsOnFloor() || !_edgeJump.IsStopped()))
            {
                _holdJump.Paused = false;
                _isJump = true;
            }

            if (Input.IsActionJustPressed("ui_accept") && !IsOnFloor() && _edgeJump.IsStopped())
            {
                _bufferingJump.Paused = false;
            }

            if (Input.IsActionPressed("ui_accept") && IsOnFloor() && !_bufferingJump.IsStopped())
            {
                _isJump = true;
                _holdJump.Start(time_sec = -1);
                _holdJump.Paused = false;
            }

            //删除IsOnFloor()
            if (Input.IsActionPressed("ui_accept") && !_holdJump.IsStopped()  && _isJump)
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
                velocity.x = Mathf.MoveToward(Velocity.x, 0, MoveSpeed * 10);
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