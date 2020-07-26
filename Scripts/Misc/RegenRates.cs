using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Necromancy;
using Server.Spells.Ninjitsu;
using Server.Spells.SkillMasteries;

namespace Server.Misc
{
    public delegate Int32 RegenBonusHandler(Mobile from);

    public class RegenRates
    {
        public static List<RegenBonusHandler> HitsBonusHandlers = new List<RegenBonusHandler>();
        public static List<RegenBonusHandler> StamBonusHandlers = new List<RegenBonusHandler>();
        public static List<RegenBonusHandler> ManaBonusHandlers = new List<RegenBonusHandler>();

        [CallPriority(10)]
        public static void Configure()
        {
            Mobile.DefaultHitsRate = TimeSpan.FromSeconds(11.0);
            Mobile.DefaultStamRate = TimeSpan.FromSeconds(4.0);
            Mobile.DefaultManaRate = TimeSpan.FromSeconds(2.0);

            Mobile.ManaRegenRateHandler = new RegenRateHandler(Mobile_ManaRegenRate);

            if (Core.AOS)
            {
                Mobile.StamRegenRateHandler = new RegenRateHandler(Mobile_StamRegenRate);
                Mobile.HitsRegenRateHandler = new RegenRateHandler(Mobile_HitsRegenRate);
            }
        }

        public static double GetArmorOffset(Mobile from)
        {
            double rating = 0.0;

            //if (!Core.AOS)
                rating += GetShieldMeditationPenalty(from.ShieldArmor as BaseShield)*0.35;

            rating += GetArmorMaterialPenalty(from.NeckArmor as BaseArmor)*0.07;
            rating += GetArmorMaterialPenalty(from.HandArmor as BaseArmor)*0.07;
            rating += GetArmorMaterialPenalty(from.HeadArmor as BaseArmor)*0.14;
            rating += GetArmorMaterialPenalty(from.ArmsArmor as BaseArmor)*0.15;
            rating += GetArmorMaterialPenalty(from.LegsArmor as BaseArmor)*0.22;
            rating += GetArmorMaterialPenalty(from.ChestArmor as BaseArmor)*0.35;

            return rating;
        }

        private static double GetShieldMeditationPenalty(BaseShield shield) {

            //double rating = 0;
            //Type type = shield.GetType();

            if (shield == null)
            {
                return 0;
            }

            if (shield.GetType() == typeof(WoodenKiteShield)) { return 20; }
            else if (shield.GetType() == typeof(MetalShield)) { return 40; }
            else if (shield.GetType() == typeof(BronzeShield)) { return 60; }
            else if (shield.GetType() == typeof(MetalKiteShield)) { return 80; }
            else if (shield.GetType() == typeof(HeaterShield) || shield.GetType() == typeof(ChaosShield) || shield.GetType() == typeof(OrderShield)) { return 100; }
            /*switch (type)
            {
                default:
                case WoodenKiteShield: return 20;
                case (shield == typeof(MetalShield)): return 40;
            }*/

            else return 0;

        }

        private static double GetArmorMaterialPenalty(BaseArmor armor)
        {

            if (armor == null) return 0;
            switch (armor.MaterialType)
            {

                default:
                case ArmorMaterialType.Leather:
                    return 0;
                case ArmorMaterialType.Studded:
                    return 20;
                case ArmorMaterialType.Bone:
                    return 40;
                case ArmorMaterialType.Ringmail:
                    return 60;
                case ArmorMaterialType.Chainmail:
                    return 80;
                case ArmorMaterialType.Plate:
                    return 100;
            }
        }

        private static void CheckBonusSkill(Mobile m, int cur, int max, SkillName skill)
        {
            if (!m.Alive)
                return;

            double n = (double)cur / max;
            double v = Math.Sqrt(m.Skills[skill].Value * 0.005);

            n *= (1.0 - v);
            n += v;

            m.CheckSkill(skill, n);
        }

        public static bool CheckTransform(Mobile m, Type type)
        {
            return TransformationSpellHelper.UnderTransformation(m, type);
        }

