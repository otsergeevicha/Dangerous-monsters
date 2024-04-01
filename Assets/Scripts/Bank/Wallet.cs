using System;
using Services.Bank;
using Services.SaveLoad;
using UnityEngine;

namespace Bank
{
    public class Wallet : IWallet
    {
        private readonly ISave _save;

        public Wallet(ISave save)
        {
            _save = save;
            CurrentMoney = save.AccessProgress().DataWallet.RemainingMoney;
        }

        public event Action<int> MoneyChanged; 

        public int CurrentMoney { get; private set; }

        public void ApplyMoney(int money)
        {
            CurrentMoney += money;
            Notify();
            WritingSave();
        }

        public void SpendMoney(int money)
        {
            CurrentMoney -= Mathf.Clamp(money, 0, int.MaxValue);
            Notify();
            WritingSave();
        }
        
        private void WritingSave()
        {
            _save.AccessProgress().DataWallet.Record(CurrentMoney);
            _save.Save();
        }

        private void Notify() => 
            MoneyChanged?.Invoke(CurrentMoney);
    }
}