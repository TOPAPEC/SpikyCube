using Godot;
using System;
using System.Collections.Generic;
using Object = Godot.Object;

public class SceneController : Node2D
{
    public Node CurrentScene;
    public String CurrentSceneName;

    private Object _playerStats;

    private LoadingScreen _loadingScreen;

    private CanvasLayer _gameLayer;
    
    private Dictionary<String, String> ScenePaths = new Dictionary<string, string>()
    {
        { "LevelSelection", "res://src/Interface/LevelSelection/LevelSelectionV1/LevelSelectionV1.tscn"},
        { "LevelController", "res://src/Interface/LevelController/LevelController.tscn" },
        { "MainMenu", "res://src/Interface/MainMenu/MainMenu.tscn" },
        { "Shop", "res://src/Interface/Shop/Shop.tscn" },
        { "Settings", "res://src/Interface/Settings/Settings.tscn" }
    };

    public override void _Ready()
    {
        _playerStats = GetNode<Object>("/root/PlayerStatsExtended");
        _loadingScreen = GetNode<LoadingScreen>("LoadingScreen/LoadingScreen");
        _gameLayer = GetNode<CanvasLayer>("Game");
        _loadingScreen.StartLoading();
        _playerStats.Connect("profile_is_ready", this, "StartGame");
        _playerStats.Call("init_player_data");
        CurrentScene = GetNode<Node>("Game/MainMenu");
        ((IScene)CurrentScene).EnterScene(this);
    }

    public void StartGame()
    {
        _loadingScreen.StopLoading();
    }

    public void StartTutorial()
    {
        _playerStats.Call("set_last_level", new Godot.Collections.Array() { 0, 0, 0 });
        ChangeScene("LevelController");
    }

    public void ChangeScene(String nextScene)
    {
        GD.Print($"Changing scene {nextScene}");
        ((IScene)CurrentScene).ExitScene();
        var newScenePackedScene = ResourceLoader.Load<PackedScene>(ScenePaths[nextScene]);
        var newSceneInstance = newScenePackedScene.Instance();
        _gameLayer.AddChild(newSceneInstance);
        ((IScene)newSceneInstance).EnterScene(this);
        CurrentScene = newSceneInstance;
        CurrentSceneName = nextScene;
    }
}

