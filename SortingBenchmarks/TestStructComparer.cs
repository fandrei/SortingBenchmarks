using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SortingBenchmarks
{
	class TestStructComparer : IComparer<TestStruct>
	{
		public int Compare(TestStruct x, TestStruct y)
		{
			return string.CompareOrdinal(x.Id, y.Id);
		}
	}
}
