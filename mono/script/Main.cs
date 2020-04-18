



using System;
using Godot;




/// <summary>
/// Controls game flow, presentation of player and enemies
/// </summary>
public class Main : Node2D
{



    #region variables
    // Names
    private const String _startTimerName = "StartTimer";            // What we call startTimer
    private const String _startPositionName = "StartPosition";      // What we call startPosition
    private const String _playerName = "Player";                    // What we call player
    private const String _mobTimerName = "MobTimer";                // What we call mobTimer
    private const String _scoreTimerName = "ScoreTimer";            // What we call scoreTimer
    private const String _mobPathName = "MobPath";                  // What we call mobPath
    private const String _mobSpawnLocationName = "SpawnLocation";   // What we call spawnLocation

    // Spawning
    [Export] public PackedScene mob;                                // Enemy we spawn in
    private Random _random = new Random();                          // We use 'System.Random' as an alternative to GDScript's random methods.
    private Timer _startTimer;                                      // How long until we can spawn enemies
    private Timer _mobTimer;                                        // pause between spawning enemies
    private Path2D _mobPath;                                        // Where we are allowed to spawn monsters
    private PathFollow2D _mobSpawnLocation;                         // Where we will spawn monsters
    
    // Scoring
    private int _score;                                             // How many points the player has acquired
    private Timer _scoreTimer;                                      // How we track score over time

    // Game
    private Position2D _startPosition;                              // Where player should spawn at
    private Player _player;                                         // The player
    #endregion




    #region lifecycles
    /// <summary>
    /// Loads references and signals
    /// </summary>
    public override void _Ready()
    {
        LoadNames();
        ConnectSignals();
    }
    #endregion




    #region helpers
    /// <summary>
    /// Finds the relevant objects in the scene
    /// </summary>
    private void LoadNames()
    {
        _startTimer = this.GetNode<Timer>(_startTimerName);
        _startPosition = this.GetNode<Position2D>(_startPositionName);
        _player = this.GetNode<Player>(_playerName);
        _mobTimer = this.GetNode<Timer>(_mobTimerName);
        _scoreTimer = this.GetNode<Timer>(_scoreTimerName);
        _mobPath = this.GetNode<Path2D>(_mobPathName);
        _mobSpawnLocation = _mobPath.GetNode<PathFollow2D>(_mobSpawnLocationName);
    }

    /// <summary>
    /// Connects the timers to their functions
    /// </summary>
    private void ConnectSignals()
    {
        _player.Connect("Hit", this, nameof(GameOver));
        _startTimer.Connect("timeout", this, nameof(OnStartTimerTimeout));
        _mobTimer.Connect("timeout", this, nameof(OnMobTimerTimeout));
        _scoreTimer.Connect("timeout", this, nameof(OnScoreTimerTimeout));
    }

    /// <summary>
    /// Because C# doesn't support GDScript's randi().
    /// </summary>
    private float RandRange(float min, float max)
    {
        return (float) _random.NextDouble() * (max - min) + min;
    }
    #endregion




    #region spawning
    /// <summary>
    /// Allow mobs to begin spawning
    /// </summary>
    public void OnStartTimerTimeout()
    {
        _mobTimer.Start();
        _scoreTimer.Start();
    }

    /// <summary>
    /// Spawns a mob with interesting variation
    /// </summary>
    public void OnMobTimerTimeout()
    {
        // Choose a random angle
        _mobSpawnLocation.Offset = _random.Next();

        // Create a Mob instance and add it to the scene.
        var mobInstance = (RigidBody2D) mob.Instance();
        this.AddChild(mobInstance);

        // Set the mob's direction perpendicular to the path direction.
        float direction = _mobSpawnLocation.Rotation + Mathf.Pi / 2;

        // Set the mob's position to a random location.
        mobInstance.Position = _mobSpawnLocation.Position;

        // Add some randomness to the direction.
        direction += RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        mobInstance.Rotation = direction;

        // Choose the velocity.
        mobInstance.LinearVelocity = new Vector2(RandRange(150f, 250f), 0).Rotated(direction);
    }
    #endregion




    #region gameplay
    /// <summary>
    /// Player has earned a point for living
    /// </summary>
    public void OnScoreTimerTimeout()
    {
        _score++;
    }

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



