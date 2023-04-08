using Godot;
using System;
using System.Collections.Generic;
using SpikyCube.SceneController;

public class SceneController : Node2D
{
    public Node2D CurrentLevel;
    public readonly String CurrentLevelName = "GameLayer/CurrentLevel";
    public static readonly LevelsDict Levels = new LevelsDict();
    public HUDV1 HUD;
    private MenuV1 _pauseMenu;
    private CanvasLayer _gameLayer;
    private CanvasLayer _UILayer;
    private ColorRect _greyScaleShader;
    
    public class LevelsDict
    {
        public Dictionary<String, String> LevelPaths = new Dictionary<string, String>()
        {
            {
                "LevelSelection", "res://src/Interface/LevelSelection/LevelSelectionV1/LevelSelectionV1.tscn"
            },
            {
                "Chapter1Level1", "res://src/Field/FixedField/FixedField.tscn"
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
        CurrentLevel = GetNode<Node2D>(CurrentLevelName);
        _pauseMenu = GetNode<MenuV1>("UILayer/PauseMenu");
        _UILayer = GetNode<CanvasLayer>("UILayer");
        _gameLayer = GetNode<CanvasLayer>("GameLayer");
        _greyScaleShader = GetNode<ColorRect>("GameLayer/GreyScaleShader");
        HUD.ShowHud();
        HUD.Connect("HudPauseButtonPressed", this, "PauseGame");
        _pauseMenu.Connect("ResumeGamePressed", this, "ResumeGame");
        _pauseMenu.Connect("ButtonsHidden", this, "HideUI");
    }

    public void PauseGame()
    {
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
        if (!(CurrentLevel is null))
        {
            ((IScene)CurrentLevel).ExitScene();
            RemoveChild(GetNode(CurrentLevelName));
            CurrentLevel.QueueFree();
        }

        if (_checkIfSceneIsInterface(newSceneName))
        {
            HUD.Visible = false;
        }
        else
        {
            HUD.Visible = true;
        }
        

        var newScene = Levels[newSceneName];
        var newSceneInstance = newScene.Instance();
        newSceneInstance.Name = CurrentLevelName;
        ((IScene)newSceneInstance).EnterScene();
        CurrentLevel = (Node2D)newSceneInstance;
        AddChild(newSceneInstance);
    }
}