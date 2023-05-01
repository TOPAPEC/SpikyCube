using Godot;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using Godot.Collections;
using SpikyCube.SceneController;
using Object = Godot.Object;

public class Shop : Control, IScene
{
    private TextureButton _backButton;
    public void EnterScene(Node2D sceneController)
    {
        this.Connect("ChangeLevelTo", sceneController, "ChangeScene", new Godot.Collections.Array() {"LevelController"});
        _backButton = GetNode<TextureButton>("BackButton");
        _backButton.Connect("pressed", sceneController, "ChangeScene", new Godot.Collections.Array() { "MainMenu" });
    }

    public void ExitScene()
    {
        QueueFree();
    }
}
