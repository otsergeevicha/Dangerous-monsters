using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Player.ShootingModule
{
    public class HeroRegeneration
    {
        private const float DelayRegeneration = .5f;
        
        private readonly CancellationTokenSource _tokenReplenishment = new();
        private readonly IMagazine _magazine;

        private bool _isWaiting;
        private bool _isReplenishment;

        public HeroRegeneration(IMagazine magazine) => 
            _magazine = magazine;

        public bool IsWaiting =>
            _isWaiting;
        
        public async UniTaskVoid Launch(int delayRegeneration)
        {
            _isWaiting = true;
            await UniTask.Delay(delayRegeneration);
            _isWaiting = false;

            Replenishment().Forget();
        }
        
        private async UniTaskVoid Replenishment()
        {
            _isReplenishment = true;

            while (_isReplenishment)
            {
                _magazine.Replenishment(() => _isReplenishment = false);
                await UniTask.Delay(TimeSpan.FromSeconds(DelayRegeneration));
            }
        }

        public void StopReplenishment() =>
            _tokenReplenishment.Cancel();
    }
}