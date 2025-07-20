using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.angryDad.Scripts.Trigers.LogicTrigers
{
    public class TriggerTimerManager : MonoBehaviour
    {
        public static TriggerTimerManager Instance { get; private set; }

        private TriggerTimerController _controller;
        private TriggerRestartEventBus _eventBus;
        private bool _initialized = false;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            _controller = new TriggerTimerController(this);
            _eventBus = new TriggerRestartEventBus();
            _eventBus.OnRestartRequested += _controller.RestartWithDelay;

            _initialized = true;
        }

        public void RegisterNewTrigger(ITriggerTimerTarget newTarget)
        {
            if (!_initialized || _controller.IsRegistered(newTarget)) return;

            _controller.Register(newTarget);

            if (newTarget is BaseSourceTrigger baseTrigger)
                baseTrigger.InjectRestartBus(_eventBus);
        }

        private void OnDisable()
        {
            _controller?.StopAll();
        }
    }
}