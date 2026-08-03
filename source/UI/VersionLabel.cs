using Godot;

public partial class VersionLabel : Label
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Text = ProjectSettings.GetSetting("application/config/version").ToString();

    }
}
