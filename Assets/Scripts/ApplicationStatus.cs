using System;
using UnityEngine;

namespace Coderman
{
    public class ApplicationStatus : MonoBehaviour
    {
        public static bool IsQuitting { get; private set; } = false;
        public static bool IsPopUpActive { get; private set; } = false;

        #region Unity Methods

        private void Start()
        {
            IsPopUpActive = false;
            IsQuitting = false;
        }

        private void OnEnable()
        {
            Events.Instance.popUpStatus += PopUpStatus;
        }

        private void OnDisable()
        {
            Events.Instance.popUpStatus -= PopUpStatus;
        }

        private void OnApplicationQuit()
        {
            IsQuitting = true;
        }

        #endregion

        private void PopUpStatus(bool val)
        {
            IsPopUpActive = val;
        }
    }
}