using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Newtonsoft;

/// <summary lang='zh-CN'>
///     玩家单位
/// </summary>
public partial class Player : Unit
{
	private AnimatedSprite3D _animatedSprite;
	private Label _name;
	private Camera2D _camera;
	public bool BodyDirection = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//_animatedSprite = GetNode<AnimatedSprite3D>("Sprite");
		_name = GetNode<Label>("Name");
		_name.Text = Name;
		if (Name == Global.Session.Username)
		{
			_camera = GetNode<Camera2D>("Camera2D");
			_camera.Current = true;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (Name == Global.Session.Username)
		{
			Vector2 velocity = Velocity;
			if (!IsOnFloor())
			{
				velocity.y -= Gravity * (float)delta;
			}

			if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			{
				velocity.y = JumpVelocity;
			}


			Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

			//if (inputDir.x > 0)
			//{
			//    _animatedSprite.FlipH = false;
			//    BodyDirection = false;
			//}
			//else if (inputDir.x < 0)
			//{
			//    _animatedSprite.FlipH = true;
			//    BodyDirection = true;
			//}
			//else
			//{
			//    _animatedSprite.FlipH = BodyDirection;
			//}

			Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
			if (direction != Vector2.Zero)
			{
				velocity.x = direction.x * MoveSpeed;
			}
			else
			{
				velocity.x = Mathf.MoveToward(Velocity.x, 0, MoveSpeed);
			}

			//if (velocity.x == 0)
			//{
			//    _animatedSprite.Play("Idle");
			//}
			//else
			//{
			//    _animatedSprite.Play("Run");
			//}
			Velocity = velocity;
			MoveAndSlide();
			var basicState = new GlobalScene.BasicState();
			basicState.Pos = Position;
			basicState.Anim = _animatedSprite.Animation;
			basicState.Flip = _animatedSprite.FlipH;
			Global.Socket.SendMatchStateAsync(Global.Match.Id, 1, Newtonsoft.Json.JsonConvert.SerializeObject(basicState));
		}
	}
}