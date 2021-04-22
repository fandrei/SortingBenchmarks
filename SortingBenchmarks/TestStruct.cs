using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SortingBenchmarks
{
	class TestStruct
	{
		public TestStruct(string id)
		{
			Id = id;
			Val = id;
		}

		public string Id;
		public string Val;

		public override string ToString()
		{
			return Id;
		}

		public override bool Equals(object? obj)
		{
			var that = obj as TestStruct;
			if (that == null)
				return false;
			return Id.Equals(that.Id);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
