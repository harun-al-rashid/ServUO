using System;

namespace Server.Mobiles
{
    [CorpseName("a daemon corpse")]
    public class SummonedDaemon : BaseCreature
    {
        private int m_type;

        [Constructable]
        public SummonedDaemon(int type)
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {

            switch (type)
            {


                case 1:

                    this.Name = "an oil baron";
                    this.SetStr(530);
                    this.SetDex(105);
                    this.SetInt(350);

                    this.SetHits(400);

                    this.SetDamage(16, 23);
                    this.SetSkill(SkillName.MagicResist, 100.0);
                    this.SetSkill(SkillName.Tactics, 85.0);
                    this.SetSkill(SkillName.Wrestling, 80.0);

                    this.SetSkill(SkillName.Meditation, 85.0);
                    this.SetSkill(SkillName.EvalInt, 85.0);
                    this.SetSkill(SkillName.Magery, 85.0);
                    this.VirtualArmor = 62;
                    this.ControlSlots = 5;
                    break;
                case 2:

                    this.Name = "a coal baron";
                    this.SetStr(560);
                    this.SetDex(115);
                    this.SetInt(375);

                    this.SetHits(500);

                    this.SetDamage(18, 25);
                    this.SetSkill(SkillName.MagicResist, 110.0);
                    this.SetSkill(SkillName.Tactics, 95.0);
                    this.SetSkill(SkillName.Wrestling, 90.0);

                    this.SetSkill(SkillName.Meditation, 95.0);
                    this.SetSkill(SkillName.EvalInt, 95.0);
                    this.SetSkill(SkillName.Magery, 95.0);
                    this.VirtualArmor = 65;
                    this.ControlSlots = 5;
                    break;
                case 3:

                    this.Name = "Hitler";
                    this.SetStr(600);
                    this.SetDex(125);
                    this.SetInt(400);

                    this.SetHits(600);

                    this.SetDamage(20, 27);
                    this.SetSkill(SkillName.MagicResist, 120.0);
                    this.SetSkill(SkillName.Tactics, 105.0);
                    this.SetSkill(SkillName.Wrestling, 100.0);

                    this.SetSkill(SkillName.Meditation, 105.0);
                    this.SetSkill(SkillName.EvalInt, 105.0);
                    this.SetSkill(SkillName.Magery, 105.0);
                    this.VirtualArmor = 68;
                    this.ControlSlots = 5;
                    break;
                case 4:

                    this.Name = "Donald Trump";
                    this.SetStr(650);
                    this.SetDex(150);
                    this.SetInt(450);

                    this.SetHits(750);

                    this.SetDamage(25, 30);
                    this.SetSkill(SkillName.MagicResist, 130.0);
                    this.SetSkill(SkillName.Tactics, 115.0);
                    this.SetSkill(SkillName.Wrestling, 110.0);

                    this.SetSkill(SkillName.Meditation, 120.0);
                    this.SetSkill(SkillName.EvalInt, 115.0);
                    this.SetSkill(SkillName.Magery, 115.0);
                    this.VirtualArmor = 75;
                    this.ControlSlots = 5;
                    break;
                default:
                    
                    this.Name = NameList.RandomName("daemon");

                    this.SetStr(500);
                    this.SetDex(95);
                    this.SetInt(325);

                    SetHits(300);

                    this.SetDamage(14, 21);

                    SetSkill(SkillName.EvalInt, 75.0);
                    SetSkill(SkillName.Magery, 75.0);
                    SetSkill(SkillName.MagicResist, 90.0);
                    SetSkill(SkillName.Tactics, 75.0);
                    SetSkill(SkillName.Wrestling, 70.0);
                    SetSkill(SkillName.Meditation, 75.0);

                    this.VirtualArmor = 58;
                    this.ControlSlots = 5;
                    break;
            }

            this.Body = Core.AOS ? 10 : 9;
            this.BaseSoundID = 357;

            this.SetDamageType(ResistanceType.Physical, 0);
            this.SetDamageType(ResistanceType.Poison, 100);

            this.SetResistance(ResistanceType.Physical, 45, 55);
            this.SetResistance(ResistanceType.Fire, 50, 60);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 70, 80);
            this.SetResistance(ResistanceType.Energy, 40, 50);
        }

        public SummonedDaemon(Serial serial)
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

                //return 125.0;
            }
        }
        public override double DispelFocus
        {
            get
            {
                return 45.0;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Regular;
            }
        }// TODO: Immune to poison?
        public override bool CanFly
        {
            get
            {
                return true;
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
