shader_type canvas_item;

uniform vec4 base_A: source_color = vec4(0.94,0.96,0.94,1.0);
uniform vec4 base_B: source_color = vec4(0.13,0.13,0.13,1);
const float precision = 0.1;

uniform vec4 replace_A: source_color = vec4(0.94,0.96,0.94,1.0);
uniform vec4 replace_B: source_color = vec4(0.13,0.13,0.13,1);

vec4 swap_color(vec4 color){
	vec4 original[2] = vec4[2] (base_A,base_B);
	vec4 replace[2] = vec4[2] (replace_A,replace_B);
	for(int i = 0; i< 2; i++)	{
		if (distance(color, original[i]) <= precision){
			return replace[i];
		}
	}
	return color;
}

void fragment() {
	vec4 col = texture(TEXTURE,UV);
	vec4 resCol = swap_color(col);
	resCol.a = col.a;
	COLOR = resCol;
}