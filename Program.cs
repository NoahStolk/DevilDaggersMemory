using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

const int ddstatsMarkerOffset = 2259936;

for (; ; )
{
	Process? process = Process.GetProcessesByName("dd").FirstOrDefault(p => p.MainWindowTitle == "Devil Daggers");
	if (process?.MainModule == null)
		continue;

	nint processAddress = OpenProcess(0x0010, 1, (uint)process.Id);
	nint baseAddress = (nint)ReadLong(processAddress, process.MainModule.BaseAddress, ddstatsMarkerOffset);

	// Full list of offsets: https://github.com/NoahStolk/DevilDaggersCustomLeaderboards/blob/master/DOCUMENTATION.md
	Console.WriteLine($"Player ID: {ReadInt(processAddress, baseAddress, 16)}");
	Console.WriteLine($"Username: {ReadString(processAddress, baseAddress, 20, 32)}");
	Console.WriteLine($"Timer: {ReadFloat(processAddress, baseAddress, 52):0.0000}");
	Console.WriteLine($"Homing: {ReadInt(processAddress, baseAddress, 80)}");

	Thread.Sleep(150);
	Console.Clear();
}

static T Read<T>(nint processAddress, nint baseAddress, int offset, uint sizeOfT, Func<byte[], T> converter)
{
	byte[] bytes = new byte[sizeOfT];
	ReadProcessMemory(processAddress, baseAddress + offset, bytes, sizeOfT, out _);
	return converter(bytes);
}

static float ReadFloat(nint processAddress, nint baseAddress, int offset) => Read(processAddress, baseAddress, offset, sizeof(float), (byteArray) => BitConverter.ToSingle(byteArray));
static int ReadInt(nint processAddress, nint baseAddress, int offset) => Read(processAddress, baseAddress, offset, sizeof(int), (byteArray) => BitConverter.ToInt32(byteArray));
static long ReadLong(nint processAddress, nint baseAddress, int offset) => Read(processAddress, baseAddress, offset, sizeof(long), (byteArray) => BitConverter.ToInt64(byteArray));
static string ReadString(nint processAddress, nint baseAddress, int offset, uint stringLength) => Read(processAddress, baseAddress, offset, stringLength, (byteArray) => Encoding.UTF8.GetString(byteArray[0..Array.IndexOf(byteArray, (byte)0)]));

[DllImport("kernel32.dll")]
static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

[DllImport("kernel32.dll")]
static extern bool ReadProcessMemory(nint hProcess, nint lpBaseAddress, [In, Out] byte[] buffer, uint size, out uint lpNumberOfBytesRead);
