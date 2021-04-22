using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SortingBenchmarks
{
	class IntComparer : IComparer<int>
	{
		public int Compare(int x, int y)
		{
			return x.CompareTo(y);
		}
	}
}
