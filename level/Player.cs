using Godot;
using Godot.Collections;
using LinwoodWorld.WorldSystem;

namespace LinwoodWorld.Level
{
    public class Player : KinematicBody
    {
        [Export]
        private int gravity = -24;
        [Export]
        public readonly int speed = 5;
        [Export]
        public readonly float jumpHeight = 10;
        [Export]
        public readonly float jumps = 2;
        [Export]
        public float accel = 9.5f;
        [Export]
        public float deaccel = 4.5f;
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
        private int currentJumps = 0;
        private Spatial rightHand;
        private Tool currentTool = Tool.Hand;

        public Tool CurrentTool
        {
            get => currentTool; set
            {
                var last = currentTool;
                currentTool = value;
                EmitSignal(nameof(ToolChanged), last, value);
            }
        }

        [Signal]
        public delegate void ToolChanged(Tool last, Tool current);

        public override void _Ready()
        {
            camera = GetNode<Camera>("Rotation_Helper/Camera");
            rotationHelper = GetNode<Spatial>("Rotation_Helper");
            pauseCanvas = GetNode<PauseMenuScript>("Rotation_Helper/Camera/PauseCanvas");
            pauseCanvas.Visible = false;
            Input.SetMouseMode(Input.MouseMode.Captured);
            if (pathToVoxelWorld != null)
                voxelWorld = GetNode<VoxelWorld>(pathToVoxelWorld);
            if (voxelWorld == null)
                voxelWorld = GetParent<VoxelWorld>();
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
            if (IsOnFloor() && Input.IsActionPressed("movement_jump") || jumps > currentJumps && Input.IsActionJustPressed("movement_jump"))
            {
                velocity.y += jumpHeight;
                currentJumps++;
            }
            if (IsOnFloor() && currentJumps != 0)
                currentJumps = 0;
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

            if (GlobalTransform.origin.y <= -100)
            {
                GlobalTransform = new Transform(GlobalTransform.basis, new Vector3(GlobalTransform.origin.x, voxelWorld.worldSize.y * voxelWorld.chunkSize.y + 100, GlobalTransform.origin.z));
                velocity = new Vector3(0, 0, 0);
            }
            velocity = MoveAndSlide(velocity, new Vector3(0, 1, 0), floorMaxAngle: Mathf.Deg2Rad(maxSlopeAngle));
        }
        private void ProcessInput(float delta)
        {
            if (pathToVoxelWorld != null)
                WorldManipulationInput();
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

        private void WorldManipulationInput()
        {
            if (Input.IsActionJustPressed("break"))
            {
                var ray = RayCast();
                if (ray != null && ray.Contains("position") && ray.Contains("normal"))
                {
                    var position = (Vector3)ray["position"] - ((Vector3)ray["normal"] / 2);
                    voxelWorld.SetWorldVoxel(position, null);
                }
            }
            if (Input.IsActionJustPressed("use"))
            {
                var ray = RayCast();
                if (ray != null && ray.Contains("position") && ray.Contains("normal"))
                {
                    voxelWorld.SetWorldVoxel((Vector3)ray["position"], "res://mods/main/blocks/GrassBlock.cs");
                }
            }
        }
        private Dictionary RayCast()
        {
            var spaceState = GetWorld().DirectSpaceState;
            var from = camera.ProjectRayOrigin(GetViewport().GetMousePosition());
            var to = from + camera.ProjectRayNormal(GetViewport().GetMousePosition()) * 100;
            var result = spaceState.IntersectRay(from, to, new Array { this });
            return result;
        }
    }
}