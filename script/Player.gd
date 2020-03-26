



# SETUP
extends Area2D
# A Simple Player Script for Dodging Projectiles in 2D



# CONSTANTS
const my_sprite_name := "Sprite"		# How we call what we animate




# VARIABLES
export var speed = 400 				# How fast the player will move (pixels/sec).
var screen_size  					# Size of the game window.
var my_sprite							# What we actually animate




# LIFECYCLES
func _ready():
	_set_screen_size()
	_set_my_sprite()

func _process(delta):
	_read_input(delta)




# HELPERS
func _set_screen_size():
	# Sets the `screen_size` variable
	screen_size = get_viewport_rect().size

func _set_my_sprite():
	my_sprite = self.get_node(my_sprite_name)

func _read_input(delta):
	# Decodes the player keypresses and trigger the corresponding events
	# Calls each of the input functions to check their events
	# Also handles actually using the velocity
	
	# Reset the player's movement vector.
	var velocity = Vector2()
	
	# Check each input option
	velocity = _input_up(velocity)
	velocity = _input_down(velocity)
	velocity = _input_left(velocity)
	velocity = _input_right(velocity)
	velocity = _produce_velocity(velocity)
	_apply_animation(velocity)
	_apply_position(velocity, delta)




# INPUT EVENTS
func _input_up(velocity):
	# Player wants to move up _check_ and _effect_
	if Input.is_action_pressed("ui_up"):
		velocity.y -= 1
	return velocity

func _input_down(velocity):
	# Player wants to move down _check_ and _effect_
	if Input.is_action_pressed("ui_down"):
		velocity.y += 1
	return velocity

func _input_left(velocity):
	# Player wants to move left _check_ and _effect_
	if Input.is_action_pressed("ui_left"):
		velocity.x -= 1
	return velocity

func _input_right(velocity):
	# Player wants to move right _check_ and _effect_
	if Input.is_action_pressed("ui_right"):
		velocity.x += 1
	return velocity




# OUTPUT EFFECTS
func _produce_velocity(velocity):
	# Apply Speed and Normalization
	velocity = velocity.normalized() * speed
	return velocity

func _apply_animation(velocity):
	# Apply Animation Updates
	if velocity.length() > 0:
		my_sprite.play()
	else:
		my_sprite.stop()

func _apply_position(velocity, delta):
	# Apply Position Updates
	position += velocity * delta
	position.x = clamp(position.x, 0, screen_size.x)
	position.y = clamp(position.y, 0, screen_size.y)
















