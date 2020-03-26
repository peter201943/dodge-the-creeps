



# SETUP
extends Node2D
# Controls players, mobs, and score




# CONSTANTS
const player_name := "Player"
const start_pos_name := "StartPosition"
const start_timer_name := "StartTimer"
const score_timer_name := "ScoreTimer"
const mob_timer_name := "MobTimer"




# VARIABLES
export (PackedScene) var Mob
var score
var player
var start_pos
var start_timer
var score_timer
var mob_timer




# LIFECYCLES
func _ready():
	_load_names()
	randomize()




# HELPERS
func _load_names():
	# Assigns the Variables to Nodes in Scene
	player = self.get_parent().get_node(player_name)
	start_pos = self.get_parent().get_node(start_pos_name)
	start_timer = self.get_parent().get_node(start_timer_name)
	score_timer = self.get_parent().get_node(score_timer_name)
	mob_timer = self.get_parent().get_node(mob_timer_name)

func game_over():
	# Stops the Game
	score_timer.stop()
	mob_timer.stop()

func new_game():
	# Starts a New Game
	score = 0
	player.start(start_pos.position)
	start_timer.start()








