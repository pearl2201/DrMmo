using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MmoConsole
{
	/// <summary>
	/// A cache for items. Each World has one item cache.
	/// </summary>
	/// <remarks>
	/// It uses an ReaderWriterLockSlim to ensure thread safety.
	/// All members are thread safe.
	/// </remarks>
	public class ItemCache : IDisposable
	{
		private readonly Dictionary<string, Item> items = new Dictionary<string, Item>();

		private readonly int maxLockMilliseconds;

		private readonly ReaderWriterLockSlim readerWriterLock;

		public ItemCache()
		{
			this.maxLockMilliseconds = Settings.MaxLockWaitTimeMilliseconds;
			this.readerWriterLock = new ReaderWriterLockSlim();
		}

		~ItemCache()
		{
			this.Dispose(false);
		}

		public bool AddItem(Item item)
		{

			if (readerWriterLock.TryEnterWriteLock(maxLockMilliseconds))
			{
				try
				{
					if (this.items.ContainsKey(item.Id))
					{
						return false;
					}

					this.items.Add(item.Id, item);
				}
				finally
				{
					readerWriterLock.ExitWriteLock();
				}
				return true;
			}
			else
			{
				return false;
			}

		}

		public bool RemoveItem(string itemId)
		{
			if (readerWriterLock.TryEnterWriteLock(maxLockMilliseconds))
			{
				try
				{
					this.items.Remove(itemId);
				}
				finally
				{
					readerWriterLock.ExitWriteLock();
				}
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool TryGetItem(string itemId, out Item item)
		{
			if (readerWriterLock.TryEnterWriteLock(maxLockMilliseconds))
			{
				try
				{
					return this.items.TryGetValue(itemId, out item);
				}
				finally
				{
					readerWriterLock.ExitWriteLock();
				}
				return true;
			}
			else
			{
				item = null;
				return false;
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.readerWriterLock.Dispose();
				this.items.Clear();
			}

		}
	}
}
