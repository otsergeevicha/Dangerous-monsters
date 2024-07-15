using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Player.ShootingModule
{
    public class MagazineReload
    {
        private const float DelayReload = .5f;
        
        private readonly CancellationTokenSource _tokenReplenishment = new();
        private readonly IMagazine _magazine;

        private bool _isCharge;
        private bool _isReplenishment;

        public MagazineReload(IMagazine magazine) => 
            _magazine = magazine;

        public bool IsCharge =>
            _isCharge;
        
        public async UniTaskVoid Launch(int delayRegeneration)
        {
            _isCharge = true;
            await UniTask.Delay(delayRegeneration);
            _isCharge = false;

            Replenishment().Forget();
        }
        
        private async UniTaskVoid Replenishment()
        {
            _isReplenishment = true;

            while (_isReplenishment)
            {
                _magazine.Replenishment(() => _isReplenishment = false);
                await UniTask.Delay(TimeSpan.FromSeconds(DelayReload));
            }
        }

        public void StopReplenishment() =>
            _tokenReplenishment.Cancel();
    }
}