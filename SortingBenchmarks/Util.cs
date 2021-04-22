using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;

namespace SortingBenchmarks
{
	static class Util
	{
		public static string GenerateString(int maxChars = 0x80)
		{
			var buf = new StringBuilder();
			var count = RandomGen.Next(maxChars);

			for (var i = 0; i < count; i++)
			{
				var ch = (char)(0x21 + RandomGen.Next(0x7D - 0x21));
				buf.Append(ch);
			}

			return buf.ToString();
		}

		static readonly Random RandomGen = new(1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Swap<T>(this T[] vals, int i, int j)
		{
			var tmp = vals[i];
			vals[i] = vals[j];
			vals[j] = tmp;
		}

		public static void CollectAll()
		{
			GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
			GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
		}
	}
}
