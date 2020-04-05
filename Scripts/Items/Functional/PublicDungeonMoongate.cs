#region References
using System;
using System.Collections.Generic;
using System.Linq;

using Server.Commands;
using Server.Engines.CityLoyalty;
using Server.Factions;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
#endregion

namespace Server.Items
{
    public class PublicDungeonMoongate : Item
    {
        public static List<PublicDungeonMoongate> DungeonMoongates { get; private set; }

        static PublicDungeonMoongate()
        {
            DungeonMoongates = new List<PublicDungeonMoongate>();
        }

        public static void Initialize()
        {
            CommandSystem.Register("MoonDunGen", AccessLevel.Administrator, MoonDunGen_OnCommand);
            CommandSystem.Register("MoonDunGenDelete", AccessLevel.Administrator, MoonDunGenDelete_OnCommand);
        }

        [Usage("MoonDunGen")]
        [Description("Generates public dungeon moongates. Removes all old moongates.")]
        public static void MoonDunGen_OnCommand(CommandEventArgs e)
        {
            DeleteAll();

            var count = 0;

            //if (!Siege.SiegeShard)
            //{
            //    count += MoonGen(PDMList.Trammel);
            //}

            //count += MoonGen(PDMList.Felucca);
            //count += MoonGen(PDMList.Ilshenar);
            //count += MoonGen(PDMList.Malas);
            //count += MoonGen(PDMList.Tokuno);
            //count += MoonGen(PDMList.TerMur);
            //count += MoonGen(PDMList.Dungeons);

            count += MoonDunGen(PDMList.Dungeons);

            World.Broadcast(0x35, true, "{0} moongates generated.", count);
        }

        private static int MoonDunGen(PDMList list)
        {
            foreach (var entry in list.Entries)
            {
                var o = new PublicDungeonMoongate();

                o.MoveToWorld(entry.Location, list.Map);

                //if (entry.Number == 1060642) // Umbra
                //{
                //    o.Hue = 0x497;
                //}

                //if (entry.Number == 1075706)
                //{
                    o.Hue = 0x4AA;
                //}


            }

            return list.Entries.Length;
        }

        [Usage("MoonDunGenDelete")]
        [Description("Removes all public moongates.")]
        public static void MoonDunGenDelete_OnCommand(CommandEventArgs e)
        {
            DeleteAll();
        }

        public static void DeleteAll()
        {
            var count = DungeonMoongates.Count;

            var index = count;

            while (--index >= 0)
            {
                if (index < DungeonMoongates.Count)
                    DungeonMoongates[index].Delete();
            }

            DungeonMoongates.Clear();

            if (count > 0)
            {
                World.Broadcast(0x35, true, "{0:#,0} moongates removed.", count);
            }
        }

        public static IEnumerable<PublicDungeonMoongate> FindGates(Map map)
        {
            PublicDungeonMoongate o;

            var i = DungeonMoongates.Count;

            while (--i >= 0)
            {
                o = DungeonMoongates[i];

                if (o == null || o.Deleted)
                {
                    DungeonMoongates.RemoveAt(i);
                }
                else if (o.Map == map)
                {
                    yield return o;
                }
            }
        }

        public override string DefaultName { get { return "Moongate"; } }

        public override bool HandlesOnMovement { get { return true; } }
        public override bool ForceShowProperties { get { return true; } }

        [Constructable]
        public PublicDungeonMoongate()
            : base(0xF6C)//0xF6C
        {
            Movable = false;
            Light = LightType.Circle300;

            DungeonMoongates.Add(this);
        }

        public PublicDungeonMoongate(Serial serial)
            : base(serial)
        {
            DungeonMoongates.Add(this);
        }

        public override void OnDelete()
        {
            base.OnDelete();

            DungeonMoongates.Remove(this);
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            DungeonMoongates.Remove(this);
        }

        public override void OnDoubleClick(Mobile m)
        {
            UseGate(m);
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (m.Player && m.CanSee(this))
            {
                UseGate(m);
            }

            return m.Map == Map && m.InRange(this, 1);
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m.Player && !Utility.InRange(m.Location, Location, 1) && Utility.InRange(oldLocation, Location, 1))
            {
                m.CloseGump(typeof(DungeonMoongateGump));
            }
        }

