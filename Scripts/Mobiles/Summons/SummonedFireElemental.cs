using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a fire elemental corpse")]
    public class SummonedFireElemental : BaseCreature
    {
        private int m_type;

        [Constructable]
        public SummonedFireElemental(int type)
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            //this.Name = "a fire elemental";
            this.Body = 15;
            this.BaseSoundID = 838;
            m_type = type;

            switch (type)
            {


                case 1:

                    this.Name = "aged fire elemental";
                    this.SetStr(185);
                    this.SetDex(180);
                    this.SetInt(130);

                    this.SetHits(90);

                    this.SetDamage(9, 11);
                    this.SetSkill(SkillName.MagicResist, 75.0);
                    this.SetSkill(SkillName.Tactics, 85.0);
                    this.SetSkill(SkillName.Wrestling, 85.0);

                    this.SetSkill(SkillName.Meditation, 100.0);
                    this.SetSkill(SkillName.EvalInt, 80.0);
                    this.SetSkill(SkillName.Magery, 80.0);
                    this.VirtualArmor = 48;
                    this.ControlSlots = 4;
                    break;
                case 2:

                    this.Name = "elder fire elemental";
                    this.SetStr(200);
                    this.SetDex(190);
                    this.SetInt(150);

                    this.SetHits(95);

                    this.SetDamage(10, 12);
                    this.SetSkill(SkillName.MagicResist, 85.0);
                    this.SetSkill(SkillName.Tactics, 90.0);
                    this.SetSkill(SkillName.Wrestling, 90.0);

                    this.SetSkill(SkillName.Meditation, 110.0);
                    this.SetSkill(SkillName.EvalInt, 90.0);
                    this.SetSkill(SkillName.Magery, 90.0);
                    this.VirtualArmor = 50;
                    this.ControlSlots = 4;
                    break;
                case 3:

                    this.Name = "ancient fire elemental";
                    this.SetStr(225);
                    this.SetDex(200);
                    this.SetInt(175);

                    this.SetHits(200);

                    this.SetDamage(13, 15);
                    this.SetSkill(SkillName.MagicResist, 95.0);
                    this.SetSkill(SkillName.Tactics, 90.0);
                    this.SetSkill(SkillName.Wrestling, 90.0);

                    this.SetSkill(SkillName.Meditation, 120.0);
                    this.SetSkill(SkillName.EvalInt, 100.0);
                    this.SetSkill(SkillName.Magery, 100.0);
                    this.VirtualArmor = 52;
                    this.ControlSlots = 4;
                    break;
                case 4:

                    this.Name = "a fire spirit";
                    this.SetStr(250);
                    this.SetDex(225);
                    this.SetInt(200);

                    this.SetHits(250);

                    this.SetDamage(15, 17);
                    this.SetSkill(SkillName.MagicResist, 120.0);
                    this.SetSkill(SkillName.Tactics, 120.0);
                    this.SetSkill(SkillName.Wrestling, 120.0);

                    this.SetSkill(SkillName.Meditation, 120.0);
                    this.SetSkill(SkillName.EvalInt, 120.0);
                    this.SetSkill(SkillName.Magery, 120.0);
                    this.VirtualArmor = 55;
                    this.ControlSlots = 4;
                    break;
                default:
                    this.Name = "a fire elemental";

                    this.SetStr(175);
                    this.SetDex(175);
                    this.SetInt(110);

                    this.SetHits(85);

                    this.SetDamage(7, 9);

                    this.SetSkill(SkillName.Meditation, 90.0);
                    this.SetSkill(SkillName.EvalInt, 75.0);
                    this.SetSkill(SkillName.Magery, 75.0);
                    this.SetSkill(SkillName.MagicResist, 85.0);
                    this.SetSkill(SkillName.Tactics, 90.0);
                    this.SetSkill(SkillName.Wrestling, 90.0);
                    this.VirtualArmor = 45;
                    this.ControlSlots = 4;
                    break;
            }

            //this.SetStr(200);
            //this.SetDex(200);
            //this.SetInt(100);

            this.SetDamageType(ResistanceType.Physical, 0);
            this.SetDamageType(ResistanceType.Fire, 100);

            this.SetResistance(ResistanceType.Physical, 50, 60);
            this.SetResistance(ResistanceType.Fire, 70, 80);
            this.SetResistance(ResistanceType.Cold, 0, 10);
            this.SetResistance(ResistanceType.Poison, 50, 60);
            this.SetResistance(ResistanceType.Energy, 50, 60);

            //this.VirtualArmor = 40;
            //this.ControlSlots = 3;

            this.AddItem(new LightSource());
        }

        public SummonedFireElemental(Serial serial)
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
