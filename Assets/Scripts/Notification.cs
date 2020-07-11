using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Coderman
{
    public class Notification : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI closeText;
        [SerializeField] private TextMeshProUGUI openText;
        [SerializeField] private TextMeshProUGUI summary;
        [SerializeField] private Slider openSlider;
        [SerializeField] private Slider closeSlider;

        public PopUpInfo PopUpInfo { get; private set; }
        private int Id { get; set; }
        private KeyCode _openKey;
        private Action<int> _closeAction;
        private Action<int> _openAction;
        private int _index = 0;
        private float _acceptDelay = 0;
        private float _closeDelay = 0;

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

        private void Update()
        {
            if (ApplicationStatus.IsPaused) return;

            if (_closeDelay > 0)
            {
                _closeDelay -= Time.deltaTime;
                closeSlider.gameObject.SetActive(true);
                closeSlider.value = _closeDelay / PopUpInfo.closeTimer;
            }
            else if (PopUpInfo.canDeny)
                closeSlider.gameObject.SetActive(false);
            else
            {
                closeSlider.gameObject.SetActive(true);
                closeSlider.value = 1;
            }

            if (_acceptDelay > 0)
            {
                _acceptDelay -= Time.deltaTime;
                openSlider.gameObject.SetActive(true);
                openSlider.value = _acceptDelay / PopUpInfo.acceptTimer;
            }
            else
                openSlider.gameObject.SetActive(false);
        }

        #endregion

        public void Init(int id, Action<int> closeAction, Action<int> openAction)
        {
            Id = id;
            _closeAction = closeAction;
            _openAction = openAction;
        }

        public void Set(List<KeyCode> closeCombo, KeyCode openKey, in PopUpInfo popUpInfo)
        {
            CloseCombo = closeCombo;
            _openKey = openKey;
            PopUpInfo = popUpInfo;
            summary.text = PopUpInfo.header;
            closeText.gameObject.SetActive(PopUpInfo.canDeny);
            if (PopUpInfo.canDeny)
            {
                string closeString = CloseCombo[0].ToString();
                for (int i = 1; i < CloseCombo.Count; i++)
                    closeString += CloseCombo[i].ToString();
                closeText.text = closeString;
            }

            openText.text = openKey.ToString();

            _index = 0;


            _acceptDelay = PopUpInfo.acceptTimer;
            _closeDelay = PopUpInfo.closeTimer;
        }

        private void GotKey(KeyCode key)
        {
            if (ApplicationStatus.IsPaused) return;

            if (_acceptDelay <= 0 && key == _openKey)
            {
                _openAction(Id);
                return;
            }

            if (_closeDelay > 0 || key != CloseCombo[_index])
            {
                _index = 0;
                return;
            }

            _index++;

            if (_index < CloseCombo.Count) return;

            _closeAction(Id);
        }
    }
}