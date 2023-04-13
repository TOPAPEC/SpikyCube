extends Node

var current_coins = 0 setget set_current_coins
var current_keys = 0 setget set_current_keys
var callback_rewarded_ad = JavaScript.create_callback(self, '_rewarded_ad')
var callback_ad = JavaScript.create_callback(self, '_ad')
var level_scores = []
onready var win = JavaScript.get_interface("window")

signal coins_amount_changed(new_amount)
signal keys_amount_changed(new_amount)

func _ready():
	level_scores.append([])
	for i in range(20):
		level_scores[0].append(0)

func get_level_score(chapter_id, level_id):
	return level_scores[chapter_id][level_id]

func set_current_coins(value):
	current_coins = int(value)
	emit_signal("coins_amount_changed", int(value))

func set_current_keys(value):
	current_keys = int(value)
	emit_signal("keys_amount_changed", int(value))

func save_level_progress(chapter_id, level_id):
	level_scores[int(chapter_id)][int(level_id)] = current_coins
	reset_current_state()

func reset_current_state():
	set_current_coins(0)
	set_current_keys(0)

