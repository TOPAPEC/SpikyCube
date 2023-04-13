using Godot;
using System;
using SpikyCube.SceneController;
using Object = Godot.Object;

public class Key : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    //[Signal]
    //delegate void CollectKey(string name);
    private Object _playerStats;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Connect("area_entered", this, "Freee");
        _playerStats = GetNode<Object>("/root/PlayerStatsExtended");
    }

    public void Freee(Area2D other)
    {
        if (_playerStats.Get("current_keys") is int currentKeys)
        {
            _playerStats.Call("set_current_keys", currentKeys + 1);
            QueueFree();
        }
        else
        {
            GD.Print("Error not an int!");
        }
    }
    
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
