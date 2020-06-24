using System;

namespace Server.Items
{
    public abstract class BaseHides : Item, ICommodity
    {
        protected virtual CraftResource DefaultResource { get { return CraftResource.RegularLeather; } }

        private CraftResource m_Resource;
        public BaseHides(CraftResource resource)
            : this(resource, 1)
        {
        }

        public BaseHides(CraftResource resource, int amount)
            : base(0x1079)
        {
            this.Stackable = true;
            this.Weight = 5.0;
            this.Amount = amount;
            this.Hue = CraftResources.GetHue(resource);

            this.m_Resource = resource;
        }

        public BaseHides(Serial serial)
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
                    return 1049687 + (int)(this.m_Resource - CraftResource.SpinedLeather);

                return 1047023;
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
                list.Add(1050039, "{0}\t#{1}", this.Amount, 1024216); // ~1_NUMBER~ ~2_ITEMNAME~
            else
                list.Add(1024216); // pile of hides
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

    [FlipableAttribute(0x1079, 0x1078)]
    public class Hides : BaseHides, IScissorable
    {
        [Constructable]
        public Hides()
            : this(1)
        {
        }

        [Constructable]
        public Hides(int amount)
            : base(CraftResource.RegularLeather, amount)
        {
        }

        public Hides(Serial serial)
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

        public bool Scissor(Mobile from, Scissors scissors)
        {
            if (this.Deleted || !from.CanSee(this))
                return false;

            if (Core.AOS && !this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(502437); // Items you wish to cut must be in your backpack
                return false;
            }
            base.ScissorHelper(from, new Leather(), 1);

            return true;
        }
    }

