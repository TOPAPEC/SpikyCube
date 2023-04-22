    using Godot;
using System;
using System.Collections.Generic;
using SpikyCube.SceneController;
using System.Text.RegularExpressions;
    using Object = Godot.Object;

    public class SceneController : Node2D
{
    public Node CurrentLevel;
    public readonly String CurrentLevelName = "GameLayer/Chapter0Level0";
    public static readonly LevelsDict Levels = new LevelsDict();
    public HUDV1 HUD;
    private MenuV1 _pauseMenu;
    private CanvasLayer _gameLayer;
    private CanvasLayer _UILayer;
    private ColorRect _greyScaleShader;
    private AudioStreamPlayer _gameAudio;
    private float _audioPlaybackPosition;
    private String _levelId = "Chapter0Level0";
    private KinematicBody2D _player;
    private Object _playerStats;
    private CanvasLayer _loadingScreen;
    private CanvasLayer _levelSelectionLayer;
    private LevelSelectionV1 _levelSelectionPanel;
    private bool _isInterfaceOnScreen;
    private bool _isGameInitializing = true;
    
    public class LevelsDict
    {
        public Dictionary<String, String> LevelPaths = new Dictionary<string, String>();

        public LevelsDict()
        {
            _fillLevelDict(0, 0, 20);
        }
        
        private void _fillLevelDict(int chapter, int fromLevel, int toLevel)
        {
            for (int i = fromLevel; i < toLevel; ++i)
            {
                String levelName = $"Chapter{chapter}Level{i}";
                LevelPaths[levelName] = $"res://src/Field/Levels/Chapter{chapter}/Level{i}/Level_{chapter}_{i}.tscn";
            }
        }

        public PackedScene this[String levelName]
        {
            get => (PackedScene)ResourceLoader.Load(LevelPaths[levelName]);
        }
    }

    public override void _Ready()
    {
        HUD = GetNode<HUDV1>("GameLayer/HUD");
        _pauseMenu = GetNode<MenuV1>("UILayer/PauseMenu");
        _UILayer = GetNode<CanvasLayer>("UILayer");
        _gameLayer = GetNode<CanvasLayer>("GameLayer");
        _greyScaleShader = GetNode<ColorRect>("GameLayer/GreyScaleShader");
        _gameAudio = GetNode<AudioStreamPlayer>("GameLayer/GameAudio");
        _playerStats = GetNode<Object>("/root/PlayerStatsExtended");
        _loadingScreen = GetNode<CanvasLayer>("LoadingScreen");
        _levelSelectionLayer = GetNode<CanvasLayer>("LevelSelection");
        _levelSelectionPanel = GetNode<LevelSelectionV1>("LevelSelection/LevelSelectionPanel");
        _levelSelectionPanel.Connect("ChangeLevelTo", this, "LevelSelectionChangeScene");
        _playerStats.Connect("load_init_level", this, "LevelSelectionChangeScene");
        try
        {
            _playerStats.Call("init_sdk");
        }
        catch (Exception ex)
        {
            GD.Print($"Failed to load player data. {ex}");
        }
        ChangeScene(_levelId);
        CurrentLevel = GetNode<Node>(CurrentLevelName);
        _player = CurrentLevel.GetNode<KinematicBody2D>("DummyPlayer");
        HUD.ShowHud();
        HUD.Connect("HudPauseButtonPressed", this, "PauseGame");
        _pauseMenu.Connect("RestartGamePressed", this, "ChangeToCurrentLevel");
        _gameAudio.Playing = true;
        _pauseMenu.Connect("ResumeGamePressed", this, "ResumeGame");
        _pauseMenu.Connect("ButtonsHidden", this, "HideUI");
        _pauseMenu.Connect("SelectLevelPressed", this, "ShowLevelSelection");
        

        GetTree().Root.Connect("size_changed", this, "_centerLevel");
    }

    private void _centerLevel()
    {
        if (!(_checkIfSceneIsInterface(_levelId)))
        {
            var levelTileMap = CurrentLevel.GetNode<TileMap>("TileMap");
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
            ((Node2D)CurrentLevel).Scale = new Vector2(xLevelScale, yLevelScale);
            GD.Print(((Node2D)CurrentLevel).Transform.Scale);
            var levelSizeInPx = levelSize * levelTileMap.CellSize * ((Node2D)CurrentLevel).Transform.Scale;
            var freeScreenSize = currentResolution - levelSizeInPx;
            var levelOffsetFractions = new Vector2(0.5f, 0f);
            ((Node2D)CurrentLevel).Position = new Vector2(freeScreenSize * levelOffsetFractions) + new Vector2(0, topYMargin);           
        }

    }

