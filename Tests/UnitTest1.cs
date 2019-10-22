using NickStrupat;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;

namespace Tests
{
	public class UnitTest1
	{
		[Fact]
		public unsafe void Test1()
		{
			using var alignedMemoryManager = new AlignedMemoryManager<Byte>(CacheLine.Size, Environment.SystemPageSize);
			var memory = alignedMemoryManager.Memory;
			Assert.Equal(Environment.SystemPageSize, memory.Length);
			var ptr = Unsafe.AsPointer(ref MemoryMarshal.GetReference(memory.Span));
			Assert.Equal(0, ((IntPtr)ptr).ToInt64() % CacheLine.Size);
		}
	}
}
