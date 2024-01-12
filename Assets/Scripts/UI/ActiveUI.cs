using Assets.Scripts.Common.SignalBus;
using Assets.Scripts.Common.SignalBus.AllSignals;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ActiveUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _timeLabel;
        private void Awake()
        {
            SignalBus.Instance.Subscribe((ActiveUltaSignal signal) =>
            {
                gameObject.SetActive(signal.IsActive);
            });

            SignalBus.Instance.Subscribe((ChangedTimeSignal signal) =>
            {
                _timeLabel.text = signal.Time == 0 ? string.Empty : Math.Round(signal.Time).ToString();
            });
        }
    }
}
