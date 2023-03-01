using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MmoCommon;

namespace MmoConsole
{
	/// <summary>
	/// This is a cache for Worlds that have a unique name.
	/// </summary>
	public sealed class WorldCache
	{
		public static readonly WorldCache Instance = new WorldCache();

		private readonly Dictionary<string, World> dict;

		// Used to synchronize access to the cache.
		private readonly ReaderWriterLockSlim readWriteLock;

		// Prevents a default instance of the WorldCache class from being created. 
		private WorldCache()
		{
			this.dict = new Dictionary<string, World>();
			this.readWriteLock = new ReaderWriterLockSlim();
		}

		~WorldCache()
		{
			this.Dispose(false);
		}

		public void Clear()
		{
			if (readWriteLock.TryEnterWriteLock(Settings.MaxLockWaitTimeMilliseconds))
			{
				try
				{
					this.dict.Clear();
				}
				finally
				{
					readWriteLock.ExitWriteLock();
				}

			}
		}

		public bool TryCreate(string name, BoundingBox boundingBox, Vector tileDimensions, out World world)
		{
			if (readWriteLock.TryEnterWriteLock(Settings.MaxLockWaitTimeMilliseconds))
			{
				try
				{
					if (this.dict.TryGetValue(name, out world))
					{
						return false;
					}

					world = new World(name, boundingBox, tileDimensions);
					this.dict.Add(name, world);
				}
				finally
				{
					readWriteLock.ExitWriteLock();
				}
				return true;
			}
			world = null;
			return false;
		}

		public bool TryGet(string name, out World world)
		{
			if (readWriteLock.TryEnterWriteLock(Settings.MaxLockWaitTimeMilliseconds))
			{
				try
				{
					this.dict.TryGetValue(name, out world);
				}
				finally
				{
					readWriteLock.ExitWriteLock();
				}
				return true;
			}
			world = null;
			return false;
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (World world in this.dict.Values)
				{
					world.Dispose();
				}
			}

			this.readWriteLock.Dispose();
		}
	}
}
