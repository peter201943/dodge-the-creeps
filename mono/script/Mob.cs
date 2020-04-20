



using System;
using Godot;




/// <summary>
/// A mostly empty projectile enemy with multiple sprite variants
/// </summary>
public class Mob : RigidBody2D
{



    #region variables
    // Sprite
    private const String _spriteName = "Sprite";                // How we find the Sprite
    private AnimatedSprite _sprite;                             // What we animate
    private String[] _mobTypes = { "walk", "swim", "fly" };     // Which sprite to render; Changes animation, speed

    // Movement
    [Export] public int minSpeed = 150;                         // Minimum speed range.
    [Export] public int maxSpeed = 250;                         // Maximum speed range.

    // Randomness
    static private Random _random = new Random();               // C# doesn't implement GDScript's random methods
    #endregion




    #region lifecycles
    /// <summary>
    /// Set Sprite and Animate
    /// </summary>
    public override void _Ready()
    {
        _sprite = this.GetNode<AnimatedSprite>(_spriteName);
        _sprite.Animation = _mobTypes[_random.Next(0, _mobTypes.Length)];
    }

    /// <summary>
    /// Destroy this mob when off screen
    /// </summary>
    public void OnVisibilityScreenExited()
    {
        this.QueueFree();
    }
    #endregion



    #region helpers
    /// <summary>
    /// Destroy this mob on new game
    /// </summary>
    public void OnStartGame()
    {
        this.QueueFree();
    }
    #endregion




}



