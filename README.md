# AlignedMemoryManager
Derived from `MemoryManager<T>` to support aligned `Memory<T>`

## Usage

```csharp
using var amm = new AlignedMemoryManager(alignment: 64, length: 64);
var cacheLine = amm.Memory;
```