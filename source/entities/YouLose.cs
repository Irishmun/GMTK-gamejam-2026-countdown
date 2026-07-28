using Godot;
using System;

public partial class YouLose : Node
{
    [Export] private string scene;

    private void Instance_TimeIsUp()
    {
        GlobalTime.Instance.ResetTimer();
        SceneChangeUI.Instance.FilledScreen += Instance_FilledScreen;
        SceneChangeUI.Instance.FillScreen();
    }

    private void Instance_FilledScreen()
    {
        SceneChangeUI.Instance.FilledScreen -= Instance_FilledScreen;
        GetTree().ChangeSceneToFile(scene);
    }

    public override void _Input(InputEvent e)
    {
        if (e.IsActionReleased("PauseGame"))
        {
            Instance_TimeIsUp();
        }
    }
}
