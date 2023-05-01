using Godot;
using System;

public interface IScene
{
    void EnterScene(Node2D sceneController);
    void ExitScene();
}