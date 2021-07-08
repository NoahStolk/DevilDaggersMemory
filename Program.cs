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

	IntPtr processAddress = NativeMethods.OpenProcess(0x0010, 1, (uint)process.Id);
	IntPtr baseAddress = new(ReadLong(processAddress, new IntPtr(process.MainModule.BaseAddress.ToInt64()), ddstatsMarkerOffset));

	// Full list of offsets: https://github.com/NoahStolk/DevilDaggersCustomLeaderboards/blob/master/DOCUMENTATION.md
	Console.WriteLine($"Player ID: {ReadInt(processAddress, baseAddress, 16)}");
	Console.WriteLine($"Username: {ReadString(processAddress, baseAddress, 20, 32)}");
	Console.WriteLine($"Timer: {ReadFloat(processAddress, baseAddress, 52):0.0000}");
	Console.WriteLine($"Homing: {ReadInt(processAddress, baseAddress, 80)}");

	Thread.Sleep(150);
	Console.Clear();
}

static T Read<T>(IntPtr processAddress, IntPtr baseAddress, int offset, uint sizeOfT, Func<byte[], T> converter)
{
	byte[] bytes = new byte[sizeOfT];
	NativeMethods.ReadProcessMemory(processAddress, baseAddress + offset, bytes, sizeOfT, out _);
	return converter(bytes);
}

static float ReadFloat(IntPtr processAddress, IntPtr baseAddress, int offset) => Read(processAddress, baseAddress, offset, sizeof(float), (byteArray) => BitConverter.ToSingle(byteArray));
static int ReadInt(IntPtr processAddress, IntPtr baseAddress, int offset) => Read(processAddress, baseAddress, offset, sizeof(int), (byteArray) => BitConverter.ToInt32(byteArray));
static long ReadLong(IntPtr processAddress, IntPtr baseAddress, int offset) => Read(processAddress, baseAddress, offset, sizeof(long), (byteArray) => BitConverter.ToInt64(byteArray));
static string ReadString(IntPtr processAddress, IntPtr baseAddress, int offset, uint stringLength) => Read(processAddress, baseAddress, offset, stringLength, (byteArray) => Encoding.UTF8.GetString(byteArray[0..Array.IndexOf(byteArray, (byte)0)]));

static class NativeMethods
{
	[DllImport("kernel32.dll")]
	internal static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

	[DllImport("kernel32.dll")]
	internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, uint size, out uint lpNumberOfBytesRead);
}
