#ifndef ZPGAME_STEREO_FUNC
#define ZPGAME_STEREO_FUNC

inline float4 OutScreenExcessiveRecompensation(float4 pos, float4 par)
{
			if (pos.w < par.z && par.w)
			{
				if (UNITY_MATRIX_P[0][2] > 0)
					pos.x = pos.x - par.x*UNITY_MATRIX_P[0][0] + pos.w*UNITY_MATRIX_P[0][2] + (par.x*UNITY_MATRIX_P[0][0] - par.z*UNITY_MATRIX_P[0][2]) / par.z*pos.w;
				else
					pos.x = pos.x + par.x*UNITY_MATRIX_P[0][0] + pos.w*UNITY_MATRIX_P[0][2] - (par.x*UNITY_MATRIX_P[0][0] + par.z*UNITY_MATRIX_P[0][2]) / par.z*pos.w;
			}
			
			return pos;
}
#endif