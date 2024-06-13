using System;

namespace Infrastructure.GroupData
{
    [Serializable]
    public class DataWallet
    {
        public int RemainingMoney { get; private set; }
        public int RemainingGem { get; set; }

        public void RecordMoney(int remainingMoney) => 
            RemainingMoney = remainingMoney;
        
        public void RecordGem(int remainingGem) => 
            RemainingGem = remainingGem;
    }
}