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
                GetTree().ChangeSceneToFile(mainScenePath);
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
}
