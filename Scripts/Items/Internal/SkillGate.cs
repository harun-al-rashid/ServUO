#region References

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Server.Gumps;
using Server.Network;

#endregion

namespace Server
{
    public class SkillGate : Item
    {
        public const int NumberOfChoices = 7;
        public const double AmountToRaiseTo = 100.0;

        [Constructable]
        public SkillGate() : base(0xf6c)
        {
            Name = "Enter to boost your stats";
            Weight = 1.0;
            Hue = 1462;
            LootType = LootType.Blessed;
        }

        public SkillGate( Serial serial ) : base( serial ) 
		{ 
		}

        public override bool OnMoveOver(Mobile from)
        {
			/*
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1060640);
                return true;
            }
			*/

            if(from.HasGump(typeof(GateOfCharacterDevelopmentGump)))
            {
                from.CloseGump(typeof (GateOfCharacterDevelopmentGump));
            }

            Movable = false;
            from.SendGump(new GateOfCharacterDevelopmentGump(this)); 

			return true;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            list.Add("Sets All Skills to 0, Then Lets You Choose {0} Skills To Boost To {1}", NumberOfChoices, AmountToRaiseTo);
        }

        public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 0 ); // version 
		}

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
		} 
    }

    public class GateOfCharacterDevelopmentGump : Gump
    {
        private readonly SkillGate m_Tome;

        private static readonly List<SEntry> Skills = new List<SEntry>
        {
            new SEntry(0, SkillName.Alchemy, "Alchemy"),
            new SEntry(1, SkillName.Anatomy, "Anatomy"),
            new SEntry(2, SkillName.AnimalLore, "Animal Lore"),
            new SEntry(3, SkillName.ItemID, "Item Identification"),
            new SEntry(4, SkillName.ArmsLore, "Arms Lore"),
            new SEntry(5, SkillName.Parry, "Parrying"),
            new SEntry(6, SkillName.Begging, "Begging"),
            new SEntry(7, SkillName.Blacksmith, "Blacksmithy"),
            new SEntry(8, SkillName.Fletching, "Fletching"),
            new SEntry(9, SkillName.Peacemaking, "Peacemaking"),
            new SEntry(10, SkillName.Camping, "Camping"),
            new SEntry(11, SkillName.Carpentry, "Carpentry"),
            new SEntry(12, SkillName.Cartography, "Cartography"),
            new SEntry(13, SkillName.Cooking, "Cooking"),
            new SEntry(14, SkillName.DetectHidden, "Detect Hidden"),
            new SEntry(15, SkillName.Discordance, "Discordance"),
            new SEntry(16, SkillName.EvalInt, "Evaluating Intelligence"),
            new SEntry(17, SkillName.Healing, "Healing"),
            new SEntry(18, SkillName.Fishing, "Fishing"),
            new SEntry(19, SkillName.Forensics, "Forensic Evaluation"),
            new SEntry(20, SkillName.Herding, "Herding"),
            new SEntry(21, SkillName.Hiding, "Hiding"),
            new SEntry(22, SkillName.Provocation, "Provocation"),
            new SEntry(23, SkillName.Inscribe, "Inscription"),
            new SEntry(24, SkillName.Lockpicking, "Lock Picking"),
            new SEntry(25, SkillName.Magery, "Magery"),
            new SEntry(26, SkillName.MagicResist, "Resisting Magic"),
            new SEntry(27, SkillName.Tactics, "Tactics"),
            new SEntry(28, SkillName.Snooping, "Snooping"),
            new SEntry(29, SkillName.Musicianship, "Musicianship"),
            new SEntry(30, SkillName.Poisoning, "Poisoning"),
            new SEntry(31, SkillName.Archery, "Archery"),
            new SEntry(32, SkillName.SpiritSpeak, "Spirit Speak"),
            new SEntry(33, SkillName.Stealing, "Stealing"),
            new SEntry(34, SkillName.Tailoring, "Tailoring"),
            new SEntry(35, SkillName.AnimalTaming, "Animal Taming"),
            new SEntry(36, SkillName.TasteID, "Taste Identification"),
            new SEntry(37, SkillName.Tinkering, "Tinkering"),
            new SEntry(38, SkillName.Tracking, "Tracking"),
            new SEntry(39, SkillName.Veterinary, "Veterinary"),
            new SEntry(40, SkillName.Swords, "Swordsmanship"),
            new SEntry(41, SkillName.Macing, "Mace Fighting"),
            new SEntry(42, SkillName.Fencing, "Fencing"),
            new SEntry(43, SkillName.Wrestling, "Wrestling"),
            new SEntry(44, SkillName.Lumberjacking, "Lumberjacking"),
            new SEntry(45, SkillName.Mining, "Mining"),
            new SEntry(46, SkillName.Meditation, "Meditation"),
            new SEntry(47, SkillName.Stealth, "Stealth"),
            new SEntry(48, SkillName.RemoveTrap, "Removing Traps")
        };

        public GateOfCharacterDevelopmentGump(SkillGate tome) : base(0, 0)
        {
            m_Tome = tome;

            if (Core.AOS)
            {
                Skills.Add(new SEntry(49, SkillName.Necromancy, "Necromancy"));
                Skills.Add(new SEntry(50, SkillName.Focus, "Focus"));
                Skills.Add(new SEntry(51, SkillName.Chivalry, "Chivalry"));
            }

            if (Core.SE)
            {
                Skills.Add(new SEntry(52, SkillName.Bushido, "Bushido"));
                Skills.Add(new SEntry(53, SkillName.Ninjitsu, "Ninjitsu"));
            }

            if (Core.ML)
            {
                Skills.Add(new SEntry(54, SkillName.Spellweaving, "Spellweaving"));
            }

            if (Core.SA)
            {
                Skills.Add(new SEntry(55, SkillName.Mysticism, "Mysticism"));
                Skills.Add(new SEntry(56, SkillName.Imbuing, "Imbuing"));
                Skills.Add(new SEntry(57, SkillName.Throwing, "Throwing"));
            }

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(5, 4, 718, 573, 9380);
            AddImage(679, 108, 10441);
            AddLabel(43, 7, 167,
                String.Format("Select {0} skills to raise to {1}.", SkillGate.NumberOfChoices, (int) SkillGate.AmountToRaiseTo));
            AddButton(344, 553, 247, 248, 1, GumpButtonType.Reply, 0);

            int checkx = 43;
            int textx = 73;
            int y = 48;     

            bool firstcolumnpassed = false;
            bool secondcolumnpassed = false;

            foreach (SEntry skill in Skills)
            {
                AddCheck(checkx, y, 210, 211, false, skill.SkillID);
                AddLabel(textx, y, 887, skill.SkillString);        
          
                if (y > 491 && !firstcolumnpassed)
                {
                    y = 48;
                    checkx = 220;
                    textx = 250;
                    firstcolumnpassed = true;
                }
                else if (y > 491 && firstcolumnpassed && !secondcolumnpassed)
                {
                    y = 48;
                    checkx = 393;
                    textx = 423;
                    secondcolumnpassed = true;
                }
                else if (y > 491 && secondcolumnpassed)
                {
                    y = 48;
                    checkx = 546;
                    textx = 576;
                }        
                else
                {
                    y += 30;
                }
            }  
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile m = sender.Mobile;

            if (m == null || m.Deleted)
                return;

            switch (info.ButtonID)
            {
                case 0:
                {
                    m.SendMessage("You decide not to use the Gate of Skils just yet.");
                    m_Tome.Movable = true;
                    break;
                }
                case 1:
                {
                    if (info.Switches.Length > SkillGate.NumberOfChoices || info.Switches.Length < SkillGate.NumberOfChoices)
                    {
                        m.SendMessage("You must choose {0} skills, but you chose {1}. Please try again.", SkillGate.NumberOfChoices, info.Switches.Length);
                        return;
                    }
        
                    Skills skills = m.Skills;
                    for (int i = 0; i < skills.Length; ++i)
                        skills[i].Base = 0;              

                    foreach (SEntry entry in Skills.Where(entry => info.IsSwitched(entry.SkillID)))
                    {
                        m.Skills[entry.SkillName].Base = SkillGate.AmountToRaiseTo;
                    }

                    //m_Tome.Delete();

                    break;
                }
            }
        }
    }

    public class SEntry
    {
        private readonly SkillName m_SkillName;
        private readonly int m_SkillID;
        private readonly string m_SkillString;

        public SkillName SkillName
        {
            get { return m_SkillName; }
        }

        public int SkillID
        {
            get { return m_SkillID; }
        }

        public string SkillString
        {
            get { return m_SkillString; }
        }

        public SEntry(int id, SkillName skillname, string skillstring)
        {
            m_SkillID = id;
            m_SkillName = skillname;
            m_SkillString = skillstring;
        }
    }
}