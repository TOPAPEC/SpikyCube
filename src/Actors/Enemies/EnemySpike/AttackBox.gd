extends Area2D


func _ready():
    pass


func _on_AttackBox_area_entered(area):
    get_tree().reload_current_scene()
