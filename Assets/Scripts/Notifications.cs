using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Coderman
{
    public class Notifications : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform content;
        [SerializeField] private float minTime = 3;
        [SerializeField] private float maxTime = 10;
        [SerializeField] private int maxNotifications = 20;
        [SerializeField] private List<KeyCode> possibleKeys = new List<KeyCode>();
        [SerializeField] private PopUpInfo testPopup;

        private List<KeyCode> _keysInUse = new List<KeyCode>();
        private List<Notification> _notifications = new List<Notification>();
        private float _nextNotificationTime = 5;

        #region Unity Methods

        private void Update()
        {
            _nextNotificationTime -= Time.deltaTime;
            if (_nextNotificationTime > 0) return;

            _nextNotificationTime = Random.Range(minTime, maxTime);
            SetNotification();
        }

        #endregion

        private void SetNotification()
        {
            for (int i = 0; i < _notifications.Count; i++)
            {
                if (_notifications[i].gameObject.activeSelf) continue;
                _notifications[i].transform.SetAsLastSibling();
                _notifications[i].Set(GetUniqueKeys(3), GetUniqueKeys(1)[0], testPopup);
                _notifications[i].gameObject.SetActive(true);
                return;
            }

            if (_notifications.Count >= maxNotifications) return;
            _notifications.Add(Instantiate(prefab, content).GetComponent<Notification>());

            _notifications[_notifications.Count - 1]
                .Init(_notifications.Count - 1, CloseNotification, OpenNotification);
            _notifications[_notifications.Count - 1].Set(GetUniqueKeys(3), GetUniqueKeys(1)[0], testPopup);
        }

        private void CloseNotification(int index)
        {
            _notifications[index].gameObject.SetActive(false);
            for (int i = 0; i < _notifications[index].CloseCombo.Count; i++)
                _keysInUse.Remove(_notifications[index].CloseCombo[i]);
        }

        private void OpenNotification(int index)
        {
            Events.Instance.setPopUp?.Invoke(_notifications[index].PopUpInfo);
            CloseNotification(index);
        }

        private List<KeyCode> GetUniqueKeys(int count)
        {
            if (possibleKeys.Count <= _keysInUse.Count + count)
            {
                Debug.LogError($"Used all possible keys ({possibleKeys.Count.ToString()}), cannot generate anymore!");
                return null;
            }

            List<KeyCode> retVal = new List<KeyCode>(count);
            possibleKeys.Shuffle();

            for (int i = 0; i < possibleKeys.Count; i++)
            {
                if (_keysInUse.Contains(possibleKeys[i])) continue;
                retVal.Add(possibleKeys[i]);
                _keysInUse.Add(possibleKeys[i]);
                if (retVal.Count == count) break;
            }

            return retVal;
        }

#if UNITY_EDITOR
        [ContextMenu("Get Keyboard Keys")]
        private void GetKeyboardKeys()
        {
            possibleKeys = KeyboardInputHandler.Instance.GetKeys();
        }
#endif
    }
}