using Godot;

public partial class DistanceValue : Node2D
{
    [Signal] public delegate void OnValueChangedEventHandler(float value);

    [Export] private Pusheable slider;
    [Export] private Node2D lowEnd;
    [Export] private Node2D highEnd;
    [Export] private float minValue = 0;
    [Export] private float maxValue = 100;

    private float _lastDistanceSquare = 0;
    private float _travelDistance;

    public override void _Ready()
    {
        _lastDistanceSquare = lowEnd.GlobalPosition.DistanceSquaredTo(slider.GlobalPosition);
        _travelDistance = lowEnd.GlobalPosition.DistanceTo(highEnd.GlobalPosition);
    }

    public override void _PhysicsProcess(double delta)
    {
        DetermineValue();
    }

    private void DetermineValue()
    {
        float curDistanceSquare = lowEnd.GlobalPosition.DistanceSquaredTo(slider.GlobalPosition);
        if (!Mathf.IsEqualApprox(curDistanceSquare, _lastDistanceSquare))
        {
            _lastDistanceSquare = curDistanceSquare;
            float dist = lowEnd.GlobalPosition.DistanceTo(slider.GlobalPosition);
            GD.Print($"Value changed: {(int)dist} mapped:{ReMapDistanceToMinMax((int)dist)}");
            EmitSignalOnValueChanged(Mathf.Clamp(ReMapDistanceToMinMax((int)dist), minValue, maxValue));
            //remap dist to minValue and maxValue
        }
    }

    public void SetValue(float value)
    {
        float clampedValue = Mathf.Clamp(value, minValue, maxValue);
        slider.GlobalPosition = lowEnd.GlobalPosition.Lerp(highEnd.GlobalPosition, clampedValue / maxValue);
        EmitSignalOnValueChanged(clampedValue);
    }

    public float GetValuePercent()
    {
        float dist = lowEnd.GlobalPosition.DistanceTo(slider.GlobalPosition);
        float clampedDist = Mathf.Clamp(ReMapDistanceToMinMax((int)dist), minValue, maxValue);
        return clampedDist * 0.01f;
    }

    public int GetValueRaw()
    {
        float dist = lowEnd.GlobalPosition.DistanceTo(slider.GlobalPosition);
        return (int)Mathf.Clamp(ReMapDistanceToMinMax((int)dist), minValue, maxValue);
    }

    private float ReMapDistanceToMinMax(float value)
    {
        return value / _travelDistance * (maxValue - minValue) + minValue;
    }
}
