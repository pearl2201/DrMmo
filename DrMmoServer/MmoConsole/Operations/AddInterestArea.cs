using DarkRift;
using MmoCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MmoConsole.Operations
{
    public struct AddInterestArea : IDarkRiftSerializable
    {

        public byte InterestAreaId;
        public string ItemId;
        public Vector Position;
        public Vector ViewDistanceEnter;
        public Vector ViewDistanceExit;

        public AddInterestArea(byte interestAreaId, string itemId, Vector position, Vector viewDistanceEnter, Vector viewDistanceExit)
        {
            InterestAreaId = interestAreaId;
            ItemId = itemId;
            Position = position;
            ViewDistanceEnter = viewDistanceEnter;
            ViewDistanceExit = viewDistanceExit;
        }

        public void Deserialize(DeserializeEvent e)
        {
            InterestAreaId = e.Reader.ReadByte();
            ItemId = e.Reader.ReadString();
            Position = e.Reader.ReadSerializable<Vector>();
            ViewDistanceEnter = e.Reader.ReadSerializable<Vector>();
            ViewDistanceExit = e.Reader.ReadSerializable<Vector>();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(InterestAreaId);
            e.Writer.Write(ItemId);
            e.Writer.Write(Position);
            e.Writer.Write(ViewDistanceEnter);
            e.Writer.Write(ViewDistanceExit);
        }
    }

    public class AddInterestAreaResponse : IDarkRiftSerializable
    {
        public byte InterestAreaId;

        public AddInterestAreaResponse(byte interestAreaId)
        {
            InterestAreaId = interestAreaId;
        }

        public void Deserialize(DeserializeEvent e)
        {
            InterestAreaId = e.Reader.ReadByte();
           
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(InterestAreaId);
        }
    }
}
