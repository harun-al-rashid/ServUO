using Server;
using Server.Commands;
using Server.Targeting;
using Server.Mobiles;

namespace Custom.Commands
{
    public static class Tame
    {
        public static void Initialize()
        {
            CommandSystem.Register("Tame", AccessLevel.Counselor, new CommandEventHandler(OnCommand));
        }

        [Usage("Tame")]
        [Description("Tames the selected target")]
        static void OnCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new TameTarget();
        }

        class TameTarget : Target
        {
            public TameTarget() : base(10, false, TargetFlags.None) { }

            protected override void OnTarget(Mobile from, object targeted)
            {
                var target = targeted as BaseCreature;

                if (target != null)
                {
                    if (!target.Tamable)
                    {
                        from.SendMessage("The target must be tamable");
                        return;
                    }

                    else if (from.Followers + target.ControlSlots > target.FollowersMax)
                    {
                        from.SendLocalizedMessage(1049611); // You have too many followers to tame that creature.
                        return;
                    }

                    else if (target.Controlled == true)
                    {
                        from.SendMessage("This creature is already tamed");
                        return;
                    }
                    else
                    {
                        target.PrivateOverheadMessage(Server.Network.MessageType.Regular, 0x3B2, 502799, from.NetState); // It seems to accept you as master.
                        target.Owners.Add(from);
                        target.SetControlMaster(from);
                        target.IsBonded = true;
                    }
                }
            }
        }
    }
}
