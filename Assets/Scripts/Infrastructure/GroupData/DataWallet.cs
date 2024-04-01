using System;

namespace Infrastructure.GroupData
{
    [Serializable]
    public class DataWallet
    {
        public int RemainingMoney { get; private set; }

        public void Record(int remainingMoney) => 
            RemainingMoney = remainingMoney;
    }
}