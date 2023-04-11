using Godot;
using System;
using System.Collections.Generic;
using SpikyCube.SceneController;

public class SceneController : Node2D
{
    public Node CurrentLevel;
    public readonly String CurrentLevelName = "GameLayer/CurrentLevel";
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
    
    public class LevelsDict
    {
        public Dictionary<String, String> LevelPaths = new Dictionary<string, String>()
        {
            {
                "LevelSelection", "res://src/Interface/LevelSelection/LevelSelectionV1/LevelSelectionV1.tscn"
            },
            {
                "Chapter0Level0", "res://src/Field/Levels/Chapter1/Level1/Level.tscn"
            },
            {
                "Chapter0Level1", "res://src/Field/Levels/Chapter1/Level2/Level2.tscn"
            }
            
        };

        public PackedScene this[String levelName]
        {
            get => (PackedScene)ResourceLoader.Load(LevelPaths[levelName]);
        }
    }

    public override void _Ready()
    {
        HUD = GetNode<HUDV1>("GameLayer/HUD");
        CurrentLevel = GetNode<Node>(CurrentLevelName);
        _pauseMenu = GetNode<MenuV1>("UILayer/PauseMenu");
        _UILayer = GetNode<CanvasLayer>("UILayer");
        _gameLayer = GetNode<CanvasLayer>("GameLayer");
        _greyScaleShader = GetNode<ColorRect>("GameLayer/GreyScaleShader");
        _gameAudio = GetNode<AudioStreamPlayer>("GameLayer/GameAudio");
        HUD.ShowHud();
        HUD.Connect("HudPauseButtonPressed", this, "PauseGame");
        _gameAudio.Playing = true;
        _pauseMenu.Connect("ResumeGamePressed", this, "ResumeGame");
        _pauseMenu.Connect("ButtonsHidden", this, "HideUI");
        _pauseMenu.Connect("SelectLevelPressed", this, "ShowLevelSelection");

        _player = CurrentLevel.GetNode<KinematicBody2D>("DummyPlayer");
        _player.Connect("RestartLevel", this, "ChangeToCurrentLevel");
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
         _pauseMenu.HideButtons();
    }

    public void HideUI()
    {
        GD.Print("UI");
        _UILayer.Visible = false;
        GetTree().Paused = false;
        if (!_checkIfSceneIsInterface(_levelId))
        {
            _gameAudio.Play();
            _gameAudio.Seek(_audioPlaybackPosition);
        } 
    }

    private bool _checkIfSceneIsInterface(String sceneName)
    {
        switch (sceneName)
        {
            case "LevelSelection":
                return true;
            default:
                return false;
        }
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
        newSceneInstance.Name = CurrentLevelName;
        ((IScene)newSceneInstance).EnterScene();
        CurrentLevel = (Node)newSceneInstance;
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
        }
        _gameLayer.AddChild(newSceneInstance);
    }

    public void ChangeToCurrentLevel()
    {
        ChangeScene(_levelId);
    }

    public void ShowLevelSelection()
    {
        ChangeScene("LevelSelection");
        CurrentLevel.Connect("ChangeLevelTo", this, "ChangeScene");
    }
}