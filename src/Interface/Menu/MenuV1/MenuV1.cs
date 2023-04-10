using Godot;
using System;

public class MenuV1 : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Signal]
    public delegate void ResumeGamePressed();

    [Signal]
    public delegate void SelectLevelPressed();

    [Signal]
    public delegate void ButtonsHidden();

    private Control _buttonsBlock;
    private TextureButton _resumeGameButton;
    private TextureButton _selectLevelButton;
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
        _pauseButtonsAudio = GetNode<AudioStreamPlayer>("PauseButtonsAudio");
        _buttonsBlock.Visible = false;
        _resumeGameButton.Connect("pressed", this, "_emitResumeGamePressed");
        _selectLevelButton.Connect("pressed", this, "_emitSelectLevelPressed");
        _selectLevelButton.Connect("pressed", _pauseButtonsAudio, "play");
        _resumeGameButton.Connect("pressed", _pauseButtonsAudio, "play");
    }

    private void _emitResumeGamePressed()
    {
        EmitSignal("ResumeGamePressed");
    }

    private void _emitSelectLevelPressed()
    {
        EmitSignal("SelectLevelPressed");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
         switch (_buttonsBlockState)
         {
             case "entering": 
                 _buttonsBlock.MarginLeft = Mathf.Lerp(_buttonsBlock.MarginLeft, _visibleButtonsBlockMarginLeft, 4f * delta);
                 break;
             case "exiting":
                 _buttonsBlock.MarginLeft = Mathf.Lerp(_buttonsBlock.MarginLeft, _hiddenButtonsBlockMarginLeft, 64f * delta);
                 if (Mathf.Abs(_buttonsBlock.MarginLeft - _hiddenButtonsBlockMarginLeft) < 0.01f)
                 {
                     _buttonsBlockState = "idle";
                     _buttonsBlock.Visible = false;
                     Visible = false;
                     _buttonsBlock.SetProcess(false);
                     _buttonsBlock.SetPhysicsProcess(false);
                     EmitSignal("ButtonsHidden");
                     GD.Print("PAUSE HIDDEN");
                 }
                 break;
         }
    }

    public void ShowButtons()
    {
        _buttonsBlock.MarginLeft = _hiddenButtonsBlockMarginLeft;
        _buttonsBlockState = "entering";
        _buttonsBlock.Visible = true;
        Visible = true;
        _buttonsBlock.SetProcess(true);
        _buttonsBlock.SetPhysicsProcess(true);
    }

    public void HideButtons()
    {
        _buttonsBlock.MarginLeft = _visibleButtonsBlockMarginLeft;
        _buttonsBlockState = "exiting";
    }
}
