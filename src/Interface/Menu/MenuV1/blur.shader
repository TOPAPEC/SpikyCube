shader_type canvas_item;

void fragment() {
    vec4 color = texture(SCREEN_TEXTURE, SCREEN_UV);
    COLOR = color;
}