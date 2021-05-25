using System;

namespace Server.Mobiles
{
    [CorpseName("a water elemental corpse")]
    public class SummonedWaterElemental : BaseCreature
    {
        private int m_type;

        [Constructable]
        public SummonedWaterElemental(int type)
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            //this.Name = "a water elemental";
            this.Body = 16;
            this.BaseSoundID = 278;
            m_type = type;

            switch (type)
            {


                case 1:

                    this.Name = "aged water elemental";
                    this.SetStr(140);
                    this.SetDex(80);
                    this.SetInt(120);

                    this.SetHits(110);

                    this.SetDamage(8, 10);
                    this.SetSkill(SkillName.MagicResist, 120.0);
                    this.SetSkill(SkillName.Tactics, 85.0);
                    this.SetSkill(SkillName.Wrestling, 85.0);

                    this.SetSkill(SkillName.Meditation, 100.0);
                    this.SetSkill(SkillName.EvalInt, 80.0);
                    this.SetSkill(SkillName.Magery, 80.0);
                    this.VirtualArmor = 38;
                    this.ControlSlots = 3;
                    break;
                case 2:

                    this.Name = "elder water elemental";
                    this.SetStr(150);
                    this.SetDex(90);
                    this.SetInt(130);

                    this.SetHits(140);

                    this.SetDamage(9, 11);
                    this.SetSkill(SkillName.MagicResist, 125.0);
                    this.SetSkill(SkillName.Tactics, 95.0);
                    this.SetSkill(SkillName.Wrestling, 95.0);

                    this.SetSkill(SkillName.Meditation, 110.0);
                    this.SetSkill(SkillName.EvalInt, 90.0);
                    this.SetSkill(SkillName.Magery, 90.0);
                    this.VirtualArmor = 40;
                    this.ControlSlots = 3;
                    break;
                case 3:

                    this.Name = "ancient water elemental";
                    this.SetStr(160);
                    this.SetDex(100);
                    this.SetInt(140);

                    this.SetHits(170);

                    this.SetDamage(10, 12);
                    this.SetSkill(SkillName.MagicResist, 130.0);
                    this.SetSkill(SkillName.Tactics, 100.0);
                    this.SetSkill(SkillName.Wrestling, 100.0);

                    this.SetSkill(SkillName.Meditation, 120.0);
                    this.SetSkill(SkillName.EvalInt, 95.0);
                    this.SetSkill(SkillName.Magery, 95.0);
                    this.VirtualArmor = 42;
                    this.ControlSlots = 3;
                    break;
                case 4:

                    this.Name = "a water spirit";
                    this.SetStr(175);
                    this.SetDex(120);
                    this.SetInt(200);

                    this.SetHits(200);

                    this.SetDamage(11, 14);
                    this.SetSkill(SkillName.MagicResist, 140.0);
                    this.SetSkill(SkillName.Tactics, 105.0);
                    this.SetSkill(SkillName.Wrestling, 105.0);

                    this.SetSkill(SkillName.Meditation, 130.0);
                    this.SetSkill(SkillName.EvalInt, 100.0);
                    this.SetSkill(SkillName.Magery, 100.0);
                    this.VirtualArmor = 45;
                    this.ControlSlots = 3;
                    break;
                default:
                    this.Name = "a water elemental";

                    this.SetStr(130);
                    this.SetDex(70);
                    this.SetInt(110);

                    this.SetHits(80);

                    this.SetDamage(7, 9);

                    this.SetSkill(SkillName.Meditation, 90.0);
                    this.SetSkill(SkillName.EvalInt, 70.0);
                    this.SetSkill(SkillName.Magery, 70.0);
                    this.SetSkill(SkillName.MagicResist, 110.0);
                    this.SetSkill(SkillName.Tactics, 80.0);
                    this.SetSkill(SkillName.Wrestling, 80.0);

                    this.VirtualArmor = 35;
                    this.ControlSlots = 3;
                    break;
            }

            //this.SetStr(200);
            //this.SetDex(70);
            //this.SetInt(100);

            //this.SetHits(165);

            //this.SetDamage(7, 9);

            this.SetDamageType(ResistanceType.Physical, 0);
            this.SetDamageType(ResistanceType.Cold, 100);

            this.SetResistance(ResistanceType.Physical, 50, 60);
            this.SetResistance(ResistanceType.Fire, 20, 30);
            this.SetResistance(ResistanceType.Cold, 70, 80);
            this.SetResistance(ResistanceType.Poison, 45, 55);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            //this.SetSkill(SkillName.Meditation, 90.0);
           // this.SetSkill(SkillName.EvalInt, 80.0);
            //this.SetSkill(SkillName.Magery, 80.0);
            //this.SetSkill(SkillName.MagicResist, 75.0);
            //this.SetSkill(SkillName.Tactics, 100.0);
            //this.SetSkill(SkillName.Wrestling, 85.0);

            //this.VirtualArmor = 40;
            //this.ControlSlots = 3;
            this.CanSwim = true;
        }

        public SummonedWaterElemental(Serial serial)
            : base(serial)
        {
        }

        public override double DispelDifficulty
        {
            get
            {
                switch (m_type)
                {
                    case 0:
                        return 117.5;
                    case 1:
                        return 137.5;
                    case 2:
                        return 157.5;
                    case 3:
                        return 177.5;
                    case 4:
                        return 200;
                    default:
                        return 117.5;

                }

                //return 117.5;
            }
        }
        public override double DispelFocus
        {
            get
            {
                return 45.0;
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
