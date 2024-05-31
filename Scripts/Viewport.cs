using System;
using Godot;

public partial class Viewport : Node3D
{
    private const float ZoomSpeed = 0.05f;
    private const float ZoomMin = 5f;
    private const float ZoomMax = 15f;
    private const float ZoomDamp = 0.85f;
    private const float RotationSpeed = 0.75f / 1000f;
    private const float RotationDamp = 0.85f;
    private const float PitchMax = (float)Math.PI / 2f;
    private float _twistInput;
    private float _pitchInput;
    private float _zoomInput;
    private Node3D TwistPivot => GetNode<Node3D>("TwistPivot");
    private Node3D PitchPivot => GetNode<Node3D>("TwistPivot/PitchPivot");
    private Camera3D Camera => GetNode<Camera3D>("TwistPivot/PitchPivot/Camera");

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
            if (Input.IsMouseButtonPressed(MouseButton.Right))
                CameraRotate(motion);

        if (Input.IsActionJustPressed("ZoomIn")) CameraZoomIn();
        if (Input.IsActionJustPressed("ZoomOut")) CameraZoomOut();
    }

    private void CameraRotate(InputEventMouseMotion motion)
    {
        _twistInput -= motion.Relative.X * RotationSpeed;
        _pitchInput -= motion.Relative.Y * RotationSpeed;
    }

    private void CameraRotation()
    {
        TwistPivot.Rotation = new Vector3(
            TwistPivot.Rotation.X,
            TwistPivot.Rotation.Y + _twistInput,
            TwistPivot.Rotation.Z
        );

        PitchPivot.Rotation = new Vector3(
            Math.Clamp(PitchPivot.Rotation.X + _pitchInput, -PitchMax, PitchMax),
            PitchPivot.Rotation.Y,
            PitchPivot.Rotation.Z
        );

        _twistInput *= RotationDamp;
        _pitchInput *= RotationDamp;
    }

    private void CameraZoomIn()
    {
        _zoomInput -= ZoomSpeed;
    }

    private void CameraZoomOut()
    {
        _zoomInput += ZoomSpeed;
    }

    private void CameraZoom()
    {
        Camera.Position = new Vector3(
            Camera.Position.X,
            Camera.Position.Y,
            Math.Clamp(Camera.Position.Z + _zoomInput, ZoomMin, ZoomMax)
        );

        _zoomInput *= ZoomDamp;
    }
}