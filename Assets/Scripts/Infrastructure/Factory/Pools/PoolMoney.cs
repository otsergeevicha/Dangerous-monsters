using System.Collections.Generic;
using Bank;
using Loots;
using Services.Factory;

namespace Infrastructure.Factory.Pools
{
    public class PoolMoney
    {
        public List<Money> Moneys { get; private set; } = new();
        
        public PoolMoney(IGameFactory factory, int maxCountMoney)
        {
            for (int i = 0; i < maxCountMoney; i++)
            {
                Money money = factory.CreateMoney();
                money.InActive();
                Moneys.Add(money);
            }
        }
    }
}