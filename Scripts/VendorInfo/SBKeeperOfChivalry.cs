using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    public class SBKeeperOfChivalry : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBKeeperOfChivalry()
        {
        }

        public override IShopSellInfo SellInfo
        {
            get
            {
                return m_SellInfo;
            }
        }
        public override List<GenericBuyInfo> BuyInfo
        {
            get
            {
                return m_BuyInfo;
            }
        }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo(typeof(BookOfChivalry), 140, 20, 0x2252, 0));
                Add(new GenericBuyInfo("1155771", typeof(PowerScroll), 100000, 1, 0x14F0, 0, new object[] { SkillName.Chivalry, 120 }));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
            }
        }
    }
}