        public static bool CheckAnimal(Mobile m, Type type)
        {
            return AnimalForm.UnderTransformation(m, type);
        }

        private static TimeSpan Mobile_HitsRegenRate(Mobile from)
        {
            return TimeSpan.FromSeconds(1.0 / (0.1 * (1 + HitPointRegen(from))));
        }

        private static TimeSpan Mobile_StamRegenRate(Mobile from)
        {
            if (from.Skills == null)
                return Mobile.DefaultStamRate;

            CheckBonusSkill(from, from.Stam, from.StamMax, SkillName.Focus);

            int bonus = (int)(from.Skills[SkillName.Focus].Value * 0.1);

            bonus += StamRegen(from);

            if (Core.SA)
            {
                return TimeSpan.FromSeconds(1.0 / (1.42 + (bonus / 100)));
            }
            else
            {
                return TimeSpan.FromSeconds(1.0 / (0.1 * (2 + bonus)));
            }
        }

        private static TimeSpan Mobile_ManaRegenRate(Mobile from)
        {
            if (from.Skills == null)
                return Mobile.DefaultManaRate;

            if (!from.Meditating)
                CheckBonusSkill(from, from.Mana, from.ManaMax, SkillName.Meditation);


            double rate;
            double armorPenalty = GetArmorOffset(from);
            double regen_penalty = armorPenalty * 2;

            /*if (Core.ML)
            {
                double med = from.Skills[SkillName.Meditation].Value;
                double focus = from.Skills[SkillName.Focus].Value;

                double focusBonus = focus / 200;
                double medBonus = 0;

                CheckBonusSkill(from, from.Mana, from.ManaMax, SkillName.Focus);

                if (armorPenalty == 0)
                {
                    medBonus = (0.0075 * med) + (0.0025 * from.Int);

                    if (medBonus >= 100.0)
                        medBonus *= 1.1;

                    if (from.Meditating)
                    {
                        medBonus *= 2;
                    }
                }

                double itemBase = ((((med / 2) + (focus / 4)) / 90) * .65) + 2.35;
                double intensityBonus = Math.Sqrt(ManaRegen(from));

                if (intensityBonus > 5.5)
                    intensityBonus = 5.5;

                double itemBonus = ((itemBase * intensityBonus) - (itemBase - 1)) / 10;

                rate = 1.0 / (0.2 + focusBonus + medBonus + itemBonus);
            }
            else if (Core.AOS)
            {
                double medPoints = from.Int + (from.Skills[SkillName.Meditation].Value * 3);

                medPoints *= (from.Skills[SkillName.Meditation].Value < 100.0) ? 0.025 : 0.0275;

                CheckBonusSkill(from, from.Mana, from.ManaMax, SkillName.Focus);

                double focusPoints = (from.Skills[SkillName.Focus].Value * 0.05);

                if (armorPenalty > 0)
                    medPoints = 0; // In AOS, wearing any meditation-blocking armor completely removes meditation bonus

                double totalPoints = focusPoints + medPoints + (from.Meditating ? (medPoints > 13.0 ? 13.0 : medPoints) : 0.0);

                totalPoints += ManaRegen(from);

                if (totalPoints < -1)
                    totalPoints = -1;

                if (Core.ML)
                    totalPoints = Math.Floor(totalPoints);

                rate = 1.0 / (0.1 * (2 + totalPoints));
            }
            else
            {*/
            //double medPoints = (from.Int + from.Skills[SkillName.Meditation].Value) * 0.5;

            double skill = from.Skills[SkillName.Meditation].Value - armorPenalty;


            //if (medPoints <= 0)

            /*else if (medPoints <= 100)
                rate = 7.0 - (239 * medPoints / 2400) + (19 * medPoints * medPoints / 48000);
            else if (medPoints < 120)
                rate = 1.0;
            else
                rate = 0.75;*/


            rate = 2.0;

            if (regen_penalty < 100)
            {
                rate *= (regen_penalty / 100) + 1;
            }
            else if (regen_penalty < 200)
            {
                rate *= 2;
                rate *= ((regen_penalty - 100) / 100) + 1;
            }
            else
            {
                rate *= 4;
                rate *= ((regen_penalty - 200) / 100) + 1;
            }

                if (from.Meditating)
            {
                rate *= 0.5;
            }
            if (skill > 0)
            {
                rate *= 1 / ((skill / 100) + 1);
            }

                if (rate < 0.5)
                    rate = 0.5;
                else if (rate > 8.0)
                    rate = 8.0;

                
            

            return TimeSpan.FromSeconds(rate);
        }

