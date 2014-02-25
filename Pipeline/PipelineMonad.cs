namespace Pipeline
{
	internal static class PipelineMonad
	{
		internal static PipelineContinuation<TOut> Then<T, TOut>(this PipelineContinuation<T> @this, IPipelineStep<T, TOut> nextStep)
		{
			switch (@this.Action)
			{
				case PipelineContinuationAction.Abort:
				case PipelineContinuationAction.Terminate:
					throw new PipelineAbortException() { Result = @this.Result };
				default:
					return nextStep.Process(@this.Result);
			}
		}

		internal static PipelineContinuation<T> Then<T>(this PipelineContinuation<T> @this, IPipelineStep<T> nextStep)
		{
			switch (@this.Action)
			{
				case PipelineContinuationAction.Abort:
					throw new PipelineAbortException() { Result = @this.Result };
				case PipelineContinuationAction.Terminate:
					return new PipelineContinuation<T>(PipelineContinuationAction.Terminate, @this.Result);
				default:
					return nextStep.Process(@this.Result);
			}
		}
	}
}