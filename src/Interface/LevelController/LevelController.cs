using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using SpikyCube.SceneController;
using Object = Godot.Object;


public class LevelController : Node2D, IScene
{
    [Signal]
    public delegate void ToMainMenu();
    
    private List<List<String>> _levelsList = new List<List<String>>(1);
    private Node2D _currentLevel;
    private int[] _currentLevelNumber;

    private TextureRect _greyScaleShader;
    private MenuV1 _pauseMenu;
    private CanvasLayer _pauseMenuCanvasLayer;

    private float _audioPlaybackPosition;
    private AudioStreamPlayer _gameSong;

    private HUDV1 _hudv1;
    private CanvasLayer _gameLayer;

    private Object _playerStats;
    
    public override void _Ready()
    {
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
        _currentLevel.Scale = new Vector2(xLevelScale, yLevelScale);
        GD.Print(_currentLevel.Transform.Scale);
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
        _pauseMenuCanvasLayer.Visible = true;
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
    
    public void ChangeScene(String newSceneName)
    {
        // _levelId = newSceneName;
        ResumeGame();
        _hudv1.ShowHud();
        if (!(_currentLevel is null))
        {
            ((IScene)_currentLevel).ExitScene();
            _currentLevel.QueueFree();
        }

        // var newScene = _levelsList[newSceneName];
        // var newSceneInstance = newScene.Instance();
        // newSceneInstance.Name = _levelId;
        // ((IScene)newSceneInstance).EnterScene();
        // _currentLevel = newSceneInstance;
        // _hudv1.ShowHud();
        // ((Node2D)_currentLevel).ZIndex = -1;
        // _player = _currentLevel.GetNode<KinematicBody2D>("DummyPlayer");
        // _player.Connect("RestartLevel", this, "ChangeToCurrentLevel");
        // _player.Connect("NextLevel", this, "ChangeToNextLevel");
        // _centerLevel();
        // _gameLayer.AddChild(newSceneInstance);
        // _playerStats.Call("reset_current_state");
        // String sceneName = _levelId;
        // string pattern = "Chapter(.+)Level(.+)";
        // int chapter = 0;
        // int level = 0;
        // foreach (Match match in Regex.Matches(sceneName, pattern, RegexOptions.IgnoreCase)) 
        // {
        //     int.TryParse(match.Groups[1].Value, out chapter);
        //     int.TryParse(match.Groups[2].Value, out level);
        // }
        //
        // if (!_isGameInitializing)
        // {
        //     _playerStats.Call("set_last_level", level);
        // }
    }
    
    public void ChangeToNextLevel()
    {
        // String sceneName = _levelId;
        // string pattern = "Chapter(.+)Level(.+)";
        // int chapter = 0;
        // int level = 0;
        // foreach (Match match in Regex.Matches(sceneName, pattern, RegexOptions.IgnoreCase)) 
        // {
        //     int.TryParse(match.Groups[1].Value, out chapter);
        //     int.TryParse(match.Groups[2].Value, out level);
        // }
        // _playerStats.Call("save_level_progress", chapter, level);
        // String sceneNameNextLevel = String.Format("Chapter{0}Level{1}", chapter, level + 1);
        // String sceneNameNextChapter = String.Format("Chapter{0}Level{1}", chapter + 1, 0);
        // if (_levelsList.LevelPaths.ContainsKey(sceneNameNextLevel)) 
        // {
        //     ChangeScene(sceneNameNextLevel);
        // } 
        // else if (_levelsList.LevelPaths.ContainsKey(sceneNameNextChapter)) 
        // {
        //     ChangeScene(sceneNameNextChapter);
        // } 
        // else 
        // {
        //     ShowLevelSelection();
        // }
    }
    
    public void ChangeToCurrentLevel()
    {
        _audioPlaybackPosition = _gameSong.GetPlaybackPosition() + 0.2f;
        // ChangeScene(_levelId);
    }
    public void EnterScene()
    {
        
    }

    public void ExitScene()
    {
        QueueFree();
    }
    

    private void _fillLevelList()
    {
        int chapterNumber = 1;
        _levelsList.Append(new List<String>(20));
        for (int chapter = 0; chapter < chapterNumber; ++chapter)
        {
            for (int level = 0; level < 20; ++level)
            {
                _levelsList[chapter][level] = $"res://src/Field/Levels/Chapter{chapter}/Level{level}/Level_{chapter}_{level}.tscn";
            }
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
