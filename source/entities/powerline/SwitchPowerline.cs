using Godot;
using System.Linq;

public partial class SwitchPowerline : BasePowerline
{
    [Export] private BasePowerline[] dependencies;
    [Export] private BasePowerline[] blockers;

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
            blockers[i].PowerStateChanged += DependantPowerline_PowerStateChanged;
        }
        CheckAllPowered();
    }

    private void DependantPowerline_PowerStateChanged(bool powered)
    {
        GD.Print(this.Name + " powered: " + powered);
        CheckAllPowered();
    }

    private void CheckAllPowered()
    {
        Powered = dependencies.All(x => x.Powered == true) && blockers.All(x => x.Powered == false);
    }
}
