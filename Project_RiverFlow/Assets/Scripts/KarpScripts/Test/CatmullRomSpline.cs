using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatmullRomSpline : MonoBehaviour
{

	[Range(0, 1)] public float alpha = 0.5f;
	int PointCount => transform.childCount;
	public int SegmentCount => PointCount - 3;
	Vector2 GetPoint(int i) => transform.GetChild(i).position;

	public int detail = 32;

	CatmullRomCurve GetCurve(int i)
	{
		return new CatmullRomCurve(GetPoint(i), GetPoint(i + 1), GetPoint(i + 2), GetPoint(i + 3), alpha);
	}

	void OnDrawGizmos()
	{
		for (int i = 0; i < SegmentCount; i++)
			DrawCurveSegment(GetCurve(i));
	}

	void DrawCurveSegment(CatmullRomCurve curve)
	{
		Vector2 prev = curve.p1;
		for (int i = 1; i < detail; i++)
		{
			float t = i / (detail - 1f);
			Vector2 pt = curve.GetPoint(t);
			Gizmos.DrawLine(prev, pt);
			prev = pt;
		}
	}
}
