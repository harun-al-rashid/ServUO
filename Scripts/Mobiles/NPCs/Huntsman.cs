using System;

namespace Server.Engines.Quests
{ 
    public class Huntsman : MondainQuester
    { 
        [Constructable]
        public Huntsman()
            : base("Huntsman")
        { 
        }

        public Huntsman(Serial serial)
            : base(serial)
        {
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[] 
                {
                    typeof(TheBalanceOfNatureQuest)
                };
            }
        }
        public override void InitBody()
        {
            this.InitStats(100, 100, 25);
			
            this.Female = false;
            this.Body = 101;
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