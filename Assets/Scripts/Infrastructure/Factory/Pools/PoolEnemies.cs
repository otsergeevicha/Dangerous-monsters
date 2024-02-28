using System.Collections.Generic;
using Services.Factory;
using SO;

namespace Infrastructure.Factory.Pools
{
    public class PoolEnemies
    {
        public List<Enemy> Enemies { get; private set; } = new();
        
        private readonly Dictionary<int, string[]> _levelEnemies = new Dictionary<int, string[]>
        {
            { 1, new[] { Constants.ZeroPath, Constants.OnePath, Constants.TwoPath } },
            { 2, new[] { Constants.ZeroPath, Constants.OnePath, Constants.TwoPath, Constants.ThreePath } },
            { 3, new[] { Constants.ZeroPath, Constants.OnePath, Constants.TwoPath, Constants.ThreePath, Constants.FourPath } },
            { 4, new[] { Constants.OnePath, Constants.TwoPath, Constants.ThreePath, Constants.FourPath, Constants.FivePath } },
            { 5, new[] { Constants.TwoPath, Constants.ThreePath, Constants.FourPath, Constants.FivePath, Constants.SixPath } },
            { 6, new[] { Constants.ThreePath, Constants.FourPath, Constants.FivePath, Constants.SixPath, Constants.SevenPath } },
            { 7, new[] { Constants.FourPath, Constants.FivePath, Constants.SixPath, Constants.SevenPath, Constants.EightPath } },
            { 8, new[] { Constants.FivePath, Constants.SixPath, Constants.SevenPath, Constants.EightPath, Constants.NinePath } },
            { 9, new[] { Constants.SixPath, Constants.SevenPath, Constants.EightPath, Constants.NinePath } },
            { 10, new[] { Constants.SevenPath, Constants.EightPath, Constants.NinePath } }
        };

        public PoolEnemies(IGameFactory factory, PoolData poolData, EnemyData enemyData)
        {
            int[] levelCounts =
            {
                poolData.OneLevelCountEnemy, poolData.TwoLevelCountEnemy, poolData.ThreeLevelCountEnemy,
                poolData.FourLevelCountEnemy, poolData.FiveLevelCountEnemy, poolData.SixLevelCountEnemy, 
                poolData.SevenLevelCountEnemy, poolData.EightLevelCountEnemy, poolData.NineLevelCountEnemy, 
                poolData.TenLevelCountEnemy
            };

            string[] paths = _levelEnemies[poolData.CurrentLevelGame];

            foreach (string path in paths)
            {
                Enemy[] enemies = new Enemy[levelCounts[poolData.CurrentLevelGame - 1]];
                CreateEnemies(levelCounts[poolData.CurrentLevelGame - 1], factory, enemies, path, enemyData);
            }
        }

        private void CreateEnemies(int requiredCount, IGameFactory factory, Enemy[] enemies, string currentPath,
            EnemyData enemyData)
        {
            for (int i = 0; i < requiredCount; i++)
            {
                Enemy enemy = factory.CreateEnemy(currentPath);
                enemy.Construct(enemyData);
                enemy.InActive();
                enemies[i] = enemy;
            }

            Enemies.AddRange(enemies);
        }
    }
}