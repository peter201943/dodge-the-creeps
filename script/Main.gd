



# SETUP
extends Node2D
# Controls players, mobs, and score for a level




# CONSTANTS
const player_name := "Player"				# What we call the player
const start_pos_name := "StartPosition"		# What we call the player spawn location
const start_timer_name := "StartTimer"		# What we call the start timer
const score_timer_name := "ScoreTimer"		# What we call the score timer
const mob_timer_name := "MobTimer"			# What we call the mob timer
const mob_path_name := "MobPath"			# What we call the mob path
const spawn_finder_name := "SpawnFinder"	# What we call the spawner
const hud_name := "HUD"						# What we call the hud




# VARIABLES
export (PackedScene) var Mob				# How we describe a mob
var score									# How long the player has survived
var player									# How we control the player
var start_pos								# Where the player starts
var start_timer								# How long until spawning/scoring starts
var score_timer								# Time between points
var mob_timer								# Time between spawns
var mob_path								# Where we can spawn mobs
var spawn_finder							# Where we spawn a particular mob
var hud										# How we display messages




# LIFECYCLES
func _ready():
	_load_names()
	_connect_signals()
	randomize()




# SETUP HELPERS
func _load_names():
	# Assigns the Variables to Nodes in Scene
	player = self.get_node(player_name)
	start_pos = self.get_node(start_pos_name)
	start_timer = self.get_node(start_timer_name)
	score_timer = self.get_node(score_timer_name)
	mob_timer = self.get_node(mob_timer_name)
	mob_path = self.get_node(mob_path_name)
	hud = self.get_node(hud_name)


func _connect_signals():
	# Connects to each timer and player
	player.connect("hit", self, "game_over")
	start_timer.connect("timeout", self, "_start_timers")
	score_timer.connect("timeout", self, "_score_point")
	mob_timer.connect("timeout", self, "_spawn_mob")
	hud.connect("start_game", self, "new_game")




# PLAY HELPERS
func game_over():
	# Stops the Game
	score_timer.stop()
	mob_timer.stop()


func new_game():
	# Starts a New Game
	score = 0
	player.start(start_pos.position)
	start_timer.start()


func _start_timers():
	# Starts gameplay timers after spawn protection expires
	mob_timer.start()
	score_timer.start()


func _score_point():
	# Every Cycle of the Score Timer
	score += 1


func _spawn_mob():
	# Randomly Spawns a Mob
	
	# Choose a random location on Path2D.
	spawn_finder = mob_path.get_node(spawn_finder_name)
	spawn_finder.offset = randi()
	
	# Create a Mob instance and add it to the scene.
	var mob = Mob.instance()
	add_child(mob)
	
	# Set the mob's direction perpendicular to the path direction.
	var direction = spawn_finder.rotation + (PI / 2)
	
	# Set the mob's position to a random location.
	mob.position = spawn_finder.position
	
	# Add some randomness to the direction.
	direction += rand_range(-PI / 4, PI / 4)
	mob.rotation = direction
	
	# Set the velocity (speed & direction).
	mob.linear_velocity = Vector2(rand_range(mob.min_speed, mob.max_speed), 0)
	mob.linear_velocity = mob.linear_velocity.rotated(direction)




