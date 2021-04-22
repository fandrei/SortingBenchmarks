using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingBenchmarks
{
	class Sorting
	{
		public static void QuickSort<T>(T[] vals, IComparer<T> comparer, bool parallel)
		{
			QuickSort(vals, comparer, parallel, 0, vals.Length - 1);
		}

		public static void QuickSort<T>(T[] vals, IComparer<T> comparer, bool parallel, int left, int right)
		{
			if (right <= left)
				return;
			if (right - left < Threshold)
			{
				Array.Sort(vals, left, right - left + 1, comparer);
				return;
			}

			var i = left;
			var j = right;
			var mid = (int)(((long)left + right) / 2);
			var pivot = vals[mid];

			while (true)
			{
				while (comparer.Compare(vals[i], pivot) < 0)
				{
					i++;
				}

				while (comparer.Compare(pivot, vals[j]) < 0)
				{
					j--;
				}

				if (i >= j)
					break;

				vals.Swap(i, j);
				i++;
				j--;
			}

			if (parallel)
			{
				var task = Task.Run(() => QuickSort(vals, comparer, true, i, right));
				QuickSort(vals, comparer, true, left, j);
				task.Wait();
			}
			else
			{
				QuickSort(vals, comparer, false, left, j);
				QuickSort(vals, comparer, false, i, right);
			}
		}

		const int Threshold = 0x800;
	}
}
