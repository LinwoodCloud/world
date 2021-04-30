using Godot;
using Godot.Collections;
using LinwoodWorld.WorldSystem;
using LinwoodWorld.Particles;

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
        public readonly NodePath pathTocurrentWorld;
        private Camera camera;
        private Spatial rotationHelper;
        private PauseMenuScript pauseCanvas;
        [Export]
        private float MOUSE_SENSITIVITY = 0.05f;
        private Vector3 velocity = new Vector3();
        private VoxelWorld currentWorld;
        private int currentJumps = 0;
        private Spatial rightHand;
        private Tool currentTool;
        [Export]
        public NodePath pauseMenu;
        [Export]
        public NodePath backpack;
        private BlockManipulation blockManipulation = null;
        public string[] hotbar = new string[3];
        public Tool CurrentTool
        {
            get => currentTool; set
            {
                var last = currentTool;
                if (currentTool == Tool.Build && value != Tool.Build)
                    RemoveBlockPreview();
                if (currentTool != Tool.Build && value == Tool.Build)
                    ManipulateBlockPreview();
                currentTool = value;
                EmitSignal(nameof(ToolChanged), last, value);
            }
        }
        private int currentSlot = 0;
        public int CurrentSlot
        {
            get => currentSlot; set
            {
                while(value < 0)
                    value += hotbar.Length;
                var last = currentSlot;
                currentSlot = value % hotbar.Length;
                EmitSignal(nameof(SlotChanged), last, currentSlot);
            }
        }
        public string CurrentHotbar
        {
            get => hotbar[currentSlot];
            set
            {
                var last = hotbar[currentSlot];
                hotbar[currentSlot] = value;
                EmitSignal(nameof(HotbarChanged), last, value);
            }
        }
        public VoxelWorld CurrentWorld { get => currentWorld; }

        [Signal]
        public delegate void ToolChanged(Tool last, Tool current);

        [Signal]
        public delegate void SlotChanged(int last, int current);

        [Signal]
        public delegate void HotbarChanged(string last, string current);

        public override void _Ready()
        {
            camera = GetNode<Camera>("Rotation_Helper/Camera");
            rotationHelper = GetNode<Spatial>("Rotation_Helper");
            pauseCanvas = GetNode<PauseMenuScript>("Rotation_Helper/Camera/PauseCanvas");
            pauseCanvas.Visible = false;
            Input.SetMouseMode(Input.MouseMode.Captured);
            if (pathTocurrentWorld != null)
                currentWorld = GetNode<VoxelWorld>(pathTocurrentWorld);
            if (currentWorld == null)
                currentWorld = GetParent<VoxelWorld>();
            CurrentTool = Tool.Build;
        }
        public void Setup(VoxelWorld world)
        {
            currentWorld = world;
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
                GlobalTransform = new Transform(GlobalTransform.basis, new Vector3(GlobalTransform.origin.x, currentWorld.worldSize.y * currentWorld.chunkSize.y + 100, GlobalTransform.origin.z));
                velocity = new Vector3(0, 0, 0);
            }
            velocity = MoveAndSlide(velocity, new Vector3(0, 1, 0), floorMaxAngle: Mathf.Deg2Rad(maxSlopeAngle));
        }

        private void ProcessInput(float delta)
        {
            if (currentWorld != null)
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
                ManipulateBlockPreview();
            }
        }
        private void ManipulateBlockPreview()
        {
            if (IsInstanceValid(blockManipulation) && blockManipulation != null)
            {
                if (currentTool == Tool.Build && CurrentHotbar != null)
                {
                    var ray = RayCast();
                    if (ray != null && ray.Contains("position") && ray.Contains("normal"))
                    {
                        var position = (Vector3)ray["position"] + ((Vector3)ray["normal"] / 2);
                        position = position.Floor();
                        blockManipulation.GlobalTransform = new Transform(blockManipulation.GlobalTransform.basis, position);
                    }
                }
                else
                    RemoveBlockPreview();
            }
        }
        private void RemoveBlockPreview()
        {
            if (IsInstanceValid(blockManipulation) && blockManipulation != null)
            {
                blockManipulation.Connect("OnStop", this, nameof(RemoveCurrentBlock));
                blockManipulation.Stop();
                blockManipulation = null;
            }
        }

        private void WorldManipulationInput()
        {
            if (Input.IsActionJustPressed("break"))
            {
                var ray = RayCast();
                if (ray != null && ray.Contains("position") && ray.Contains("normal") && blockManipulation == null && currentBlock == null)
                {
                    var position = (Vector3)ray["position"] - ((Vector3)ray["normal"] / 2);
                    var manipulateScene = ResourceLoader.Load<PackedScene>("res://particles/BlockManipulation.tscn");
                    blockManipulation = manipulateScene.Instance() as BlockManipulation;
                    currentWorld.AddChild(blockManipulation);
                    var chunk = currentWorld.GetChunk(position);
                    var currentBlock = chunk.GetVoxel(chunk.GlobalTransform.XformInv(position));
                    blockManipulation.Setup(chunk, currentBlock);
                    blockManipulation.GlobalTransform = new Transform(blockManipulation.GlobalTransform.basis, position.Floor());
                    currentWorld.SetWorldVoxel(position, null);
                    CurrentHotbar = currentBlock;
                }
            }
            else if (Input.IsActionJustPressed("use"))
            {
                var ray = RayCast();
                if (ray != null && ray.Contains("position") && ray.Contains("normal") && currentBlock == null && CurrentHotbar != null)
                {
                    CurrentHotbar = null;
                    currentBlock = (Vector3)ray["position"] + ((Vector3)ray["normal"] / 2);
                    RemoveBlockPreview();
                }
            }
            if (Input.IsActionJustReleased("slot_next"))
            {
                GD.Print("TESTET");
                CurrentSlot++;
            }
            if (Input.IsActionJustReleased("slot_previous"))
                CurrentSlot--;
        }
        private Vector3? currentBlock = null;
        private void RemoveCurrentBlock()
        {
            if(currentBlock != null)
                currentWorld.SetWorldVoxel((Vector3) currentBlock, "res://mods/main/blocks/GrassBlock.cs");
            currentBlock = null;
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