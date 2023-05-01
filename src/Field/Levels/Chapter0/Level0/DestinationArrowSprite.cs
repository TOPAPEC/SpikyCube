using Godot;
using System;
using System.CodeDom.Compiler;

public class DestinationArrowSprite : Node2D
{
    [Export] public float BounceInterval = 15f;
    private float _timePassed;
    private Sprite _sprite;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _sprite = GetNode<Sprite>("Sprite");

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        _timePassed += delta * 5f;
        _sprite.Position = new Vector2(0, Mathf.Lerp(0, Mathf.Sin(_timePassed) * BounceInterval, 0.5f));
    }
}
