using Godot;
using System;
using SpikyCube.SceneController;

public class Key : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    //[Signal]
    //delegate void CollectKey(string name);
    private PlayerStats _ps;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Connect("area_entered", this, "Freee");
        _ps = GetNode<PlayerStats>("/root/PlayerStats");
    }

    public void Freee(Area2D other)
    {
        _ps.KeysCollected += 1;

        QueueFree();
    }
    
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
