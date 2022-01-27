using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

[System.Serializable]
public struct RiverPoint
{
	[Header("Data")]
	public Vector3 pos;
	[ColorUsage(true,false)] public Color color;
	[Range(0f,2f)] public float thickness;
	public float lake;

	/*//=> Sub Struct ?
	[Header("Tangent")]
	public Vector3 tangDir;
	public float tangStrenght;
	*/

	#region Constructor
	//Vec3 version
	public RiverPoint(Vector3 pos)
	{
		this.pos = pos;
		this.color = Color.white;
		this.thickness = 1f;
		this.lake = 0.0f;
	}
	public RiverPoint(Vector3 pos, float thickness = 1f)
	{
		this.pos = pos;
		this.color = Color.white;
		this.thickness = thickness;
		this.lake = 0.0f;
	}
	public RiverPoint(Vector3 pos, Color col, float thickness = 1f, float lake = 0.0f)
	{
		this.pos = pos;
		this.color = col;
		this.thickness = thickness;
		this.lake = lake;
	}
	//Vec2 version
	public RiverPoint(Vector2 pos)
	{
		this.pos = pos;
		this.color = Color.white;
		this.thickness = 1f;
		this.lake = 0.0f;
	}
	public RiverPoint(Vector2 pos, float thickness = 1f)
	{
		this.pos = pos;
		this.color = Color.white;
		this.thickness = thickness;
		this.lake = 0.0f;
	}
	public RiverPoint(Vector2 pos, Color col, float thickness = 1f)
	{
		this.pos = pos;
		this.color = col;
		this.thickness = thickness;
		this.lake = 0.0f;
	}
	public RiverPoint(Vector2 pos, Color col, float thickness = 1f, float lake = 0.0f)
	{
		this.pos = pos;
		this.color = col;
		this.thickness = thickness;
		this.lake = lake;
	}
	#endregion

	public PolylinePoint ToPolyLine()
    {
		return new PolylinePoint(this.pos, this.color, this.thickness);
    }
	public static RiverPoint Lerp(RiverPoint a, RiverPoint b, float t)
	{
		return new RiverPoint(	Vector3.Lerp(a.pos,b.pos, EaseInOutSine(t)),
								Color.Lerp(a.color, b.color, EaseInOutSine(t)),
								Mathf.Lerp(a.thickness, b.thickness, EaseInOutSine(t)),
								Mathf.Lerp(a.lake, b.lake, EaseInOutSine(t)));
	}
	private static float EaseInOutSine(float t)
	{
		return -(Mathf.Cos(Mathf.PI * t) - 1.0f) / 2.0f;
	}
}
