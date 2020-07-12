using System.Collections.Generic;
using UnityEngine;
using ExtraTools;

namespace Coderman
{
    public class KeyboardInputHandler : Singleton<KeyboardInputHandler>
    {
        [SerializeField] private KeyCode[] acceptedKeys;

        #region Unity Methods

        private void OnEnable()
        {
            if (acceptedKeys?.Length != 0) return;

            Debug.LogError("No keys are set for the keyboard!", this);
            enabled = false;
        }

        private void Update()
        {
            for (int i = 0; i < acceptedKeys.Length; i++)
            {
                if (Input.GetKeyDown(acceptedKeys[i]))
                    Events.Instance.pressedKeyboardKey?.Invoke(acceptedKeys[i]);
            }
        }

        #endregion

        public List<KeyCode> GetKeys()
        {
            return new List<KeyCode>(acceptedKeys);
        }
        
    }
}