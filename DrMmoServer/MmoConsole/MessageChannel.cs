using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MmoConsole
{
	public class MessageChannel<T>: IDisposable
	{
		private readonly object subscriberLock = new object();

		public bool HasSubscriptions => this._subscribers != null;

		//
		// Summary:
		//     Number of subscribers
		public int NumSubscribers
		{
			get
			{
				if (this._subscribers != null)
				{
					return this._subscribers.GetInvocationList().Length;
				}

				return 0;
			}
		}

		private event Action<T> _subscribers;

		//
		// Summary:
		//     ExitGames.Concurrency.Channels.ISubscriber`1.Subscribe(ExitGames.Concurrency.Fibers.IFiber,System.Action{`0})
		//
		// Parameters:
		//   fiber:
		//
		//   receive:
		public IDisposable Subscribe(IFiber fiber, Action<T> receive)
		{
			return SubscribeOnProducerThreads(new ChannelSubscription<T>(fiber, receive));
		}

		//
		// Summary:
		//     ExitGames.Concurrency.Channels.ISubscriber`1.SubscribeToBatch(ExitGames.Concurrency.Fibers.IFiber,System.Action{System.Collections.Generic.IList{`0}},System.Int32)
		//
		// Parameters:
		//   fiber:
		//
		//   receive:
		//
		//   intervalInMs:
		public IDisposable SubscribeToBatch(IFiber fiber, Action<IList<T>> receive, int intervalInMs)
		{
			return SubscribeOnProducerThreads(new BatchSubscriber<T>(fiber, receive, intervalInMs));
		}

		//
		// Summary:
		//     ExitGames.Concurrency.Channels.ISubscriber`1.SubscribeToKeyedBatch``1(ExitGames.Concurrency.Fibers.IFiber,System.Converter{`0,``0},System.Action{System.Collections.Generic.IDictionary{``0,`0}},System.Int32)
		//
		// Parameters:
		//   fiber:
		//
		//   keyResolver:
		//
		//   receive:
		//
		//   intervalInMs:
		//
		// Type parameters:
		//   K:
		public IDisposable SubscribeToKeyedBatch<K>(IFiber fiber, Converter<T, K> keyResolver, Action<IDictionary<K, T>> receive, int intervalInMs)
		{
			return SubscribeOnProducerThreads(new KeyedBatchSubscriber<K, T>(keyResolver, receive, fiber, intervalInMs));
		}

		//
		// Summary:
		//     Subscription that delivers the latest message to the consuming thread. If a newer
		//     message arrives before the consuming thread has a chance to process the message,
		//     the pending message is replaced by the newer message. The old message is discarded.
		//
		// Parameters:
		//   fiber:
		//
		//   receive:
		//
		//   intervalInMs:
		public IDisposable SubscribeToLast(IFiber fiber, Action<T> receive, int intervalInMs)
		{
			return SubscribeOnProducerThreads(new LastSubscriber<T>(receive, fiber, intervalInMs));
		}

		//
		// Summary:
		//     Subscribes to actions on producer threads. Subscriber could be called from multiple
		//     threads.
		//
		// Parameters:
		//   subscriber:
		public IDisposable SubscribeOnProducerThreads(IProducerThreadSubscriber<T> subscriber)
		{
			return SubscribeOnProducerThreads(subscriber.ReceiveOnProducerThread, subscriber.Subscriptions);
		}

		//
		// Summary:
		//     Subscribes an action to be executed for every action posted to the channel. Action
		//     should be thread safe. Action may be invoked on multiple threads.
		//
		// Parameters:
		//   subscriber:
		//
		//   subscriptions:
		private IDisposable SubscribeOnProducerThreads(Action<T> subscriber, ISubscriptionRegistry subscriptions)
		{
			lock (subscriberLock)
			{
				_subscribers += subscriber;
			}

			Unsubscriber<T> unsubscriber = new Unsubscriber<T>(subscriber, this, subscriptions);
			subscriptions.RegisterSubscription(unsubscriber);
			return unsubscriber;
		}

		internal void Unsubscribe(Action<T> toUnsubscribe)
		{
			lock (subscriberLock)
			{
				_subscribers -= toUnsubscribe;
			}
		}

		//
		// Summary:
		//     ExitGames.Concurrency.Channels.IPublisher`1.Publish(`0)
		//
		// Parameters:
		//   msg:
		public bool Publish(T msg)
		{
			Action<T> subscribers = this._subscribers;
			if (subscribers != null)
			{
				subscribers(msg);
				return true;
			}

			return false;
		}

		//
		// Summary:
		//     Remove all subscribers.
		public void ClearSubscribers()
		{
			this._subscribers = null;
		}
	}

	public class MessageChannelSubscription: IDisposable
	{
		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
