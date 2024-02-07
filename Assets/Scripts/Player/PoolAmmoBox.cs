using System.Collections.Generic;
using System.Linq;
using Services.Factory;
using UnityEngine;

namespace Player
{
    public class PoolAmmoBox
    {
        private readonly List<AmmoBox> _ammoBoxes = new();
        private AmmoBox _box;

        public PoolAmmoBox(IGameFactory factory, int maxCountAmmoBox)
        {
            for (int i = 0; i < maxCountAmmoBox; i++)
            {
                _box = factory.CreateAmmoBox();
                _box.InActive();
                _ammoBoxes.Add(_box);
            }

            _box = null;
        }

        public void AcceptBox()
        {
            _box = _ammoBoxes.FirstOrDefault(box =>
                box.isActiveAndEnabled == false);

            if (_box != null)
                _box.OnActive();
            
            _box = null;
        }

        public void SpendBox()
        {
            _box = _ammoBoxes.LastOrDefault(box =>
                box.isActiveAndEnabled);

            if (_box != null)
                _box.InActive();
            
            _box = null;
        }

        public void FirstPointPosition(Transform basketTransform)
        {
            float ratePositionY = 0;

            for (int i = 0; i < _ammoBoxes.Count; i++)
            {
                _ammoBoxes[i].transform.parent = basketTransform;
                _ammoBoxes[i].SetPosition(new Vector3(basketTransform.position.x, basketTransform.position.y + ratePositionY, basketTransform.position.z));
                ratePositionY += .4f;
            }
        }
    }
}