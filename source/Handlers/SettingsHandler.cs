using Godot;
using System.Text;

public partial class SettingsHandler : Node
{
    private const string FILENAME_SETTINGS = "/settings.ini";
    private const string SPECIAL_GAMES = "/My Games";
    private const string GAME_FOLDER = "/AfterDarkGames/RunTheClock";

    private const string UID_COLORMAT = "uid://drdyrxvsowpgd";

    [Signal] public delegate void MasterVolumeChangedEventHandler(float value);
    [Signal] public delegate void SfxVolumeChangedEventHandler(float value);
    [Signal] public delegate void BackgroundVolumeChangedEventHandler(float value);
    [Signal] public delegate void StartMaximizedChangedEventHandler(bool value);
    //[Signal] public delegate void ColorAChangedEventHandler(Color value);
    //[Signal] public delegate void ColorBChangedEventHandler(Color value);
    [Signal] public delegate void ColorAIndexChangedEventHandler(int value);
    [Signal] public delegate void ColorBIndexChangedEventHandler(int value);

    [Export] private DistanceValue masterVolumeSlider;
    [Export] private DistanceValue sfxVolumeSlider;
    [Export] private DistanceValue backgroundVolumeSlider;
    [Export] private ToggleSwitch fullScreenCheckbox;
    [Export] private ColorPickButtons colorAPickButton;
    [Export] private ColorPickButtons colorBPickButton;


    [Export] private Settings defaultSettings;

    public override void _Ready()
    {
        //if ini doesn't exist, use defaults
        SetSettings(ReadSettings());
        ApplySettings();
    }

    private void SetSettings(Settings settings)
    {
        EmitSignalMasterVolumeChanged(settings.MasterVolume);
        EmitSignalSfxVolumeChanged(settings.SfxVolume);
        EmitSignalBackgroundVolumeChanged(settings.AmbientVolume);
        EmitSignalStartMaximizedChanged(settings.StartMaximized);
        EmitSignalColorAIndexChanged(settings.ColorA);
        EmitSignalColorBIndexChanged(settings.ColorB);
    }

    private void ApplySettings()
    {
        SetAudioBusLevel("Master", masterVolumeSlider.GetValuePercent());
        SetAudioBusLevel("SFX", sfxVolumeSlider.GetValuePercent());
        SetAudioBusLevel("Background", backgroundVolumeSlider.GetValuePercent());
        SetWindowMaximized();
        SetMaterialColors();
    }

    private void SaveSettings()
    {
        string docs = OS.GetSystemDir(OS.SystemDir.Documents);
        using (DirAccess dir = DirAccess.Open(docs))
        {
            if (dir == null)
            {
                GD.PrintErr($"Something went wrong trying to access {docs}. exiting...");
                return;
            }

            GD.Print("Creating game directory: " + GAME_FOLDER);
            if (!dir.DirExists("." + SPECIAL_GAMES))
            {
                Error gamesErr = dir.MakeDir("." + SPECIAL_GAMES);
                if (gamesErr != Error.Ok)
                {
                    GD.PrintErr($"Something went wrong trying to make {SPECIAL_GAMES}.({gamesErr}) exiting...");
                    return;
                }
            }

            dir.ChangeDir("." + SPECIAL_GAMES);

            GD.Print($"current dir: " + dir.GetCurrentDir());

            Error err = dir.MakeDirRecursive("." + GAME_FOLDER);
            if (err != Error.Ok)
            {
                GD.PrintErr($"Something went wrong trying to make {GAME_FOLDER}.({err}) exiting...");
                return;
            }

            dir.ChangeDir("." + GAME_FOLDER);

            string filePath = string.Concat(dir.GetCurrentDir(), FILENAME_SETTINGS);
            GD.Print("opening file: " + filePath);
            using (FileAccess file = FileAccess.Open(filePath, FileAccess.ModeFlags.Write))
            {
                StringBuilder str = new StringBuilder();
                str.AppendLine($"volume_master: {masterVolumeSlider.GetValueRaw()}");
                str.AppendLine($"volume_sfx: {sfxVolumeSlider.GetValueRaw()}");
                str.AppendLine($"volume_ambient: {backgroundVolumeSlider.GetValueRaw()}");
                str.AppendLine($"start_maximized: {fullScreenCheckbox.State}");
                str.AppendLine($"color_a: {colorAPickButton.SelectedIndex}");
                str.AppendLine($"color_b: {colorBPickButton.SelectedIndex}");
                file.StoreString(str.ToString());
                GD.Print($"wrote settings to: {file.GetPathAbsolute()}");
            }
        }
    }

