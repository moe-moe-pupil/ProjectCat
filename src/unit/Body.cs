using Godot;
using System;

public partial class Body : CharacterBody3D
{
	public const float Speed = 4.5f;
	public const float JumpVelocity = 4.5f;
	public bool BodyDirection = false;
    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	private AnimatedSprite2D _animatedSprite;
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("Sprite2d");
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
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

		if (!IsOnFloor())
		{

			velocity.y -= gravity * (float)delta;
        }

		Vector3 direction = (Transform.basis * new Vector3(inputDir.x, inputDir.y, 0)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.x = direction.x * Speed;
			velocity.y = direction.y * Speed;
		}
		else
		{
			velocity.x = Mathf.MoveToward(Velocity.x, 0, Speed);
			velocity.y = Mathf.MoveToward(Velocity.y, 0, Speed);
		}

        if (velocity.x == 0)
		{
			_animatedSprite.Play("Idle");

        }
		else
		{
            _animatedSprite.Play("Run");
        }
		
		

        /*
            if (velocity.x < 0)
        {
			_animatedSprite.Play("Run");
			_animatedSprite.FlipH = true;
        } 
		else
        {
			if(velocity.x != 0)
            {
				_animatedSprite.Play("Run");
			} 
			else
            {
				_animatedSprite.Play("Idle");
            }
			_animatedSprite.FlipH = false;
		}
		*/
        Velocity = velocity;
		MoveAndSlide();
	}
}
