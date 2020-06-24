﻿using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a unicorn princess corpse")]
    public class UnicornPrincess : BaseMount
    {

        public override double HealChance { get { return 1.0; } }

        [Constructable]
        public UnicornPrincess()
            : this("a unicorn princess")
        {
        }

        [Constructable]
        public UnicornPrincess(string name)
            : base(name, 0x7A, 0x3EB4, AIType.AI_Mage, FightMode.Evil, 10, 1, 0.2, 0.4)
        {

            Hue = 0x492;

            BaseSoundID = 0x4BC;

            SetStr(450, 575);
            SetDex(96, 115);
            SetInt(350, 375);

            SetHits(476, 501);

            SetDamage(16, 22);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 25, 40);
            SetResistance(ResistanceType.Cold, 25, 40);
            SetResistance(ResistanceType.Poison, 85, 95);
            SetResistance(ResistanceType.Energy, 65, 75);

            SetSkill(SkillName.EvalInt, 100.0, 112.0);
            SetSkill(SkillName.Magery, 100.0, 112.0);
            SetSkill(SkillName.Meditation, 50.1, 60.0);
            SetSkill(SkillName.MagicResist, 100.0, 112.0);
            SetSkill(SkillName.Tactics, 20.1, 22.5);
            SetSkill(SkillName.Wrestling, 80.5, 92.5);

            Fame = 9000;
            Karma = 9000;

            Tamable = true;
            ControlSlots = 4;
            MinTameSkill = 109.1;

            SetWeaponAbility(WeaponAbility.ArmorIgnore);
        }

        public UnicornPrincess(Serial serial)
            : base(serial)
        {
        }

        public override bool CanHealOwner
        {
            get
            {
                return true;
            }
        }

        public override bool AllowMaleRider
        {
            get
            {
                return false;
            }
        }
        public override bool AllowMaleTamer
        {
            get
            {
                return false;
            }
        }
        public override bool InitialInnocent
        {
            get
            {
                return true;
            }
        }
        public override TimeSpan MountAbilityDelay
        {
            get
            {
                return TimeSpan.FromMinutes(1.0);
            }
        }

        public override TribeType Tribe { get { return TribeType.Fey; } }

        public override OppositionGroup OppositionGroup
        {
            get
            {
                return OppositionGroup.FeyAndUndead;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lethal;
            }
        }
        public override int Meat
        {
            get
            {
                return 3;
            }
        }
        public override int Hides
        {
            get
            {
                return 10;
            }
        }
        public override HideType HideType
        {
            get
            {
                return HideType.Vere;
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.FruitsAndVegies | FoodType.GrainsAndHay;
            }
        }
        public override void OnDisallowedRider(Mobile m)
        {
            m.SendLocalizedMessage(1042318); // The unicorn refuses to allow you to ride it.
        }

        public override bool DoMountAbility(int damage, Mobile attacker)
        {
            if (Rider == null || attacker == null)	//sanity
                return false;

            if (Rider.Poisoned && ((Rider.Hits - damage) < 80))
            {
                Poison p = Rider.Poison;

                if (p != null)
                {
                    int chanceToCure = 20000 + (int)(Skills[SkillName.Magery].Value * 75) - ((p.RealLevel + 1) * (Core.AOS ? (p.RealLevel < 4 ? 3300 : 3100) : 1750));
                    chanceToCure /= 100;

                    if (chanceToCure > Utility.Random(100))
                    {
                        if (Rider.CurePoison(this))	//TODO: Confirm if mount is the one flagged for curing it or the rider is
                        {
                            Rider.LocalOverheadMessage(Server.Network.MessageType.Regular, 0x3B2, true, "Your mount senses you are in danger and aids you with magic.");
                            Rider.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
                            Rider.PlaySound(0x1E0);	// Cure spell effect.
                            Rider.PlaySound(0xA9);		// Unicorn's whinny.

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.MedScrolls);
            AddLoot(LootPack.Potions);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.3)
                c.DropItem(new UnicornRibs());
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0)
            {
                SetWeaponAbility(WeaponAbility.ArmorIgnore);
            }
        }
    }
}