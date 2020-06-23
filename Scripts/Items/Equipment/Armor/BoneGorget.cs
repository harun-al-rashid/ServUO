using System;

namespace Server.Items
{
    public class BoneGorget : BaseArmor
    {
        [Constructable]
        public BoneGorget()
            : base(0x13D6)
        {
            this.Weight = 2.0;
        }

        public BoneGorget(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1063496;
            }
        }

        public override int BasePhysicalResistance
        {
            get
            {
                return 7;
            }
        }
        public override int BaseFireResistance
        {
            get
            {
                return 5;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return 4;
            }
        }
        public override int BasePoisonResistance
        {
            get
            {
                return 5;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return 4;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 50;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 65;
            }
        }
        public override int AosStrReq
        {
            get
            {
                return 45;
            }
        }
        public override int OldStrReq
        {
            get
            {
                return 30;
            }
        }
        public override int OldDexBonus
        {
            get
            {
                return -1;
            }
        }
        public override int ArmorBase
        {
            get
            {
                return 35;
            }
        }
        public override ArmorMaterialType MaterialType
        {
            get
            {
                return ArmorMaterialType.Bone;
            }
        }

        public override CraftResource DefaultResource
        {
            get
            {
                return CraftResource.RegularLeather;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}