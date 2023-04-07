using Godot;
using System;
using System.Collections.Generic;
using SpikyCube.SceneController;

public class SceneController : Node2D
{
    public Node CurrentScene;
    public String CurrentSceneChildName = "CurrentLevel";
    public static readonly LevelsDict Levels = new LevelsDict();
    public HUDV1 HUD;
    public PlayerStats CurrentPlayerStats;

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
        HUD = GetNode<HUDV1>("HUD");
        HUD.ShowHud();
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
        if (!(CurrentScene is null))
        {
            ((IScene)CurrentScene).ExitScene();
            RemoveChild(GetNode(CurrentSceneChildName));
            CurrentScene.QueueFree();
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
        ((IScene)newSceneInstance).EnterScene();
        CurrentScene = newSceneInstance;
        CurrentSceneChildName = newSceneName;
        AddChild(newSceneInstance);
    }
}