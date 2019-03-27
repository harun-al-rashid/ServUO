using System;

namespace Server.Mobiles
{
    [CorpseName("a silver steed corpse")]
    public class SilverSteed : BaseMount
    {
        [Constructable]
        public SilverSteed()
            : this("a silver steed")
        {
        }

        [Constructable]
        public SilverSteed(string name)
            : base(name, 0x75, 0x3EA8, AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
           
            this.SetStr(496, 525);
            this.SetDex(98, 105);
            this.SetInt(235, 245);

            this.SetHits(298, 315);

            this.SetDamage(16, 22);

            this.SetDamageType(ResistanceType.Physical, 40);
            this.SetDamageType(ResistanceType.Fire, 20);
            this.SetDamageType(ResistanceType.Energy, 40);

            this.SetResistance(ResistanceType.Physical, 60, 65);
            this.SetResistance(ResistanceType.Fire, 35, 40);
            this.SetResistance(ResistanceType.Cold, 35, 40);
            this.SetResistance(ResistanceType.Poison, 45, 50);
            this.SetResistance(ResistanceType.Energy, 55, 60);

            this.SetSkill(SkillName.EvalInt, 95.0, 100.0);
            this.SetSkill(SkillName.Magery, 95.0, 100.0);
            this.SetSkill(SkillName.MagicResist, 85.3, 100.0);
            this.SetSkill(SkillName.Tactics, 97.6, 100.0);
            this.SetSkill(SkillName.Wrestling, 80.5, 92.5);

            this.Fame = 14000;
            this.Karma = 14000;

            this.VirtualArmor = 60;

            this.Tamable = true;
            this.ControlSlots = 2;
            this.MinTameSkill = 103.1;
        }

        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.FruitsAndVegies | FoodType.GrainsAndHay;
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls);
            AddLoot(LootPack.Potions);
        }

        public SilverSteed(Serial serial)
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