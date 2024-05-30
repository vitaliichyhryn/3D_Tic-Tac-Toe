shader_type spatial;

instance uniform vec3 color : source_color;
instance uniform float alpha;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	ALBEDO = color;
	ALPHA = alpha;
}

//void light() {
	 //Called for every pixel for every light affecting the material.
	 //Uncomment to replace the default light processing function with this one.
//}
