using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MmoCommon;

namespace MmoConsole
{
	public class Radar
	{

		private readonly Dictionary<Item, Vector> itemPositions;

		private readonly Dictionary<Item, IDisposable> itemSubscriptions;

		public Radar()
		{

		}

		public void AddItem(Item item, Vector position)
		{

		}

		private static object GetUpdateEvent(Item item, Vector position, bool remove)
		{
			return new RadarUpdate { ItemId = item.Id, ItemType = item.Type, Position = position, Remove = remove };
		}

		private void PublishAll(Peer receiver)
		{

		}

		private void PublishUpdate(Item item, Vector position, bool remove, bool unreliable)
		{

		}

		private void RemoveItem(ItemDisposedMessage message)
		{

		}

		private void UpdatePosition(ItemPositionMessage message)
		{
			Item item = message.Source;
			if (this.itemPositions.ContainsKey(item))
			{
				this.itemPositions[item] = message.Position;
				this.PublishUpdate(item, message.Position, false, true);
			}
		}
	}
}
