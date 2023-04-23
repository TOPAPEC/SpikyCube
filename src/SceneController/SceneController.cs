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
    
    private Dictionary<String, String> ScenePaths = new Dictionary<string, string>()
    {
        { "LevelSelection", "res://src/Interface/LevelSelection/LevelSelectionV1/LevelSelectionV1.tscn"},
        { "LevelController", "res://src/Interface/LevelController/LevelController.tscn" },
        { "Shop", "" },
        { "Settings", "" }
    };

    public override void _Ready()
    {
        _playerStats = GetNode<Object>("/root/PlayerStatsExtended");
        _loadingScreen = GetNode<LoadingScreen>("LoadingScreen/LoadingScreen");
        _loadingScreen.StartLoading();
        _playerStats.Call("init_sdk");
        
        _playerStats.Connect("profile_is_ready", this, "StartGame");
    }

    public void StartGame()
    {
        
        _loadingScreen.StopLoading();
    }
}

