using Godot;

public partial class PositionAudio : Area2D
{
    [Export] private AudioStreamPlayer2D localAudioPlayer;
    [Export] private AudioStreamPlayer globalAudioPlayer;

    private PlayerMovement _trackedPlayer;
    private float _localDistance;

    public override void _Ready()
    {
        this.BodyEntered += PositionAudio_BodyEntered;
        this.BodyExited += PositionAudio_BodyExited;
        this.SetPhysicsProcess(false);
        _localDistance = localAudioPlayer.MaxDistance * 0.5f;
        _localDistance = _localDistance * _localDistance;//_localDistance^2
        localAudioPlayer.VolumeLinear = 0;
        globalAudioPlayer.VolumeLinear = 1;
        localAudioPlayer.Play();
        globalAudioPlayer.Play();
    }

    private void PositionAudio_BodyEntered(Node2D body)
    {
        if (body is not PlayerMovement)
        { return; }
        _trackedPlayer = (PlayerMovement)body;
        this.SetPhysicsProcess(true);
    }
    private void PositionAudio_BodyExited(Node2D body)
    {
        if (body is not PlayerMovement)//add check for if body is tracked player if local multiplayer
        { return; }
        _trackedPlayer = null;
        localAudioPlayer.VolumeLinear = 0;
        globalAudioPlayer.VolumeLinear = 1;
        this.SetPhysicsProcess(false);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_trackedPlayer == null)
        { return; }

        float dist = localAudioPlayer.GlobalPosition.DistanceSquaredTo(_trackedPlayer.GlobalPosition);

        if (dist > _localDistance)
        {
            localAudioPlayer.VolumeLinear = 0;
            globalAudioPlayer.VolumeLinear = 1;
        }
        float vol = dist / _localDistance;
        vol = Mathf.Clamp(vol, 0, 1);
        GD.Print(vol);

        localAudioPlayer.VolumeLinear = 1;
        globalAudioPlayer.VolumeLinear = 1f - vol;

    }
}
