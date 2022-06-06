using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.EventBroker
{
	public class EventBrokerComponent
	{
		private static Dictionary<Type, List<Delegate>> subscriptions;

		public EventBrokerComponent()
		{
			if (subscriptions == null)
			{
				subscriptions = new Dictionary<Type, List<Delegate>>();
			}
		}

		public void Publish<T>(object sender, T payload)
		{
			if (sender == null || payload == null) return;
			if (!subscriptions.ContainsKey(typeof(T))) return;

			List<Delegate> subs = subscriptions[typeof(T)];

			if (subs == null || subs.Count == 0) return;

			BrokerEvent<T> brokerEvent =  new BrokerEvent<T>(sender, payload);

			subs.ForEach(sub => sub.DynamicInvoke(brokerEvent));
		}

		public void Subscribe<T>(Action<BrokerEvent<T>> subscription)
		{
			List<Delegate> subs = subscriptions.ContainsKey(typeof(T))
					? subscriptions[typeof(T)]
					: new List<Delegate>();
			if (!subs.Contains(subscription))
			{
				subs.Add(subscription);
			}

			subscriptions[typeof(T)] = subs;
		}

		public void Unsubscribe<T>(Action<BrokerEvent<T>> subscription)
		{
			if (!subscriptions.ContainsKey(typeof(T))) return;

			List<Delegate> subs = subscriptions[typeof(T)];
			subs.Remove(subscription);

			if (subs.Count == 0)
			{
				subscriptions.Remove(typeof(T));
			}
		}
	}
}