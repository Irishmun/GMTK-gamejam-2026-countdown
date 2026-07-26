using Godot;
using System;

public partial class YouWinScreen : Node
{
	[Export] private AnimationPlayer animationPlayer;

	public void PlayAnimation()
	{
		animationPlayer.Play("youwin");
	}
}
