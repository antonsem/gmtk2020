using TMPro;
using UnityEngine;

namespace Coderman
{
    public class Terminal : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI terminal;

        #region Unity Methods

        private void OnEnable()
        {
            Events.Instance.pressedKeyboardKey += GotKey;
        }

        private void OnDisable()
        {
            Events.Instance.pressedKeyboardKey -= GotKey;
        }

        #endregion

        private void GotKey(KeyCode key)
        {
            terminal.text += key.ToString();
        }
    }
}
