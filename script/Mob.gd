



extends RigidBody2D


const sprite_name := "Sprite"



export var min_speed = 150  # Minimum speed range.
export var max_speed = 250  # Maximum speed range.
var mob_types = ["walk", "swim", "fly"]
var sprite




func _ready():
	sprite = self.get_node(sprite_name)
	sprite.animation = mob_types[randi() % mob_types.size()]

func _on_Visibility_screen_exited():
	# Destroy this mob
	queue_free()

