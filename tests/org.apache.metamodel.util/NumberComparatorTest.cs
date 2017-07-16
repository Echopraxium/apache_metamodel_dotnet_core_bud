using System;
using System.Collections.Generic;
using Xunit;

namespace org.apache.metamodel.util
{
	public class AggregateBuilderTest
	{
		[Fact]
		public void TestCompareSimple() {
			IComparer<object> comparer = NumberComparator.getComparator();
			var result = comparer.Compare(1, 2);
			Assert.Equal(1, result);
		}
	}
}