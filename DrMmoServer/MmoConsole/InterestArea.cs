using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace MmoConsole
{
	public class InterestArea
	{
		public InterestArea()
		{

		}

		protected virtual void OnRegionEnter(Region region)
		{
			var ts = new CancellationTokenSource();
			CancellationToken ct = ts.Token;
			var subscription = Task.Factory.StartNew(async () =>
			{
				bool isContinue = true;
				while (await region.ItemRegionChangedChannel.Reader.WaitToReadAsync() && isContinue)
				{
					while (region.ItemRegionChangedChannel.Reader.TryRead(out ItemRegionChangedMessage message) && isContinue)
					{
						if (ct.IsCancellationRequested)
						{
							isContinue = false;
							break;
						}
					}
				}
			});
		}
	}
}
