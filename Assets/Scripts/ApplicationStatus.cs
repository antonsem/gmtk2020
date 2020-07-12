using UnityEngine;

namespace Coderman
{
    public class ApplicationStatus : MonoBehaviour
    {
        public static bool IsQuitting { get; set; } = false;
        public static bool IsPaused => IsHelpActive || IsCreditsActive || IsExitActive || IsNewCareerActive || IsGameOverActive;
        public static bool IsPopUpActive { get; private set; } = false;
        public static bool IsCareerActive { get; private set; } = false;
        public static bool IsNewCareerActive { get; private set; } = false;
        public static bool IsHelpActive { get; private set; } = false;
        public static bool IsCreditsActive { get; private set; } = false;
        public static bool IsExitActive { get; private set; } = false;
        public static bool IsGameOverActive { get; private set; } = false;
        public static float Effectiveness { get; set; } = 1;
        public static float DeadlineTime { get; set; } = 20;

        public static float CompletionPercentage { get; set; } = 0;

        #region Unity Methods

        private void Start()
        {
#if !UNITY_EDITOR
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
#endif
            IsQuitting = false;
            IsPopUpActive = false;
            IsHelpActive = false;
            IsCreditsActive = false;
            IsExitActive = false;
            IsCareerActive = false;
            IsGameOverActive = false;
            IsNewCareerActive = false;
            DeadlineTime = 60;
        }

        private void OnEnable()
        {
            Events.Instance.popUpStatus += PopUpStatus;
            Events.Instance.startCareer += StartCareer;
            Events.Instance.helpStatus += HelpStatus;
            Events.Instance.creditsStatus += CreditsStatus;
            Events.Instance.exitStatus += ExitStatus;
            Events.Instance.gameOver += OnGameOver;
            Events.Instance.beatTheGame += OnGameDone;
            Events.Instance.careerStatus += CareerStatus;
            Events.Instance.timeChange += OnTimeUpdated;
        }

        private void OnDisable()
        {
            if (IsQuitting) return;
            Events.Instance.popUpStatus -= PopUpStatus;
            Events.Instance.startCareer -= StartCareer;
            Events.Instance.helpStatus -= HelpStatus;
            Events.Instance.creditsStatus -= CreditsStatus;
            Events.Instance.exitStatus -= ExitStatus;
            Events.Instance.gameOver -= OnGameOver;
            Events.Instance.beatTheGame -= OnGameDone;
            Events.Instance.careerStatus -= CareerStatus;
            Events.Instance.timeChange -= OnTimeUpdated;
        }

        private void OnApplicationQuit()
        {
            IsQuitting = true;
        }

        private void Update()
        {
            if (IsPaused || !IsCareerActive) return;

            if (DeadlineTime > 0)
                DeadlineTime -= Time.deltaTime;
            else
            {
                Events.Instance.gameOver?.Invoke();
                DeadlineTime = 0;
            }
        }

        #endregion

        private void OnTimeUpdated(float val)
        {
            DeadlineTime += val;
        }

        private void CareerStatus(bool val)
        {
            IsNewCareerActive = val;
        }

        private void OnGameDone()
        {
            IsCareerActive = false;
        }

        private void OnGameOver()
        {
            IsGameOverActive = true;
            IsCareerActive = false;
        }

        private void StartCareer(bool val)
        {
            IsCareerActive = val;
        }

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