namespace Pipeline
{
	public interface IPipelineStep<T> : IPipelineStep<T, T>
	{
	}

	public interface IPipelineStep<TIn, TOut>
	{
		PipelineContinuation<TOut> Process(TIn input);
	}
}