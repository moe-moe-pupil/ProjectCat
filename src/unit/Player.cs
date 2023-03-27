// --------------------------------------------------------------------------------------------------------------
// <copyright file="Player.cs" company="ProjectCat Technologies and contributors.">
// 此源代码的使用受 GNU AFFERO GENERAL PUBLIC LICENSE version 3 许可证的约束, 可以在以下链接找到该许可证.
// Use of this source code is governed by the GNU AGPLv3 license that can be found through the following link.
// https://github.com/moe-moe-pupil/ProjectCat/blob/main/LICENSE
// </copyright>
// --------------------------------------------------------------------------------------------------------------

using Godot;
using GodotUtilities;
using PlayerStates;
using RDKitTools.Utils;

/// <summary lang='zh-CN'>
///     玩家单位.
/// </summary>
public partial class Player : Unit
{
    [Export]
    public float JumpVelocity { get; private set; } = 30;

    [Node("/root/GlobalScene")]
    private GlobalScene _global;
    [Node]
    private AnimatedSprite2D _animatedSprite;
    [Node]
    private Label _label;
    [Node("/root/GlobalScene/ProjectileSpawner")]
    private ProjectileSpawner _projectileSpawner;
    [Export]
    private double _coyoteJumpTime = 0.07, _holdJumpTime = 0.15, _bufferingJumpTime = 0.2;
    [Export]
    private float _moveSpeed = 5f;
    private SmartTimer<TimerAction> _jumpTimers = new SmartTimer<TimerAction>();
    private bool _hasJumped = false;

    public void HandleHoldJump(ref Vector2 velocity, double delta)
    {
        if (!_jumpTimers.IsActionTimeGone(nameof(_holdJumpTime))
        && _jumpTimers.IsActionActive(nameof(_holdJumpTime)))
        {
            if (_hasJumped && Input.IsActionPressed("jump"))
            {
                velocity.Y -= 110 * JumpVelocity * (float)delta;
            }
        }
        else
        {
            if (!IsOnFloor())
            {
                velocity.Y += Gravity * (float)delta * 130;
            }
        }

        if (Input.IsActionJustReleased("jump"))
        {
            _jumpTimers.Stop(nameof(_holdJumpTime));
        }
    }

    public override void _Ready()
    {
        this.WireNodes();
        _label.Text = Name;
        _jumpTimers.AddAction(_coyoteJumpTime, nameof(_coyoteJumpTime), true);
        _jumpTimers.AddAction(_holdJumpTime, nameof(_holdJumpTime), true);
        _jumpTimers.AddAction(_bufferingJumpTime, nameof(_bufferingJumpTime), false);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Name == _global.Session.Username)
        {
            Vector2 velocity = Velocity;
            if (IsOnFloor())
            {
                _hasJumped = false;
                _jumpTimers.ResetAction(nameof(_holdJumpTime));
                _jumpTimers.ResetAction(nameof(_coyoteJumpTime));
                if (Input.IsActionJustPressed("jump") || (_jumpTimers.IsActionActive(nameof(_bufferingJumpTime))
                    && !_jumpTimers.IsActionTimeGone(nameof(_bufferingJumpTime))))
                {
                    _jumpTimers.ResetAction(nameof(_bufferingJumpTime));
                    _hasJumped = true;
                    _jumpTimers.Start(nameof(_holdJumpTime));
                    velocity.Y = -200 * JumpVelocity * (float)delta;
                }
            }
            else
            {
                // only update timers after player leaving the floor.
                _jumpTimers.Update(delta, false);
                if (Input.IsActionJustPressed("jump"))
                {
                    if (!_jumpTimers.IsActionTimeGone(nameof(_coyoteJumpTime)))
                    {
                        _hasJumped = true;
                        velocity.Y = -200 * JumpVelocity * (float)delta;
                        _jumpTimers.Start(nameof(_holdJumpTime));
                    }
                    else if (!_jumpTimers.IsActionActive(nameof(_holdJumpTime)) || _jumpTimers.IsActionTimeGone(nameof(_holdJumpTime)))
                    {
                        _jumpTimers.ResetAction(nameof(_bufferingJumpTime));
                        _jumpTimers.Start(nameof(_bufferingJumpTime));
                    }
                }
            }

            HandleHoldJump(ref velocity, delta);

            Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

            if (inputDir != Vector2.Zero)
            {
                velocity.X = inputDir.X * _moveSpeed * 100;
                if (inputDir.X > 0)
                {
                    _animatedSprite.FlipH = false;
                }
                else if (inputDir.X < 0)
                {
                    _animatedSprite.FlipH = true;
                }
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
            var basicState = new SBaiscState
            {
                Anim = _animatedSprite.Animation,
                Pos = Position,
                Flip = _animatedSprite.FlipH,
            };

            if (Input.IsActionPressed("normalAttack"))
            {
                _projectileSpawner.SpawnBullet(GetGlobalMousePosition() - GlobalPosition, 10, GlobalPosition);
            }

            _global.Socket.SendMatchStateAsync(_global.Match.Id, 1, Newtonsoft.Json.JsonConvert.SerializeObject(basicState));
        }
    }
}