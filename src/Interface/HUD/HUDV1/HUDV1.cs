using Godot;
using System;
using SpikyCube.SceneController;
using Object = Godot.Object;

public class HUDV1 : Control, IScene
{
    [Signal]
    public delegate void HudPauseButtonPressed();
    public float ElapsedTime = 0;
    private bool _isTimerStarted;
    private long _timerStartTime;
    private RichTextLabel _elapsedTimeLabel;
    private TextureRect _coinsCounter;
    private TextureRect _keysCounter;
    private TextureButton _pauseButton;
    private Object _playerStats;
    
    public override void _Ready()
    {        
        _playerStats = GetNode<Object>("/root/PlayerStatsExtended");
        _playerStats.Connect("coins_amount_changed", this, "_handleCoinAmountChange");
        _playerStats.Connect("keys_amount_changed", this, "_handleKeysAmountChange");
        _playerStats.Connect("end_level", this, "StopTime");
        _elapsedTimeLabel = GetNode<RichTextLabel>("ElapsedTime");
        _coinsCounter = GetNode<TextureRect>("CoinsCounter");
        _keysCounter = GetNode<TextureRect>("KeyCounter");
        _pauseButton = GetNode<TextureButton>("PauseButton");
    }

    private void _handleCoinAmountChange(int newAmount)
    {
        newAmount = Math.Max(0, newAmount);
        newAmount = Math.Min(3, newAmount);
        _coinsCounter.Texture = ResourceLoader.Load<Texture>($"res://resources/GUIItems/GoldCoinCount{newAmount.ToString()}_128.png");
    }

    private void _handleKeysAmountChange(int newAmount)
    {
        newAmount = Math.Max(0, newAmount);
        newAmount = Math.Min(1, newAmount);
        _keysCounter.Texture = ResourceLoader.Load<Texture>($"res://resources/GUIItems/KeyCount{newAmount.ToString()}.png");
    }

    public void ShowHud()
    {
        _isTimerStarted = true;
        ElapsedTime = 0;
        Visible = true;
    }

    public void StopTime()
    {
        GD.Print("STOP TIME");
        _isTimerStarted = false;
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

    public void ExitScene()
    {
        QueueFree();
    }

    public void EnterScene(Node2D sceneController)
    {
        _pauseButton.Connect("pressed", sceneController, "PauseGame");
    }
}
