shader_type spatial;
render_mode depth_draw_alpha_prepass;


uniform sampler2D block_texture: hint_albedo;
uniform float fuwafuwa_speed : hint_range(0, 10) = 1.5;
uniform float fuwafuwa_size : hint_range(0, 1) = 0.5;
uniform float fade : hint_range(0, 1) = 1;

uniform vec4 recolored : hint_color;
uniform float alpha : hint_range(0, 1) = 1;

void vertex()
{
	if(fuwafuwa_speed > 0f)
		VERTEX *= 1.0 + fade * cos(TIME * fuwafuwa_speed) * fuwafuwa_size * 0.1;
}
void fragment() {
	vec4 albedo = texture(block_texture, UV);
	ALPHA = (1.0-fade) * alpha + alpha;
	ALBEDO = albedo.rgb - (1.0 - recolored.rgb) * recolored.a * fade;
    // COLOR.rgb = vec3(1.0) - texture(TEXTURE, UV);
}