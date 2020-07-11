using System;
using System.Collections.Generic;
using UnityEngine;

namespace Coderman
{
    public class ProgressionEvents : MonoBehaviour
    {
        [SerializeField] private PopUpInfo gotTheJob;

        private Dictionary<string, PopUpInfo> _infos;
        private Dictionary<string, Action> _gameEvents;

        #region Unity Methods

        private void Start()
        {
            _infos = new Dictionary<string, PopUpInfo>
            {
                {"intro", gotTheJob}
            };

            _gameEvents = new Dictionary<string, Action>
            {
                {"gotTheJob", () => Events.Instance.startCareer(true)}
            };
        }

        private void OnEnable()
        {
            Events.Instance.doneWithCode += OnDoneWithCode;
            Events.Instance.popUpAcceptStatus += PopUpAccepted;
        }

        private void PopUpAccepted(string id, bool status)
        {
            if (_gameEvents.TryGetValue(id, out Action action))
                action.Invoke();
        }

        private void OnDisable()
        {
            if (ApplicationStatus.IsQuitting) return;
            Events.Instance.doneWithCode -= OnDoneWithCode;
        }

        private void OnDoneWithCode(string key)
        {
            if (_infos.TryGetValue(key, out PopUpInfo info))
                Events.Instance.pushNotification?.Invoke(info);
        }

        #endregion
    }
}