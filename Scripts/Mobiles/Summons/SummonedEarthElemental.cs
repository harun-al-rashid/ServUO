using System;

namespace Server.Mobiles
{
    [CorpseName("an earth elemental corpse")]
    public class SummonedEarthElemental : BaseCreature
    {
        private int m_type;

        [Constructable]
        public SummonedEarthElemental(int type)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            
            this.Body = 14;
            this.BaseSoundID = 268;
            m_type = type;

            switch (type) {
                
                
                case 1:
            
                this.Name = "aged earth elemental";
                this.SetStr(150, 180);
                this.SetDex(75, 90);
                this.SetInt(71, 92);

                this.SetHits(95, 120);

                this.SetDamage(12, 19);
                this.SetSkill(SkillName.MagicResist, 75.0);
                this.SetSkill(SkillName.Tactics, 105.0);
                this.SetSkill(SkillName.Wrestling, 95.0);
                this.VirtualArmor = 38;
                this.ControlSlots = 2;
                    break;
                case 2:
            
                this.Name = "elder earth elemental";
                this.SetStr(160, 210);
                this.SetDex(80, 95);
                this.SetInt(71, 92);

                this.SetHits(110, 150);

                this.SetDamage(15, 22);
                this.SetSkill(SkillName.MagicResist, 85.0);
                this.SetSkill(SkillName.Tactics, 110.0);
                this.SetSkill(SkillName.Wrestling, 100.0);
                this.VirtualArmor = 42;
                this.ControlSlots = 2;
                    break;
                case 3:
            
                this.Name = "ancient earth elemental";
                this.SetStr(180, 230);
                this.SetDex(85, 100);
                this.SetInt(71, 92);

                this.SetHits(140, 180);

                this.SetDamage(18, 26);
                this.SetSkill(SkillName.MagicResist, 95.0);
                this.SetSkill(SkillName.Tactics, 115.0);
                this.SetSkill(SkillName.Wrestling, 105.0);
                this.VirtualArmor = 46;
                this.ControlSlots = 2;
                    break;
                case 4:
            
                this.Name = "an earth spirit";
                this.SetStr(200, 250);
                this.SetDex(90, 110);
                this.SetInt(71, 92);

                this.SetHits(170, 220);

                this.SetDamage(21, 30);
                this.SetSkill(SkillName.MagicResist, 105.0);
                this.SetSkill(SkillName.Tactics, 120.0);
                this.SetSkill(SkillName.Wrestling, 110.0);
                this.VirtualArmor = 50;
                this.ControlSlots = 2;
                    break;
                default:
                    this.Name = "an earth elemental";
                    this.SetStr(126, 155);
                    this.SetDex(66, 85);
                    this.SetInt(71, 92);

                    this.SetHits(76, 93);

                    this.SetDamage(9, 16);
                    this.SetSkill(SkillName.MagicResist, 65.0);
                    this.SetSkill(SkillName.Tactics, 100.0);
                    this.SetSkill(SkillName.Wrestling, 90.0);
                    this.VirtualArmor = 34;
                    this.ControlSlots = 2;
                    break;

            }
            //this.SetStr(200);
            //this.SetDex(70);
            //this.SetInt(70);

            //this.SetHits(180);

            //this.SetDamage(14, 21);

            

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 65, 75);
            this.SetResistance(ResistanceType.Fire, 40, 50);
            this.SetResistance(ResistanceType.Cold, 40, 50);
            this.SetResistance(ResistanceType.Poison, 40, 50);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            

            
        }

        public SummonedEarthElemental(Serial serial)
            : base(serial)
        {
        }

        public override double DispelDifficulty
        {
            get
            {
                //return 300;//117.5
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
