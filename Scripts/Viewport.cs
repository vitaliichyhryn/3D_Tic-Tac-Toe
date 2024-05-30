using Godot;
using System;

public partial class Viewport : Node3D
{   
    public const float ZoomSpeed = 0.05f;
    public const float ZoomMin = 5f;
    public const float ZoomMax = 15f;
    public const float ZoomDamp = 0.85f;

    public const float RotationSpeed = 0.75f / 1000f;
    public const float RotationDamp = 0.85f;
    public const float PitchMax = (float)Math.PI / 2f;

    public float TwistInput = 0;
    public float PitchInput = 0;
    public float ZoomInput = 0;

    public Node3D TwistPivot => GetNode<Node3D>("TwistPivot");
    public Node3D PitchPivot => GetNode<Node3D>("TwistPivot/PitchPivot");
    public Camera3D Camera => GetNode<Camera3D>("TwistPivot/PitchPivot/Camera");

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        CameraRotation();
        CameraZoom();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

        if (@event is InputEventMouseMotion motion)
        {
            if (Input.IsMouseButtonPressed(MouseButton.Right))
            {
                CameraRotate(motion);
            }
        }

        if (Input.IsActionJustPressed("ZoomIn"))
        {
            CameraZoomIn();
        }
        if (Input.IsActionJustPressed("ZoomOut"))
        {
            CameraZoomOut();
        }
    }

    private void CameraRotate(InputEventMouseMotion motion)
    {
        TwistInput -= motion.Relative.X * RotationSpeed;
        PitchInput -= motion.Relative.Y * RotationSpeed;
    }

    private void CameraRotation()
    {
        TwistPivot.Rotation = new Vector3(
            TwistPivot.Rotation.X,
            TwistPivot.Rotation.Y + TwistInput,
            TwistPivot.Rotation.Z
        );
        
        PitchPivot.Rotation = new Vector3(
            Math.Clamp(PitchPivot.Rotation.X + PitchInput, -PitchMax, PitchMax),
            PitchPivot.Rotation.Y,
            PitchPivot.Rotation.Z
        );

        TwistInput *= RotationDamp;
        PitchInput *= RotationDamp;
    }
    
    private void CameraZoomIn()
    {
        ZoomInput -= ZoomSpeed;
    }

    private void CameraZoomOut()
    {
        ZoomInput += ZoomSpeed;
    }

    private void CameraZoom()
    {
        Camera.Position = new Vector3(
            Camera.Position.X,
            Camera.Position.Y,
            Math.Clamp(Camera.Position.Z + ZoomInput, ZoomMin, ZoomMax)
        );

        ZoomInput *= ZoomDamp;
    }
}