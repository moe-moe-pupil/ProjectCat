// --------------------------------------------------------------------------------------------------------------
// <copyright file="Player.cs" company="ProjectCat Technologies and contributors.">
// 此源代码的使用受 GNU AFFERO GENERAL PUBLIC LICENSE version 3 许可证的约束, 可以在以下链接找到该许可证.
// Use of this source code is governed by the GNU AGPLv3 license that can be found through the following link.
// https://github.com/moe-moe-pupil/ProjectCat/blob/main/LICENSE
// </copyright>
// --------------------------------------------------------------------------------------------------------------

using Godot;
using GodotUtilities;

/// <summary lang='zh-CN'>
///     玩家单位.
/// </summary>
public partial class Player : Unit
{
    private bool _bodyDirection = false;
    private float _timeSec;
    [Node]
    private AnimatedSprite2D _animatedSprite;
    [Node]
    private Label _name;
    private bool _isJump = false;
    private Camera2D _camera;
    [Node]
    private Timer _edgeJump, _holdJump, _bufferingJump;

    public override void _Ready()
    {
        this.WireNodes();
        this._name.Text = this.Name;
        if (this.Name == this.Global.Session.Username)
        {
            this._camera = this.GetNode<Camera2D>("Camera2D");
            this._camera.Current = true;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
      if (this.Name == this.Global.Session.Username)
        {
            Vector2 velocity = this.Velocity;
            if (this.IsOnFloor() && !Input.IsActionPressed("ui_accept"))
            {
                this._edgeJump.Start(-1);
                this._edgeJump.Paused = true;

                this._holdJump.Start(-1);
                this._holdJump.Paused = true;

                this._bufferingJump.Start(-1);
                this._bufferingJump.Paused = true;

            }

            if (Input.IsActionJustReleased("ui_accept"))
            {
                this._bufferingJump.Start(-1);
                this._bufferingJump.Paused = true;
            }

            if (this.IsOnFloor())
            {
                this._isJump = false;
            }
            else
            {
                this._edgeJump.Paused = false;
                velocity.y += this.Gravity * (float)delta * 130;
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

            if (Input.IsActionPressed("ui_accept") && this.IsOnFloor() && !this._bufferingJump.IsStopped())
            {
                this._isJump = true;
                this._holdJump.Start(-1);
                this._holdJump.Paused = false;
            }

            // 删除IsOnFloor()
            if (Input.IsActionPressed("ui_accept") && !this._holdJump.IsStopped()  && this._isJump)
            {
                velocity.y -= 14 * this.JumpVelocity;
            }

            Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

            if (inputDir.x > 0)
            {
                this._animatedSprite.FlipH = false;
                this._bodyDirection = false;
            }
            else if (inputDir.x < 0)
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
                velocity.x = direction.x * this.MoveSpeed * 100;
            }
            else if(this.IsOnFloor())
            {
                velocity.x = Mathf.MoveToward(this.Velocity.x, 0, this.MoveSpeed * 20);
            }
            else
            {
                velocity.x = Mathf.MoveToward(this.Velocity.x, 0, this.MoveSpeed * 10);
            }

            if (velocity.x == 0)
            {
                this._animatedSprite.Play("Idle");
            }
            else
            {
                this._animatedSprite.Play("Run");
            }
            this.Velocity = velocity;
            this.MoveAndSlide();
            var basicState = default(PlayerState.BasicState);
            basicState.setValues(this.Position, this._animatedSprite.Animation, this._animatedSprite.FlipH);
            this.Global.Socket.SendMatchStateAsync(this.Global.Match.Id, 1, Newtonsoft.Json.JsonConvert.SerializeObject(basicState));
      }
    }
}