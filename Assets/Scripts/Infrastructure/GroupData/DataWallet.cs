﻿using System;

namespace Infrastructure.GroupData
{
    [Serializable]
    public class DataWallet
    {
        public int RemainingMoney { get; private set; } = 0;
        public int RemainingGem { get; set; } = 0;

        public void RecordMoney(int remainingMoney) => 
            RemainingMoney = remainingMoney;
        
        public void RecordGem(int remainingGem) => 
            RemainingGem = remainingGem;
    }
}