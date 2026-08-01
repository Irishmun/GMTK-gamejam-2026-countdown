using Godot;

public partial class GlobalTime : Node
{
    public static GlobalTime Instance { get; private set; }

    [Signal] public delegate void TimeIsUpEventHandler();
    [Signal] public delegate void TimeLeftEventHandler(int time);

    [Export] private int startTime = 120;
    [Export] private Timer timer;

    private int _currentTime;

    public override void _EnterTree()
    {
        Instance = this;
    }

    public override void _Ready()
    {
        _currentTime = startTime;
        timer.Timeout += Timer_Timeout;
    }

    private void Timer_Timeout()
    {
        _currentTime -= 1;
        //GD.Print("Time left: " + _currentTime.ToString());
        if (_currentTime < 0)
        {
            EmitSignalTimeIsUp();
            StopTimer();
            return;
        }
        EmitSignalTimeLeft(_currentTime);
    }

    public void StartTimer()
    {
        timer.Start();
    }

    public void StopTimer()
    {
        timer.Stop();
    }

    public void ResetTimer(bool stoptimer = false)
    {
        if (stoptimer)
        {
            StopTimer();
        }
        _currentTime = startTime;
    }


    public int CurrentTime => _currentTime;
}
