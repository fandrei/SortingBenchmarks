using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

using static SortingBenchmarks.Util;

namespace SortingBenchmarks
{
	public static class SortRunner
	{
		const int RepeatCount = 8;
		const int MaxItemsCount = 0x100_000;

		public static void Test()
		{
			{
				Console.WriteLine(new string('-', 40));
				Console.WriteLine("Int32");

				var comparer = new IntComparer();
				Random random = new(1);
				for (var itemsCount = 0x800; itemsCount <= MaxItemsCount; itemsCount *= 2)
				{
					var source = new int[itemsCount];
					for (var i = 0; i < itemsCount; i++)
					{
						source[i] = random.Next();
					}

					Test(source, comparer);
				}
			}

			{
				Console.WriteLine(new string('-', 40));
				Console.WriteLine("String");

				var comparer = StringComparer.Ordinal;
				for (var itemsCount = 0x800; itemsCount <= MaxItemsCount; itemsCount *= 2)
				{
					var source = new string[itemsCount];
					for (var i = 0; i < itemsCount; i++)
					{
						source[i] = GenerateString();
					}

					Test(source, comparer);
				}
			}

			Console.WriteLine(new string('-', 40));

			if (Debugger.IsAttached)
				Debugger.Break();
		}

		static void Test<T>(T[] source, IComparer<T> comparer)
		{
			Console.Write($"{source.Length.ToString("0,000,000", _numberFormat)}{new string(' ', 10)}");

			T[] expected = null;
			expected = RunTest("Array.Sort()", vals => Array.Sort(vals, comparer));

			//RunTest("QuickSort", vals => Sorting.QuickSort(vals, comparer, false));
			RunTest("Parallel QuickSort", vals => Sorting.QuickSort(vals, comparer, true));

			RunTest("Parallel OrderBy", vals =>
			{
				var res = vals.AsParallel().WithDegreeOfParallelism(12).OrderBy(cur => cur, comparer).ToArray();
				Array.Copy(res, vals, res.Length);
			});

			Console.WriteLine();

			T[] RunTest(string name, Action<T[]> sort)
			{
				try
				{
					var watch = new Stopwatch();

					T[] res = null;
					for (var i = 0; i < RepeatCount; i++)
					{
						CollectAll();
						var vals = source.ToArray();
						watch.Start();
						sort(vals);
						res = vals;
						watch.Stop();
					}

					Check(name, res);
					Console.Write($"{name}: {watch.Elapsed.TotalMilliseconds / RepeatCount:000.000}{new string(' ', 10)}");
					return res;
				}
				catch (Exception exc)
				{
					Console.WriteLine(exc);
					return null;
				}
				finally
				{
					CollectAll();
				}
			}

			void Check(string name, IList<T> vals)
			{
				for (var i = 0; i < vals.Count; i++)
				{
					if (expected != null && !vals[i].Equals(expected[i]))
						throw new ApplicationException($"\t{name}: validation failed at {i}");

					if (i > 1)
					{
						if (comparer.Compare(vals[i - 1], vals[i]) > 0)
							throw new ApplicationException($"\t{name}: validation failed at {i}");
					}
				}
			}
		}

		static readonly NumberFormatInfo _numberFormat = new()
		{
			NumberGroupSeparator = "'",
		};
	}
}
