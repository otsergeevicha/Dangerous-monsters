using System.Collections.Generic;
using Loots;
using Services.Factory;

namespace Infrastructure.Factory.Pools
{
    public class PoolMoney
    {
        private readonly List<Money> _moneys = new();
        
        public IReadOnlyList<Money> Moneys => 
            _moneys.AsReadOnly();

        public PoolMoney(IGameFactory factory, int maxCountMoney)
        {
            for (int i = 0; i < maxCountMoney; i++)
            {
                Money money = factory.CreateMoney();
                money.gameObject.SetActive(false);
                _moneys.Add(money);
            }
        }

        public void AdaptingLevel()
        {
            foreach (Money money in _moneys) 
                money.gameObject.SetActive(false);
        }
    }
}