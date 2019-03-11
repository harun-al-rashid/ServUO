using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DancingMaster : BaseQuest
    {
        public override bool DoneOnce
        {
            get
            {
                return true;
            }
        }

        /* En Guarde! */
        public override object Title
        {
            get
            {
                return "The Dancing Master";
            }
        }

        /* Head NW out of town and find the old cemetary. Battle monster there until you have raised your Fecning skill to 50.
        Well hello there, lad. Fighting with elegance and percision is far more enriching than slugging an enemy with a 
        club or butchering an enemy with a sword. Learn the art of Fencing if you want to master combat and look good 
        doing it! The key to being a successful fencer is to be the complement and not the opposition to your opponent's 
        strength. Watch for your opponent to become off balance. Then finish him off with finesses and flair. There are 
        some undead that need cleansing out in the Old Britanian cemetary towards the North West. Head over there and slay them, but remember, 
        do it with style! Come back to me once you have achived the rank of Apprentice Fencer, and i will reward you 
        with a prize. */
        public override object Description
        {
            get
            {
                return "Head North past the Blacksmith's to the graveyard. Battle the undead there until your Fencing skill is above 50. Come back and I will reward you with a prize.";
            }
        }

        /* I understand, lad. Being a hero isn't for eeryone. Run along, then. Come back to me if you change your mind. */
        public override object Refuse
        {
            get
            {
                return "Come back if you change your mind.";
            }
        }

        /* You're doing well so far, but you're not quite ready yet. Head back to Old Haven, to the East, and kill some 
        more undead. */
        public override object Uncomplete
        {
            get
            {
                return "You're not ready yet. Raise your skill some more.";
            }
        }

        /* Excellent! You are beginning to appreciate the art of Fencing. I told you fighting with elegance and precision 
        is more enriching than fighting like an orge. Since you have returned victorious, please take this war fork and 
        use it well. The war fork is a finesse weapon, and this one is magical! I call it "Recaro's Riposte". With it, 
        you will be able to parry and counterstrike with ease! your enemies will bask in your greatness and glory! Good 
        luck to you, lad, abd keep practicing! */
        public override object Complete
        {
            get
            {
                return "You have earned your reward! Take this blade and wear it with honour.";
            }
        }

        public DancingMaster()
            : base()
        {
            this.AddObjective(new ApprenticeObjective(SkillName.Fencing, 50, "Britain", 1078188, 1078189));

            // 1078188 You feel more dexterous and quick witted while practicing combat here. Your ability to raise your Fencing skill is enhanced in this area.
            // 1078189 You feel less dexterous here. Your Fencing learning potential is no longer enhanced.

            this.AddReward(new BaseReward(typeof(AryasNeedle), "Needle"));
        }

        public override bool CanOffer()
        {
            #region Scroll of Alacrity
            PlayerMobile pm = this.Owner as PlayerMobile;
            if (pm.AcceleratedStart > DateTime.UtcNow)
            {
                this.Owner.SendLocalizedMessage(1077951); // You are already under the effect of an accelerated skillgain scroll.
                return false;
            }
            #endregion
            else
                return this.Owner.Skills.Fencing.Base > 40;
        }

        public override void OnCompleted()
        {
            this.Owner.SendLocalizedMessage(1078193, null, 0x23); // You have achieved the rank of Apprentice Fencer. Return to Recaro in New Haven to claim your reward.
            this.Owner.PlaySound(this.CompleteSound);
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

    public class Syrio : MondainQuester
    {
        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DancingMaster)
                };
            }
        }

        [Constructable]
        public Syrio()
            : base("Syrio", "The Dancing Master")
        {
            this.SetSkill(SkillName.Anatomy, 120.0, 120.0);
            this.SetSkill(SkillName.Parry, 120.0, 120.0);
            this.SetSkill(SkillName.Healing, 120.0, 120.0);
            this.SetSkill(SkillName.Tactics, 120.0, 120.0);
            this.SetSkill(SkillName.Fencing, 120.0, 120.0);
            this.SetSkill(SkillName.Focus, 120.0, 120.0);
        }

        public Syrio(Serial serial)
            : base(serial)
        {
        }

        public override void Advertise()
        {
            this.Say(1078187); // The art of fencing requires a dexterous hand, a quick wit and fleet feet.
        }

        public override void OnOfferFailed()
        {
            this.Say(1077772); // I cannot teach you, for you know all I can teach!
        }

        public override void InitBody()
        {
            this.Female = false;
            this.CantWalk = true;
            this.Race = Race.Human;

            base.InitBody();
        }

        public override void InitOutfit()
        {
            this.AddItem(new Backpack());
            this.AddItem(new Shoes(0x455));
            this.AddItem(new Kryss());

            Item item;

            item = new LeatherLegs();
            item.Hue = 0x455;
            this.AddItem(item);

            item = new LeatherGloves();
            item.Hue = 0x455;
            this.AddItem(item);

            item = new LeatherGorget();
            item.Hue = 0x455;
            this.AddItem(item);

            item = new LeatherChest();
            item.Hue = 0x455;
            this.AddItem(item);

            item = new LeatherArms();
            item.Hue = 0x455;
            this.AddItem(item);
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