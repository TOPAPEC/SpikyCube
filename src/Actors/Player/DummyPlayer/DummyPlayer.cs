using Godot;
using System;

public class DummyPlayer : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    [Export] public int Speed = 800;
    private bool _isMoving;
    private Vector2 _currentVelocity;
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        GD.Print(_currentVelocity);
        if (!_isMoving)
        {
            if (Input.IsActionPressed("move_right"))
            {
                _isMoving = true;
                _currentVelocity = Vector2.Right * Speed;
            }    
            else if (Input.IsActionPressed("move_left"))
            {
                _isMoving = true;
                _currentVelocity = Vector2.Left * Speed;
            }
            else if (Input.IsActionPressed("move_down"))
            {
                _isMoving = true;
                _currentVelocity = Vector2.Down * Speed;
            }
            else if (Input.IsActionPressed("move_up"))
            {
                _isMoving = true;
                _currentVelocity = Vector2.Up * Speed;
            }
        }

        if (!(MoveAndCollide(_currentVelocity) is null))
        {
            _isMoving = false;
            _currentVelocity = Vector2.Zero;
        }
        
    }
}
