using UnityEngine;
using UnityEngine.UI;


namespace Assets.Code.UI.Elements
{
    public class PlayerShipControlView : MonoBehaviour
    {
        [SerializeField] private Slider _shipSpeedControlSlider;

        private GameEntity _player;

        internal void SetupPlayer(GameEntity playerEntity)
        {
            _player = playerEntity;
            _shipSpeedControlSlider.value = _player.CurrentSpeedModifier;

            _shipSpeedControlSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float arg0)
        {
            _player.ReplaceCurrentSpeedModifier(_shipSpeedControlSlider.value);
        }

        internal void Dispose()
        {
            _shipSpeedControlSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }
}