using Godot;

public partial class PercentageValue : StaticBody2D
{
    [Export] private Label label;

    public void SetTextFromVariant(Variant value)
    {
        label.Text = string.Format(value.ToString());
    }

    public void SetTextFromFloat(float value)
    {
        SetTextFromVariant((int)value);
    }
}
