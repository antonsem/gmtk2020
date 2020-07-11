using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Coderman
{
    public class Notification : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI closeText;
        [SerializeField] private TextMeshProUGUI openText;

        private int Id { get; set; }
        private KeyCode _openKey;
        private Action<int> _closeAction;
        private Action<int> _openAction;
        private int _index = 0;

        public List<KeyCode> CloseCombo { get; private set; }

        #region Unity Methods

        private void OnEnable()
        {
            Events.Instance.pressedKeyboardKey += GotKey;
        }

        private void OnDisable()
        {
            if (ApplicationStatus.IsQuitting) return;
            Events.Instance.pressedKeyboardKey -= GotKey;
        }

        #endregion

        public void Init(int id, Action<int> closeAction, Action<int> openAction)
        {
            Id = id;
            _closeAction = closeAction;
            _openAction = openAction;
        }

        public void SetKeys(List<KeyCode> closeCombo, KeyCode openKey)
        {
            CloseCombo = closeCombo;
            _openKey = openKey;

            string closeString = closeCombo[0].ToString();
            for (int i = 1; i < closeCombo.Count; i++)
                closeString += closeCombo[i].ToString();

            closeText.text = closeString;
            openText.text = openKey.ToString();

            _index = 0;
        }

        private void GotKey(KeyCode key)
        {
            if (key == _openKey)
            {
                _openAction(Id);
                Debug.Log("FAIL!");
                return;
            }

            if (key != CloseCombo[_index])
            {
                Debug.Log($"NOPE! Should have been {CloseCombo[_index]}!");
                _index = 0;
                return;
            }

            _index++;

            if (_index < CloseCombo.Count) return;

            Debug.Log("YEY");
            _closeAction(Id);
        }
    }
}