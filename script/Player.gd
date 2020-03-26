



# SETUP
class_name Player
extends Area2D
# A Simple Player Script for Dodging Projectiles in 2D




# SIGNALS
signal hit							# Tell Enemies, Game, player died




# CONSTANTS
const my_sprite_name := "Sprite"	# How we call what we animate
const hitbox_name := "Hitbox"		# How we call our collider




# VARIABLES
export var speed = 400 				# How fast the player will move (pixels/sec).
var screen_size  					# Size of the game window.
var my_sprite						# What we actually animate
var hitbox							# What stuff collides with




# LIFECYCLES
func _ready():
	# Prevent locked aspect ratio, async res loading
	_set_screen_size()
	_set_sprite()
	_set_hitbox()
	hide()

func _process(delta):
	_read_input(delta)




# HELPERS
func _set_screen_size():
	# Sets the `screen_size` variable
	screen_size = get_viewport_rect().size

func _set_sprite():
	# Sets the player's sprite and configures it
	my_sprite = self.get_node(my_sprite_name)

func _set_hitbox():
	# Sets the player's collider and configures it
	hitbox = self.get_node(hitbox_name)
	hitbox.set_deferred("disabled", true)

func _read_input(delta):
	# Decodes the player keypresses and trigger the corresponding events
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

func _apply_position(velocity, delta):
	# Apply Position Updates
	self.position += velocity * delta
	self.position.x = clamp(position.x, 0, screen_size.x)
	self.position.y = clamp(position.y, 0, screen_size.y)

func _apply_animation(velocity):
	# Apply Animation Updates
	_do_animate(velocity)
	_choose_animation(velocity)




# ANIMATION
func _do_animate(velocity):
	# Do not animate when idle
	if velocity.length() > 0:
		my_sprite.play()
	else:
		my_sprite.stop()

func _choose_animation(velocity):
	# Animate by flipping (horizontal animation overrides vertical animation)
	if velocity.x != 0:
		my_sprite.animation = "right"
		my_sprite.flip_v = false
		my_sprite.flip_h = velocity.x < 0
	elif velocity.y != 0:
		my_sprite.animation = "up"
		my_sprite.flip_v = velocity.y > 0




# GAMEPLAY EVENTS
func start(pos):
	# Re-Initializes the player at the given position
	self.position = pos
	show()
	hitbox.disabled = false

func _on_Player_body_entered(_body):
	# Whenever the player collides with _anything_, the game is over
	print("HIT!")
	hide()
	emit_signal("hit")
	hitbox.set_deferred("disabled", true)










