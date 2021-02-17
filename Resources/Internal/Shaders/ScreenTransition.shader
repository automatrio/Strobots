shader_type canvas_item;

uniform bool use_texture = true;
uniform sampler2D background_texture;
uniform vec4 background_color : hint_color = vec4(0,0,0,1.0f);

uniform float current_alpha : hint_range(0,1) = 1.0;
uniform float oscillate_speed : hint_range(0,100) = 50.0;
uniform float oscillate_intensity : hint_range(0,1) = 1.0f;

float oscillate(float alpha, float time, float speed, float intensity)
{
	float remainder = abs(sin(time * speed)) * (1.0f - alpha) * intensity;
	float new_alpha = alpha + remainder;
	
	return new_alpha;
}

void fragment()
{
	if(use_texture)
	{
	COLOR = vec4(texture(TEXTURE, UV).rgb, texture(TEXTURE, UV).a * oscillate(current_alpha, TIME, oscillate_speed, oscillate_intensity));
	}
	else
	{
	COLOR = vec4(background_color.rgb, oscillate(current_alpha, TIME, oscillate_speed, oscillate_intensity));
	}
}