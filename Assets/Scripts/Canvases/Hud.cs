using Plugins.MonoCache;
using TMPro;
using UnityEngine;

namespace Canvases
{
    public class Hud : MonoCache
    {
        [SerializeField] private TMP_Text _money;
        [SerializeField] private TMP_Text _gem;

        [SerializeField] private Canvas _weaponReload;

        public void WeaponReload(bool flag) =>
            _weaponReload.enabled = flag;

        public void UpdateMoneyView(int currentMoney) => 
            _money.text = currentMoney.ToString();
        
        public void UpdateGemView(int currentGem) => 
            _gem.text = currentGem.ToString();
    }
}