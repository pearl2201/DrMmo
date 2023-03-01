using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DarkRift.Server;

using MmoCommon;

namespace MmoConsole
{
	public class World: GridWorld
	{
		public readonly Radar Radar = new Radar();
		public World(string name, BoundingBox boundingBox, Vector tileDimensions)
		 : base(boundingBox, tileDimensions)
		{
			this.Name = name;
			this.ItemCache = new ItemCache();
		}

		public string Name { get; private set; }
		public ItemCache ItemCache { get; private set; }
	}
}
