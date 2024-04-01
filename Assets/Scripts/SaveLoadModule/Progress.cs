using System;
using Infrastructure.GroupData;

namespace SaveLoadModule
{
    [Serializable]
    public class Progress
    {
        public DataWallet DataWallet;

        public Progress() => 
            DataWallet = new DataWallet();
    }
}