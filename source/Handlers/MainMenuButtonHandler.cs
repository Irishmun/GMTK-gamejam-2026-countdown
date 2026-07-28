using Godot;

public partial class MainMenuButtonHandler : Node
{
    [Export] private string mainScenePath;
    //Gotten to through in scene signals
    public void HandleButtonActivated(string buttonName)
    {

        switch (buttonName)
        {
            case "BT_START":
                GD.Print("Start game: " + buttonName);
                SceneChangeUI.Instance.FilledScreen += Instance_FilledScreen;
                SceneChangeUI.Instance.FillScreen();
                break;
            case "BT_OPTION":
                GD.Print("Open Options: " + buttonName);
                break;
            case "BT_QUIT":
                GD.Print("Quit Game: " + buttonName);
                GetViewport().SetInputAsHandled();
                GetTree().Quit();
                break;
            default:
                GD.Print("Button activated: " + buttonName);
                break;
        }
    }

    private void Instance_FilledScreen()
    {
        SceneChangeUI.Instance.FilledScreen -= Instance_FilledScreen;
        GetTree().ChangeSceneToFile(mainScenePath);
    }
}
