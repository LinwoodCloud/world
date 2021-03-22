using System;
using Godot;

namespace LinwoodWorld.Level {
    class Player : KinematicBody {
        [Export]
        private int gravity = -15;
        [Export]
        public readonly int speed = 5;
        [Export]
        public readonly float jumpHeight = 20;
        private const float accel = 4.5f;
        private const float deaccel= 4.5f;
        [Export]
        private readonly float maxSlopeAngle= 4.5f;
        [Export]
        private readonly NodePath pathToVoxelWorld;
        private Camera camera;
        private Spatial rotationHelper;
        private PauseMenuScript pauseCanvas;
        [Export]
        private float MOUSE_SENSITIVITY = 0.05f;

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
        }
        public override void _PhysicsProcess(float delta)
        {
            ProcessInput(delta);
            ProcessMovement(delta);
        }

        private void ProcessMovement(float delta)
        {
            var dir = new Vector3();
            var vel = new Vector3();
            var camTransform = camera.GlobalTransform;
            var inputMovementVector = new Vector2();

            if(Input.IsActionPressed("movement_forward"))
                inputMovementVector.y += delta * 0.1f;
            if(Input.IsActionPressed("movement_backward"))
                inputMovementVector.y -= delta * 0.1f;
            if(Input.IsActionPressed("movement_left"))
                inputMovementVector.x -= delta * 0.1f;
            if(Input.IsActionPressed("movement_right"))
                inputMovementVector.x += delta * 0.1f;
            inputMovementVector = inputMovementVector.Normalized();

            // Jumping
            if(IsOnFloor())
                if(Input.IsActionPressed("movement_jump"))
                    vel.y += jumpHeight;
        }
        private void ProcessInput(float delta)
        {
            if(Input.IsActionJustPressed("ui_cancel"))
                if(Input.GetMouseMode() == Input.MouseMode.Visible){
                    pauseCanvas.Visible = false;
                    Input.SetMouseMode(Input.MouseMode.Captured);
                }
                else {
                    pauseCanvas.Visible = true;
                    Input.SetMouseMode(Input.MouseMode.Visible);
                }
        }
        public override void _Input(InputEvent @event){
	        if (@event is InputEventMouseMotion && Input.GetMouseMode() == Input.MouseMode.Captured){
                var motionInput = @event as InputEventMouseMotion;
	        	rotationHelper.RotateX(Mathf.Deg2Rad(motionInput.Relative.y * MOUSE_SENSITIVITY * -1));
	        	this.RotateY(Mathf.Deg2Rad(motionInput.Relative.x * MOUSE_SENSITIVITY * -1));
	        	var cameraRot = rotationHelper.RotationDegrees;
	        	cameraRot.x = Mathf.Clamp(cameraRot.x, -89, 89);
	        	rotationHelper.RotationDegrees = cameraRot;
            }
        }
    }
}