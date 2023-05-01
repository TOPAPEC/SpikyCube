using Godot;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using Godot.Collections;
using SpikyCube.SceneController;
using Object = Godot.Object;

public class LevelSelectionV1 : Control, IScene
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Signal]
    public delegate void ChangeLevelTo();

    private GridContainer _levelButtonsGrid;
    private GridContainer _coinsCounterGrid;
    private Object _playerStats;
    private AudioStreamPlayer _mainMenuClickSound;
    private AudioStreamPlayer _blockedSound;
    private TextureButton _backButton;
    private int _chapterNumber = 0;
    
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _playerStats = GetNode<Object>("/root/PlayerStatsExtended");
        _levelButtonsGrid = GetNode<GridContainer>("LevelsGrid");
        _coinsCounterGrid = GetNode<GridContainer>("StarGrid");
        _mainMenuClickSound = GetNode<AudioStreamPlayer>("LevelSelectionClickSound");
        _blockedSound = GetNode<AudioStreamPlayer>("BlockedSound");
        var levelButtons = _levelButtonsGrid.GetChildren();
        for (int i = 0; i < levelButtons.Count; ++i)
        {
            ((TextureButton)levelButtons[i]).Connect("pressed", this, "_levelChosen", new Godot.Collections.Array { i });
        }
        UpdateCoinLabels(); 
    }

    public void UpdateCoinLabels()
    {
        var counters = _coinsCounterGrid.GetChildren();
        for (int i = 0; i < counters.Count; ++i) 
        {
            if (_playerStats.Call("get_level_score", _chapterNumber, i) is int coinsCount)
            {
                ((CoinsCounter)counters[i]).ChangeCoinsCount(coinsCount);
            }
        }
    }

    private void _levelChosen(int levelNumber)
    {
        _playerStats.Call("set_last_level", _chapterNumber, levelNumber, 0);
        EmitSignal("ChangeLevelTo");
    }

    public void EnterScene(Node2D sceneController)
    {
        this.Connect("ChangeLevelTo", sceneController, "ChangeScene", new Godot.Collections.Array() {"LevelController"});
        _backButton = GetNode<TextureButton>("BackButton");
        _backButton.Connect("pressed", sceneController, "ChangeScene", new Godot.Collections.Array() { "MainMenu" });
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
