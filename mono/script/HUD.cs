



using System;
using Godot;




/// <summary>
/// Controls information shown during Entry, Play, and facilitates flow through them
/// </summary>
public class HUD : CanvasLayer
{



    #region variables
    // Starting
    [Signal] public delegate void StartGame();                  // Tell main to start the game
    private Button _startButton;                                // How we start the game
    private const String _startButtonName = "StartButton";      // What we call the startButton

    // Messaging
    private Timer _messageTimer;                                // When to hide the message
    private const String _messageTimerName = "MessageTimer";    // What we call the messageTimer
    private Label _messageLabel;                                // Where we show messages
    private const String _messageLabelName = "Message";         // What we call the messageLabel

    // Scoring
    private Label _scoreLabel;                                  // Where we display the score
    private const String _scoreLabelName = "Score";             // What we call the scoreLabel
    #endregion




    #region lifecycles
    /// <summary>
    /// Connects the components to this script
    /// </summary>
    public override void _Ready()
    {
        _messageLabel = this.GetNode<Label>(_messageLabelName);
        _messageTimer = this.GetNode<Timer>(_messageTimerName);
        _startButton = this.GetNode<Button>(_startButtonName);
        _scoreLabel = this.GetNode<Label>(_scoreLabelName);
        _startButton.Connect("pressed", this, nameof(OnStartButtonPressed));
        _messageTimer.Connect("timeout", this, nameof(OnMessageTimerTimeout));
    }
    #endregion




    #region helpers
    /// <summary>
    /// Temporarily displays a message
    /// </summary>
    public void ShowMessage(string text)
    {
        _messageLabel.Text = text;
        _messageLabel.Show();
        _messageTimer.Start();
    }

    /// <summary>
    /// Stops the score, removes player input
    /// </summary>
    async public void ShowGameOver()
    {
        this.ShowMessage("Game Over");

        await this.ToSignal(_messageTimer, "timeout");

        _messageLabel.Text = "Dodge the\nCreeps!";
        _messageLabel.Show();

        _startButton.Show();
    }

    /// <summary>
    /// Sets the given value as the displayed score
    /// </summary>
    public void UpdateScore(int score)
    {
        _scoreLabel.Text = score.ToString();
    }

    /// <summary>
    /// Let subscribers know the game has begun and remove ui elements
    /// </summary>
    public void OnStartButtonPressed()
    {
        _startButton.Hide();
        this.EmitSignal("StartGame");
    }

    /// <summary>
    /// Time to hide the message
    /// </summary>
    public void OnMessageTimerTimeout()
    {
        _messageLabel.Hide();
    }
    #endregion


}



