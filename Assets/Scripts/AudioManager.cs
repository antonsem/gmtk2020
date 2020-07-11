using UnityEngine;

namespace Coderman
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource keyboardSource;
        [SerializeField] private AudioClip keyboardSound;
        [SerializeField] private float keyboardDelay = 0.1f;

        [SerializeField] private AudioSource notificationSource;
        [SerializeField] private AudioClip gotNotification;
        [SerializeField] private AudioClip closeNotification;
        [SerializeField] private AudioClip openPopup;

        private float _keyboardDelay = 0;

        #region Unity Methods

        private void OnEnable()
        {
            Events.Instance.pressedKeyboardKey += OnKey;
            Events.Instance.playNotificationSound += PlayNotification;
            Events.Instance.playCloseNotificationSound += PlayCloseNotification;
            Events.Instance.playPopUpSound += PlayPopUpSound;
        }

        private void OnDisable()
        {
            if (ApplicationStatus.IsQuitting) return;
            Events.Instance.pressedKeyboardKey -= OnKey;
            Events.Instance.playNotificationSound -= PlayNotification;
            Events.Instance.playCloseNotificationSound -= PlayCloseNotification;
            Events.Instance.playPopUpSound -= PlayPopUpSound;
        }

        private void Update()
        {
            if (_keyboardDelay > 0)
                _keyboardDelay -= Time.deltaTime;
        }

        #endregion

        private void OnKey(KeyCode key)
        {
            if (_keyboardDelay > 0) return;
            _keyboardDelay = keyboardDelay;
            keyboardSource.pitch = Random.Range(0.9f, 1.1f);
            keyboardSource.PlayOneShot(keyboardSound);
        }

        private void PlayNotification()
        {
            notificationSource.PlayOneShot(gotNotification);
        }

        private void PlayCloseNotification()
        {
            notificationSource.PlayOneShot(closeNotification);
            
        }

        private void PlayPopUpSound()
        {
            notificationSource.PlayOneShot(openPopup);
        }
    }
}