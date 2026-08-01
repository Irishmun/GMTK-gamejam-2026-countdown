using Godot;
using System.Linq;
using System.Text;

public partial class DependantPowerline : BasePowerline
{
    [Export] private BasePowerline[] dependencies;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (dependencies.Length <= 0)
        {
            Powered = false;
            return;
        }

        for (int i = 0; i < dependencies.Length; i++)
        {
            dependencies[i].PowerStateChanged += DependantPowerline_PowerStateChanged;
        }
        CheckAllPowered();
    }

    private void DependantPowerline_PowerStateChanged(bool powered)
    {
        CheckAllPowered();
    }

    private void CheckAllPowered()
    {
        StringBuilder str = new StringBuilder();
        for (int i = 0; i < dependencies.Length; i++)
        {
            str.Append($"{dependencies[i].Name} powered: {dependencies[i].Powered}| ");
        }
        //GD.Print(this.Name + ": Check all powered: " + dependencies.All(x => x.Powered == true) + $"({str.ToString()})");

        Powered = dependencies.All(x => x.Powered == true);
    }
}
