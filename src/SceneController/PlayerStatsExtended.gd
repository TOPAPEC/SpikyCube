extends Node

var current_coins = 0 setget set_current_coins
var current_keys = 0 setget set_current_keys
var callback_rewarded_ad = JavaScript.create_callback(self, '_rewarded_ad')
var callback_ad = JavaScript.create_callback(self, '_ad')
var callback_init_scores = JavaScript.create_callback(self, "load_player_profile")
var callback_set_profile_info = JavaScript.create_callback(self, "set_profile_info")
var profile = JavaScript.create_object("Object")
onready var win = JavaScript.get_interface("window")

signal coins_amount_changed(new_amount)
signal keys_amount_changed(new_amount)
signal end_level()
signal load_init_level(level)
signal profile_is_ready()

func _ready():
    if is_instance_valid(win):
        profile["SaveSchemaVersion"] = "1.0"
        profile["LevelScores"] = JavaScript.create_object("Array", 1)
        profile["LevelScores"].push(JavaScript.create_object("Array", 20))
        for i in range(20):
            profile["LevelScores"][i] = 0
        profile["LastLevel"] = 0
        profile["IsTutorialPassed"] = 0
        profile["Skins"] = JavaScript.create_object("Object")
        profile["Skins"]["PlayerSkin"] = 0
        profile["Skins"]["WorldSkin"] = 0
        profile["Skins"]["EnemySkins"] = JavaScript.create_object("Array", 0)
    else:
        print("Cannot init profile, no js")
        profile = {}
        profile["LevelScores"] = []
        profile["LevelScores"].append([])
        for _i in range(21):
            profile["LevelScores"][0].append(0)

func get_latest_save_schema_version():
    return "1.0"

func update_profile(old_profile):
    var profile_version = old_profile["SaveSchemaVersion"]
    var updated_profile = JavaScript.create_object("Object")
#   TODO update code
    return null

func check_profile_correct(new_profile):
    for key in profile.keys():
        if not(key in new_profile):
            return false
    if profile["Skins"].keys() != new_profile["Skins"].keys():
        return false
    return true

func get_level_score(chapter_id, level_id):
    return profile["LevelScores"][chapter_id][level_id]

func set_current_coins(value):
    current_coins = int(value)
    emit_signal("coins_amount_changed", int(value))

func get_current_coins():
    return current_coins

func set_current_keys(value):
    current_keys = int(value)
    emit_signal("keys_amount_changed", int(value))

func get_current_keys():
    return current_keys

func save_level_progress(chapter_id, level_id):
    profile["LevelScores"][int(chapter_id)][int(level_id)] = current_coins
    if is_instance_valid(win):
        upload_profile()
    else:
        print("Cannot upload progress, no js")
    reset_current_state()

func reset_current_state():
    set_current_coins(0)
    set_current_keys(0)

func init_sdk():
    if is_instance_valid(win):
        win.initGame()
        win.getData(callback_set_profile_info)
    else:
        print("Cannot load profile, no js")

func load_player_profile():
    win.getData(callback_set_profile_info)

func set_profile_info(args):
    var new_profile = args[0]
    var profile_version = args[0]["SaveSchemaVersion"]
    if profile_version != get_latest_save_schema_version():	    
        var updated_profile = update_profile(new_profile)
    if check_profile_correct(new_profile):
        profile = new_profile
    emit_signal("load_init_level", "Chapter0Level%s" % profile["LastLevel"])

func upload_profile():
    win.setData(profile)

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

func _notification(what):
    if what == MainLoop.NOTIFICATION_WM_FOCUS_IN: AudioServer.set_bus_mute(0, false)
    elif what == MainLoop.NOTIFICATION_WM_FOCUS_OUT: AudioServer.set_bus_mute(0, true)
