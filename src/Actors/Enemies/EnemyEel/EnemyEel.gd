extends KinematicBody2D

var vector = Vector2()
export var dir = 1
export var hor = 1
export var stomach = 1
export var speed = 50

func _ready():
    if dir == -1:
        $AnimatedSprite.flip_h = true
    vector.x += stomach * dir * speed
    if hor == -1:
        vector.x = 0
        vector.y += stomach * dir * speed

func _on_Hitbox_area_entered(area):
    vector.x = 0
    $AttackBox.collision_layer = 0
    $AnimatedSprite.playing = false;
    $AnimatedSprite.animation = "death"
    $AnimatedSprite.flip_v = true
    for i in range(6):
        $AnimatedSprite.frame = i
        yield(get_tree().create_timer(0.05), "timeout")
    queue_free()

func _physics_process(delta):
    move_and_slide(vector)
    for index in get_slide_count():
        var coll = get_slide_collision(index)
        if hor == 1:
            vector.x *= -1
        else:
            vector.y *= -1
        dir *= -1
        if dir == -1:
            $AnimatedSprite.flip_h = true
        else:
            $AnimatedSprite.flip_h = false
        break
