#region References
using System;
using System.Collections.Generic;

using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Engines.Quests;
using System.Linq;
using Server.Engines.BulkOrders;
#endregion

namespace Server.Mobiles
{
	public class AnimalTrainer : BaseVendor
	{
		private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();

		[Constructable]
		public AnimalTrainer()
			: base("the animal trainer")
		{
			SetSkill(SkillName.AnimalLore, 64.0, 100.0);
			SetSkill(SkillName.AnimalTaming, 90.0, 100.0);
			SetSkill(SkillName.Veterinary, 65.0, 88.0);
		}

		public AnimalTrainer(Serial serial)
			: base(serial)
		{ }

		protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }
		public override VendorShoeType ShoeType { get { return Female ? VendorShoeType.ThighBoots : VendorShoeType.Boots; } }

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBAnimalTrainer());
		}

		public override int GetShoeHue()
		{
			return 0;
		}

		public override void InitOutfit()
		{
			base.InitOutfit();

			AddItem(Utility.RandomBool() ? new QuarterStaff() : (Item)new ShepherdsCrook());
		}

        #region Bulk Orders
        public override BODType BODType { get { return BODType.Taming; } }

        public override Item CreateBulkOrder(Mobile from, bool fromContextMenu)
        {
            PlayerMobile pm = from as PlayerMobile;

            if (pm != null && pm.NextSmithBulkOrder == TimeSpan.Zero && (fromContextMenu || 0.2 > Utility.RandomDouble()))
            {
                double theirSkill = pm.Skills[SkillName.AnimalTaming].Base;

                if (theirSkill >= 70.1)
                    pm.NextTamerBulkOrder = TimeSpan.FromHours(6.0);
                else if (theirSkill >= 50.1)
                    pm.NextTamerBulkOrder = TimeSpan.FromHours(2.0);
                else
                    pm.NextTamerBulkOrder = TimeSpan.FromHours(1.0);

                if (theirSkill >= 70.1 && ((theirSkill - 40.0) / 300.0) > Utility.RandomDouble())
                    return new LargeSmithBOD();

                return SmallSmithBOD.CreateRandomFor(from);
            }

            return null;
        }

        public override bool IsValidBulkOrder(Item item)
        {
            return (item is SmallSmithBOD || item is LargeSmithBOD);
        }

        public override bool SupportsBulkOrders(Mobile from)
        {
            return (from is PlayerMobile && from.Skills[SkillName.AnimalTaming].Base > 0);
        }

        public override TimeSpan GetNextBulkOrder(Mobile from)
        {
            if (from is PlayerMobile)
                return ((PlayerMobile)from).NextTamerBulkOrder;

            return TimeSpan.Zero;
        }

        public override void OnSuccessfulBulkOrderReceive(Mobile from)
        {
            if (Core.SE && from is PlayerMobile)
                ((PlayerMobile)from).NextTamerBulkOrder= TimeSpan.Zero;
        }

        #endregion

		public override void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
		{
			if (from.Alive)
			{
				list.Add(new StableEntry(this, from));

				if (from.Stabled.Count > 0)
				{
					list.Add(new ClaimAllEntry(this, from));
				}
			}

			base.AddCustomContextEntries(from, list);
		}

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (PetTrainingHelper.Enabled)
            {
                list.Add(1072269); // Quest Giver
            }
        }

        private DateTime _NextTalk;

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (PetTrainingHelper.Enabled && m.Alive && !m.Hidden && m is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)m;

                if (InLOS(m) && InRange(m, 8) && !InRange(oldLocation, 8) && DateTime.UtcNow >= _NextTalk)
                {
                    if (Utility.Random(100) < 50)
                        Say(1157526); // Such an exciting time to be an Animal Trainer! New taming techniques have been discovered!

                    _NextTalk = DateTime.UtcNow + TimeSpan.FromSeconds(15);
                }
            }
        }

        private Type[] _Quests = { typeof(TamingPetQuest), typeof(UsingAnimalLoreQuest), typeof(LeadingIntoBattleQuest), typeof(TeachingSomethingNewQuest) };

        public override void OnDoubleClick(Mobile m)
        {
            if (PetTrainingHelper.Enabled && m is PlayerMobile && m.InRange(Location, 5))
            {
                CheckQuest((PlayerMobile)m);
            }
        }

        public bool CheckQuest(PlayerMobile player)
        {
            for (int i = 0; i < _Quests.Length; i++)
            {
                var quest = player.Quests.FirstOrDefault(q => q.GetType() == _Quests[i]);

                if (quest != null)
                {
                    if (quest.Completed)
                    {
                        if (quest.GetType() != typeof(TeachingSomethingNewQuest))
                        {
                            quest.GiveRewards();
                        }
                        else
                        {
                            player.SendGump(new MondainQuestGump(quest, MondainQuestGump.Section.Complete, false, true));
                        }

                        return true;
                    }
                    else
                    {
                        player.SendGump(new MondainQuestGump(quest, MondainQuestGump.Section.InProgress, false));
                        quest.InProgress();
                    }
                }
            }

            BaseQuest questt = new TamingPetQuest();
            questt.Owner = player;
            questt.Quester = this;
            player.CloseGump(typeof(MondainQuestGump));
            player.SendGump(new MondainQuestGump(questt));

            return true;
        }

		public static int GetMaxStabled(Mobile from)
		{
			var taming = from.Skills[SkillName.AnimalTaming].Value;
			var anlore = from.Skills[SkillName.AnimalLore].Value;
			var vetern = from.Skills[SkillName.Veterinary].Value;
            var herd = from.Skills[SkillName.Herding].Value;
			var sklsum = taming + anlore + vetern + herd;

            int max = from is PlayerMobile ? ((PlayerMobile)from).RewardStableSlots : 0;

            if(sklsum >=360)
            {
                max += 25;
            }

            else if (sklsum >= 240.0)
			{
				max += 15;
			}
			else if (sklsum >= 200.0)
			{
				max += 10;
			}
			else if (sklsum >= 160.0)
			{
				max += 5;
			}
			else
			{
				max += 2;
			}
			
			// bonus SA stable slots
			if (Core.SA) 
 			{ 
 				max += 2;
 			}
 			//bonus ToL stable slots
 			if (Core.TOL) 
 			{ 
 				max += 2;
 			}
 
			if (taming >= 100.0)
			{
				max += (int)((taming - 90.0) / 10);
			}

			if (anlore >= 100.0)
			{
				max += (int)((anlore - 90.0) / 10);
			}

			if (vetern >= 100.0)
			{
				max += (int)((vetern - 90.0) / 10);
			}

            if (herd >= 100.0)
            {
                max += (int)((herd - 90.0) / 10);
            }

            return max + Server.Spells.SkillMasteries.MasteryInfo.BoardingSlotIncrease(from);
		}

		private void CloseClaimList(Mobile from)
		{
			from.CloseGump(typeof(ClaimListGump));
		}

		public void BeginClaimList(Mobile from)
		{
			if (Deleted || !from.CheckAlive())
			{
				return;
			}

			var list = new List<BaseCreature>();

			for (var i = 0; i < from.Stabled.Count; ++i)
			{
				var pet = from.Stabled[i] as BaseCreature;

				if (pet == null || pet.Deleted)
				{
					if (pet != null)
					{
						pet.IsStabled = false;
						pet.StabledBy = null;
					}

					from.Stabled.RemoveAt(i--);
					continue;
				}

				list.Add(pet);
			}

			if (list.Count > 0)
			{
				from.SendGump(new ClaimListGump(this, from, list));
			}
			else
			{
				SayTo(from, 502671); // But I have no animals stabled with me at the moment!
			}
		}

		public void EndClaimList(Mobile from, BaseCreature pet)
		{
			if (pet == null || pet.Deleted || from.Map != Map || !from.Stabled.Contains(pet) || !from.CheckAlive())
			{
				return;
			}

			if (!from.InRange(this, 14))
			{
				from.SendLocalizedMessage(500446); // That is too far away.
				return;
			}

			if (CanClaim(from, pet))
			{
				DoClaim(from, pet);

				from.Stabled.Remove(pet);

				if (from is PlayerMobile)
				{
					((PlayerMobile)from).AutoStabled.Remove(pet);
				}
			}
			else
			{
				SayTo(from, 1049612, pet.Name); // ~1_NAME~ remained in the stables because you have too many followers.
			}
		}

		public void BeginStable(Mobile from)
		{
			if (Deleted || !from.CheckAlive())
			{
				return;
			}

			if ((from.Backpack == null || from.Backpack.GetAmount(typeof(Gold)) < 30) && Banker.GetBalance(from) < 30)
			{
				SayTo(from, 1042556); // Thou dost not have enough gold, not even in thy bank account.
				return;
			}

			/* 
			 * I charge 30 gold per pet for a real week's stable time.
			 * I will withdraw it from thy bank account.
			 * Which animal wouldst thou like to stable here?
			 */
			from.SendLocalizedMessage(1042558);

			from.Target = new StableTarget(this);
		}

		public void EndStable(Mobile from, BaseCreature pet)
		{
			if (Deleted || !from.CheckAlive())
			{
				return;
			}

			if (pet.Body.IsHuman)
			{
				SayTo(from, 502672); // HA HA HA! Sorry, I am not an inn.
			}
			else if (!pet.Controlled)
			{
				SayTo(from, 1048053); // You can't stable that!
			}
			else if (pet.ControlMaster != from)
			{
				SayTo(from, 1042562); // You do not own that pet!
			}
			else if (pet.IsDeadPet)
			{
				SayTo(from, 1049668); // Living pets only, please.
			}
			else if (pet.Summoned)
			{
				SayTo(from, 502673); // I can not stable summoned creatures.
			}
			else if (pet.Allured)
			{
				SayTo(from, 1048053); // You can't stable that!
			}
			else if ((pet is PackLlama || pet is PackHorse || pet is Beetle) &&
					 (pet.Backpack != null && pet.Backpack.Items.Count > 0))
			{
				SayTo(from, 1042563); // You need to unload your pet.
			}
			else if (pet.Combatant != null && pet.InRange(pet.Combatant, 12) && pet.Map == pet.Combatant.Map)
			{
				SayTo(from, 1042564); // I'm sorry.  Your pet seems to be busy.
			}
			else if (from.Stabled.Count >= GetMaxStabled(from))
			{
				SayTo(from, 1042565); // You have too many pets in the stables!
			}
			else if ((from.Backpack != null && from.Backpack.ConsumeTotal(typeof(Gold), 30)) || Banker.Withdraw(from, 30))
			{
				pet.ControlTarget = null;
				pet.ControlOrder = OrderType.Stay;
				pet.Internalize();

				pet.SetControlMaster(null);
				pet.SummonMaster = null;

				pet.IsStabled = true;
				pet.StabledBy = from;

				if (Core.SE)
				{
					pet.Loyalty = MaxLoyalty; // Wonderfully happy
				}

				from.Stabled.Add(pet);

				SayTo(from, Core.AOS ? 1049677 : 502679);
				// [AOS: Your pet has been stabled.] Very well, thy pet is stabled. 
				// Thou mayst recover it by saying 'claim' to me. In one real world week, 
				// I shall sell it off if it is not claimed!
			}
			else
			{
				SayTo(from, 502677); // But thou hast not the funds in thy bank account!
			}
		}

		public void Claim(Mobile from)
		{
			Claim(from, null);
		}

		public void Claim(Mobile from, string petName)
		{
			if (Deleted || !from.CheckAlive())
			{
				return;
			}

			var claimed = false;
			var stabled = 0;

			var claimByName = (petName != null);

			for (var i = 0; i < from.Stabled.Count; ++i)
			{
				var pet = from.Stabled[i] as BaseCreature;

				if (pet == null || pet.Deleted)
				{
					if (pet != null)
					{
						pet.IsStabled = false;
						pet.StabledBy = null;
					}

					from.Stabled.RemoveAt(i--);
					continue;
				}

				++stabled;

				if (claimByName && !Insensitive.Equals(pet.Name, petName))
				{
					continue;
				}

				if (CanClaim(from, pet))
				{
					DoClaim(from, pet);

					from.Stabled.RemoveAt(i);

					if (from is PlayerMobile)
					{
						((PlayerMobile)from).AutoStabled.Remove(pet);
					}

					--i;

					claimed = true;
				}
				else
				{
					SayTo(from, 1049612, pet.Name); // ~1_NAME~ remained in the stables because you have too many followers.
				}
			}

			if (claimed)
			{
				SayTo(from, 1042559); // Here you go... and good day to you!
			}
			else if (stabled == 0)
			{
				SayTo(from, 502671); // But I have no animals stabled with me at the moment!
			}
			else if (claimByName)
			{
				BeginClaimList(from);
			}
		}

		public bool CanClaim(Mobile from, BaseCreature pet)
		{
			return ((from.Followers + pet.ControlSlots) <= from.FollowersMax);
		}

		private void DoClaim(Mobile from, BaseCreature pet)
		{
			pet.SetControlMaster(from);

			if (pet.Summoned)
			{
				pet.SummonMaster = from;
			}

			pet.ControlTarget = from;
			pet.ControlOrder = OrderType.Follow;

			pet.MoveToWorld(from.Location, from.Map);

			pet.IsStabled = false;
			pet.StabledBy = null;

			if (Core.SE)
			{
				pet.Loyalty = MaxLoyalty; // Wonderfully Happy
			}
		}

		public override bool HandlesOnSpeech(Mobile from)
		{
			return true;
		}

		public override void OnSpeech(SpeechEventArgs e)
		{
			if (!e.Handled && e.HasKeyword(0x0008)) // *stable*
			{
				e.Handled = true;

				CloseClaimList(e.Mobile);
				BeginStable(e.Mobile);
			}
			else if (!e.Handled && e.HasKeyword(0x0009)) // *claim*
			{
				e.Handled = true;

				CloseClaimList(e.Mobile);

				var index = e.Speech.IndexOf(' ');

				if (index != -1)
				{
					Claim(e.Mobile, e.Speech.Substring(index).Trim());
				}
				else
				{
					Claim(e.Mobile);
				}
			}
            else if (!e.Handled && e.Speech.ToLower().IndexOf("stablecount") >= 0)
            {
                IPooledEnumerable eable = e.Mobile.Map.GetMobilesInRange(e.Mobile.Location, 8);
                e.Handled = true;

                foreach (Mobile m in eable)
                {
                    if (m is AnimalTrainer)
                    {
                        e.Mobile.SendLocalizedMessage(1071250, String.Format("{0}\t{1}", e.Mobile.Stabled.Count.ToString(), GetMaxStabled(e.Mobile).ToString())); // ~1_USED~/~2_MAX~ stable stalls used.
                        break;
                    }
                }

                eable.Free();
            }
            else
            {
                base.OnSpeech(e);
            }
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.ReadInt();
		}

		private class StableEntry : ContextMenuEntry
		{
			private readonly Mobile m_From;
			private readonly AnimalTrainer m_Trainer;

			public StableEntry(AnimalTrainer trainer, Mobile from)
				: base(6126, 12)
			{
				m_Trainer = trainer;
				m_From = from;
			}

			public override void OnClick()
			{
				m_Trainer.BeginStable(m_From);
			}
		}

		private class ClaimListGump : Gump
		{
			private readonly Mobile m_From;
			private readonly List<BaseCreature> m_List;
			private readonly AnimalTrainer m_Trainer;

			public ClaimListGump(AnimalTrainer trainer, Mobile from, List<BaseCreature> list)
				: base(50, 50)
			{
				m_Trainer = trainer;
				m_From = from;
				m_List = list;

				from.CloseGump(typeof(ClaimListGump));

				AddPage(0);

				AddBackground(0, 0, 325, 50 + (list.Count * 20), 9250);
				AddAlphaRegion(5, 5, 315, 40 + (list.Count * 20));

				AddHtml(
					15,
					15,
					275,
					20,
                    "<BASEFONT COLOR=#000008>Select a pet to retrieve from the stables:</BASEFONT>",
					false,
					false);

				for (var i = 0; i < list.Count; ++i)
				{
					var pet = list[i];

					if (pet == null || pet.Deleted)
					{
						continue;
					}

					AddButton(15, 39 + (i * 20), 10006, 10006, i + 1, GumpButtonType.Reply, 0);
					AddHtml(
						32,
						35 + (i * 20),
						275,
						18,
						String.Format("<BASEFONT COLOR=#C6C6EF>{0}</BASEFONT>", pet.Name),
						false,
						false);
				}
			}

			public override void OnResponse(NetState sender, RelayInfo info)
			{
				var index = info.ButtonID - 1;

				if (index >= 0 && index < m_List.Count)
				{
					m_Trainer.EndClaimList(m_From, m_List[index]);
				}
			}
		}

		private class ClaimAllEntry : ContextMenuEntry
		{
			private readonly Mobile m_From;
			private readonly AnimalTrainer m_Trainer;

			public ClaimAllEntry(AnimalTrainer trainer, Mobile from)
				: base(6127, 12)
			{
				m_Trainer = trainer;
				m_From = from;
			}

			public override void OnClick()
			{
				m_Trainer.Claim(m_From);
			}
		}

		private class StableTarget : Target
		{
			private readonly AnimalTrainer m_Trainer;

			public StableTarget(AnimalTrainer trainer)
				: base(12, false, TargetFlags.None)
			{
				m_Trainer = trainer;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is BaseCreature)
				{
					m_Trainer.EndStable(from, (BaseCreature)targeted);
				}
				else if (targeted == from)
				{
					m_Trainer.SayTo(from, 502672); // HA HA HA! Sorry, I am not an inn.
				}
				else
				{
					m_Trainer.SayTo(from, 1048053); // You can't stable that!
				}
			}
		}
	}
}
