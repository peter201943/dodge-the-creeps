



using System;
using Godot;




/// <summary>
/// ???
/// </summary>
public class HUD : CanvasLayer
{



    #region variables
    // ???
    [Signal] public delegate void StartGame();                  // Tell main to start the game
    private Timer _messageTimer;                                // When to hide the message
    private const String _messageTimerName = "MessageTimer";    // What we call the messageTimer
    private Label _messageLabel;                                // Where we show messages
    private const String _messageLabelName = "Message";         // What we call the messageLabel
    private Button _startButton;                                // How we start the game
    private const String _startButtonName = "StartButton";      // What we call the startButton
    #endregion



    #region lifecycles
    /// <summary>
    /// Connects the label and timer
    /// </summary>
    public override void _Ready()
    {
        _messageLabel = GetNode<Label>(_messageLabelName);
        _messageTimer = GetNode<Timer>(_messageTimerName);
        _startButton = GetNode<Button>(_startButtonName);
    }
    #endregion


    #region helpers
    /// <summary>
    /// ???
    /// </summary>
    public void ShowMessage(string text)
    {
        _messageLabel.Text = text;
        _messageLabel.Show();
        _messageTimer.Start();
    }
    #endregion



    #region gameplay
    /// <summary>
    /// ???
    /// </summary>
    async public void ShowGameOver()
    {
        this.ShowMessage("Game Over");

        await ToSignal(_messageTimer, "timeout");

        _messageLabel.Text = "Dodge the\nCreeps!";
        _messageLabel.Show();

        _startButton.Show();
    }
    #endregion


}



