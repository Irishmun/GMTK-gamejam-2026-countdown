using Godot;

[GlobalClass]
public partial class Settings : Resource
{
    //[Export] private Color colorA = new Color("#f0f5f0");
    //[Export] private Color colorB =  new Color("212121");
    [Export] private int colorA = 0;
    [Export] private int colorB = 1;
    [Export] private bool startMaximized = false;

    [Export] private float masterVolume = 100f;
    [Export] private float sfxVolume = 100f;
    [Export] private float ambientVolume = 100f;

    //public Color ColorA { get => colorA; set => colorA = value; }
    public int ColorA { get => colorA; set => colorA = value; }
    //public Color ColorB { get => colorB; set => colorB = value; }
    public int ColorB { get => colorB; set => colorB = value; }
    public bool StartMaximized { get => startMaximized; set => startMaximized = value; }
    public float MasterVolume { get => masterVolume; set => masterVolume = value; }
    public float SfxVolume { get => sfxVolume; set => sfxVolume = value; }
    public float AmbientVolume { get => ambientVolume; set => ambientVolume = value; }
}
