using Godot;
using Godot.Collections;
using LinwoodWorld.System;

namespace LinwoodWorld.Level
{
    class Player : KinematicBody
    {
        [Export]
        private int gravity = -24;
        [Export]
        public readonly int speed = 5;
        [Export]
        public readonly float jumpHeight = 10;
        private const float accel = 4.5f;
        private const float deaccel = 4.5f;
        [Export]
        private readonly float maxSlopeAngle = 4.5f;
        [Export]
        private readonly NodePath pathToVoxelWorld;
        private Camera camera;
        private Spatial rotationHelper;
        private PauseMenuScript pauseCanvas;
        [Export]
        private float MOUSE_SENSITIVITY = 0.05f;
        private Vector3 velocity = new Vector3();
        private VoxelWorld voxelWorld;

        public void Resume()
        {
            pauseCanvas.Visible = false;
            Input.SetMouseMode(Input.MouseMode.Captured);
        }

        public override void _Ready()
        {
            camera = GetNode<Camera>("Rotation_Helper/Camera");
            rotationHelper = GetNode<Spatial>("Rotation_Helper");
            pauseCanvas = GetNode<PauseMenuScript>("Rotation_Helper/Camera/PauseCanvas");
            pauseCanvas.Visible = false;
            Input.SetMouseMode(Input.MouseMode.Captured);
            if (pathToVoxelWorld != null)
                voxelWorld = GetNode<VoxelWorld>(pathToVoxelWorld);
        }
        public override void _PhysicsProcess(float delta)
        {
            ProcessInput(delta);
            ProcessMovement(delta);
        }

        private void ProcessMovement(float delta)
        {
            var dir = new Vector3();
            var camTransform = camera.GlobalTransform;
            var inputMovementVector = new Vector2();

            if (Input.IsActionPressed("movement_forward"))
                inputMovementVector.y += 1;
            if (Input.IsActionPressed("movement_backward"))
                inputMovementVector.y -= 1;
            if (Input.IsActionPressed("movement_left"))
                inputMovementVector.x -= 1;
            if (Input.IsActionPressed("movement_right"))
                inputMovementVector.x += 1;
            inputMovementVector = inputMovementVector.Normalized();

            // Jumping
            if (IsOnFloor())
                if (Input.IsActionPressed("movement_jump"))
                    velocity.y += jumpHeight;
            dir += -camTransform.basis.z * inputMovementVector.y;
            dir += camTransform.basis.x * inputMovementVector.x;
            dir.y = 0;
            dir = dir.Normalized();
            velocity.y += delta * gravity;
            var hvel = velocity;
            hvel.y = 0;

            var target = dir;
            target *= speed;

            var current = (dir.Dot(hvel) > 0) ? accel : deaccel;

            hvel = hvel.LinearInterpolate(target, accel * delta);
            velocity.x = hvel.x;
            velocity.z = hvel.z;
            velocity = MoveAndSlide(velocity, new Vector3(0, 1, 0), false, 4, Mathf.Deg2Rad(maxSlopeAngle));
        }
        private void ProcessInput(float delta)
        {
            if (Input.IsActionJustPressed("ui_cancel"))
                if (Input.GetMouseMode() == Input.MouseMode.Visible)
                {
                    pauseCanvas.Visible = false;
                    Input.SetMouseMode(Input.MouseMode.Captured);
                }
                else
                {
                    pauseCanvas.Visible = true;
                    Input.SetMouseMode(Input.MouseMode.Visible);
                }
        }
        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseMotion && Input.GetMouseMode() == Input.MouseMode.Captured)
            {
                var motionInput = @event as InputEventMouseMotion;
                rotationHelper.RotateX(Mathf.Deg2Rad(motionInput.Relative.y * MOUSE_SENSITIVITY * -1));
                this.RotateY(Mathf.Deg2Rad(motionInput.Relative.x * MOUSE_SENSITIVITY * -1));
                var cameraRot = rotationHelper.RotationDegrees;
                cameraRot.x = Mathf.Clamp(cameraRot.x, -89, 89);
                rotationHelper.RotationDegrees = cameraRot;
            }
        }

        public void WorldManipulationInput()
        {
            if (Input.IsActionJustPressed("use"))
            {
                var ray = RayCast();
                voxelWorld.SetWorldVoxel(ray["position"]);
            }
        }
        public Dictionary RayCast()
        {
            var spaceState = GetWorld().DirectSpaceState;
            var from = camera.ProjectRayOrigin(GetViewport().GetMousePosition());
            var to = from + camera.ProjectRayNormal(GetViewport().GetMousePosition());
            var result = spaceState.IntersectRay(from, to, new Array { this });
            return result;
        }
    }
}