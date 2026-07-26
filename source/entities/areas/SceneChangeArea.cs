using Godot;
using System;

public partial class SceneChangeArea : Area2D
{
	[Export] private string ScenePath;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        this.BodyEntered += SceneChangeArea_BodyEntered;
	}

    private void SceneChangeArea_BodyEntered(Node2D body)
    {
		if (body is not PlayerMovement)
		{
			return;

		}

		GetTree().ChangeSceneToFile(ScenePath);
    }
}
