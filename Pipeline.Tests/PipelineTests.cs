using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pipeline.Tests
{
	[TestClass]
	public class PipelineTests
	{
		[TestMethod]
		public void ProcessWithNoProcessors()
		{
			var pipeline = new InternalPipeline<int>(new Func<int, int>[] { });
			var output = pipeline.Process(1);

			output.Should().Be(1);
		}

		[TestMethod]
		public void ProcessWithTwoFuncProcessors()
		{
			int input = 2;

			var pipeline = new InternalPipeline<int>(x => x + 1, x => x * 2);

			var output = pipeline.Process(input);
			output.Should().Be(6);
		}

		[TestMethod]
		public void ProcessWithTwoProcessors()
		{
			int input = 2;

			var pipeline = new InternalPipeline<int>(new PipelineStep<int>(x => x + 1), new PipelineStep<int>(x => x * 2));

			var output = pipeline.Process(input);
			output.Should().Be(6);
		}

		[TestMethod]
		public void ProcessInParallel()
		{
			var pipeline = new InternalPipeline<int>(new PipelineStep<int>(x => x + 1), new PipelineStep<int>(x => x * 2));

			var output = Enumerable.Range(0, 10).AsParallel().Select(i => new { Input = i, Result = pipeline.Process(i) }).ToArray();

			foreach (var item in output)
			{
				item.Result.Should().Be((item.Input + 1) * 2);
			}
		}
	}
}