    [FlipableAttribute(0x1079, 0x1078)]
    public class DullHides : BaseHides, IScissorable
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Dullhide; } }

        [Constructable]
        public DullHides()
            : this(1)
        {
        }

        [Constructable]
        public DullHides(int amount)
            : base(CraftResource.Dullhide, amount)
        {
        }

        public DullHides(Serial serial)
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

        public bool Scissor(Mobile from, Scissors scissors)
        {
            if (this.Deleted || !from.CanSee(this))
                return false;

            if (Core.AOS && !this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(502437); // Items you wish to cut must be in your backpack
                return false;
            }

            base.ScissorHelper(from, new DullLeather(), 1);

            return true;
        }
    }

    [FlipableAttribute(0x1079, 0x1078)]
    public class ShadowHides : BaseHides, IScissorable
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Shadowhide; } }

        [Constructable]
        public ShadowHides()
            : this(1)
        {
        }

        [Constructable]
        public ShadowHides(int amount)
            : base(CraftResource.Shadowhide, amount)
        {
        }

        public ShadowHides(Serial serial)
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

        public bool Scissor(Mobile from, Scissors scissors)
        {
            if (this.Deleted || !from.CanSee(this))
                return false;

            if (Core.AOS && !this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(502437); // Items you wish to cut must be in your backpack
                return false;
            }

            base.ScissorHelper(from, new ShadowLeather(), 1);

            return true;
        }
    }

    [FlipableAttribute(0x1079, 0x1078)]
    public class CopperHides : BaseHides, IScissorable
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Copperhide; } }

        [Constructable]
        public CopperHides()
            : this(1)
        {
        }

        [Constructable]
        public CopperHides(int amount)
            : base(CraftResource.Copperhide, amount)
        {
        }

        public CopperHides(Serial serial)
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

        public bool Scissor(Mobile from, Scissors scissors)
        {
            if (this.Deleted || !from.CanSee(this))
                return false;

            if (Core.AOS && !this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(502437); // Items you wish to cut must be in your backpack
                return false;
            }

            base.ScissorHelper(from, new CopperLeather(), 1);

            return true;
        }
    }

    [FlipableAttribute(0x1079, 0x1078)]
    public class BronzeHides : BaseHides, IScissorable
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Bronzehide; } }

        [Constructable]
        public BronzeHides()
            : this(1)
        {
        }

        [Constructable]
        public BronzeHides(int amount)
            : base(CraftResource.Bronzehide, amount)
        {
        }

        public BronzeHides(Serial serial)
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

        public bool Scissor(Mobile from, Scissors scissors)
        {
            if (this.Deleted || !from.CanSee(this))
                return false;

            if (Core.AOS && !this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(502437); // Items you wish to cut must be in your backpack
                return false;
            }

            base.ScissorHelper(from, new BronzeLeather(), 1);

            return true;
        }
    }

    [FlipableAttribute(0x1079, 0x1078)]
    public class GoldenHides : BaseHides, IScissorable
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Goldenhide; } }

        [Constructable]
        public GoldenHides()
            : this(1)
        {
        }

        [Constructable]
        public GoldenHides(int amount)
            : base(CraftResource.SpinedLeather, amount)
        {
        }

        public GoldenHides(Serial serial)
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

        public bool Scissor(Mobile from, Scissors scissors)
        {
            if (this.Deleted || !from.CanSee(this))
                return false;

            if (Core.AOS && !this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(502437); // Items you wish to cut must be in your backpack
                return false;
            }

            base.ScissorHelper(from, new GoldenLeather(), 1);

            return true;
        }
    }

    [FlipableAttribute(0x1079, 0x1078)]
    public class RoseHides : BaseHides, IScissorable
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Rosehide; } }

        [Constructable]
        public RoseHides()
            : this(1)
        {
        }

        [Constructable]
        public RoseHides(int amount)
            : base(CraftResource.Rosehide, amount)
        {
        }

        public RoseHides(Serial serial)
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

        public bool Scissor(Mobile from, Scissors scissors)
        {
            if (this.Deleted || !from.CanSee(this))
                return false;

            if (Core.AOS && !this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(502437); // Items you wish to cut must be in your backpack
                return false;
            }

            base.ScissorHelper(from, new RoseLeather(), 1);

            return true;
        }
    }

    [FlipableAttribute(0x1079, 0x1078)]
    public class VereHides : BaseHides, IScissorable
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Verehide; } }

        [Constructable]
        public VereHides()
            : this(1)
        {
        }

        [Constructable]
        public VereHides(int amount)
            : base(CraftResource.Verehide, amount)
        {
        }

        public VereHides(Serial serial)
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

        public bool Scissor(Mobile from, Scissors scissors)
        {
            if (this.Deleted || !from.CanSee(this))
                return false;

            if (Core.AOS && !this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(502437); // Items you wish to cut must be in your backpack
                return false;
            }

            base.ScissorHelper(from, new VereLeather(), 1);

            return true;
        }
    }

    [FlipableAttribute(0x1079, 0x1078)]
    public class ValeHides : BaseHides, IScissorable
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Valehide; } }

        [Constructable]
        public ValeHides()
            : this(1)
        {
        }

        [Constructable]
        public ValeHides(int amount)
            : base(CraftResource.Valehide, amount)
        {
        }

        public ValeHides(Serial serial)
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

        public bool Scissor(Mobile from, Scissors scissors)
        {
            if (this.Deleted || !from.CanSee(this))
                return false;

            if (Core.AOS && !this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(502437); // Items you wish to cut must be in your backpack
                return false;
            }

            base.ScissorHelper(from, new ValeLeather(), 1);

            return true;
        }
    }

    [FlipableAttribute(0x1079, 0x1078)]
    public class SpinedHides : BaseHides, IScissorable
    {
        protected override CraftResource DefaultResource { get { return CraftResource.SpinedLeather; } }

        [Constructable]
        public SpinedHides()
            : this(1)
        {
        }

        [Constructable]
        public SpinedHides(int amount)
            : base(CraftResource.SpinedLeather, amount)
        {
        }

        public SpinedHides(Serial serial)
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

        public bool Scissor(Mobile from, Scissors scissors)
        {
            if (this.Deleted || !from.CanSee(this))
                return false;

            if (Core.AOS && !this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(502437); // Items you wish to cut must be in your backpack
                return false;
            }

            base.ScissorHelper(from, new SpinedLeather(), 1);

            return true;
        }
    }

    [FlipableAttribute(0x1079, 0x1078)]
    public class HornedHides : BaseHides, IScissorable
    {
        protected override CraftResource DefaultResource { get { return CraftResource.HornedLeather; } }

        [Constructable]
        public HornedHides()
            : this(1)
        {
        }

        [Constructable]
        public HornedHides(int amount)
            : base(CraftResource.HornedLeather, amount)
        {
        }

        public HornedHides(Serial serial)
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

        public bool Scissor(Mobile from, Scissors scissors)
        {
            if (this.Deleted || !from.CanSee(this))
                return false;

            if (Core.AOS && !this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(502437); // Items you wish to cut must be in your backpack
                return false;
            }
			
            base.ScissorHelper(from, new HornedLeather(), 1);

            return true;
        }
    }

    [FlipableAttribute(0x1079, 0x1078)]
    public class BarbedHides : BaseHides, IScissorable
    {
        protected override CraftResource DefaultResource { get { return CraftResource.BarbedLeather; } }

        [Constructable]
        public BarbedHides()
            : this(1)
        {
        }

        [Constructable]
        public BarbedHides(int amount)
            : base(CraftResource.BarbedLeather, amount)
        {
        }

        public BarbedHides(Serial serial)
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

        public bool Scissor(Mobile from, Scissors scissors)
        {
            if (this.Deleted || !from.CanSee(this))
                return false;

            if (Core.AOS && !this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(502437); // Items you wish to cut must be in your backpack
                return false;
            }

            base.ScissorHelper(from, new BarbedLeather(), 1);

            return true;
        }
    }
}