extends KinematicBody2D


func _ready():
	pass


func _on_Hitbox_area_entered(area):
	$AttackBox.set_collision_layer_bit(2, false)
	set_collision_layer_bit(0, false)
	set_collision_mask_bit(0, false)
	$Hitbox.set_collision_mask_bit(12, false)
	$Hitbox.hide()
	$AttackBox.hide()
	$CollisionShape2D.hide()
	$AnimatedSprite.playing = false;
	$AnimatedSprite.animation = "death"
	for i in range(6):
		$AnimatedSprite.frame = i
		yield(get_tree().create_timer(0.05), "timeout")
	$AnimatedSprite.hide()


func _on_AttackBox_area_entered(area):
	get_tree().reload_current_scene()
