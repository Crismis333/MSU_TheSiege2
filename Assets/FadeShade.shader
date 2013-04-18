Shader "Custom/FadeShade" {
	Properties {
		_Tint ("Tint Color", Color) = (.9, .9, .9, 1.0)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_FadeTex ("Base (RGB)", 2D) = "white" {}
	}
	Category {
		ZWrite On
		Alphatest Greater 0
		Tags {Queue=Transparent}
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB

		SubShader {
			Pass {
				Material {
					Diffuse [_Tint]
					Ambient [_Tint]
				}
				Lighting On

				SetTexture [_MainTex] { combine texture }
				SetTexture [_FadeTex] { constantColor (0,0,0,[_Blend]) combine texture lerp(constant) previous }
				SetTexture [_FadeTex] { combine previous +- primary, previous * primary } 
			}
		} 
		FallBack "Diffuse", 1
	}
}
