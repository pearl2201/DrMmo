using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MmoCommon;

namespace MmoConsole
{
	/// <summary>
	/// Copy of current Item's state
	/// </summary>
	public class ItemSnapshot
	{
		public ItemSnapshot(Item source, Vector position, Vector rotation, Region worldRegion, int propertiesRevision)
		{
			this.Source = source;
			this.Position = position;
			this.Rotation = rotation;
			this.PropertiesRevision = propertiesRevision;
		}

		public Item Source { get; private set; }
		public Vector Position { get; private set; }
		public Vector Rotation { get; private set; }
		public int PropertiesRevision { get; private set; }
	}

	public class Item
	{
		private readonly string id;

		private readonly Hashtable properties;

		private readonly byte type;

		private readonly World world;

		public string Id { get { return this.id; } }

		public MmoActorOperationHandler Owner { get; private set; }

		public Vector Rotation { get; set; }

		public Vector Position { get; set; }

		public byte Type { get { return this.type; } }

		public World World { get { return this.world; } }

		public int PropertiesRevision { get; set; }

		public Region CurrentWorldRegion { get; private set; }

		public Item()
		{

		}

		public async void UpdateInterestManagement()
		{
			// inform attached interst area and radar
			ItemPositionMessage message = this.GetPositionUpdateMessage(this.Position);
			this.positionUpdateChannel.Publish(message);

			// update subscriptions if region changed
			Region prevRegion = this.CurrentWorldRegion;
			Region newRegion = this.World.GetRegion(this.Position);

			if (newRegion != this.CurrentWorldRegion)
			{
				this.CurrentWorldRegion = newRegion;

				if (this.regionSubscription != null)
				{
					this.regionSubscription.Dispose();
				}

				var snapshot = this.GetItemSnapshot();
				var regMessage = new ItemRegionChangedMessage(prevRegion, newRegion, snapshot);

				if (prevRegion != null)
				{
					await prevRegion.ItemRegionChangedChannel.Writer.WriteAsync(regMessage);
				}
				if (newRegion != null)
				{
					await newRegion.ItemRegionChangedChannel.Writer.WriteAsync(regMessage);

					this.regionSubscription = new UnsubscriberCollection(
						this.EventChannel.Subscribe(this.Fiber, (m) => newRegion.ItemEventChannel.Publish(m)), // route events through region to interest area
						newRegion.RequestItemEnterChannel.Subscribe(this.Fiber, (m) => { m.InterestArea.OnItemEnter(this.GetItemSnapshot()); }), // region entered interest area fires message to let item notify interest area about enter
						newRegion.RequestItemExitChannel.Subscribe(this.Fiber, (m) => { m.InterestArea.OnItemExit(this); }) // region exited interest area fires message to let item notify interest area about exit
					);
				}

			}
		}

		/// <summary>
		/// Updates the Properties and increments the PropertiesRevision.
		/// </summary>
		public void SetProperties(Hashtable propertiesSet, ArrayList propertiesUnset)
		{
			if (propertiesSet != null)
			{
				foreach (DictionaryEntry entry in propertiesSet)
				{
					this.properties[entry.Key] = entry.Value;
				}
			}

			if (propertiesUnset != null)
			{
				foreach (object key in propertiesUnset)
				{
					this.properties.Remove(key);
				}
			}

			this.PropertiesRevision++;
		}

		/// <summary>
		/// Creates an ItemSnapshot with a snapshot of the current attributes.
		/// </summary>
		protected internal ItemSnapshot GetItemSnapshot()
		{
			return new ItemSnapshot(this, this.Position, this.Rotation, this.CurrentWorldRegion, this.PropertiesRevision);
		}

		/// <summary>
		/// Creates an ItemPositionMessage with the current position.
		/// </summary>
		protected ItemPositionMessage GetPositionUpdateMessage(Vector position)
		{
			return new ItemPositionMessage(this, position);
		}


		/// <summary>
		/// Publishes event ItemDestroyed in the Item.EventChannel.
		/// </summary>
		protected void OnDestroy()
		{
			var eventInstance = new ItemDestroyed { ItemId = this.Id };
			var eventData = new EventData((byte)EventCode.ItemDestroyed, eventInstance);
			var message = new ItemEventMessage(this, eventData, new SendParameters { ChannelId = Settings.ItemEventChannel });
			this.EventChannel.Publish(message);
		}

		/// <summary>
		/// Moves the item.
		/// </summary>
		public void Move(Vector position)
		{
			this.Position = position;
			this.UpdateInterestManagement();
		}

		/// <summary>
		/// Spawns the item.
		/// </summary>
		public void Spawn(Vector position)
		{
			this.Position = position;
			this.UpdateInterestManagement();
		}

		/// <summary>
		/// Checks wheter the actor is allowed to change the item.
		/// </summary>
		public bool GrantWriteAccess(MmoActorOperationHandler actor)
		{
			return this.Owner == actor;
		}
	}
}
