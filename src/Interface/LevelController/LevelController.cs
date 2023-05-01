using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;
using SpikyCube.SceneController;
using Object = Godot.Object;


public class LevelController : Node2D, IScene
{
    [Signal]
    public delegate void ChangeScene(String nextScene);
    
    private List<List<String>> _levelsList = new List<List<String>>(1);
    private Node2D _currentLevel;
    private int[] _currentLevelNumber;

    private ColorRect _greyScaleShader;
    private MenuV1 _pauseMenu;

    private float _audioPlaybackPosition;
    private AudioStreamPlayer _gameSong;

    private HUDV1 _hudv1;
    private CanvasLayer _gameLayer;

    private Object _playerStats;
    
    public override void _Ready()
    {
        _gameLayer = GetNode<CanvasLayer>("GameLayer");
        _hudv1 = GetNode<HUDV1>("GameLayer/HUD");
        _pauseMenu = GetNode<MenuV1>("UILayer/PauseMenu");
        _gameSong = GetNode<AudioStreamPlayer>("GameLayer/GameAudio");
        _greyScaleShader = GetNode<ColorRect>("GameLayer/GreyScaleShader");
        _playerStats = GetNode<Object>("/root/PlayerStatsExtended");
        _hudv1.EnterScene(this);
        _pauseMenu.EnterScene(this);
        _fillLevelList();
        GetTree().Root.Connect("size_changed", this, "_centerLevel");
    }

    private void _centerLevel()
    {
        var levelTileMap = _currentLevel.GetNode<TileMap>("TileMap");
        var currentResolution = GetViewportRect().End - GetViewportRect().Position;
        var maxYCellsCount = 12;
        var bottomYMargin = 25;
        var topYMargin = 150;
        var xScreenResolution = Math.Min(720, currentResolution.x);
        var sideMargin = 10;
        var maxLevelSize = levelTileMap.CellSize.y * maxYCellsCount;
        var tileMapRect = levelTileMap.GetUsedRect();
        var yLevelScale = (currentResolution.y - topYMargin - bottomYMargin) / maxLevelSize;
        var xLevelScale = (xScreenResolution - sideMargin * 2) / maxLevelSize;
        var levelSize = tileMapRect.End - tileMapRect.Position;
        if (xLevelScale * levelSize.y > currentResolution.y - topYMargin - bottomYMargin)
        {
            xLevelScale = yLevelScale;
        }
        else
        {
            yLevelScale = Math.Min(xLevelScale, yLevelScale);
            xLevelScale = yLevelScale;
        }
        // _currentLevel.Scale = new Vector2(xLevelScale, yLevelScale);
        var levelSizeInPx = levelSize * levelTileMap.CellSize * _currentLevel.Transform.Scale;
        var freeScreenSize = currentResolution - levelSizeInPx;
        var levelOffsetFractions = new Vector2(0.5f, 0f);
        _currentLevel.Position = new Vector2(freeScreenSize * levelOffsetFractions) + new Vector2(0, topYMargin);           
    }

    public void PauseGame()
    {
        _audioPlaybackPosition = _gameSong.GetPlaybackPosition();
        _gameSong.Stop();
        _greyScaleShader.Visible = true;
        _pauseMenu.ShowButtons();
        GetTree().Paused = true;
    }

    public void ResumeGame()
    {
         _greyScaleShader.Visible = false;
         HideUI();
    }

    public void HideUI()
    {
        GetTree().Paused = false;
        _gameSong.Play();
        _gameSong.Seek(_audioPlaybackPosition);
    }
    
    public void ChangeLevel(int[] nextLevel)
    {
        if (_currentLevel is IScene)
        {
            ((IScene)_currentLevel).ExitScene();
        }
        _currentLevelNumber = nextLevel;
        var nextLevelPath = _levelsList[nextLevel[0]][nextLevel[1]];
        var nextLevelInstance = ResourceLoader.Load<PackedScene>(nextLevelPath).Instance();
        _currentLevel = (Node2D)nextLevelInstance;
        _currentLevel.ZIndex = -1;
        _gameLayer.AddChild(nextLevelInstance);
        ((IScene)nextLevelInstance).EnterScene(this);
        ResumeGame();
        _hudv1.ShowHud();
        _centerLevel();
    }

    public void ToMainMenu()
    {
        EmitSignal("ChangeScene", "MainMenu");
    }
    
    public void ChangeToNextLevel()
    {
        if (_currentLevelNumber[1] + 1 == _levelsList[_currentLevelNumber[0]].Count)
        {
            if (_currentLevelNumber[0] + 1 == _levelsList.Count)
            {
                ToMainMenu();
            }
            else
            {
                ++_currentLevelNumber[0];
                _currentLevelNumber[1] = 0;
            }
        }
        else
        {
            ++_currentLevelNumber[1];
        }
        _playerStats.Call("set_last_level", _currentLevelNumber[0], _currentLevelNumber[1], _currentLevelNumber[2]);
        RestartLevel();
    }
    
    public void RestartLevel()
    {
        _playerStats.Call("reset_current_state");
        _audioPlaybackPosition = _gameSong.GetPlaybackPosition() + 0.2f;
        ChangeLevel(_currentLevelNumber);
    }
    
    public void EnterScene(Node2D sceneController)
    {
        if (_playerStats.Call("get_last_level") is Godot.Collections.Array lastLevel)
        {
            ChangeLevel(new int[] {
                (int)lastLevel[0], (int)lastLevel[1], (int)lastLevel[2]
            });
        }
        Connect("ChangeScene", sceneController, "ChangeScene");
    }


    public void ExitScene()
    {
        ResumeGame();
        QueueFree();
    }
    

    private void _fillLevelList()
    {
        int chapterNumber = 1;
        _levelsList.Add(new List<String>(5));
        for (int chapter = 0; chapter < chapterNumber; ++chapter)
        {
            for (int level = 0; level < _levelsList[chapter].Capacity; ++level)
            {
                _levelsList[chapter].Add($"res://src/Field/Levels/Chapter{chapter}/Level{level}/Level_{chapter}_{level}.tscn");
            }
        }
    }
}
