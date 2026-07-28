using Godot;
using System.Collections.Generic;

public partial class PlayerMovement : CharacterBody2D
{
    private const string METHOD_INTERACT = "Interact";
    private const string ANIM_IDLE = "Idle";
    private const string ANIM_MOVING = "Moving";

    [Export] private float speed = 50.0f;
    [Export] private bool sideScroller = false;
    [Export] private AnimationPlayer animationPlayer;
    [Export] private Sprite2D playerSprite;
    [Export] private Node2D pickupPosition;
    [Export] private Node2D dropPosition;
    [Export] private Area2D interactArea;
    [Export] private CollisionShape2D interactAreaCollider;
    [Export] private Node2D interactSprite;

    private List<Node2D> _interActablesInArea;

    private Interactable _heldObject = null;
    private bool _canMove = true;

    public override void _Ready()
    {
        interactArea.BodyEntered += InteractArea_BodyEntered;
        interactArea.BodyExited += InteractArea_BodyExited;
        interactArea.AreaEntered += InteractArea_AreaEntered;
        interactArea.AreaExited += InteractArea_AreaExited;
        interactSprite.Visible = false;
        _interActablesInArea = new List<Node2D>();
    }

    private void InteractArea_AreaEntered(Area2D area) => InteractArea_BodyEntered((Node2D)area);
    private void InteractArea_BodyEntered(Node2D body)
    {
        if ((body is Interactable && ((Interactable)body).CanInteract) || body.HasMethod("Interact"))
        {
            _interActablesInArea.Add(body);
        }

        if (_interActablesInArea.Count > 0)
        {
            interactSprite.Visible = true;
        }
    }

    private void InteractArea_AreaExited(Area2D area) => InteractArea_BodyExited((Node2D)area);
    private void InteractArea_BodyExited(Node2D body)
    {
        if (body is Interactable || body.HasMethod("Interact"))
        {
            _interActablesInArea.Remove(body);
        }

        if (_interActablesInArea.Count == 0)
        {
            interactSprite.Visible = false;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_canMove)
        {
            Velocity = Vector2.Zero;
            animationPlayer.Play(ANIM_IDLE);
            MoveAndSlide();
            return;
        }

        Vector2 velocity = Velocity;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 direction = sideScroller ? new Vector2(Input.GetAxis("MoveLeft", "MoveRight"), 0) : Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");


        if (direction != Vector2.Zero)
        {
            if (direction.X != 0)
            {
                if (direction.X < 0)
                {
                    playerSprite.FlipH = true;
                    SetInteractArea(true);
                }
                else
                {
                    playerSprite.FlipH = false;
                    SetInteractArea(false);
                }
            }
            animationPlayer.Play(ANIM_MOVING);
            velocity.X = direction.X * speed;
            velocity.Y = sideScroller ? 0 : direction.Y * speed;
        }
        else
        {
            animationPlayer.Play(ANIM_IDLE);
            velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
            velocity.Y = sideScroller ? 0 : Mathf.MoveToward(Velocity.Y, 0, speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }


    public override void _Input(InputEvent e)
    {
        if (e.IsActionReleased(METHOD_INTERACT))
        {
            GD.Print(METHOD_INTERACT);
            TryInteract();
        }
    }

    private void SetInteractArea(bool flipped)
    {

        Vector2 interactPos = interactAreaCollider.Position;
        Vector2 dropPos = dropPosition.Position;
        if (flipped && interactPos.X > 0)
        {
            dropPos.X = -dropPos.X;
            dropPosition.Position = dropPos;
            interactPos.X = -interactPos.X;
            interactAreaCollider.Position = interactPos;
            return;
        }
        if (!flipped && interactPos.X < 0)
        {
            dropPos.X = Mathf.Abs(dropPos.X);
            dropPosition.Position = dropPos;
            interactPos.X = Mathf.Abs(interactPos.X);
            interactAreaCollider.Position = interactPos;
            return;
        }
    }

    private void TryInteract()
    {
        if (_heldObject != null)
        {
            GD.Print("Drop Object");
            _heldObject.CallDeferred("ReparentMe", _heldObject.OldParent, dropPosition.GlobalPosition);
            _heldObject = null;
            return;
        }

        if (_interActablesInArea.Count == 0)
        { return; }

        Node2D firstInteract = _interActablesInArea[0];

        if (firstInteract is Interactable)
        {
            GD.Print("Pick up Object");
            _heldObject = (Interactable)firstInteract;
            _heldObject.CallDeferred("ReparentMe", pickupPosition, pickupPosition.GlobalPosition);
        }
        else if (firstInteract.HasMethod(METHOD_INTERACT))
        {
            firstInteract.Call(METHOD_INTERACT);
        }
    }

    public void StopMovement()
    {
        _canMove = false;
    }
    public void StartMovement()
    {
        _canMove = true;
    }
}
