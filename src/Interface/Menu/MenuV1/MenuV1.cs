using Godot;
using System;

public class MenuV1 : Control, IScene
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Signal]
    public delegate void ResumeGamePressed();

    [Signal]
    public delegate void RestartGamePressed();

    [Signal]
    public delegate void SelectLevelPressed();

    [Signal]
    public delegate void ButtonsHidden();

    private Control _buttonsBlock;
    private TextureButton _resumeGameButton;
    private TextureButton _selectLevelButton;
    private TextureButton _restartButton;
    private AudioStreamPlayer _pauseButtonsAudio;

    private int _hiddenButtonsBlockMarginLeft = 700;

    private int _visibleButtonsBlockMarginLeft = 0;

    private String _buttonsBlockState;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _buttonsBlock = GetNode<Control>("ButtonsBlock");
        _resumeGameButton = GetNode<TextureButton>("ButtonsBlock/ResumeGameButton");
        _selectLevelButton = GetNode<TextureButton>("ButtonsBlock/SelectLevelButton");
        _restartButton = GetNode<TextureButton>("ButtonsBlock/RestartLevelButton");
        _pauseButtonsAudio = GetNode<AudioStreamPlayer>("PauseButtonsAudio");
        _resumeGameButton.Connect("pressed", this, "_emitResumeGamePressed");
        _selectLevelButton.Connect("pressed", this, "_emitSelectLevelPressed");
        _selectLevelButton.Connect("pressed", _pauseButtonsAudio, "play");
        _resumeGameButton.Connect("pressed", _pauseButtonsAudio, "play");
        _restartButton.Connect("pressed", this, "_emitRestartGamePressed");
    }


    private void _emitRestartGamePressed()
    {
        EmitSignal("RestartGamePressed");
        HideButtons();
    }
    
    private void _emitResumeGamePressed()
    {
        EmitSignal("ResumeGamePressed");
        HideButtons();
    }

    private void _emitSelectLevelPressed()
    {
        EmitSignal("SelectLevelPressed");
        HideButtons();
    }

    public void ShowButtons()
    {
        Visible = true;
    }

    public void HideButtons()
    {
        Visible = false;
    }

    public void EnterScene(Node2D sceneController)
    {
        _resumeGameButton.Connect("pressed", sceneController, "ResumeGame");
        _restartButton.Connect("pressed", sceneController, "RestartLevel");
        _selectLevelButton.Connect("pressed", sceneController, "ToMainMenu");
    }

    public void ExitScene()
    {
        QueueFree();
    }
}