        public static int HitPointRegen(Mobile from)
        {
            int points = AosAttributes.GetValue(from, AosAttribute.RegenHits);

            if (from is BaseCreature)
                points += ((BaseCreature)from).DefaultHitsRegen;

            if (Core.ML && from is PlayerMobile && from.Race == Race.Human)	//Is this affected by the cap?
                points += 2;

            if (points < 0)
                points = 0;

            if (Core.ML && from is PlayerMobile)	//does racial bonus go before/after?
                points = Math.Min(points, 18);

            if (CheckTransform(from, typeof(HorrificBeastSpell)))
                points += 20;

            if (CheckAnimal(from, typeof(Dog)) || CheckAnimal(from, typeof(Cat)))
                points += from.Skills[SkillName.Ninjitsu].Fixed / 30;

            // Skill Masteries - goes after cap
            points += RampageSpell.GetBonus(from, RampageSpell.BonusType.HitPointRegen);
            points += CombatTrainingSpell.RegenBonus(from);
            points += BarrabHemolymphConcentrate.HPRegenBonus(from);

            if (Core.AOS)
                foreach (RegenBonusHandler handler in HitsBonusHandlers)
                    points += handler(from);

            return points;
        }

        public static int StamRegen(Mobile from)
        {
            int points = AosAttributes.GetValue(from, AosAttribute.RegenStam);

            if (from is BaseCreature)
                points += ((BaseCreature)from).DefaultStamRegen;

            if (CheckTransform(from, typeof(VampiricEmbraceSpell)))
                points += 15;

            if (CheckAnimal(from, typeof(Kirin)))
                points += 20;

            if (Core.ML && from is PlayerMobile)
                points = Math.Min(points, 24);

            // Skill Masteries - goes after cap
            points += RampageSpell.GetBonus(from, RampageSpell.BonusType.StamRegen);

            if (points < -1)
                points = -1;

            if (Core.AOS)
                foreach (RegenBonusHandler handler in StamBonusHandlers)
                    points += handler(from);

            return points;
        }

        public static int ManaRegen(Mobile from)
        {
            int points = AosAttributes.GetValue(from, AosAttribute.RegenMana);

            if (from is BaseCreature)
                points += ((BaseCreature)from).DefaultManaRegen;

            if (CheckTransform(from, typeof(VampiricEmbraceSpell)))
                points += 3;
            else if (CheckTransform(from, typeof(LichFormSpell)))
                points += 13;

            if (from is PlayerMobile && from.Race == Race.Gargoyle)
                points += 2;

            if (!Core.ML && from is PlayerMobile)
                points = Math.Min(points, 18);

            foreach (RegenBonusHandler handler in ManaBonusHandlers)
                points += handler(from);

            return points;
        }

        private static double GetArmorMeditationValue(BaseArmor ar)
        {
            if (ar == null)// || ar.ArmorAttributes.MageArmor != 0 || ar.Attributes.SpellChanneling != 0)
                return 0.0;

            switch ( ar.MeditationAllowance )
            {
                default:
                case ArmorMeditationAllowance.None:
                    return ar.BaseArmorRatingScaled;
                case ArmorMeditationAllowance.Half:
                    return ar.BaseArmorRatingScaled / 2.0;
                case ArmorMeditationAllowance.All:
                    return 0.0;
            }
        }
    }
}
