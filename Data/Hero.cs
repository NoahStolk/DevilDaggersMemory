using System;

namespace DevilDaggersMemory.Data;

public struct Hero
{
	public const int Size = 868;
	
	public int PlayerId;
	public string PlayerName;
	public float Timer;
	public int Gems;
	public int LevelGems;
	public int TotalGems;
	public ushort Skull1;
	public ushort Skull2;
	public ushort Skull3;
	public ushort Spiderling;
	public ushort Skull4;
	public ushort Squid1;
	public ushort Squid2;
	public ushort Squid3;
	public ushort Centipede;
	public ushort Gigapede;
	public ushort Spider1;
	public ushort Spider2;
	public ushort Leviathan;
	public ushort Orb;
	public ushort Thorn;
	public ushort Ghostpede;
	public ushort SpiderEgg;
	public int DaggersFired;
	public int DaggersHit;

	public Hero(byte[] bytes)
	{
		PlayerId = BitConverter.ToInt32(bytes, 16);
		PlayerName = BitConverter.ToString(bytes, 20, 32);
		Timer = BitConverter.ToSingle(bytes, 52);
		Gems = BitConverter.ToInt32(bytes, 56);
		LevelGems = BitConverter.ToInt32(bytes, 76);
		TotalGems = BitConverter.ToInt32(bytes, 92);
		Skull1 = BitConverter.ToUInt16(bytes, 100);
		Skull2 = BitConverter.ToUInt16(bytes, 102);
		Skull3 = BitConverter.ToUInt16(bytes, 104);
		Spiderling = BitConverter.ToUInt16(bytes, 106);
		Skull4 = BitConverter.ToUInt16(bytes, 108);
		Squid1 = BitConverter.ToUInt16(bytes, 110);
		Squid2 = BitConverter.ToUInt16(bytes, 112);
		Squid3 = BitConverter.ToUInt16(bytes, 114);
		Centipede = BitConverter.ToUInt16(bytes, 116);
		Gigapede = BitConverter.ToUInt16(bytes, 118);
		Spider1 = BitConverter.ToUInt16(bytes, 120);
		Spider2 = BitConverter.ToUInt16(bytes, 122);
		Leviathan = BitConverter.ToUInt16(bytes, 124);
		Orb = BitConverter.ToUInt16(bytes, 126);
		Thorn = BitConverter.ToUInt16(bytes, 128);
		Ghostpede = BitConverter.ToUInt16(bytes, 130);
		SpiderEgg = BitConverter.ToUInt16(bytes, 132);
		DaggersFired = BitConverter.ToInt32(bytes, 860);
		DaggersHit = BitConverter.ToInt32(bytes, 864);
	}

	public override string ToString()
	{
		return $"""
			Player ID: {PlayerId}
			Player name: {PlayerName}
			Timer: {Timer}
			Gems: {Gems}
			Level gems: {LevelGems}
			Total gems: {TotalGems}
			Skull 1: {Skull1}
			Skull 2: {Skull2}
			Skull 3: {Skull3}
			Spiderling: {Spiderling}
			Skull 4: {Skull4}
			Squid 1: {Squid1}
			Squid 2: {Squid2}
			Squid 3: {Squid3}
			Centipede: {Centipede}
			Gigapede: {Gigapede}
			Spider 1: {Spider1}
			Spider 2: {Spider2}
			Leviathan: {Leviathan}
			Orb: {Orb}
			Thorn: {Thorn}
			Ghostpede: {Ghostpede}
			Spider egg: {SpiderEgg}
			Daggers fired: {DaggersFired}
			Daggers hit: {DaggersHit}
			""";
	}
}
