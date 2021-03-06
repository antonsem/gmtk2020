using TMPro;
using UnityEngine;

namespace Coderman
{
    public class PopupTerminal : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI terminal;
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private TextMeshProUGUI body;
        [SerializeField] private Canvas canvas;
        [SerializeField] private KeyCode submit = KeyCode.Return;
        [SerializeField] private KeyCode clear = KeyCode.Backspace;

        private PopUpInfo _currentPopUp;
        private int _index = 0;
        private int _currentPopUpIndex = 0;
        private KeyCode[] _confirm;
        private KeyCode[] _deny;
        private bool _isConfirming = false;

        #region Unity Methods

        private void OnEnable()
        {
            Events.Instance.pressedKeyboardKey += GotKey;
            Events.Instance.setPopUp += SetPopUp;
        }

        private void OnDisable()
        {
            if (ApplicationStatus.IsQuitting) return;
            Events.Instance.pressedKeyboardKey -= GotKey;
            Events.Instance.setPopUp -= SetPopUp;
        }

        #endregion

        private void SetPopUp(PopUpInfo info)
        {
            _isConfirming = false;
            _index = 0;
            _currentPopUpIndex = 0;
            _currentPopUp = info;
            header.text = _currentPopUp.header;
            body.text = "";
            SetInfo(_currentPopUp.actions[0]);
            canvas.enabled = true;
            Events.Instance.popUpStatus?.Invoke(true);
            Events.Instance.playPopUpSound?.Invoke();
        }

        private void SetInfo(in PopUpAction action)
        {
            _isConfirming = false;
            _index = 0;
            terminal.text = "_";
            body.text += $"\n{action.text}";
            _confirm = action.confirm;
            _deny = action.deny;
        }

        private void Done(bool accepted)
        {
            terminal.text = "_";
            body.text = "";
            canvas.enabled = false;
            Events.Instance.popUpStatus?.Invoke(false);
            Events.Instance.popUpAcceptStatus?.Invoke(_currentPopUp.id, accepted);
        }

        private void GotKey(KeyCode key)
        {
            if (!canvas.enabled || ApplicationStatus.IsPaused) return;

            if (key == clear)
            {
                _index = 0;
                terminal.text = "_";
                return;
            }

            if (_index == 0 && _confirm.Length > 0)
                _isConfirming = _deny?.Length == 0 || key == _confirm[_index];

            if (_isConfirming)
            {
                if (_index >= _confirm.Length)
                {
                    if (key != submit) return;
                    if (++_currentPopUpIndex >= _currentPopUp.actions.Length)
                    {
                        Done(true);
                        return;
                    }

                    SetInfo(_currentPopUp.actions[_currentPopUpIndex]);

                    return;
                }

                if (_confirm[_index] != key) return;

                terminal.text = $"{terminal.text.Remove(terminal.text.Length - 1)}{_confirm[_index]}_";
                if (++_index >= _confirm.Length)
                    terminal.text = $"{terminal.text.Remove(terminal.text.Length - 1)} <sprite=12 color=#FFDF00>";

                return;
            }

            if (_index >= _deny.Length)
            {
                if (key != submit) return;

                Done(false);
                return;
            }

            if (_deny[_index] != key) return;
            terminal.text = $"{terminal.text.Remove(terminal.text.Length - 1)}{_deny[_index]}_";
            if (++_index >= _deny.Length)
                terminal.text = $"{terminal.text.Remove(terminal.text.Length - 1)} <sprite=12 color=#FFDF00>";
        }
    }
}