using UnityEngine;
using UnityEngine.Rendering;

namespace Coderman
{
    public class ApplicationStatus : MonoBehaviour
    {
        public static bool IsQuitting { get; private set; } = false;
        public static bool IsPaused => IsHelpActive || IsCreditsActive || IsExitActive;
        public static bool IsPopUpActive { get; private set; } = false;
        public static bool IsHelpActive { get; private set; } = false;
        public static bool IsCreditsActive { get; private set; } = false;
        public static bool IsExitActive { get; private set; } = false;

        public static float Effectiveness { get; set; } = 1;

        public static float DeadlineTime { get; set; } = 60;

        public static float CompletionPercentage { get; set; } = 0;

        #region Unity Methods

        private void Start()
        {
            IsQuitting = false;
            DeadlineTime = 60;
        }

        private void OnEnable()
        {
            Events.Instance.popUpStatus += PopUpStatus;
            Events.Instance.helpStatus += HelpStatus;
            Events.Instance.creditsStatus += CreditsStatus;
            Events.Instance.exitStatus += ExitStatus;
        }

        private void OnDisable()
        {
            if (IsQuitting) return;
            Events.Instance.popUpStatus -= PopUpStatus;
            Events.Instance.helpStatus -= HelpStatus;
            Events.Instance.creditsStatus -= CreditsStatus;
            Events.Instance.exitStatus -= ExitStatus;
        }

        private void OnApplicationQuit()
        {
            IsQuitting = true;
        }

        private void Update()
        {
            if (IsPaused) return;

            if (DeadlineTime > 0)
                DeadlineTime -= Time.deltaTime;
            else
                DeadlineTime = 0;
        }

        #endregion

        private void PopUpStatus(bool val)
        {
            IsPopUpActive = val;
        }

        private void HelpStatus(bool val)
        {
            IsHelpActive = val;
        }

        private void CreditsStatus(bool val)
        {
            IsCreditsActive = val;
        }

        private void ExitStatus(bool val)
        {
            IsExitActive = val;
        }
    }
}