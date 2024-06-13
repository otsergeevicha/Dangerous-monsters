using System;
using Services.Bank;
using Services.SaveLoad;
using UnityEngine;

namespace Bank
{
    public class Wallet : IWallet
    {
        private readonly ISave _save;
        private int _currentMoney;

        public Wallet(ISave save)
        {
            _save = save;
            _currentMoney = save.AccessProgress().DataWallet.RemainingMoney;
        }

        public event Action<int> MoneyChanged;
        public event Action<int> GemChanged;

        public void ApplyMoney(int money)
        {
            _currentMoney += money;
            Notify();
            WritingSave();
        }

        public void SpendMoney(int money)
        {
            _currentMoney -= Mathf.Clamp(money, 0, int.MaxValue);
            Notify();
            WritingSave();
        }

        public bool Check(int price) => 
            _currentMoney >= price;

        public int ReadCurrentMoney() => 
            _save.AccessProgress().DataWallet.RemainingMoney;

        public int ReadCurrentGem() => 
            _save.AccessProgress().DataWallet.RemainingGem;

        private void WritingSave()
        {
            _save.AccessProgress().DataWallet.RecordMoney(_currentMoney);
            _save.Save();
        }

        private void Notify() => 
            MoneyChanged?.Invoke(_currentMoney);
    }
}