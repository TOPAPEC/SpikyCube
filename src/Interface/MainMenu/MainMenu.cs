using Godot;
using System;
using System.Net.Mime;
using Godot.Collections;
using Array = Godot.Collections.Array;

public class MainMenu : Control, IScene
{
    [Signal]
    public delegate void ChangeScene(String nextScene);

    [Signal]
    public delegate void StartTutorial();

    private TextureButton _startGameButton;
    private TextureButton _openShopButton;
    private TextureButton _openSettingsButton;
    private TextureButton _openLevelSelectionButton;
    private TextureButton _startTutorialButton;


    public override void _Ready()
    {
        _startGameButton = GetNode<TextureButton>("StartGameButton");
        _openShopButton = GetNode<TextureButton>("ShopButton");
        _openSettingsButton = GetNode<TextureButton>("SettingsButton");
        _openLevelSelectionButton = GetNode<TextureButton>("LevelSelectionButton");
        _startTutorialButton = GetNode<TextureButton>("TutorialButton");
        _startGameButton.Connect("pressed", this, "emit_signal", new Array() { "ChangeScene", new Array() { "LevelController" } });
        _openShopButton.Connect("pressed", this, "emit_signal", new Array() { "ChangeScene", new Array() { "Shop" } });
        _openSettingsButton.Connect("pressed", this, "emit_signal", new Array() { "ChangeScene", new Array() { "Settings" } });
        _openLevelSelectionButton.Connect("pressed", this, "emit_signal", new Array() { "ChangeScene", new Array() { "LevelSelection" } });
        _startTutorialButton.Connect("pressed", this, "emit_signal", new Array() { "StartTutorial" });
    }

    public void EnterScene()
    {
    }

    public void ExitScene()
    {
        QueueFree();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}