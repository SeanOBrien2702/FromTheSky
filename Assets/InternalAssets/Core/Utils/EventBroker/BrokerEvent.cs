using System;

namespace Core.EventBroker
{
	public class BrokerEvent<T>
	{
		public object Sender { get; private set; }
		public T Payload { get; private set; }
		public DateTime Timestamp { get; private set; }

		public BrokerEvent(object sender, T payload)
		{
			Sender = sender;
			Payload = payload;
			Timestamp = DateTime.UtcNow;
		}
	}
}