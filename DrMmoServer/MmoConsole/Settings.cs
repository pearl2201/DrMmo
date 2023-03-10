using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MmoConsole
{
	public static class Settings
	{
		static Settings()
		{
			ItemAutoUnsubcribeDelay = 5000;

			// just the radar
			RadarEventChannel = 0;

			// movement etc
			ItemEventChannel = 0;

			MaxLockWaitTimeMilliseconds = 1000;
			RadarUpdateInterval = 10000;

			DiagnosticsEventChannel = 0;
		}

		/// <summary>
		/// This property determines which enet channel to use when sending event CounterDataEvent to the client.
		/// Default value is #2.
		/// </summary>
		public static byte DiagnosticsEventChannel { get; set; }



		/// <summary>
		/// Maximum unsubscribe delay of items that leave the outer view threshold.
		/// Default value is 5 seconds.
		/// </summary>
		public static int ItemAutoUnsubcribeDelay { get; set; }

		/// <summary>
		/// The enet channel used for events that are published with the Item.EventChannel.
		/// Default value is 0.
		/// </summary>
		public static byte ItemEventChannel { get; set; }

		/// <summary>
		/// Maxium lock wait time for the lock protected dictionaries WorldCache.
		/// Default is 1 second.
		/// </summary>
		public static int MaxLockWaitTimeMilliseconds { get; set; }

		/// <summary>
		/// The enet channel used for event that are published with the Radar.
		/// Default is 2.
		/// </summary>
		public static byte RadarEventChannel { get; set; }

		/// <summary>
		/// The interval the Radar uses to publish position changes with a RadarUpdate event.
		/// Default is 10 seconds.
		/// </summary>
		public static int RadarUpdateInterval { get; set; }
	}
}
