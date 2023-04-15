extends Node

var current_coins = 0 setget set_current_coins
var current_keys = 0 setget set_current_keys
var callback_rewarded_ad = JavaScript.create_callback(self, '_rewarded_ad')
var callback_ad = JavaScript.create_callback(self, '_ad')
var callback_init_scores = JavaScript.create_callback(self, "load_player_profile")
var callback_set_profile_info = JavaScript.create_callback(self, "set_profile_info")
var level_scores = []
onready var win = JavaScript.get_interface("window")

signal coins_amount_changed(new_amount)
signal keys_amount_changed(new_amount)
signal end_level()

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
	var js_level_scores = JavaScript.create_object("Array", 1);
	print(js_level_scores)
	js_level_scores[0] = JavaScript.create_object("Array", 20);
	for i in range(js_level_scores[0].length):
		js_level_scores[0][i] = level_scores[0][i]
	print(js_level_scores)	
	win.setData(js_level_scores)
	reset_current_state()

func reset_current_state():
	set_current_coins(0)
	set_current_keys(0)

func init_sdk():
	win.initGame()
	win.getData(callback_set_profile_info)

func load_player_profile():
	win.getData(callback_set_profile_info)

func set_profile_info(args):
	var data = args[0]
	if "data" in data:
		level_scores = data["data"]

func js_show_ad():
	win.ShowAd(callback_ad)
	# Здесь можно приостановить музыку / звуки

func js_show_rewarded_ad():
	win.ShowAdRewardedVideo(callback_rewarded_ad)
	# Здесь можно приостановить музыку / звуки

func _rewarded_ad(args):
	print(args[0])
#    coins += 10
	# Здесь можно возобновить музыку / звуки

func _ad(args):
	print(args[0])
	# Здесь можно возобновить музыку / звуки

func end_current_level():
	emit_signal("end_level")

