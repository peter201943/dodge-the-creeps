



# SETUP
extends CanvasLayer
# Render Messages to the player, such as "you win" and "game over"




# SIGNALS
signal start_game							# Signal the `Main` to start the game




# CONSTANTS
const message_label_name := "Message"		# What we call the label
const message_timer_name := "MessageDelay"	# What we call the timer
const start_button_name := "Start"			# What we call the button
const score_label_name := "Score"			# What we call the score




# VARIABLES
var message_label							# How we show a message
var message_timer							# How we wait to show a message
var start_button							# How we know the player is ready
var score_label								# How the player knows their score




# LIFECYCLES
func _ready() -> void:
	_load_names()
	_connect_signals()




# LOAD HELPERS
func _load_names():
	# Assigns the labels and timers
	message_label = self.get_node(message_label_name)
	message_timer = self.get_node(message_timer_name)
	start_button = self.get_node(start_button_name)
	score_label = self.get_node(score_label_name)

func _connect_signals():
	# Connects the timer and button
	message_timer.connect("timeout", self, "_hide_message")
	start_button.connect("pressed", self, "_hide_button")




# DISPLAY HELPERS
func show_message(text: String) -> void:
	# Temporarily displays a message to the player
	message_label.text = text
	message_label.show()
	message_label.start()

func show_game_over():
	# Displays when player loses
	show_message("Game Over")
	yield(message_timer, "timeout")
	message_label.text = "Dodge the\nCreeps!"
	message_label.show()
	yield(get_tree().create_timer(1), "timeout")
	start_button.show()

func update_score(score: int):
	# Updates the score label with this score
	score_label.text = str(score)

func _hide_button():
	# Player is ready to start the game
	start_button.hide()
	emit_signal("start_game")

func _hide_message():
	# When a message has been displayed for long enough
	message_label.hide()



