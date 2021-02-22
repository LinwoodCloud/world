extends KinematicBody

export (int) var GRAVITY = -15.0
var vel = Vector3()
export (int) var MAX_SPEED = 5
export (float) var JUMP_SPEED = 20
const ACCEL = 4.5

var dir = Vector3()

const DEACCEL= 16
const MAX_SLOPE_ANGLE = 40
export (NodePath) var path_to_voxel_world;

var camera
var rotation_helper
var voxel_world

var MOUSE_SENSITIVITY = 0.05
var do_raycast = false;
var mode_remove_voxel = false;
var current_voxel = "Cobble";

func _ready():
	camera = $Rotation_Helper/Camera
	rotation_helper = $Rotation_Helper
	voxel_world = get_node(path_to_voxel_world);

	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)

func _physics_process(delta):
	process_input(delta)
	process_movement(delta)
	if (do_raycast == true):
		do_raycast = false;
		
		var space_state = get_world().direct_space_state;
		var from = camera.project_ray_origin(get_viewport().get_mouse_position());
		var to = from + camera.project_ray_normal(get_viewport().get_mouse_position()) * 100;
		
		var result = space_state.intersect_ray(from, to, [self]);
		if (result):
			if (mode_remove_voxel == false):
				voxel_world.set_world_voxel(result.position + (result.normal/2), voxel_world.get_voxel_int_from_string(current_voxel));
			else:
				voxel_world.set_world_voxel(result.position - (result.normal/2), null);

func process_input(delta):

	# ----------------------------------
	# Walking
	dir = Vector3()
	var cam_xform = camera.get_global_transform()

	var input_movement_vector = Vector2()

	if Input.is_action_pressed("movement_forward"):
		input_movement_vector.y += 1
	if Input.is_action_pressed("movement_backward"):
		input_movement_vector.y -= 1
	if Input.is_action_pressed("movement_left"):
		input_movement_vector.x -= 1
	if Input.is_action_pressed("movement_right"):
		input_movement_vector.x += 1

	input_movement_vector = input_movement_vector.normalized()

	# Basis vectors are already normalized.
	dir += -cam_xform.basis.z * input_movement_vector.y
	dir += cam_xform.basis.x * input_movement_vector.x
	# ----------------------------------

	# ----------------------------------
	# Jumping
	if is_on_floor():
		if Input.is_action_pressed("movement_jump"):
			vel.y = JUMP_SPEED
	# ----------------------------------

	# ----------------------------------
	# Capturing/Freeing the cursor
	if Input.is_action_just_pressed("ui_cancel"):
		if Input.get_mouse_mode() == Input.MOUSE_MODE_VISIBLE:
			Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
		else:
			Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
	# ----------------------------------
	if Input.is_action_just_pressed("use"):
		do_raycast = true
		mode_remove_voxel = false
	if Input.is_action_just_pressed("break"):
		do_raycast = true
		mode_remove_voxel = true
func process_movement(delta):
	dir.y = 0
	dir = dir.normalized()

	vel.y += delta * GRAVITY

	var hvel = vel
	hvel.y = 0

	var target = dir
	target *= MAX_SPEED

	var accel
	if dir.dot(hvel) > 0:
		accel = ACCEL
	else:
		accel = DEACCEL

	hvel = hvel.linear_interpolate(target, accel * delta)
	vel.x = hvel.x
	vel.z = hvel.z
	vel = move_and_slide(vel, Vector3(0, 1, 0), 0.05, 4, deg2rad(MAX_SLOPE_ANGLE))

func _input(event):
	if event is InputEventMouseMotion and Input.get_mouse_mode() == Input.MOUSE_MODE_CAPTURED:
		rotation_helper.rotate_x(deg2rad(event.relative.y * MOUSE_SENSITIVITY * -1))
		self.rotate_y(deg2rad(event.relative.x * MOUSE_SENSITIVITY * -1))

		var camera_rot = rotation_helper.rotation_degrees
		camera_rot.x = clamp(camera_rot.x, -89, 89)
		rotation_helper.rotation_degrees = camera_rot
