using System;

namespace Server.Items
{
    public class AryasNeedle : Kryss
    {
        public override bool IsArtifact { get { return true; } }
        [Constructable]

        public AryasNeedle()
        {
            Name = "Needle";
            LootType = LootType.Blessed;
            WeaponAttributes.HitLeechStam = 25;
            WeaponAttributes.ResistColdBonus = 5;
            Attributes.WeaponSpeed = 20;
            Attributes.AttackChance = 25;
            Attributes.Luck = 150;
        }

        public AryasNeedle(Serial serial)
            : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }

    }
}
