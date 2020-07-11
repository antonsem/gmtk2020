using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Coderman
{
    public class TopBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI deadline;
        [SerializeField] private Slider effectiveness;
        [SerializeField] private float maxRepetition = 5;

        private Queue<KeyCode> _latestKeys = new Queue<KeyCode>(20);

        #region Unity Methods

        private void OnEnable()
        {
            Events.Instance.pressedKeyboardKey += GetKey;
        }

        private void OnDisable()
        {
            if (ApplicationStatus.IsQuitting) return;
            Events.Instance.pressedKeyboardKey -= GetKey;
        }

        private void Update()
        {
            effectiveness.value = Mathf.Lerp(effectiveness.value, ApplicationStatus.Effectiveness, Time.deltaTime * 3);
            deadline.text = ApplicationStatus.DeadlineTime.ToString("000.0");
        }

        #endregion

        private void GetKey(KeyCode key)
        {
            if (ApplicationStatus.IsPaused) return;

            int count = _latestKeys.GetCount(key);
            ApplicationStatus.Effectiveness = 1 - count / maxRepetition;
            _latestKeys.Enqueue(key);
            if (_latestKeys.Count == 20)
                _latestKeys.Dequeue();
        }
    }
}