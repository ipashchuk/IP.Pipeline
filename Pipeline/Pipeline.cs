using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Pipeline
{
	internal class InternalPipeline<T> : IPipelineStep<T>
	{
		private readonly IEnumerable<IPipelineStep<T>> _pipelineSteps;

		public InternalPipeline(params IPipelineStep<T>[] pipelineSteps)
		{
			Contract.Requires<ArgumentNullException>(pipelineSteps != null);

			_pipelineSteps = pipelineSteps.ToArray();
		}

		public InternalPipeline(params Func<T, T>[] pipelineSteps)
			: this(pipelineSteps.Select(pipelineStep => new PipelineStep<T>(pipelineStep)))
		{
		}

		public InternalPipeline(params Func<T, PipelineContinuation<T>>[] pipelineSteps)
			: this(pipelineSteps.Select(pipelineStep => new PipelineStep<T>(pipelineStep)))
		{
		}

		public InternalPipeline(IEnumerable<IPipelineStep<T>> pipelineSteps)
		{
			Contract.Requires<ArgumentNullException>(pipelineSteps != null);

			_pipelineSteps = pipelineSteps.ToArray();
		}

		public T Process(T input)
		{
			return ((IPipelineStep<T>)this).Process(input).Result;
		}

		PipelineContinuation<T> IPipelineStep<T, T>.Process(T input)
		{
			if (!_pipelineSteps.Any())
				return new PipelineContinuation<T>(input);

			var firstStepResult = _pipelineSteps.First().Process(input);

			return _pipelineSteps.Skip(1).Aggregate(firstStepResult, (ouputFromPreviousStep, nextStep) => ouputFromPreviousStep.Then(nextStep));
		}
	}

	internal class InternalPipeline<T1, T2> : IPipelineStep<T1, T2>
	{
		private readonly IPipelineStep<T1> _stage1;
		private readonly IPipelineStep<T1, T2> _transitionToStage2;
		private readonly IPipelineStep<T2> _stage2;

		internal InternalPipeline(IPipelineStep<T1> stage1, IPipelineStep<T1, T2> transitionToStage2, IPipelineStep<T2> stage2)
		{
			Contract.Requires<ArgumentNullException>(stage1 != null);
			Contract.Requires<ArgumentNullException>(transitionToStage2 != null);
			Contract.Requires<ArgumentNullException>(stage2 != null);

			_stage1 = stage1;
			_transitionToStage2 = transitionToStage2;
			_stage2 = stage2;
		}

		PipelineContinuation<T2> IPipelineStep<T1, T2>.Process(T1 input)
		{
			return _stage1.Process(input).Then(_transitionToStage2).Then(_stage2);
		}
	}

	internal class InternalPipeline<T1, T2, T3> : InternalPipeline<T1, T2>, IPipelineStep<T1, T3>
	{
		private readonly IPipelineStep<T2, T3> _transitionToStage3;
		private readonly IPipelineStep<T3> _stage3;

		internal InternalPipeline(IPipelineStep<T1> stage1, IPipelineStep<T1, T2> transitionToStage2, IPipelineStep<T2> stage2, IPipelineStep<T2, T3> transitionToStage3, IPipelineStep<T3> stage3)
			: base(stage1, transitionToStage2, stage2)
		{
			Contract.Requires<ArgumentNullException>(transitionToStage3 != null);
			Contract.Requires<ArgumentNullException>(stage3 != null);

			_transitionToStage3 = transitionToStage3;
			_stage3 = stage3;
		}

		PipelineContinuation<T3> IPipelineStep<T1, T3>.Process(T1 input)
		{
			return ((IPipelineStep<T1, T2>)this).Process(input).Then(_transitionToStage3).Then(_stage3);
		}
	}
}