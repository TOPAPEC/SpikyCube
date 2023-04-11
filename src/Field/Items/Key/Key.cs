using Godot;
using System;

public class Key : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    //[Signal]
    //delegate void CollectKey(string name);

    public void Freee(Area2D other)
    {
        //GD.Print(name);
        //GD.Print(this.Name);
        var parent = (DummyPlayer)(other.GetParent());

        if (parent.MovingForward)
        {
            QueueFree();
        }
    }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Connect("area_entered", this, "Freee");
    }

    
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
