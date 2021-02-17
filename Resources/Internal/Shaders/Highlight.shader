shader_type spatial;
render_mode unshaded, cull_front;

uniform float thickness : hint_range(0, 1) = 0.1f;
uniform vec4 outline_color : hint_color = vec4(1, 0.85, 0.15, 1);

void vertex()
{
	VERTEX *= (1.0 + thickness * 0.25f);

}

void fragment()
{
	ALBEDO = outline_color.rgb;
	ALPHA = outline_color.a;
}