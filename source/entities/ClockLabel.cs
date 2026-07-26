using Godot;

public partial class ClockLabel : Label
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GlobalTime.Instance.TimeLeft += Instance_TimeLeft;
        GlobalTime.Instance.TimeIsUp += Instance_TimeIsUp;
        this.Text = GlobalTime.Instance.CurrentTime.ToString();
    }

    public override void _ExitTree()
    {
        GlobalTime.Instance.TimeLeft -= Instance_TimeLeft;
        GlobalTime.Instance.TimeIsUp -= Instance_TimeIsUp;
    }

    private void Instance_TimeIsUp()
    {
        GlobalTime.Instance.TimeLeft -= Instance_TimeLeft;
        GlobalTime.Instance.TimeIsUp -= Instance_TimeIsUp;
        this.Text = "0";
    }


    private void Instance_TimeLeft(int time)
    {
        this.Text = time.ToString();
    }
}
