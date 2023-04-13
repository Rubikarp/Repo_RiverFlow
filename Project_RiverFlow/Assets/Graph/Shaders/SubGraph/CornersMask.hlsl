void CornersMask_float( in float2 uv, in uint4 bitmask, out float4 mask)
{
	mask = float4(1,1,1,1);
	
	uint right = 1; // 0001
	uint left = 2;  // 0010
	uint top = 4;   // 0100
	uint down = 8;  // 1000
	//Combine
	uint tl = top + left;    // 0110
	uint tr = top + right;   // 0101
	uint dl = down + left;   // 1010
	uint dr = down + right;  // 1001


	//Slit pixel in 9 blocs
	uv = frac(uv);
	uv *= 3;

	//Center
	if (uv.x>=1 && uv.x <=2 || uv.y>=1 && uv.y<=2)
		mask = 0;

	//Corner
	if (uv.y >= 2 && uv.x >= 2) mask = ( (bitmask & tr) != 0 )?0:1;
	if (uv.y >= 2 && uv.x <= 1) mask = ( (bitmask & tl) != 0 )?0:1;
	if (uv.y <= 1 && uv.x >= 2) mask = ( (bitmask & dr) != 0 )?0:1;
	if (uv.y <= 1 && uv.x <= 1) mask = ( (bitmask & dl) != 0 )?0:1;
}