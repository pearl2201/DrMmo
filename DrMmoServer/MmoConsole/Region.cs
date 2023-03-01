using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MmoConsole
{
	/// <summary>
	/// Item notifies interest areas via regions this item exits and enters.
	/// </summary>
	public class ItemRegionChangedMessage
	{
		public ItemRegionChangedMessage(Region r0, Region r1, ItemSnapshot snaphot)
		{
			this.Region0 = r0;
			this.Region1 = r1;
			this.ItemSnapshot = snaphot;
		}
		public Region Region0 { get; private set; }
		public Region Region1 { get; private set; }
		public ItemSnapshot ItemSnapshot { get; private set; }
	};

	public class Region : IDisposable
	{

		public Channel<ItemRegionChangedMessage> ItemRegionChangedChannel { get; private set; }
		public Region(int x, int y)
		{
			this.X = x;
			this.Y = y;
			ItemRegionChangedChannel = Channel.CreateUnbounded<ItemRegionChangedMessage>(); 
		}

		public int X { get; private set; }

		// grid cell Y (debug only)
		public int Y { get; private set; }

		public void Dispose()
		{

		}
	}
}
