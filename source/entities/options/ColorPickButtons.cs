using Godot;

public partial class ColorPickButtons : Node2D
{
    private const string TEX_COLORS = "uid://cj37vfql4byov";

    [Export] private ColorPickButtons otherPicker;
    [Export] private Sprite2D[] selectors;
    [Export] private bool wrapAround = true;

    private int _selectedIndex = 0;

    public void SetIndex(int index)
    {
        _selectedIndex = Mathf.Clamp(index, 0, selectors.Length);
        GD.Print($"selected color index: " + _selectedIndex);
        ToggleSelected();
    }

    public void IncreaseIndex(bool actuallyChange = true)
    {
        if (!actuallyChange)
        { return; }

        int ind = _selectedIndex;
        ind++;

        if (wrapAround == true)
        { ind = ind >= selectors.Length ? 0 : ind; }

        GD.Print("increase index: " + ind);
        SetIndex(ind);

        if (ind == otherPicker.SelectedIndex)
        {
            IncreaseIndex();
        }
    }

    public void DecreaseIndex(bool actuallyChange = true)
    {
        if (!actuallyChange)
        { return; }

        int ind = _selectedIndex;
        ind--;

        if (wrapAround == true)
        { ind = ind < 0 ? selectors.Length - 1 : ind; }

        GD.Print("decrease index: " + ind);
        SetIndex(ind);

        if (ind == otherPicker.SelectedIndex)
        {
            DecreaseIndex();
        }
    }

    private void ToggleSelected()
    {
        for (int i = 0; i < selectors.Length; i++)
        {
            if (i == _selectedIndex)
            {
                selectors[i].Frame = 1;
            }
            else
            {
                selectors[i].Frame = 0;
            }
        }
    }

    public Color GetColorFromIndex(int index)
    {
        int pos = index;
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                pos--;
                if (pos < 0)
                {
                    GD.Print($"position of index {index}, hor:{x} vert:{y}");
                    return GetColorFromTexCoords(x, y);
                }
            }
        }

        return Colors.White;

        Color GetColorFromTexCoords(int x, int y)
        {
            Texture2D tex = GD.Load<Texture2D>(TEX_COLORS);
            Image img = tex.GetImage();
            return img.GetPixel(x, y);
        }
    }

    public int SelectedIndex => _selectedIndex;
}
