using System;

namespace DevilDaggersMemory.Data;

public struct Thorn
{
	public const int Size = 0x8F0;

	public int State; // 0 = idle, 1 = emerging, 2 = dead
	public bool IsEmerged; // This becomes false after death, then after 5 seconds it becomes true again.
	public float TimeSinceEmerge; // This resets to 0 after death, then after 5 seconds it goes back to the original value.
	public int Hp;
	public float X;
	public float Y;
	public float Z;

	public Thorn(byte[] bytes)
	{
		State = BitConverter.ToInt32(bytes, 0);
		IsEmerged = BitConverter.ToBoolean(bytes, 4);
		TimeSinceEmerge = BitConverter.ToSingle(bytes, 8);
		Hp = BitConverter.ToInt32(bytes, 12);
		X = BitConverter.ToSingle(bytes, 16);
		Y = BitConverter.ToSingle(bytes, 20);
		Z = BitConverter.ToSingle(bytes, 24);
	}

	public override string ToString()
	{
		return $"""
			State: {State}
			IsEmerged: {IsEmerged}
			TimeSinceEmerge: {TimeSinceEmerge}
			Hp: {Hp}
			X: {X}
			Y: {Y}
			Z: {Z}

			""";
	}
}
