using System;
using System.Buffers;

namespace NickStrupat
{
	/// <summary>
	/// A MemoryManager over a raw pointer
	/// </summary>
	/// <remarks>The pointer is assumed to be fully unmanaged, or externally pinned - no attempt will be made to pin this data</remarks>
	public sealed class AlignedMemoryManager<T> : MemoryManager<T>
		where T : unmanaged
	{
		private readonly Int32 length;
		private readonly IntPtr intPtr;

		/// <summary>
		/// Create a new AlignedMemoryManager instance at the given pointer and size
		/// </summary>
		/// <remarks>It is assumed that the span provided is already unmanaged or externally pinned</remarks>
		public unsafe AlignedMemoryManager(Int32 alignment, Int32 length)
		{
			this.length = length;
			intPtr = MarshalEx.AllocHGlobalAligned(length * sizeof(T), alignment);
		}

		/// <summary>
		/// Obtains a span that represents the region
		/// </summary>
		public override unsafe Span<T> GetSpan() => new Span<T>(intPtr.ToPointer(), length);

		/// <summary>
		/// Provides access to a pointer that represents the data (note: no actual pin occurs)
		/// </summary>
		public override unsafe MemoryHandle Pin(Int32 elementIndex = 0)
		{
			if (elementIndex < 0 || elementIndex >= length)
				throw new ArgumentOutOfRangeException(nameof(elementIndex));
			return new MemoryHandle((T*)intPtr.ToPointer() + elementIndex);
		}

		/// <summary>
		/// Has no effect
		/// </summary>
		public override void Unpin() { }

		/// <summary>
		/// Releases all resources associated with this object
		/// </summary>
		protected override void Dispose(Boolean disposing) => MarshalEx.FreeHGlobalAligned(intPtr);
	}
}