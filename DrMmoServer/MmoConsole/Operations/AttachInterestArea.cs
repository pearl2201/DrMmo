using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MmoConsole.Operations
{
    public struct AttachInterestArea : IDarkRiftSerializable
    {
        public byte InterestAreaId;

        public string ItemId;

        public AttachInterestArea(byte interestAreaId, string itemId)
        {
            InterestAreaId = interestAreaId;
            ItemId = itemId;
        }

        public void Deserialize(DeserializeEvent e)
        {
            InterestAreaId = e.Reader.ReadByte();
            ItemId = e.Reader.ReadString();

        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(InterestAreaId);
            e.Writer.Write(ItemId);
        }
    }
}
