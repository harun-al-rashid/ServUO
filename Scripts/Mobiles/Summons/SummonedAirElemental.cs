using System;

namespace Server.Mobiles
{
    [CorpseName("an air elemental corpse")]
    public class SummonedAirElemental : BaseCreature
    {
        private int m_type;

        [Constructable]
        public SummonedAirElemental(int type)
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            //this.Name = "an air elemental";
            this.Body = 13;
            this.Hue = 0x4001;
            this.BaseSoundID = 655;
            m_type = type;

            switch (type)
            {


                case 1:

                    this.Name = "aged air elemental";
                    this.SetStr(130);
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
                    this.VirtualArmor = 38;
                    this.ControlSlots = 2;
                    break;
                case 2:

                    this.Name = "elder air elemental";
                    this.SetStr(135);
                    this.SetDex(190);
                    this.SetInt(140);

                    this.SetHits(95);

                    this.SetDamage(10, 12);
                    this.SetSkill(SkillName.MagicResist, 85.0);
                    this.SetSkill(SkillName.Tactics, 90.0);
                    this.SetSkill(SkillName.Wrestling, 90.0);

                    this.SetSkill(SkillName.Meditation, 110.0);
                    this.SetSkill(SkillName.EvalInt, 90.0);
                    this.SetSkill(SkillName.Magery, 90.0);
                    this.VirtualArmor = 42;
                    this.ControlSlots = 2;
                    break;
                case 3:

                    this.Name = "ancient air elemental";
                    this.SetStr(135);
                    this.SetDex(200);
                    this.SetInt(150);

                    this.SetHits(100);

                    this.SetDamage(11, 13);
                    this.SetSkill(SkillName.MagicResist, 95.0);
                    this.SetSkill(SkillName.Tactics, 90.0);
                    this.SetSkill(SkillName.Wrestling, 90.0);

                    this.SetSkill(SkillName.Meditation, 120.0);
                    this.SetSkill(SkillName.EvalInt, 100.0);
                    this.SetSkill(SkillName.Magery, 100.0);
                    this.VirtualArmor = 46;
                    this.ControlSlots = 2;
                    break;
                case 4:

                    this.Name = "an air spirit";
                    this.SetStr(140);
                    this.SetDex(210);
                    this.SetInt(170);

                    this.SetHits(110);

                    this.SetDamage(13, 15);
                    this.SetSkill(SkillName.MagicResist, 110.0);
                    this.SetSkill(SkillName.Tactics, 95.0);
                    this.SetSkill(SkillName.Wrestling, 95.0);

                    this.SetSkill(SkillName.Meditation, 130.0);
                    this.SetSkill(SkillName.EvalInt, 110.0);
                    this.SetSkill(SkillName.Magery, 110.0);
                    this.VirtualArmor = 50;
                    this.ControlSlots = 2;
                    break;
                default:
                    this.Name = "an air elemental";

                    this.SetStr(125);
                    this.SetDex(170);
                    this.SetInt(120);

                    this.SetHits(85);

                    this.SetDamage(8, 10);

                    this.SetSkill(SkillName.Meditation, 90.0);
                    this.SetSkill(SkillName.EvalInt, 70.0);
                    this.SetSkill(SkillName.Magery, 70.0);
                    this.SetSkill(SkillName.MagicResist, 60.0);
                    this.SetSkill(SkillName.Tactics, 80.0);
                    this.SetSkill(SkillName.Wrestling, 80.0);
                    this.VirtualArmor = 40;
                    this.ControlSlots = 2;
                    break;
            }

                    this.SetStr(200);
            this.SetDex(200);
            this.SetInt(100);

            this.SetHits(150);
            this.SetStam(50);

            this.SetDamage(6, 9);

            this.SetDamageType(ResistanceType.Physical, 50);
            this.SetDamageType(ResistanceType.Energy, 50);

            this.SetResistance(ResistanceType.Physical, 40, 50);
            this.SetResistance(ResistanceType.Fire, 30, 40);
            this.SetResistance(ResistanceType.Cold, 35, 45);
            this.SetResistance(ResistanceType.Poison, 50, 60);
            this.SetResistance(ResistanceType.Energy, 70, 80);

            

            this.VirtualArmor = 40;
            this.ControlSlots = 2;
        }

        public SummonedAirElemental(Serial serial)
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

            if (this.BaseSoundID == 263)
                this.BaseSoundID = 655;
        }
    }
}
