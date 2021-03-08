using System;

namespace Server.Items
{
    public class ArcaneScroll : Item, ICommodity
    {
        public override int LabelNumber
        {
            get
            {
                return 1063546;
            }
        }
            [Constructable]
        public ArcaneScroll()
            : this(1)
        {
        }

        [Constructable]
        public ArcaneScroll(int amount)
            : base(0xEF3)
        {
            this.Stackable = true;
            this.Weight = 1.0;
            this.Amount = amount;
            this.Hue = 0x7bc;
        }

        public ArcaneScroll(Serial serial)
            : base(serial)
        {
        }

        TextDefinition ICommodity.Description
        {
            get
            {
                return this.LabelNumber;
            }
        }
        bool ICommodity.IsDeedable
        {
            get
            {
                return (Core.ML);
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        /*public int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, ITool tool, CraftItem craftItem, int resHue)
        {
            Amount = 5;
            return 1;
        }*/
    }
}
