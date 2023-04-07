using Godot;
using System;

public class MenuV1 : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    private Control _buttonsBlock;

    private int _hiddenButtonsBlockMarginLeft = 700;

    private int _visibleButtonsBlockMarginLeft = 0;

    private String _buttonsBlockState;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _buttonsBlock = GetNode<Control>("ButtonsBlock");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
         switch (_buttonsBlockState)
         {
             case "entering": 
                 _buttonsBlock.MarginLeft = Mathf.Lerp(_buttonsBlock.MarginLeft, _visibleButtonsBlockMarginLeft, 4f * delta);
                 break;
             case "exiting":
                 _buttonsBlock.MarginLeft = Mathf.Lerp(_buttonsBlock.MarginLeft, _hiddenButtonsBlockMarginLeft, 4f * delta);
                 if (Mathf.Abs(_buttonsBlock.MarginLeft - _hiddenButtonsBlockMarginLeft) < 0.0001f)
                 {
                     _buttonsBlockState = "idle";
                     _buttonsBlock.Visible = false;
                     _buttonsBlock.SetProcess(false);
                     _buttonsBlock.SetPhysicsProcess(false);
                 }
                 break;
         }
    }

    public void ShowButtons()
    {
        _buttonsBlock.MarginLeft = _hiddenButtonsBlockMarginLeft;
        _buttonsBlockState = "entering";
        _buttonsBlock.Visible = true;
        _buttonsBlock.SetProcess(true);
        _buttonsBlock.SetPhysicsProcess(true);
    }

    public void HideButtons()
    {
        _buttonsBlock.MarginLeft = _visibleButtonsBlockMarginLeft;
        _buttonsBlockState = "exiting";
    }
}