        public virtual bool CanUseGate(Mobile m, bool message)
        {
            if (m.IsStaff())
            {
                //Staff can always use a gate!
                return true;
            }

            if (m.Criminal)
            {
                // Thou'rt a criminal and cannot escape so easily.
                m.SendLocalizedMessage(1005561, "", 0x22);
                return false;
            }

            if (SpellHelper.CheckCombat(m))
            {
                // Wouldst thou flee during the heat of battle??
                m.SendLocalizedMessage(1005564, "", 0x22);
                return false;
            }

            if (m.Spell != null)
            {
                // You are too busy to do that at the moment.
                m.SendLocalizedMessage(1049616);
                return false;
            }

            if (m.Holding != null)
            {
                // You cannot teleport while dragging an object.
                m.SendLocalizedMessage(1071955);
                return false;
            }

            return true;
        }

        public bool UseGate(Mobile m)
        {
            if (!CanUseGate(m, true))
            {
                return false;
            }

            m.CloseGump(typeof(DungeonMoongateGump));
            m.SendGump(new DungeonMoongateGump(m, this));

            PlaySound(m);

            return true;
        }

        public virtual void PlaySound(Mobile m)
        {
            if (!m.Hidden || m.IsPlayer())
            {
                Effects.PlaySound(m.Location, m.Map, 0x20E);
            }
            else
            {
                m.SendSound(0x20E);
            }
        }

        protected PDMEntry FindEntry()
        {
            return FindEntry(PDMList.GetList(Map));
        }

