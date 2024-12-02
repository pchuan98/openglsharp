#version 330 core

in vec3 Color;
in vec3 Position;

out vec4 color;

uniform float time;

void main(){
    
    float intensity = (sin(time)+1.0)/2.0;

    color = vec4(vec3(intensity) + Color ,1f);
}