using Godot;
using System;

public class MainMenu : Control, IScene
{
    [Signal]
    public delegate void ContinueGame();
    private TextureButton _startGameButton;
    public override void _Ready()
    {
        _startGameButton = GetNode<TextureButton>("StartGameButton");
        _startGameButton.Connect("pressed", this, "ContinueGame");
    }

    public void EnterScene()
    {
        
    }

    public void ExitScene()
    {
        QueueFree();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
