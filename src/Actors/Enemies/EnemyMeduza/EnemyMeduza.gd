extends KinematicBody2D


func _ready():
    pass


func _on_Hitbox_area_entered(area):
    $AttackBox.collision_layer = 0
    $AnimatedSprite.playing = false
    $AnimatedSprite.animation = "death"
    for i in range(6):
        $AnimatedSprite.frame = i
        yield(get_tree().create_timer(0.05), "timeout")
    queue_free()
