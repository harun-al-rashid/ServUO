using System;

namespace Server.Items
{
    public abstract class BaseLeather : Item, ICommodity
    {
        protected virtual CraftResource DefaultResource { get { return CraftResource.RegularLeather; } }

        private CraftResource m_Resource;
        public BaseLeather(CraftResource resource)
            : this(resource, 1)
        {
        }

        public BaseLeather(CraftResource resource, int amount)
            : base(0x1081)
        {
            this.Stackable = true;
            this.Weight = 1.0;
            this.Amount = amount;
            this.Hue = CraftResources.GetHue(resource);

            this.m_Resource = resource;
        }

        public BaseLeather(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get
            {
                return this.m_Resource;
            }
            set
            {
                this.m_Resource = value;
                this.InvalidateProperties();
            }
        }
        public override int LabelNumber
        {
            get
            {
                if (this.m_Resource >= CraftResource.SpinedLeather && this.m_Resource <= CraftResource.BarbedLeather)
                    return 1049684 + (int)(this.m_Resource - CraftResource.SpinedLeather);

                return 1047022;
            }
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
                return true;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write((int)this.m_Resource);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch ( version )
            {
                case 2: // Reset from Resource System
                    this.m_Resource = this.DefaultResource;
                    reader.ReadString();
                    break;
                case 1:
                    {
                        this.m_Resource = (CraftResource)reader.ReadInt();
                        break;
                    }
                case 0:
                    {
                        OreInfo info = new OreInfo(reader.ReadInt(), reader.ReadInt(), reader.ReadString());

                        this.m_Resource = CraftResources.GetFromOreInfo(info);
                        break;
                    }
            }
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            if (this.Amount > 1)
                list.Add(1050039, "{0}\t#{1}", this.Amount, 1024199); // ~1_NUMBER~ ~2_ITEMNAME~
            else
                list.Add(1024199); // cut leather
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (!CraftResources.IsStandard(this.m_Resource))
            {
                int num = CraftResources.GetLocalizationNumber(this.m_Resource);

                if (num > 0)
                    list.Add(num);
                else
                    list.Add(CraftResources.GetName(this.m_Resource));
            }
        }
    }

    [FlipableAttribute(0x1081, 0x1082)]
    public class Leather : BaseLeather
    {
        [Constructable]
        public Leather()
            : this(1)
        {
        }

        [Constructable]
        public Leather(int amount)
            : base(CraftResource.RegularLeather, amount)
        {
        }

        public Leather(Serial serial)
            : base(serial)
        {
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
    }

    [FlipableAttribute(0x1081, 0x1082)]
    public class DullLeather : BaseLeather
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Dullhide; } }

        [Constructable]
        public DullLeather()
            : this(1)
        {
        }

        [Constructable]
        public DullLeather(int amount)
            : base(CraftResource.SpinedLeather, amount)
        {
        }

        public DullLeather(Serial serial)
            : base(serial)
        {
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
    }

    [FlipableAttribute(0x1081, 0x1082)]
    public class ShadowLeather : BaseLeather
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Shadowhide; } }

        [Constructable]
        public ShadowLeather()
            : this(1)
        {
        }

        [Constructable]
        public ShadowLeather(int amount)
            : base(CraftResource.SpinedLeather, amount)
        {
        }

        public ShadowLeather(Serial serial)
            : base(serial)
        {
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
    }

    [FlipableAttribute(0x1081, 0x1082)]
    public class CopperLeather : BaseLeather
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Copperhide; } }

        [Constructable]
        public CopperLeather()
            : this(1)
        {
        }

        [Constructable]
        public CopperLeather(int amount)
            : base(CraftResource.SpinedLeather, amount)
        {
        }

        public CopperLeather(Serial serial)
            : base(serial)
        {
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
    }

    [FlipableAttribute(0x1081, 0x1082)]
    public class BronzeLeather : BaseLeather
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Bronzehide; } }

        [Constructable]
        public BronzeLeather()
            : this(1)
        {
        }

        [Constructable]
        public BronzeLeather(int amount)
            : base(CraftResource.SpinedLeather, amount)
        {
        }

        public BronzeLeather(Serial serial)
            : base(serial)
        {
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
    }

    [FlipableAttribute(0x1081, 0x1082)]
    public class GoldenLeather : BaseLeather
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Goldenhide; } }

        [Constructable]
        public GoldenLeather()
            : this(1)
        {
        }

        [Constructable]
        public GoldenLeather(int amount)
            : base(CraftResource.SpinedLeather, amount)
        {
        }

        public GoldenLeather(Serial serial)
            : base(serial)
        {
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
    }

    [FlipableAttribute(0x1081, 0x1082)]
    public class RoseLeather : BaseLeather
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Rosehide; } }

        [Constructable]
        public RoseLeather()
            : this(1)
        {
        }

        [Constructable]
        public RoseLeather(int amount)
            : base(CraftResource.SpinedLeather, amount)
        {
        }

        public RoseLeather(Serial serial)
            : base(serial)
        {
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
    }

    [FlipableAttribute(0x1081, 0x1082)]
    public class ValeLeather : BaseLeather
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Valehide; } }

        [Constructable]
        public ValeLeather()
            : this(1)
        {
        }

        [Constructable]
        public ValeLeather(int amount)
            : base(CraftResource.SpinedLeather, amount)
        {
        }

        public ValeLeather(Serial serial)
            : base(serial)
        {
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
    }

    public class VereLeather : BaseLeather
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Verehide; } }

        [Constructable]
        public VereLeather()
            : this(1)
        {
        }

        [Constructable]
        public VereLeather(int amount)
            : base(CraftResource.SpinedLeather, amount)
        {
        }

        public VereLeather(Serial serial)
            : base(serial)
        {
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
    }

    [FlipableAttribute(0x1081, 0x1082)]
    public class SpinedLeather : BaseLeather
    {
        protected override CraftResource DefaultResource { get { return CraftResource.SpinedLeather; } }

        [Constructable]
        public SpinedLeather()
            : this(1)
        {
        }

        [Constructable]
        public SpinedLeather(int amount)
            : base(CraftResource.SpinedLeather, amount)
        {
        }

        public SpinedLeather(Serial serial)
            : base(serial)
        {
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
    }

    [FlipableAttribute(0x1081, 0x1082)]
    public class HornedLeather : BaseLeather
    {
        protected override CraftResource DefaultResource { get { return CraftResource.HornedLeather; } }

        [Constructable]
        public HornedLeather()
            : this(1)
        {
        }

        [Constructable]
        public HornedLeather(int amount)
            : base(CraftResource.HornedLeather, amount)
        {
        }

        public HornedLeather(Serial serial)
            : base(serial)
        {
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
    }

    [FlipableAttribute(0x1081, 0x1082)]
    public class BarbedLeather : BaseLeather
    {
        protected override CraftResource DefaultResource { get { return CraftResource.BarbedLeather; } }

        [Constructable]
        public BarbedLeather()
            : this(1)
        {
        }

        [Constructable]
        public BarbedLeather(int amount)
            : base(CraftResource.BarbedLeather, amount)
        {
        }

        public BarbedLeather(Serial serial)
            : base(serial)
        {
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
    }
}