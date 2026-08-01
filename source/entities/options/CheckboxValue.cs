using Godot;

public partial class CheckboxValue : StaticBody2D
{
    [Export] private Sprite2D sprite;

    private bool _checked = false;

    public void SetChecked(bool value)
    {
        _checked = value;
        sprite.Frame = value ? 1 : 0;
    }

    public bool GetChecked()
    {
        return _checked;
    }
}
