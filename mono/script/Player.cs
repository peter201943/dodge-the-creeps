



using Godot;
using System;




/// <summary>
/// A Simple Player Script for Dodging Projectiles in 2D
/// </summary>
public class Player : Area2D
{



	#region variables
	// Death
	[Signal]
	public delegate void Hit();								// Tell Enemies, Game, player died
	// Movement
    [Export]
	public int speed = 400; 								// How fast the player will move (pixels/sec).
    private Vector2 _screenSize; 							// Size of the game window.
	private Position _moveTarget;							// Where we move towards
	// Sprite
	private const String _spriteName = "Sprite";			// What we call the sprite
	private AnimatedSprite _sprite;							// What gets rendered to screen
	// Hitbox
	private const String _hitBoxName = "Hitbox";			// What we call the hitbox
	private CollisionShape2d _hitBox;						// What collides with enemies
	#endregion



	#region lifecycles
	/// <summary>
	/// Prevent locked aspect ratio, async res loading
	/// </summary>
	public override void _Ready()
	{
		// Console.WriteLine("HELLO");
		SetScreenSize();
		SetSprite();
		SetHitBox();
		// Hide();
		// _moveTarget = self.Position;
	}

	public override void _Process(float delta)
	{
		ReadInput(delta);
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
		_hitBox = self.GetNode<CollisionShape2d>(_hitBoxName);
		_hitBox.SetDeferred("disabled", true);
	}
	#endregion




	// INPUT HELPERS
	/// <summary>
	/// Decodes the player keypresses and trigger the corresponding events
	/// </summary>
	private void ReadInput(float delta)
	{
		// Get Input Direction
		var velocity = new Vector2(); // The player's movement vector.

		if (Input.IsActionPressed("ui_right"))
		{
			velocity.x += 1;
		}

		if (Input.IsActionPressed("ui_left"))
		{
			velocity.x -= 1;
		}

		if (Input.IsActionPressed("ui_down"))
		{
			velocity.y += 1;
		}

		if (Input.IsActionPressed("ui_up"))
		{
			velocity.y -= 1;
		}

				// Activate the Sprite
		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * speed;
			_sprite.Play();
		}
		else
		{
			_sprite.Stop();
		}


		// Move the Character
		Position += velocity * delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.x, 0, _screenSize.x),
			y: Mathf.Clamp(Position.y, 0, _screenSize.y)
		);


		// Choose the Animation
		if (velocity.x != 0)
		{
			_sprite.Animation = "right";
			_sprite.FlipV = false;
			_sprite.FlipH = velocity.x < 0;
		}
		else if (velocity.y != 0)
		{
			_sprite.Animation = "up";
			_sprite.FlipV = velocity.y > 0;
		}
	}




	// INPUT EVENTS




	// OUTPUT HELPERS




	// ANIMATION HELPERS




	// GAMEPLAY HELPERS




}
