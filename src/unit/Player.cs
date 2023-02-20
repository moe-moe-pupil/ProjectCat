// --------------------------------------------------------------------------------------------------------------
// <copyright file="Player.cs" company="ProjectCat Technologies and contributors.">
// 此源代码的使用受 GNU AFFERO GENERAL PUBLIC LICENSE version 3 许可证的约束, 可以在以下链接找到该许可证.
// Use of this source code is governed by the GNU AGPLv3 license that can be found through the following link.
// https://github.com/moe-moe-pupil/ProjectCat/blob/main/LICENSE
// </copyright>
// --------------------------------------------------------------------------------------------------------------

using System;
using Godot;
using GodotUtilities;

/// <summary lang='zh-CN'>
///     玩家单位.
/// </summary>
public partial class Player : Unit
{
    private bool _bodyDirection = false;
    [Node]
    private AnimatedSprite2D _animatedSprite;
    private bool _isJump = false;
    private bool _isHoldJump = false;
    private Camera2D _camera;
    public bool CouldTrans = false;
    [Node]
    private Timer _edgeJump, _holdJump, _bufferingJump;
    [Export]
    private float _power = 0.5f;

    public override void _Ready()
    {
        this.WireNodes();
    }

    public async void OnEnter(Node2D node)
    {
        GD.Print("OK");
    }

    public override void _PhysicsProcess(double delta)
    {
        var collision = this.MoveAndCollide(this.Velocity * new Vector2((float)delta, (float)delta) * _power, true, 0.08f, true);
        Vector2 velocity = this.Velocity;
        if (this.IsOnFloor() && !Input.IsActionPressed("ui_accept"))
        {
            this._edgeJump.Start(timeSec: -1);
            this._edgeJump.Paused = true;

            this._holdJump.Start(timeSec: -1);
            this._holdJump.Paused = true;

            this._bufferingJump.Start(timeSec: -1);
            this._bufferingJump.Paused = true;
        }

        if (Input.IsActionJustReleased("ui_accept"))
        {
            this._bufferingJump.Start(timeSec: -1);
            this._bufferingJump.Paused = true;
            this._isHoldJump = false;
        }

        if (this.IsOnFloor())
        {
            this._isJump = false;
        }
        else
        {
            this._edgeJump.Paused = false;
            velocity.Y += this.Gravity * (float)delta * 130;
        }

        // 处理边缘跳跃和长按跳跃 成功触发跳跃
        if (Input.IsActionJustPressed("ui_accept") && (this.IsOnFloor() || !this._edgeJump.IsStopped()))
        {
            this._holdJump.Paused = false;
            this._isJump = true;
        }

        if (Input.IsActionJustPressed("ui_accept") && !this.IsOnFloor() && this._edgeJump.IsStopped())
        {
            this._bufferingJump.Paused = false;
        }

        if (Input.IsActionJustPressed("restart") && !this.IsOnFloor() && this._edgeJump.IsStopped())
        {
            this.GetTree().ReloadCurrentScene();
        }

        if (Input.IsActionPressed("ui_accept") && this.IsOnFloor() && !this._bufferingJump.IsStopped() && !_isHoldJump)
        {
            this._isJump = true;
            this._holdJump.Start(timeSec: -1);
            this._holdJump.Paused = false;
            this._isHoldJump = true;
        }

        // 删除IsOnFloor()
        if (Input.IsActionPressed("ui_accept") && !this._holdJump.IsStopped() && this._isJump)
        {
            velocity.Y -= 14 * this.JumpVelocity;
        }

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

        if (collision != null)
        {
            if (((Node)collision.GetCollider()).Name == "Terrian")
            {
                float dirVec = collision.GetNormal().Dot(new Vector2(0, -1));
                if (dirVec > 0.8f && velocity.Y > 500)
                {
                    TileMap terrain = (TileMap)collision.GetCollider();
                    Vector2I cell = terrain.LocalToMap(collision.GetPosition() - collision.GetNormal());
                    terrain.SetCell(0, cell, 0);
                    velocity.Y += this.Gravity * (float)delta * 130;
                }
            }
        }

        this.Velocity = velocity;
        this.MoveAndSlide();
    }
}
