using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DevilDaggersMemory;

public static class Native
{
	[DllImport("kernel32.dll")]
	internal static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

	[DllImport("kernel32.dll")]
	private static extern bool ReadProcessMemory(nint hProcess, nint lpBaseAddress, [In, Out] byte[] buffer, uint size, out uint lpNumberOfBytesRead);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool WriteProcessMemory(nint hProcess, nint lpBaseAddress, [In, Out] byte[] buffer, uint size, out uint lpNumberOfBytesWritten);

	public static byte[] ReadBytes(IntPtr processAddress, nint baseAddress, int offset, uint length)
	{
		byte[] bytes = new byte[length];
		if (!ReadProcessMemory(processAddress, baseAddress + offset, bytes, length, out _))
			Console.WriteLine("Failed to read memory.");

		return bytes;
	}

	private static T Read<T>(nint processAddress, nint baseAddress, int offset, uint sizeOfT, Func<byte[], T> converter)
	{
		byte[] bytes = ReadBytes(processAddress, baseAddress, offset, sizeOfT);
		return converter(bytes);
	}

	private static void Write(nint processAddress, nint baseAddress, int offset, byte[] bytes)
	{
		if (!WriteProcessMemory(processAddress, baseAddress + offset, bytes, (uint)bytes.Length, out _))
			Console.WriteLine("Failed to write to memory.");
	}

	public static float ReadFloat(nint processAddress, nint baseAddress, int offset)
	{
		return Read(processAddress, baseAddress, offset, sizeof(float), byteArray => BitConverter.ToSingle(byteArray));
	}

	public static ushort ReadUShort(nint processAddress, nint baseAddress, int offset)
	{
		return Read(processAddress, baseAddress, offset, sizeof(ushort), byteArray => BitConverter.ToUInt16(byteArray));
	}

	public static int ReadInt(nint processAddress, nint baseAddress, int offset)
	{
		return Read(processAddress, baseAddress, offset, sizeof(int), byteArray => BitConverter.ToInt32(byteArray));
	}

	public static long ReadLong(nint processAddress, nint baseAddress, int offset)
	{
		return Read(processAddress, baseAddress, offset, sizeof(long), byteArray => BitConverter.ToInt64(byteArray));
	}

	public static string ReadString(nint processAddress, nint baseAddress, int offset, uint stringLength)
	{
		return Read(processAddress, baseAddress, offset, stringLength, byteArray => Encoding.UTF8.GetString(byteArray[0..Array.IndexOf(byteArray, (byte)0)]));
	}

	public static void WriteUShort(nint processAddress, nint baseAddress, int offset, ushort value)
	{
		Write(processAddress, baseAddress, offset, BitConverter.GetBytes(value));
	}

	public static void WriteFloat(nint processAddress, nint baseAddress, int offset, float value)
	{
		Write(processAddress, baseAddress, offset, BitConverter.GetBytes(value));
	}

	public static void WriteInt(nint processAddress, nint baseAddress, int offset, int value)
	{
		Write(processAddress, baseAddress, offset, BitConverter.GetBytes(value));
	}
}
