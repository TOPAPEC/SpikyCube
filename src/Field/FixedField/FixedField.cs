using Godot;
using System;
using System.Collections.Generic;


public class GridTranslator
{
    public Tuple<int, int> XYGridStep { get; set; }
    public Tuple<int, int> XYGridOffset { get; set; }
    public GridTranslator(Tuple<int, int> XYGridStep, Tuple<int, int> XYGridOffset)
    {
        this.XYGridOffset = XYGridOffset;
        this.XYGridStep = XYGridStep;
    }

    public Vector2 PositionToGridPosition(Vector2 position)
    {
        int x = (int)position.x;
        int y = (int)position.y;
        return new Vector2((x - XYGridOffset.Item1) / XYGridStep.Item1, (y - XYGridOffset.Item2) / XYGridStep.Item2);
    }

    public Vector2 GridPostionToPosition(Vector2 gridPosition)
    {
        return new Vector2((0.5f + gridPosition.x) * XYGridStep.Item1, (0.5f + gridPosition.y) * XYGridOffset.Item2);
    }

    public Vector2 SnapPositionToGrid(Vector2 position)
    {
        return GridPostionToPosition(PositionToGridPosition(position));
    }
}


public class FixedField : Node2D
{
    private GridTranslator _gridTranslator;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _gridTranslator = new GridTranslator(new Tuple<int, int>(64, 64), new Tuple<int, int>(64, 64));
        GetNode<DummyPlayer>("DummyPlayer").GridTranslator = _gridTranslator;
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
