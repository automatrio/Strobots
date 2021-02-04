shader_type canvas_item;

uniform float scanline_count : hint_range(0, 1000) = 50.0;
uniform float PI = 3.14;
uniform float speed = 20;
uniform sampler2D ViewportTexture;

vec2 uv_curve(vec2 uv) {
	uv = (uv - 0.5) * 2.0;
	uv.x *= 1.0 + pow(abs(uv.y) / 2.0, 2.0);
	uv.y *= 1.0 + pow(abs(uv.x) / 2.0, 3.0);
	
	uv = (uv / 2.0) + 0.5;
	return uv;
}

float get_alpha(vec2 uv) {
	float new_alpha = 1.0;
	if(uv.y < 0.0 || uv.y > 1.0) {
		new_alpha = 0.0;
	}
	if(uv.x < 0.0 || uv.x > 1.0) {
		new_alpha = 0.0;
	}
	
	return new_alpha;
}

void fragment()
{
	float r = texture(TEXTURE, uv_curve(UV) + vec2(TEXTURE_PIXEL_SIZE.x * 5.0, 0)).r;
	float g = texture(TEXTURE, uv_curve(UV) + vec2(TEXTURE_PIXEL_SIZE * 1.0)).g;
	float b = texture(TEXTURE, uv_curve(UV) + vec2(TEXTURE_PIXEL_SIZE * -1.0)).b;
	
	float a = get_alpha(uv_curve(UV));
	
	float s = sin(uv_curve(UV).y * scanline_count * PI * 2.0 + (TIME * speed));
	s = (s * 0.5 + 0.5) * 0.9 + 0.1;
	vec4 scanline = vec4(vec3(pow(s, 0.1)), 1.0);
	
//	float horizontal_shadow = -4.0 * pow((UV.x - 0.5) * 0.75, 2.0) + 1.0;
//	float vertical_shadow = -4.0 * pow((UV.y - 0.5) * 0.75, 2.0) + 1.0;
	
	float horizontal_shadow = -16.0 * pow(max(uv_curve(UV).x, 0.75) - 0.75, 2.0) + 1.0;
	horizontal_shadow *= -16.0 * pow(min(uv_curve(UV).x, 0.25) - 0.25, 2.0) + 1.0;
	float vertical_shadow = -16.0 * pow(max(uv_curve(UV).y, 0.75) - 0.75, 2.0) + 1.0;
	vertical_shadow *= -16.0 * pow(min(uv_curve(UV).y, 0.25) - 0.25, 2.0) + 1.0;
	
	COLOR = vec4(r, g, b, a) * scanline * (horizontal_shadow * vertical_shadow);
	
}