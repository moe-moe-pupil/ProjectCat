// --------------------------------------------------------------------------------------------------------------
// <copyright file="Player.cs" company="ProjectCat Technologies and contributors.">
// 此源代码的使用受 GNU AFFERO GENERAL PUBLIC LICENSE version 3 许可证的约束, 可以在以下链接找到该许可证.
// Use of this source code is governed by the GNU AGPLv3 license that can be found through the following link.
// https://github.com/moe-moe-pupil/ProjectCat/blob/main/LICENSE
// </copyright>
// --------------------------------------------------------------------------------------------------------------

using Godot;
using GodotUtilities;
using RDKitTools.Utils;

/// <summary lang='zh-CN'>
///     玩家单位.
/// </summary>
public partial class Player : Unit
{
    private bool _bodyDirection = false;
    [Node]
    private AnimatedSprite2D _animatedSprite;
    private Camera2D _camera;
    [Export]
    private double _coyoteJumpTime = 0.07, _holdJumpTime = 0.15, _bufferingJumpTime = 0.2;
    [Export]
    private float _moveSpeed = 5f;
    private SmartTimer<TimerAction> _jumpTimer = new SmartTimer<TimerAction>();
    private bool _hasJumped = false;

    public void HandleHoldJump(ref Vector2 velocity)
    {
        if (_hasJumped && Input.IsActionPressed("jump") && !_jumpTimer.IsActionTimeGone(nameof(_holdJumpTime)))
        {
            velocity.Y -= 14 * JumpVelocity;
        }

        if (Input.IsActionJustReleased("jump"))
        {
            _jumpTimer.Stop(nameof(_holdJumpTime));
        }
    }

    public override void _Ready()
    {
        this.WireNodes();
        _jumpTimer.AddAction(_coyoteJumpTime, nameof(_coyoteJumpTime), true);
        _jumpTimer.AddAction(_holdJumpTime, nameof(_holdJumpTime), false);
        _jumpTimer.AddAction(_bufferingJumpTime, nameof(_bufferingJumpTime), true);
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        if (IsOnFloor())
        {
            _jumpTimer.ResetAllActions();
            _hasJumped = false;
            if (Input.IsActionJustPressed("jump"))
            {
                _hasJumped = true;
                _jumpTimer.Start(nameof(_holdJumpTime));
            }
        }
        else
        {
            _jumpTimer.Update(delta, false);
            velocity.Y += Gravity * (float)delta * 130;
            if (Input.IsActionJustPressed("jump") && !_jumpTimer.IsActionTimeGone(nameof(_coyoteJumpTime)))
            {
                _hasJumped = true;
                _jumpTimer.Start(nameof(_holdJumpTime));
            }
        }

        HandleHoldJump(ref velocity);

        Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

        if (inputDir.X > 0)
        {
            _animatedSprite.FlipH = false;
            _bodyDirection = false;
        }
        else if (inputDir.X < 0)
        {
            _animatedSprite.FlipH = true;
            _bodyDirection = true;
        }
        else
        {
            _animatedSprite.FlipH = _bodyDirection;
        }

        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * _moveSpeed * 100;
        }
        else if (IsOnFloor())
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, _moveSpeed * 20);
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, _moveSpeed * 10);
        }

        if (velocity.X == 0)
        {
            _animatedSprite.Play("Idle");
        }
        else
        {
            _animatedSprite.Play("Run");
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}