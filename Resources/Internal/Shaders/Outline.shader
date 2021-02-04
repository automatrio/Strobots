shader_type canvas_item;

uniform bool outline;
uniform float thickness : hint_range(0, 5) = 10.0;
uniform vec4 line_color : hint_color = vec4(1.0);

void fragment() {
	
	if (outline)
	{
		vec2 size = TEXTURE_PIXEL_SIZE * thickness;
		
		float offset = texture(TEXTURE, UV + vec2(size.x,0.0)).a;
		offset += texture(TEXTURE, UV - vec2(size.x,0)).a;
		offset += texture(TEXTURE, UV + vec2(0, size.y)).a;
		offset += texture(TEXTURE, UV + vec2(0, -size.y)).a;

		offset = min(offset, 1.0);
		
		vec4 color = texture(TEXTURE, UV);
		COLOR = mix(color, line_color, offset - color.a);
	}
	else
	{
		COLOR = texture(TEXTURE, UV);
	}
}