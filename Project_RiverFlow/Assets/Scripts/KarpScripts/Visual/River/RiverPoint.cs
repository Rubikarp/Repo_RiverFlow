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

	[Header("Tangent")]//=> Sub Struct ?
	public Vector3 tangDir;
	public float tangStrenght;

	#region Constructor
	//Vec3 version
	public RiverPoint(Vector3 pos)
	{
		this.pos = pos;
		this.color = Color.white;
		this.thickness = 1f;

		this.tangDir = Vector3.right;
		this.tangStrenght = 0.5f;
	}
	public RiverPoint(Vector3 pos, Vector3 tangent)
	{
		this.pos = pos;
		this.color = Color.white;
		this.thickness = 1f;

		this.tangDir = tangent.normalized;
		this.tangStrenght = 0.5f;
	}
	public RiverPoint(Vector3 pos, Vector3 tangent, float tangentMagnitude = 0.5f)
	{
		this.pos = pos;
		this.color = Color.white;
		this.thickness = 1f;

		this.tangDir = tangent.normalized;
		this.tangStrenght = tangentMagnitude;
	}
	public RiverPoint(Vector3 pos, Vector3 tangent, float tangentMagnitude = 0.5f, float thickness = 1f)
	{
		this.pos = pos;
		this.color = Color.white;
		this.thickness = thickness;

		this.tangDir = tangent.normalized;
		this.tangStrenght = tangentMagnitude;
	}
	public RiverPoint(Vector3 pos, Vector3 tangent, Color col, float tangentMagnitude = 0.5f, float thickness = 1f)
	{
		this.pos = pos;
		this.color = col;
		this.thickness = thickness;

		this.tangDir = tangent.normalized;
		this.tangStrenght = tangentMagnitude;
	}
	//Vec2 version
	public RiverPoint(Vector2 pos)
	{
		this.pos = pos;
		this.color = Color.white;
		this.thickness = 1f;

		this.tangDir = Vector3.right;
		this.tangStrenght = 0.5f;
	}
	public RiverPoint(Vector2 pos, Vector3 tangent)
	{
		this.pos = pos;
		this.color = Color.white;
		this.thickness = 1f;

		this.tangDir = tangent.normalized;
		this.tangStrenght = 0.5f;
	}
	public RiverPoint(Vector2 pos, Vector3 tangent, float tangentMagnitude = 0.5f)
	{
		this.pos = pos;
		this.color = Color.white;
		this.thickness = 1f;

		this.tangDir = tangent.normalized;
		this.tangStrenght = tangentMagnitude;
	}
	public RiverPoint(Vector2 pos, Vector3 tangent, float tangentMagnitude = 0.5f, float thickness = 1f)
	{
		this.pos = pos;
		this.color = Color.white;
		this.thickness = thickness;

		this.tangDir = tangent.normalized;
		this.tangStrenght = tangentMagnitude;
	}
	public RiverPoint(Vector2 pos, Vector3 tangent, Color col, float tangentMagnitude = 0.5f, float thickness = 1f)
	{
		this.pos = pos;
		this.color = col;
		this.thickness = thickness;

		this.tangDir = tangent.normalized;
		this.tangStrenght = tangentMagnitude;
	}
	#endregion

	public PolylinePoint ToPolyLine()
    {
		return new PolylinePoint(this.pos, this.color, this.thickness);
    }
	public static RiverPoint Lerp(RiverPoint a, RiverPoint b, float t)
	{
		return new RiverPoint(	Vector3.Lerp(a.pos,b.pos,t),
								Vector3.Lerp(a.tangDir, b.tangDir, t),
								Color.Lerp(a.color, b.color, t),
								Mathf.Lerp(a.tangStrenght,b.tangStrenght,t), 
								Mathf.Lerp(a.thickness, b.thickness, t)
								);
	}

}
