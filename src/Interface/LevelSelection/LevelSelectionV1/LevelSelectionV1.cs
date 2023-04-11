using Godot;
using System;
using System.Collections.Generic;
using SpikyCube.SceneController;

public class LevelSelectionV1 : Control, IScene
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Signal]
    public delegate void ChangeLevelTo(String levelName);

    private GridContainer _levelButtonsGrid;
    private GridContainer _coinsCounterGrid;
    private TextureButton _nextChapter;
    private TextureButton _previousChapter;
    private PlayerStats _playerStats;
    private AudioStreamPlayer _mainMenuClickSound;
    private AudioStreamPlayer _blockedSound;
    private int _chapterNumber = 0;
    
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _playerStats = GetNode<PlayerStats>("/root/PlayerStats");
        _levelButtonsGrid = GetNode<GridContainer>("LevelsGrid");
        _coinsCounterGrid = GetNode<GridContainer>("StarGrid");
        _mainMenuClickSound = GetNode<AudioStreamPlayer>("LevelSelectionClickSound");
        _blockedSound = GetNode<AudioStreamPlayer>("BlockedSound");
        _nextChapter = GetNode<TextureButton>("GridContainer/PreviousChapterButton");
        _previousChapter = GetNode<TextureButton>("GridContainer/NextChapterButton");
 
        var levelButtons = _levelButtonsGrid.GetChildren();
        for (int i = 0; i < levelButtons.Count; ++i)
        {
            GD.Print(i);
            ((TextureButton)levelButtons[i]).Connect("pressed", this, "_levelChosen", new Godot.Collections.Array { i });
        }
        
        var counters = _coinsCounterGrid.GetChildren();
        for (int i = 0; i < counters.Count; ++i) 
        {
            ((CoinsCounter)counters[i]).ChangeCoinsCount(_playerStats.LevelScores[_chapterNumber][i]);
        }

        _nextChapter.Connect("pressed", _blockedSound, "play");
        _previousChapter.Connect("pressed", _blockedSound, "play");
    }

    private void _levelChosen(int levelNumber)
    {
        _mainMenuClickSound.Play();
        System.Threading.Thread.Sleep(500);
        EmitSignal("ChangeLevelTo", $"Chapter{_chapterNumber}Level{levelNumber}");
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
