shader_type canvas_item;

uniform float blur_amount : hint_range(0, 5);

void fragment() {
	vec4 color = texture(SCREEN_TEXTURE, SCREEN_UV);
	vec4 textureColor = texture(TEXTURE, UV);
	COLOR = vec4(textureColor.rgb * (1.0 - color.rgb), textureColor.a);


    // COLOR.rgb = vec3(1.0) - texture(TEXTURE, UV);
}
