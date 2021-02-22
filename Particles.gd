extends Particles


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.

var deathtime = 1

var time = 0

var start = false
func _ready():
	emitting = true
func _process(delta):
	 if not emitting:
		  start = true
	 if start:
		  time += delta
	 if time >= deathtime:
		  get_parent().queue_free()
