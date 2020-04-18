



using System;
using Godot;




/// <summary>
/// ???
/// </summary>
public class Main : Node2D
{



    #region variables
    // Names
    private const String _startTimerName = "StartTimer";
    private const String _startPositionName = "StartPosition";
    private const String _playerName = "Player";
    private const String _mobTimerName = "MobTimer";
    private const String _scoreTimerName = "ScoreTimer";

    // Spawning
    [Export] public PackedScene mob;            // Enemy we spawn in
    private Random _random = new Random();      // We use 'System.Random' as an alternative to GDScript's random methods.
    private Timer _startTimer;
    private Timer _mobTimer;
    
    // Scoring
    private int _score;                         // How many points the player has acquired
    private Timer _scoreTimer;

    // Game
    private Position2D _startPosition;
    private Player _player;
    #endregion




    #region lifecycles
    /// <summary>
    /// Loads references
    /// </summary>
    public override void _Ready()
    {
        LoadNames();
    }
    #endregion




    #region helpers
    /// <summary>
    /// Finds the relevant objects in the scene
    /// </summary>
    private void LoadNames()
    {
        _startTimer = GetNode<Timer>(_startTimerName);
        _startPosition = GetNode<Position2D>(_startPositionName);
        _player = GetNode<Player>(_playerName);
        _mobTimer = GetNode<Timer>(_mobTimerName);
        _scoreTimer = GetNode<Timer>(_scoreTimerName);
    }

    /// <summary>
    /// Because C# doesn't support GDScript's randi().
    /// </summary>
    private float RandRange(float min, float max)
    {
        return (float)_random.NextDouble() * (max - min) + min;
    }
    #endregion




    #region gameplay
    /// <summary>
    /// Stops the Timers, displays score
    /// </summary>
    public void GameOver()
    {
        _mobTimer.Stop();
        _scoreTimer.Stop();
    }

    /// <summary>
    /// Clears screen, resets player and score
    /// </summary>
    public void NewGame()
    {
        _score = 0;
        _player.Start(_startPosition.Position);
        _startTimer.Start();
    }
    #endregion
}



