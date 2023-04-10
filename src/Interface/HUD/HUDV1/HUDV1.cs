using Godot;
using System;
using SpikyCube.SceneController;

public class HUDV1 : CanvasLayer
{
    [Signal]
    public delegate void HudPauseButtonPressed();
    public float ElapsedTime = 0;
    private bool _isTimerStarted;
    private long _timerStartTime;
    private RichTextLabel _elapsedTimeLabel;
    private RichTextLabel _coinsCountLabel;
    private RichTextLabel _keysCountLabel;
    private TextureButton _pauseButton;
    private PlayerStats PlayerStatsLink;
    
    public override void _Ready()
    {
        PlayerStatsLink = GetNode<PlayerStats>("/root/PlayerStats");
        PlayerStatsLink.Connect("CoinsAmountChanged", this, "_handleCoinAmountChange");
        PlayerStatsLink.Connect("KeysAmountChanged", this, "_handleKeysAmountChange");
        _elapsedTimeLabel = GetNode<RichTextLabel>("ElapsedTime");
        _coinsCountLabel = GetNode<RichTextLabel>("CoinsKeysCount/CoinsCounter");
        _keysCountLabel = GetNode<RichTextLabel>("CoinsKeysCount/KeyCounter");
        _pauseButton = GetNode<TextureButton>("PauseButton");
        _pauseButton.Connect("pressed", this, "_emitPauseSignal");
    }

    public void _emitPauseSignal()
    {
        EmitSignal("HudPauseButtonPressed");
    }

    private void _handleCoinAmountChange(int newAmount)
    {
        _coinsCountLabel.Text = $"[center] {newAmount.ToString()} [/center]";
    }

    private void _handleKeysAmountChange(int newAmount)
    {
        _keysCountLabel.Text = $"[center] {newAmount.ToString()} [/center]";
    }

    public void ShowHud()
    {
        _isTimerStarted = true;
        ElapsedTime = 0;
        Visible = true;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (_isTimerStarted)
        {
            ElapsedTime += delta;
            int seconds = (int)ElapsedTime;
            float mseconds = ElapsedTime - seconds;
            int minutes = seconds / 60;
            _elapsedTimeLabel.BbcodeText = $"[center] {minutes:00}:{(seconds % 60):00}:{(int)(mseconds * 100)} [/center]";
        }
    }

    public void HideHud()
    {
        Visible = false;
        _isTimerStarted = false;
    }

}