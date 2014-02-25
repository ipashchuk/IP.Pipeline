using System;
using System.Runtime.Serialization;

namespace Pipeline
{
	public class PipelineAbortException : ApplicationException
	{
		public PipelineAbortException()
			: base()
		{
		}

		public PipelineAbortException(string message)
			: base(message)
		{
		}

		public PipelineAbortException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public PipelineAbortException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public object Result { get; set; }
	}
}