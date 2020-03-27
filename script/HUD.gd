

# SETUP
extends CanvasLayer
# Render Messages to the player, such as "you win" and "game over"


# SIGNALS
signal start_game							# Signal the `Main` to start the game


# CONSTANTS
const message_label_name := "Message"		# What we call the label
const message_timer_name := "MessageDelay"	# What we call the timer


# VARIABLES
var message_label							# How we show a message
var message_timer							# How we wait to show a message


# LIFECYCLES
func _ready() -> void:
	# Assigns the label and timer
	message_label = self.get_node(message_label_name)
	message_timer = self.get_node(message_timer_name)


# HELPERS
func show_message(text: String) -> void:
	# Temporarily displays a message to the player
	message_label.text = text
	message_label.show()
	message_label.start()