    public Settings ReadSettings()
    {
        string settingsPath = string.Concat(OS.GetSystemDir(OS.SystemDir.Documents), SPECIAL_GAMES, GAME_FOLDER, FILENAME_SETTINGS);
        if (!FileAccess.FileExists(settingsPath))
        {
            GD.Print($"Couldn't find settings at {settingsPath}. using defaults...");
            return defaultSettings;
        }

        string fileContents;

        using (FileAccess file = FileAccess.Open(settingsPath, FileAccess.ModeFlags.Read))
        {
            fileContents = file.GetAsText();
            GD.Print(fileContents);
        }

        string[] settingsArray = fileContents.Split('\n', System.StringSplitOptions.RemoveEmptyEntries | System.StringSplitOptions.TrimEntries);
        float masterVolume = defaultSettings.MasterVolume;
        float sfxVolume = defaultSettings.SfxVolume;
        float ambientVolume = defaultSettings.AmbientVolume;
        bool startMaximized = defaultSettings.StartMaximized;
        int colorA = defaultSettings.ColorA;
        int colorB = defaultSettings.ColorB;

        for (int i = 0; i < settingsArray.Length; i++)
        {
            string[] setting = settingsArray[i].Split(':', 2, System.StringSplitOptions.TrimEntries);
            switch (setting[0])
            {

                case "volume_master":
                    masterVolume = float.Parse(setting[1]);
                    continue;
                case "volume_sfx":
                    sfxVolume = float.Parse(setting[1]);
                    continue;
                case "volume_ambient":
                    ambientVolume = float.Parse(setting[1]);
                    continue;
                case "start_maximized":
                    startMaximized = bool.Parse(setting[1]);
                    continue;
                case "color_a":
                    colorA = int.Parse(setting[1]);
                    continue;
                case "color_b":
                    colorB = int.Parse(setting[1]);
                    continue;
                default:
                    continue;
            }
        }

        GD.Print($"volume_master: {masterVolume}");
        GD.Print($"volume_sfx: {sfxVolume}");
        GD.Print($"volume_ambient: {ambientVolume}");
        GD.Print($"start_maximized: {startMaximized}");
        GD.Print($"color_a: {colorA}");
        GD.Print($"color_b: {colorB}");

        return new Settings
        {
            MasterVolume = masterVolume,
            SfxVolume = sfxVolume,
            AmbientVolume = ambientVolume,
            StartMaximized = startMaximized,
            ColorA = colorA,
            ColorB = colorB,
        };
    }

    public void HandleButtons(string buttonName)
    {
        switch (buttonName)
        {
            case "BT_ACCEPT":
                ApplySettings();
                SaveSettings();
                break;
            case "BT_RESET":
                SetSettings(ReadSettings());
                break;
            default:
                break;
        }
    }

    private void SetAudioBusLevel(string busName, float percentage)
    {
        int index = AudioServer.GetBusIndex(busName);
        AudioServer.SetBusVolumeLinear(index, percentage);
    }

    private void SetMaterialColors()
    {
        Color colorA = colorAPickButton.GetColorFromIndex(colorAPickButton.SelectedIndex);
        Color colorB = colorBPickButton.GetColorFromIndex(colorBPickButton.SelectedIndex);

        Material mat = GD.Load<Material>(UID_COLORMAT);
        mat.Set("shader_parameter/replace_A", colorA);
        mat.Set("shader_parameter/replace_B", colorB);
    }

    //TODO: replace with screen size multipliers (1x, 2x, 3x, etc. Will go maximized if multiplier is beyond screen res)
    private void SetWindowMaximized()//(int multiplier)
    {
        //DisplayServer.WindowSetSize(Vector2I(400,240)*multiplier);
        if (fullScreenCheckbox.State)
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Maximized);
        }
        else
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        }
    }
}
