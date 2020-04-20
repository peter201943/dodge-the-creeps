



using System;
using Godot;




/// <summary>
/// A Simple Player Script for Dodging Projectiles in 2D
/// </summary>
public class Player : Area2D
{



	#region variables
	// Death
	[Signal] public delegate void Hit();					// Tell Enemies, Game, player died

	// Movement
    [Export] public int speed = 400;						// How fast the player will move (pixels/sec).
    private Vector2 _screenSize; 							// Size of the game window.
	private Vector2 _velocity;								// How we move plavers
	private Vector2 _moveTarget;							// Where we move towards
	
	// Sprite
	private const String _spriteName = "Sprite";			// What we call the sprite
	private AnimatedSprite _sprite;							// What gets rendered to screen
	
	// Hitbox
	private const String _hitBoxName = "Hitbox";			// What we call the hitbox
	private CollisionShape2D _hitBox;						// What collides with enemies
	#endregion




	#region lifecycles
	/// <summary>
	/// Prevent locked aspect ratio, async res loading
	/// </summary>
	public override void _Ready()
	{
		SetScreenSize();
		SetSprite();
		SetHitBox();
		Hide();
		_moveTarget = this.Position;
	}

	public override void _Process(float delta)
	{
		ReadInput(delta);
	}

	/// <summary>
	/// Change the target whenever a touch event happens
	/// </summary>
	public override void _Input(InputEvent @event)
	{
        if (@event is InputEventScreenTouch eventMouseButton && eventMouseButton.Pressed)
        {
            _moveTarget = (@event as InputEventScreenTouch).Position;
        }
	}
	#endregion




	#region ready helpers
	/// <summary>
	/// Sets the `screen_size` variable
	/// </summary>
	private void SetScreenSize()
	{
		_screenSize = GetViewport().Size;
	}

	/// <summary>
	/// Sets the player's sprite and configures it
	/// </summary>
	private void SetSprite()
	{
		_sprite = GetNode<AnimatedSprite>(_spriteName);
	}

	/// <summary>
	/// Sets the player's collider and configures it
	/// </summary>
	private void SetHitBox()
	{
		_hitBox = this.GetNode<CollisionShape2D>(_hitBoxName);
		_hitBox.SetDeferred("disabled", true);
	}
	#endregion




	#region input helpers
	/// <summary>
	/// Decodes the player keypresses and trigger the corresponding events
	/// </summary>
	private void ReadInput(float delta)
	{
		// Reset the player's movement vector.
		_velocity = new Vector2();
		// Check each input by mode
		_velocity = ReadKeyboard(_velocity);
		_velocity = ReadCursor(_velocity);
		// Apply polishing effects
		_velocity = ProduceVelocity(_velocity);
		ApplyAnimation(_velocity);
		ApplyMovement(_velocity, delta);
	}

	/// <summary>
	/// Check each input option for keys
	/// </summary>
	private Vector2 ReadKeyboard(Vector2 _velocity)
	{
		_velocity = InputUp(_velocity);
		_velocity = InputDown(_velocity);
		_velocity = InputLeft(_velocity);
		_velocity = InputRight(_velocity);
		return _velocity;
	}

	/// <summary>
	/// Check the input event for cursor
	/// </summary>
	private Vector2 ReadCursor(Vector2 _velocity)
	{
        if ((this.Position.DistanceTo(_moveTarget) > 10) && (_velocity == new Vector2(0,0)))
        {
            _velocity = (_moveTarget - this.Position).Normalized() * speed;
        }
        else
        {
            _moveTarget = this.Position;
        }
		return _velocity;
	}
	#endregion




	#region input events
	/// <summary>
	/// Player wants to move up _check_ and _effect_
	/// </summary>
	private Vector2 InputUp(Vector2 _velocity)
	{
		if (Input.IsActionPressed("ui_up"))
		{
			_velocity.y -= 1;
		}
		return _velocity;
	}

	/// <summary>
	/// Player wants to move down _check_ and _effect_
	/// </summary>
	private Vector2 InputDown(Vector2 _velocity)
	{
		if (Input.IsActionPressed("ui_down"))
		{
			_velocity.y += 1;
		}
		return _velocity;
	}

	/// <summary>
	/// Player wants to move left _check_ and _effect_
	/// </summary>
	private Vector2 InputLeft(Vector2 _velocity)
	{
		if (Input.IsActionPressed("ui_left"))
		{
			_velocity.x -= 1;
		}
		return _velocity;
	}

	/// <summary>
	/// Player wants to move right _check_ and _effect_
	/// </summary>
	private Vector2 InputRight(Vector2 _velocity)
	{
		if (Input.IsActionPressed("ui_right"))
		{
			_velocity.x += 1;
		}
		return _velocity;
	}
	#endregion




	#region output helpers
	/// <summary>
	/// Apply Speed and Normalization
	/// </summary>
	private Vector2 ProduceVelocity(Vector2 _velocity)
	{
		_velocity = _velocity.Normalized() * speed;
		return _velocity;
	}

	/// <summmary>
	/// Apply Animation Updates
	/// </summary>
	private void ApplyAnimation(Vector2 _velocity)
	{
		DoAnimate(_velocity);
		ChooseAnimation(_velocity);
	}

	/// <summary>
	/// Apply Position Updates
	/// </summary>
	private void ApplyMovement(Vector2 _velocity, float delta)
	{
		this.Position += _velocity * delta;
		this.Position = new Vector2(
			x: Mathf.Clamp(this.Position.x, 0, _screenSize.x),
			y: Mathf.Clamp(this.Position.y, 0, _screenSize.y)
		);
	}
	#endregion




	#region animation helpers
	/// <summary>
	/// Do not animate when idle (stay still)
	/// </summary>
	private void DoAnimate(Vector2 _velocity)
	{
		if (_velocity.Length() > 0)
		{
			_sprite.Play();
		}
		else
		{
			_sprite.Stop();
		}
	}

	/// <summary>
	/// Animate by flipping (horizontal animation overrides vertical animation)
	/// </summary>
	private void ChooseAnimation(Vector2 _velocity)
	{
		if (_velocity.x != 0)
		{
			_sprite.Animation = "right";
			_sprite.FlipV = false;
			_sprite.FlipH = _velocity.x < 0;
		}
		else if (_velocity.y != 0)
		{
			_sprite.Animation = "up";
			_sprite.FlipV = _velocity.y > 0;
		}
	}
	#endregion




	#region gameplay helpers
	/// <summary>
	/// Re-Initializes the player at the given position
	/// </summary>
	public void Start(Vector2 pos)
	{
		this.Position = pos;
        _moveTarget = pos;
        this.Show();
        _hitBox.Disabled = false;
	}

	/// <summary>
	/// Whenever the player collides with _anything_, the game is over
	/// </summary>
    public void OnPlayerBodyEntered(PhysicsBody2D body)
    {
        this.Hide();
        this.EmitSignal("Hit");
        _hitBox.SetDeferred("disabled", true);
    }
	#endregion
}
