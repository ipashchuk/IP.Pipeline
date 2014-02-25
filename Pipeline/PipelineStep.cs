using System;

namespace Pipeline
{
	public class PipelineStep<T1, T2> : IPipelineStep<T1, T2>
	{
		private readonly Func<T1, PipelineContinuation<T2>> _pipelineStep;

		public PipelineStep(Func<T1, PipelineContinuation<T2>> pipelineStep)
		{
			_pipelineStep = pipelineStep;
		}

		public PipelineStep(Func<T1, T2> pipelineStep)
			: this(arg => new PipelineContinuation<T2>(pipelineStep(arg)))
		{ }

		public PipelineContinuation<T2> Process(T1 input)
		{
			return _pipelineStep(input);
		}
	}

	public class PipelineStep<T> : PipelineStep<T, T>, IPipelineStep<T>
	{
		public PipelineStep(Func<T, PipelineContinuation<T>> pipelineStep)
			: base(pipelineStep)
		{ }

		public PipelineStep(Func<T, T> pipelineStep)
			: base(pipelineStep)
		{
		}
	}
}