        protected PDMEntry FindEntry(PDMList list)
        {
            if (list != null)
            {
                return PDMList.FindEntry(list, Location);
            }

            return null;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.ReadInt();
        }
    }

    public class PDMEntry
    {
        public Point3D Location { get; private set; }
        public int Number { get; private set; }
        public TextDefinition Desc { get; private set; }

        public PDMEntry(Point3D loc, int number)
            : this(loc, number, String.Empty)
        { }

        public PDMEntry(Point3D loc, int number, TextDefinition desc)
        {
            Location = loc;
            Number = number;
            Desc = desc;
        }
    }

    public class PDMList
    {
        /*public static readonly PMList Trammel = new PMList(
            1012000,
            1012012,
            Map.Trammel,
            new[]
            {
                new PMEntry(new Point3D(4467, 1283, 5), 1012003), // Moonglow
				new PMEntry(new Point3D(1336, 1997, 5), 1012004), // Britain
				new PMEntry(new Point3D(1499, 3771, 5), 1012005), // Jhelom
				new PMEntry(new Point3D(771, 752, 5), 1012006), // Yew
				new PMEntry(new Point3D(2701, 692, 5), 1012007), // Minoc
				new PMEntry(new Point3D(1828, 2948, -20), 1012008), // Trinsic
				new PMEntry(new Point3D(643, 2067, 5), 1012009), // Skara Brae
				/* Dynamic Z for Magincia to support both old and new maps. 
				new PMEntry(new Point3D(3563, 2139, Map.Trammel.GetAverageZ(3563, 2139)), 1012010), // (New) Magincia
				new PMEntry(new Point3D(3450, 2677, 25), 1078098) // New Haven
            });

        public static readonly PMList Felucca = new PMList(
            1012001,
            1012013,
            Map.Felucca,
            new[]
            {
                new PMEntry(new Point3D(4467, 1283, 5), 1012003), // Moonglow
				new PMEntry(new Point3D(1336, 1997, 5), 1012004), // Britain
				new PMEntry(new Point3D(1499, 3771, 5), 1012005), // Jhelom
				new PMEntry(new Point3D(771, 752, 5), 1012006), // Yew
				new PMEntry(new Point3D(2701, 692, 5), 1012007), // Minoc
				new PMEntry(new Point3D(1828, 2948, -20), 1012008), // Trinsic
				new PMEntry(new Point3D(643, 2067, 5), 1012009), // Skara Brae
				/* Dynamic Z for Magincia to support both old and new maps. 
				new PMEntry(new Point3D(3563, 2139, Map.Felucca.GetAverageZ(3563, 2139)), 1012010), // (New) Magincia
				new PMEntry(new Point3D(2711, 2234, 0), 1019001), // Buccaneer's Den
                new PMEntry(new Point3D(2495, 948, 5), 1075706)
            });
    */
        public static readonly PDMList Dungeons = new PDMList(
            1078373,
            1078373,
            Map.Trammel,
            new[]
            {
                new PDMEntry(new Point3D(1372, 1463, 10), 1011028), //Brit Cemetary
                new PDMEntry(new Point3D(2495, 948, 0), 1075706), //Covetous
                new PDMEntry(new Point3D(2016, 239, 14), 1078301), //Wrong
                new PDMEntry(new Point3D(4119, 442, 5), 1078299), //Deceit
                new PDMEntry(new Point3D(1179, 2650, 0), 1078300), //Destard
                new PDMEntry(new Point3D(4732, 3831, 0), 1075705), //Hythloth
                new PDMEntry(new Point3D(495, 1581, 0), 1075707), //Shame
                new PDMEntry(new Point3D(1325, 1092, 0), 1075704) //Despise
            });
/*

        public static readonly PMList Ilshenar = new PMList(
            1012002,
            1012014,
            Map.Ilshenar,
            new[]
            {
                new PMEntry(new Point3D(1215, 467, -13), 1012015), // Compassion
				new PMEntry(new Point3D(722, 1366, -60), 1012016), // Honesty
				new PMEntry(new Point3D(744, 724, -28), 1012017), // Honor
				new PMEntry(new Point3D(281, 1016, 0), 1012018), // Humility
				new PMEntry(new Point3D(987, 1011, -32), 1012019), // Justice
				new PMEntry(new Point3D(1174, 1286, -30), 1012020), // Sacrifice
				new PMEntry(new Point3D(1532, 1340, -3), 1012021), // Spirituality
				new PMEntry(new Point3D(528, 216, -45), 1012022), // Valor
				new PMEntry(new Point3D(1721, 218, 96), 1019000) // Chaos
			});

        public static readonly PMList Malas = new PMList(
            1060643,
            1062039,
            Map.Malas,
            new[]
            {
                new PMEntry(new Point3D(1015, 527, -65), 1060641), // Luna
				new PMEntry(new Point3D(1997, 1386, -85), 1060642) // Umbra
			});

        public static readonly PMList Tokuno = new PMList(
            1063258,
            1063415,
            Map.Tokuno,
            new[]
            {
                new PMEntry(new Point3D(1169, 998, 41), 1063412), // Isamu-Jima
				new PMEntry(new Point3D(802, 1204, 25), 1063413), // Makoto-Jima
				new PMEntry(new Point3D(270, 628, 15), 1063414) // Homare-Jima
			});

        public static readonly PMList TerMur = new PMList(
            1113602,
            1113604,
            Map.TerMur,
            new[]
            {
                new PMEntry(new Point3D(850, 3525, -38), 1113603), // Royal City
				Core.TOL
                    ? new PMEntry(new Point3D(719, 1863, 40), 1156262)
                    : new PMEntry(new Point3D(926, 3989, -36), 1112572) // Valley of Eodon
				// Holy City
			});
*/
        public static readonly PDMList[] UORLists = { Dungeons };
        public static readonly PDMList[] UORListsYoung = { Dungeons };
        public static readonly PDMList[] LBRLists = { Dungeons };
        public static readonly PDMList[] LBRListsYoung = { Dungeons };
        public static readonly PDMList[] AOSLists = { Dungeons };
        public static readonly PDMList[] AOSListsYoung = { Dungeons };
        public static readonly PDMList[] SELists = { Dungeons };
        public static readonly PDMList[] SEListsYoung = { Dungeons };
        public static readonly PDMList[] SALists = { Dungeons };
        public static readonly PDMList[] SAListsYoung = { Dungeons };
        public static readonly PDMList[] RedLists = { Dungeons };
        public static readonly PDMList[] SigilLists = { Dungeons };

        public static readonly PDMList[] AllLists = { Dungeons };

        public static PDMList GetList(Map map)
        {
            if (map == null || map == Map.Internal)
            {
                return null;
            }

            if (map == Map.Trammel)
            {
                return Dungeons;
            }

            if (map == Map.Felucca)
            {
                return Dungeons;
            }

            if (map == Map.Ilshenar)
            {
                return Dungeons;
            }

            if (map == Map.Malas)
            {
                return Dungeons;
            }

            if (map == Map.Tokuno)
            {
                return Dungeons;
            }

            if (map == Map.TerMur)
            {
                return Dungeons;
            }

            return null;
        }

        public static int IndexOfEntry(PDMEntry entry)
        {
            var list = AllLists.FirstOrDefault(o => o.Entries.Contains(entry));

            return IndexOfEntry(list, entry);
        }

        public static int IndexOfEntry(PDMList list, PDMEntry entry)
        {
            if (list != null && entry != null)
            {
                return Array.IndexOf(list.Entries, entry);
            }

            return -1;
        }

        public static PDMEntry FindEntry(PDMList list, Point3D loc)
        {
            if (list != null)
            {
                return list.Entries.FirstOrDefault(o => o.Location == loc);
            }

            return null;
        }

        public static PDMEntry FindEntry(Map map, Point3D loc)
        {
            var list = GetList(map);

            if (list != null)
            {
                return FindEntry(list, loc);
            }

            return null;
        }

        private readonly int m_Number;
        private readonly int m_SelNumber;
        private readonly Map m_Map;
        private readonly PDMEntry[] m_Entries;

        public PDMList(int number, int selNumber, Map map, PDMEntry[] entries)
        {
            m_Number = number;
            m_SelNumber = selNumber;
            m_Map = map;
            m_Entries = entries;
        }

        public int Number { get { return m_Number; } }
        public int SelNumber { get { return m_SelNumber; } }
        public Map Map { get { return m_Map; } }
        public PDMEntry[] Entries { get { return m_Entries; } }
    }

    public class DungeonMoongateGump : Gump
    {
        private readonly Mobile m_Mobile;
        private readonly Item m_Moongate;
        private readonly PDMList[] m_Lists;

        public DungeonMoongateGump(Mobile mobile, Item moongate)
            : base(100, 100)
        {
            m_Mobile = mobile;
            m_Moongate = moongate;

            PDMList[] checkLists;

            if (mobile.Player)
            {
                if (mobile.IsStaff())
                {
                    var flags = mobile.NetState == null ? ClientFlags.None : mobile.NetState.Flags;

                    if (Core.SA && (flags & ClientFlags.TerMur) != 0)
                    {
                        checkLists = PDMList.SALists;
                    }
                    else if (Core.SE && (flags & ClientFlags.Tokuno) != 0)
                    {
                        checkLists = PDMList.SELists;
                    }
                    else if (Core.AOS && (flags & ClientFlags.Malas) != 0)
                    {
                        checkLists = PDMList.AOSLists;
                    }
                    else if ((flags & ClientFlags.Ilshenar) != 0)
                    {
                        checkLists = PDMList.LBRLists;
                    }
                    else
                    {
                        checkLists = PDMList.UORLists;
                    }
                }
                else if (Sigil.ExistsOn(mobile))
                {
                    checkLists = PDMList.SigilLists;
                }
                else if (mobile.Murderer && !Siege.SiegeShard)
                {
                    checkLists = PDMList.RedLists;
                }
                else
                {
                    var flags = mobile.NetState == null ? ClientFlags.None : mobile.NetState.Flags;
                    var young = mobile is PlayerMobile && ((PlayerMobile)mobile).Young;

                    if (Core.SA && (flags & ClientFlags.TerMur) != 0)
                    {
                        checkLists = young ? PDMList.SAListsYoung : PDMList.SALists;
                    }
                    else if (Core.SE && (flags & ClientFlags.Tokuno) != 0)
                    {
                        checkLists = young ? PDMList.SEListsYoung : PDMList.SELists;
                    }
                    else if (Core.AOS && (flags & ClientFlags.Malas) != 0)
                    {
                        checkLists = young ? PDMList.AOSListsYoung : PDMList.AOSLists;
                    }
                    else if ((flags & ClientFlags.Ilshenar) != 0)
                    {
                        checkLists = young ? PDMList.LBRListsYoung : PDMList.LBRLists;
                    }
                    else
                    {
                        checkLists = young ? PDMList.UORListsYoung : PDMList.UORLists;
                    }
                }
            }
            else
            {
                checkLists = PDMList.SELists;
            }

            m_Lists = new PDMList[checkLists.Length];

            for (var i = 0; i < m_Lists.Length; ++i)
            {
                m_Lists[i] = checkLists[i];
            }

            for (var i = 0; i < m_Lists.Length; ++i)
            {
                if (m_Lists[i].Map == mobile.Map)
                {
                    var temp = m_Lists[i];

                    m_Lists[i] = m_Lists[0];
                    m_Lists[0] = temp;

                    break;
                }
            }

            AddPage(0);

            AddBackground(0, 0, 380, 280, 5054);

            AddButton(10, 210, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddHtmlLocalized(45, 210, 140, 25, 1011036, false, false); // OKAY

            AddButton(10, 235, 4005, 4007, 0, GumpButtonType.Reply, 0);
            AddHtmlLocalized(45, 235, 140, 25, 1011012, false, false); // CANCEL

            AddHtmlLocalized(5, 5, 200, 20, 1012011, false, false); // Pick your destination:

            for (var i = 0; i < checkLists.Length; ++i)
            {
                if (Siege.SiegeShard && checkLists[i].Number == 1012000) // Trammel
                {
                    continue;
                }

                AddButton(10, 35 + (i * 25), 2117, 2118, 0, GumpButtonType.Page, Array.IndexOf(m_Lists, checkLists[i]) + 1);
                AddHtmlLocalized(30, 35 + (i * 25), 150, 20, checkLists[i].Number, false, false);
            }

            for (var i = 0; i < m_Lists.Length; ++i)
            {
                RenderPage(i, Array.IndexOf(checkLists, m_Lists[i]));
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (info.ButtonID == 0) // Cancel
            {
                return;
            }
            if (m_Mobile.Deleted || m_Moongate.Deleted || m_Mobile.Map == null)
            {
                return;
            }

            var switches = info.Switches;

            if (switches.Length == 0)
            {
                return;
            }

            var switchID = switches[0];
            var listIndex = switchID / 100;
            var listEntry = switchID % 100;

            if (listIndex < 0 || listIndex >= m_Lists.Length)
            {
                return;
            }

            var list = m_Lists[listIndex];

            if (listEntry < 0 || listEntry >= list.Entries.Length)
            {
                return;
            }

            var entry = list.Entries[listEntry];

            if (m_Mobile.Map == list.Map && m_Mobile.InRange(entry.Location, 1))
            {
                m_Mobile.SendLocalizedMessage(1019003); // You are already there.
                return;
            }
            if (m_Mobile.IsStaff())
            {
                //Staff can always use a gate!
            }
            else if (!m_Mobile.InRange(m_Moongate.GetWorldLocation(), 1) || m_Mobile.Map != m_Moongate.Map)
            {
                m_Mobile.SendLocalizedMessage(1019002); // You are too far away to use the gate.
                return;
            }
            else if (m_Mobile.Player && m_Mobile.Murderer && list.Map != Map.Felucca && !Siege.SiegeShard)
            {
                m_Mobile.SendLocalizedMessage(1019004); // You are not allowed to travel there.
                return;
            }
            else if (Sigil.ExistsOn(m_Mobile) && list.Map != Faction.Facet)
            {
                m_Mobile.SendLocalizedMessage(1019004); // You are not allowed to travel there.
                return;
            }
            else if (m_Mobile.Criminal)
            {
                m_Mobile.SendLocalizedMessage(1005561, "", 0x22); // Thou'rt a criminal and cannot escape so easily.
                return;
            }
            else if (SpellHelper.CheckCombat(m_Mobile))
            {
                m_Mobile.SendLocalizedMessage(1005564, "", 0x22); // Wouldst thou flee during the heat of battle??
                return;
            }
            else if (m_Mobile.Spell != null)
            {
                m_Mobile.SendLocalizedMessage(1049616); // You are too busy to do that at the moment.
                return;
            }

            BaseCreature.TeleportPets(m_Mobile, entry.Location, list.Map);

            m_Mobile.Combatant = null;
            m_Mobile.Warmode = false;
            m_Mobile.Hidden = true;

            m_Mobile.MoveToWorld(entry.Location, list.Map);

            Effects.PlaySound(entry.Location, list.Map, 0x1FE);

            CityTradeSystem.OnPublicMoongateUsed(m_Mobile);
        }

        private void RenderPage(int index, int offset)
        {
            var list = m_Lists[index];

            if (Siege.SiegeShard && list.Number == 1012000) // Trammel
                return;

            AddPage(index + 1);

            AddButton(10, 35 + (offset * 25), 2117, 2118, 0, GumpButtonType.Page, index + 1);
            AddHtmlLocalized(30, 35 + (offset * 25), 150, 20, list.SelNumber, false, false);

            var entries = list.Entries;

            for (var i = 0; i < entries.Length; ++i)
            {
                AddRadio(200, 35 + (i * 25), 210, 211, false, (index * 100) + i);
                AddHtmlLocalized(225, 35 + (i * 25), 150, 20, entries[i].Number, false, false);
            }
        }
    }
}