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
        return new Vector2(XYGridOffset.Item1 + (0.5f + gridPosition.x) * XYGridStep.Item1,
            XYGridOffset.Item2 + (0.5f + gridPosition.y) * XYGridStep.Item2);
    }

    public Vector2 SnapPositionToGrid(Vector2 position)
    {
        return GridPostionToPosition(PositionToGridPosition(position));
    }
}


public class FixedField : Node2D, IScene
{
    private GridTranslator _gridTranslator;
    private TileMap _tileMap;

    public override void _Ready()
    {
        _tileMap = GetNode<TileMap>("TileMap");
        _gridTranslator = new GridTranslator(new Tuple<int, int>(64, 64), new Tuple<int, int>((int)_tileMap.Position.x, (int)_tileMap.Position.y));
        GetNode<DummyPlayer>("DummyPlayer").GridTranslator = _gridTranslator;
    }

    public void EnterScene()
    {
    }

    public void ExitScene()
    {
        QueueFree();
    }
}