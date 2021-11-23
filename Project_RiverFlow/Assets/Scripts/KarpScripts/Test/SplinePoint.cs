using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SplinePoint
{
	[Header("Data")]
	public Vector3 pos;

	[Header("Tengente")]
	public Vector3 tangDir;
	public float tangStrenght;

	#region Constructor
	public SplinePoint(Vector3 pos)
	{
		this.pos = pos;
		this.tangDir = Vector3.right;
		this.tangStrenght = 0.5f;
	}
	public SplinePoint(Vector2 pos)
	{
		this.pos = pos;
		this.tangDir = Vector3.right;
		this.tangStrenght = 0.5f;
	}
	public SplinePoint(Vector3 pos, Vector3 tangent)
	{
		this.pos = pos;
		this.tangDir = tangent.normalized;
		this.tangStrenght = tangent.magnitude * 0.5f;
	}
	public SplinePoint(Vector3 pos, Vector3 tangent, float tangentMagnitude)
	{
		this.pos = pos;
		this.tangDir = tangent.normalized;
		this.tangStrenght = tangentMagnitude;
	}
	#endregion

	#region Operator
	public static SplinePoint operator +(SplinePoint a, SplinePoint b)
	{
		return new SplinePoint(a.pos + b.pos, (a.tangDir + b.tangDir).normalized, (a.tangStrenght + b.tangStrenght)*0.5f);
	}
	public static SplinePoint operator -(SplinePoint a, SplinePoint b)
	{
		return new SplinePoint(a.pos - b.pos, (a.tangDir - b.tangDir).normalized, Mathf.Abs((a.tangStrenght - b.tangStrenght) * 0.5f));
	}
	public static SplinePoint operator *(float b, SplinePoint a)
	{
		return new SplinePoint(a.pos * b, a.tangDir, a.tangStrenght * b * 0.5f);
	}
	#endregion
}
