using Jam.Scripts.Utils.Timer;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Jam.Scripts.Audio.Domain
{
    public class AmbientSoundPlayer : MonoBehaviour
    {
        [SerializeField] private int _intervalBetweenAmbientSfx = 50;
        [Inject] private TimerService _timerService;
        [Inject] private AudioService _audioService;
        private TimerHandle _timerHandle;
        private float _playChance = 0.5f;


        private void Start() =>
            _timerHandle = _timerService.AddTimer(_intervalBetweenAmbientSfx, TryPlayAmbient, true);

        private void TryPlayAmbient()
        {
            var chance = Random.Range(0f, 1f);
            if (chance < _playChance) 
                _audioService.PlaySound(Sounds.ambientSound.ToString());
        }

        private void OnDestroy()
        {
            _timerHandle?.FinalizeTimer();
            _timerHandle = null;
        }
    }
}