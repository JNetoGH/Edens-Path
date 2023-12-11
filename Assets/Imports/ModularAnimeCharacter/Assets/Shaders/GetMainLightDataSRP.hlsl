#ifndef MAINLIGHT_INCLUDED
#define MAINLIGHT_INCLUDED

void GetMainLightData_half(out half3 direction, out half3 color)
{
#ifdef SHADERGRAPH_PREVIEW
    direction = half3(-0.3, -0.8, 0.6);
    color = half3(1, 1, 1);
#else
    Light mainLight = GetMainLight();
    direction = mainLight.direction;
    color = mainLight.color;
#endif
}
#endif