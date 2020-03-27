



extends RigidBody2D
# Simple Blob (Lit. does nothing)




# CONSTANTS
const sprite_name := "Sprite"		# How we find the sprite




# VARIABLES
export var min_speed = 150
export var max_speed = 250
var mob_types = ["walk", "swim", "fly"]		# Changes animation; speed
var sprite									# How we animate




# LIFECYCLES
func _ready():
	# Set Sprite and Animate
	sprite = self.get_node(sprite_name)
	sprite.animation = mob_types[randi() % mob_types.size()]
	self.get_node("Visibility").connect("screen_exited", self, "_exit_screen")




# HELPERS
func _exit_screen():
	# Destroy this mob when off screen
	queue_free()

func _game_start():
	# Destroy this mobe when new game
	queue_free()



