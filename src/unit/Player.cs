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
    private SmartTimer<TimerAction> _jumpTimer = new SmartTimer<TimerAction>();
    private bool _hasJumped = false;

    public void HandleHoldJump(ref Vector2 velocity)
    {
        if (this._hasJumped && Input.IsActionPressed("jump") && !this._jumpTimer.IsActionTimeGone(nameof(this._holdJumpTime)))
        {
            velocity.Y -= 14 * this.JumpVelocity;
        }

        if (Input.IsActionJustReleased("jump"))
        {
            this._jumpTimer.Stop(nameof(this._holdJumpTime));
        }
    }

    public override void _Ready()
    {
        this.WireNodes();
        this._jumpTimer.AddAction(this._coyoteJumpTime, nameof(this._coyoteJumpTime), true);
        this._jumpTimer.AddAction(this._holdJumpTime, nameof(this._holdJumpTime), false);
        this._jumpTimer.AddAction(this._bufferingJumpTime, nameof(this._bufferingJumpTime), true);
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = this.Velocity;
        if (this.IsOnFloor())
        {
            this._jumpTimer.ResetAllActions();
            this._hasJumped = false;
            if (Input.IsActionJustPressed("jump"))
            {
                this._hasJumped = true;
                this._jumpTimer.Start(nameof(this._holdJumpTime));
            }
        }
        else
        {
            this._jumpTimer.Update(delta, false);
            velocity.Y += this.Gravity * (float)delta * 130;
            if (Input.IsActionJustPressed("jump") && !this._jumpTimer.IsActionTimeGone(nameof(this._coyoteJumpTime)))
            {
                this._hasJumped = true;
                this._jumpTimer.Start(nameof(this._holdJumpTime));
            }
        }

        this.HandleHoldJump(ref velocity);

        Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

        if (inputDir.X > 0)
        {
            this._animatedSprite.FlipH = false;
            this._bodyDirection = false;
        }
        else if (inputDir.X < 0)
        {
            this._animatedSprite.FlipH = true;
            this._bodyDirection = true;
        }
        else
        {
            this._animatedSprite.FlipH = this._bodyDirection;
        }

        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * this.MoveSpeed * 100;
        }
        else if (this.IsOnFloor())
        {
            velocity.X = Mathf.MoveToward(this.Velocity.X, 0, this.MoveSpeed * 20);
        }
        else
        {
            velocity.X = Mathf.MoveToward(this.Velocity.X, 0, this.MoveSpeed * 10);
        }

        if (velocity.X == 0)
        {
            this._animatedSprite.Play("Idle");
        }
        else
        {
            this._animatedSprite.Play("Run");
        }

        this.Velocity = velocity;
        this.MoveAndSlide();
    }
}
