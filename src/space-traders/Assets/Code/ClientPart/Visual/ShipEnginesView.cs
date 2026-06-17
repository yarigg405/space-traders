using Assets.Code.ClientPart.View;
using UnityEngine;


namespace Assets.Code.ClientPart.Visual
{
    internal sealed class ShipEnginesView : MonoBehaviour
    {
        [SerializeField] private EntityBehaviour _behaviour;

        [SerializeField] private GameObject[] _commonMoveObjects;
        [SerializeField] private GameObject[] _warpMoveObjects;

        private void Start()
        {
            _behaviour.Entity.ViewModel.IsWarping.OnChange += OnWarpChanged;
            OnWarpChanged(_behaviour.Entity.ViewModel.IsWarping);
        }

        private void OnDestroy()
        {
            if (_behaviour == null) return;
            if (_behaviour.Entity == null) return;

            _behaviour.Entity.ViewModel.IsWarping.OnChange -= OnWarpChanged;
        }

        private void OnWarpChanged(bool isWarping)
        {
            foreach (var obj in _commonMoveObjects)
            {
                obj.SetActive(!isWarping);
            }

            foreach (var obj in _warpMoveObjects)
            {
                obj.SetActive(isWarping);
            }
        }
    }
}
