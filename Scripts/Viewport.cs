using Godot;
using System;

public partial class Viewport : Node3D
{
    public const float zoomSpeed = 0.75f;
    public const float zoomDamp = 0.5f;
    public const float zoomMin = 5f;
    public const float zoomMax = 15f;
    public const float sensitivity = 5f / 1000f;
    public const float PI_2 = (float)Math.PI / 2f;

    public Camera3D Camera => GetNode<Camera3D>("Camera3D");

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

        if (@event is InputEventMouseMotion motion)
        {
            if (Input.IsMouseButtonPressed(MouseButton.Right))
            {
                Rotate(motion);
            }
        }

        if (@event is InputEventMouseButton button)
        {
            if (button.Pressed)
            {
                if (button.ButtonIndex == MouseButton.WheelUp)
                {
                    ZoomIn(Camera);
                }

                if (button.ButtonIndex == MouseButton.WheelDown)
                {
                    ZoomOut(Camera);
                }
            }
        }

        if (Input.IsKeyPressed(Key.P))
        {
            ChangeProjection(Camera);
        }

        if (Input.IsKeyPressed(Key.Q))
        {
            GetTree().Quit();
        }
    }

    private void Rotate(InputEventMouseMotion motion)
    {
        Rotation = new Vector3(
            Rotation.X,
            Rotation.Y - motion.Relative.X * sensitivity,
            Rotation.Z
            );
        Rotation = new Vector3(
            Math.Clamp(Rotation.X - motion.Relative.Y * sensitivity, -PI_2, PI_2),
            Rotation.Y,
            Rotation.Z
            );
    }

    private void ZoomIn(Camera3D camera)
    {
        if (camera.Projection == Camera3D.ProjectionType.Perspective)
        {
            camera.Position = new Vector3(
                camera.Position.X,
                camera.Position.Y,
                Math.Clamp(camera.Position.Z - zoomSpeed, zoomMin, zoomMax)
                );
        }
        else
        {
            camera.Size = Math.Clamp(camera.Size - zoomSpeed, zoomMin, zoomMax);
        }
    }

    private void ZoomOut(Camera3D camera)
    {
        if (camera.Projection == Camera3D.ProjectionType.Perspective)
        {
            camera.Position = new Vector3(
                camera.Position.X,
                camera.Position.Y,
                Math.Clamp(camera.Position.Z + zoomSpeed, zoomMin, zoomMax)
                );
        }
        else
        {
            camera.Size = Math.Clamp(camera.Size + zoomSpeed, zoomMin, zoomMax);
        }
    }

    private void ChangeProjection(Camera3D camera)
    {
        if (camera.Projection == Camera3D.ProjectionType.Perspective)
        {
            camera.Size = camera.Position.Z;
            camera.Projection = Camera3D.ProjectionType.Orthogonal;
        }
        else
        {
            camera.Position = new Vector3(
                camera.Position.X,
                camera.Position.Y,
                camera.Size
                );
            camera.Projection = Camera3D.ProjectionType.Perspective;
        }
    }
}