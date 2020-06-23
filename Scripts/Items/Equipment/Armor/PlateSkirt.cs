using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [FlipableAttribute(0x1c08, 0x1c09)]
    public class PlateSkirt : BaseArmor
    {
        [Constructable]
        public PlateSkirt()
            : base(0x1C08)
        {
            this.Weight = 1.0;
        }

        public PlateSkirt(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1063500;
            }
        }

        public override int BasePhysicalResistance
        {
            get
            {
                return 2;
            }
        }
        public override int BaseFireResistance
        {
            get
            {
                return 4;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return 3;
            }
        }
        public override int BasePoisonResistance
        {
            get
            {
                return 3;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return 3;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 30;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 40;
            }
        }
        public override int AosStrReq
        {
            get
            {
                return 20;
            }
        }
        public override int OldStrReq
        {
            get
            {
                return 10;
            }
        }
        public override int ArmorBase
        {
            get
            {
                return 50;
            }
        }
        public override ArmorMaterialType MaterialType
        {
            get
            {
                return ArmorMaterialType.Plate;
            }
        }
        public override CraftResource DefaultResource
        {
            get
            {
                return CraftResource.Iron;
            }
        }
        public override ArmorMeditationAllowance DefMedAllowance
        {
            get
            {
                return ArmorMeditationAllowance.None;
            }
        }
        public override bool AllowMaleWearer
        {
            get
            {
                return false;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);

            if (this.Weight == 3.0)
                this.Weight = 1.0;
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}