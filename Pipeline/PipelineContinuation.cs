namespace Pipeline
{
	public struct PipelineContinuation<T>
	{
		private readonly PipelineContinuationAction _action;
		private readonly T _result;

		public PipelineContinuation(PipelineContinuationAction action, T result)
		{
			_action = action;
			_result = result;
		}

		public PipelineContinuation(T result)
			: this(PipelineContinuationAction.Continue, result)
		{
		}

		public PipelineContinuationAction Action
		{
			get
			{
				return _action;
			}
		}

		public T Result
		{
			get
			{
				return _result;
			}
		}
	}
}