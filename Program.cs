using DevilDaggersMemory;
using DevilDaggersMemory.Data;
using System;
using System.Diagnostics;
using System.Linq;

const int heroAddressOffset = 0x258BA8;
const int hpThornAddressOffset = 0x251350;

for (; ; )
{
	Process? process = Process.GetProcessesByName("dd").FirstOrDefault(p => p.MainWindowTitle == "Devil Daggers");
	if (process?.MainModule == null)
		continue;

	nint processAddress = Native.OpenProcess(0xFFFF, 1, (uint)process.Id);
	
	nint heroAddress = (nint)Native.ReadLong(processAddress, process.MainModule.BaseAddress, heroAddressOffset);
	byte[] heroBytes = Native.ReadBytes(processAddress, heroAddress, 0, Hero.Size);
	Hero hero = new(heroBytes);
	// Console.WriteLine(hero);

	nint thornAddress = (nint)Native.ReadLong(processAddress, process.MainModule.BaseAddress, hpThornAddressOffset);
	int thornCount = 5;//hero.Thorn; // TODO: Find actual count from internal list.
	for (int i = 0; i < thornCount; i++)
	{
		int offset = 0x4ED1D0 + i * 0x8F0;
		// byte[] thornBytes = Native.ReadBytes(processAddress, thornAddress, offset, Thorn.Size);
		// Thorn thorn = new(thornBytes);
		//Console.WriteLine(thorn);
		
		float angle = i * 360f / thornCount;
		float radians = angle * MathF.PI / 180f + hero.Timer * 3;
		Native.WriteFloat(processAddress, thornAddress, offset + 16, MathF.Sin(radians) * 16);
		Native.WriteFloat(processAddress, thornAddress, offset + 20, 1.5f + MathF.Sin(hero.Timer * 16 + i * 3) * 3);
		Native.WriteFloat(processAddress, thornAddress, offset + 24, MathF.Cos(radians) * 8);
	}

	//Console.Clear();
}

// thorn struct:
// float32: timer since emerge
// int32: hp
