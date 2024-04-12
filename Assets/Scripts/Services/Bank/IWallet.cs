using System;

namespace Services.Bank
{
    public interface IWallet
    {
        event Action<int> MoneyChanged; 
        void ApplyMoney(int money);
        void SpendMoney(int money);
        bool Check(int price);
    }
}