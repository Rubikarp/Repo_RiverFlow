using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CatmullRiverSegment
{
	public Vector3 previous, from, to, next;

	///https://en.wikipedia.org/wiki/Centripetal_Catmull%E2%80%93Rom_spline
	[Tooltip("0.0 for the uniform spline,\n 0.5 for the centripetal spline,\n 1.0 for the chordal spline")]
	public const float strength = 0.5f;

	public CatmullRiverSegment(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		(previous, from, to, next) = (p0, p1, p2, p3);
	}

	// Evaluates a point at the given t-value from 0 to 1
	public Vector3 GetPointPos(float t)
	{
		// calculate knots
		const float k0 = 0;
		float k1 = GetKnotInterval(previous, from);
		float k2 = GetKnotInterval(from, to) + k1;
		float k3 = GetKnotInterval(to, next) + k2;

		// evaluate the point
		float u = Mathf.LerpUnclamped(k1, k2, t);
		Vector3 A1 = Remap(k0, k1, previous, from, u);
		Vector3 A2 = Remap(k1, k2, from, to, u);
		Vector3 A3 = Remap(k2, k3, to, next, u);
		Vector3 B1 = Remap(k0, k2, A1, A2, u);
		Vector3 B2 = Remap(k1, k3, A2, A3, u);
		return Remap(k1, k2, B1, B2, u);
	}

	static Vector3 Remap(float a, float b, Vector3 c, Vector3 d, float u)
	{
		return Vector3.LerpUnclamped(c, d, (u - a) / (b - a));
	}

	float GetKnotInterval(Vector3 a, Vector3 b)
	{
		return Mathf.Pow(Vector3.SqrMagnitude(a - b), 0.5f * strength);
	}
}