    public void PauseGame()
    {
        _audioPlaybackPosition = _gameAudio.GetPlaybackPosition();
        _gameAudio.Stop();
        _greyScaleShader.Visible = true;
        _pauseMenu.ShowButtons();
        _UILayer.Visible = true;
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
        if (!_checkIfSceneIsInterface(_levelId))
        {
            _gameAudio.Play();
            _gameAudio.Seek(_audioPlaybackPosition);
        } 
    }

    private bool _checkIfSceneIsInterface(String sceneName)
    {
        return _isInterfaceOnScreen;
    }

    public void ShowLevelSelection()
    {
        _isInterfaceOnScreen = true;
        _levelSelectionPanel.UpdateCoinLabels();
        HUD.Hide();
        ResumeGame();
        _levelSelectionLayer.Show();
    }

    public void LevelSelectionChangeScene(String newScene)
    {
        _isGameInitializing = false;
        _loadingScreen.Show();
        _isInterfaceOnScreen = false;
        _levelSelectionLayer.Hide();
        ChangeScene(newScene);
        _loadingScreen.Hide();
    }

    public void ChangeScene(String newSceneName)
    {
        _levelId = newSceneName;
        ResumeGame();
        HUD.ShowHud();
        if (!(CurrentLevel is null))
        {
            ((IScene)CurrentLevel).ExitScene();
            CurrentLevel.QueueFree();
        }

        var newScene = Levels[newSceneName];
        var newSceneInstance = newScene.Instance();
        newSceneInstance.Name = _levelId;
        ((IScene)newSceneInstance).EnterScene();
        CurrentLevel = newSceneInstance;
        if (_checkIfSceneIsInterface(newSceneName))
        {
            HUD.HideHud();
        }
        else
        {
            HUD.ShowHud();
            ((Node2D)CurrentLevel).ZIndex = -1;
            _player = CurrentLevel.GetNode<KinematicBody2D>("DummyPlayer");
            _player.Connect("RestartLevel", this, "ChangeToCurrentLevel");
            _player.Connect("NextLevel", this, "ChangeToNextLevel");
            _centerLevel();
        }
        _gameLayer.AddChild(newSceneInstance);
        _playerStats.Call("reset_current_state");
        String sceneName = _levelId;
        string pattern = "Chapter(.+)Level(.+)";
        int chapter = 0;
        int level = 0;
        foreach (Match match in Regex.Matches(sceneName, pattern, RegexOptions.IgnoreCase)) 
        {
            int.TryParse(match.Groups[1].Value, out chapter);
            int.TryParse(match.Groups[2].Value, out level);
        }

        if (!_isGameInitializing)
        {
            _playerStats.Call("set_last_level", level);
        }
    }

    public void ChangeToCurrentLevel()
    {
        _audioPlaybackPosition = _gameAudio.GetPlaybackPosition() + 0.2f;
        ChangeScene(_levelId);
    }
    
    public void ChangeToNextLevel()
    {
        String sceneName = _levelId;
        string pattern = "Chapter(.+)Level(.+)";
        int chapter = 0;
        int level = 0;
        foreach (Match match in Regex.Matches(sceneName, pattern, RegexOptions.IgnoreCase)) 
        {
            int.TryParse(match.Groups[1].Value, out chapter);
            int.TryParse(match.Groups[2].Value, out level);
        }
        _playerStats.Call("save_level_progress", chapter, level);
        String sceneNameNextLevel = String.Format("Chapter{0}Level{1}", chapter, level + 1);
        String sceneNameNextChapter = String.Format("Chapter{0}Level{1}", chapter + 1, 0);
        if (Levels.LevelPaths.ContainsKey(sceneNameNextLevel)) 
        {
            ChangeScene(sceneNameNextLevel);
        } 
        else if (Levels.LevelPaths.ContainsKey(sceneNameNextChapter)) 
        {
            ChangeScene(sceneNameNextChapter);
        } 
        else 
        {
            ShowLevelSelection();
        }
    }
}
