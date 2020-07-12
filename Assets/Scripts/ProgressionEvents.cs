using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Coderman
{
    public class ProgressionEvents : MonoBehaviour
    {
        [Header("Boss")]
        [SerializeField] private PopUpInfo gotTheJob;
        [SerializeField] private PopUpInfo bossDeadlineDelay;
        [SerializeField] private PopUpInfo bossFavor;
        [SerializeField] private PopUpInfo bossReturnTheFavor;
        [SerializeField] private PopUpInfo bossStatus;
        [SerializeField] private PopUpInfo bossFinalNotice;
        [SerializeField] private PopUpInfo bossProjectIsDone;

        [Header("GF")]
        [SerializeField] private PopUpInfo gfHangout;
        [SerializeField] private PopUpInfo gfDogWalk;
        [SerializeField] private PopUpInfo gfSick;
        [SerializeField] private PopUpInfo gfMeetParents;
        [SerializeField] private PopUpInfo gfBDay;
        [SerializeField] private PopUpInfo gfBadBreakup;
        [SerializeField] private PopUpInfo gfBreakup;
        [SerializeField] private PopUpInfo gfSurprise;

        private Dictionary<string, PopUpInfo> _infos;
        private Dictionary<string, PopUpInfo> _gfInfos;
        private Dictionary<string, Action<bool>> _gameEvents;
        private PopUpInfo _dummy = new PopUpInfo() {id = ""};
        private List<PopUpInfo> _toBeInvoked = new List<PopUpInfo>();
        private float _gfPoints = 0;
        private float _notificationTimer = 0;

        #region Unity Methods

        private void Start()
        {
            _infos = new Dictionary<string, PopUpInfo>
            {
                {"intro", gotTheJob},
                {"HelpPanel", bossDeadlineDelay},
                {"AudioManager", bossFavor},
                {"BeatTheGamePanel", bossReturnTheFavor},
                {"CreditsPanel", bossStatus},
                {"Events", bossFinalNotice},
                {"GameOverPanel", bossProjectIsDone},
            };

            _gfInfos = new Dictionary<string, PopUpInfo>
            {
                {"HelpPanel", gfHangout},
                {"AudioManager", gfDogWalk},
                {"BeatTheGamePanel", gfSick},
                {"CreditsPanel", gfBDay},
                {"Events", gfMeetParents},
                {"GameOverPanel", gfBadBreakup}
            };

            _gameEvents = new Dictionary<string, Action<bool>>
            {
                {"spam", val =>
                    {
                        if (val) Events.Instance.spam.Invoke();
                    }
                },
                {"gotTheJob", val => Events.Instance.startCareer?.Invoke(val)},
                {"bossDeadline", val => Events.Instance.timeChange?.Invoke(val ? 60 : 0)},
                {
                    "bossFavor", val =>
                    {
                        _infos["BeatTheGamePanel"] = val ? bossReturnTheFavor : _dummy;
                        Events.Instance.timeChange?.Invoke(val ? -20 : 0);
                    }
                },
                {"bossReturnFavor", val => Events.Instance.timeChange?.Invoke(60)},
                {"bossStatus", val => Events.Instance.timeChange?.Invoke(val ? 45 : 0)},
                {"bossFinal", val => Events.Instance.timeChange?.Invoke(val ? 45 : 0)},
                {"bossProjectDone", val => Events.Instance.beatTheGame?.Invoke()},
                //GF Events
                {
                    "gfHangout", val =>
                    {
                        _gfPoints += !val ? 15 : 0;
                        UpdateGFEnding();
                        Events.Instance.timeChange?.Invoke(!val ? -20 : 0);
                    }
                },
                {
                    "gfDogWalk", val =>
                    {
                        _gfPoints += !val ? 10 : 0;
                        UpdateGFEnding();
                        Events.Instance.timeChange?.Invoke(!val ? -10 : 0);
                    }
                },
                {
                    "gfSick", val =>
                    {
                        _gfPoints += !val ? 15 : 0;
                        UpdateGFEnding();
                        Events.Instance.timeChange?.Invoke(!val ? -15 : 0);
                    }
                },
                {
                    "gfMeetParents", val =>
                    {
                        _gfPoints += !val ? 45 : 0;
                        UpdateGFEnding();
                        Events.Instance.timeChange?.Invoke(!val ? -30 : 0);
                    }
                },
                {
                    "gfBDay", val =>
                    {
                        _gfPoints += !val ? 10 : 0;
                        UpdateGFEnding();
                        Events.Instance.timeChange?.Invoke(!val ? -10 : 0);
                    }
                }
            };
        }

        private void OnEnable()
        {
            Events.Instance.doneWithCode += OnDoneWithCode;
            Events.Instance.popUpAcceptStatus += PopUpAccepted;
        }

        private void OnDisable()
        {
            if (ApplicationStatus.IsQuitting) return;
            Events.Instance.doneWithCode -= OnDoneWithCode;
            Events.Instance.popUpAcceptStatus -= PopUpAccepted;
        }

        private void Update()
        {
            if (ApplicationStatus.IsPaused || ApplicationStatus.IsPopUpActive || _toBeInvoked.Count == 0) return;
            if (_notificationTimer > 0)
            {
                _notificationTimer -= Time.deltaTime;
                return;
            }

            _notificationTimer = Random.Range(1f, 3f);

            _toBeInvoked.Shuffle();
            Events.Instance.pushNotification?.Invoke(_toBeInvoked[0]);
            _toBeInvoked.RemoveAt(0);
        }

        #endregion

        private void UpdateGFEnding()
        {
            if (_gfPoints >= 50)
                _gfInfos["GameOverPanel"] = gfSurprise;
            else if (_gfPoints > 30)
                _gfInfos["GameOverPanel"] = gfBreakup;
        }

        private void PopUpAccepted(string id, bool status)
        {
            if (!_gameEvents.TryGetValue(id, out Action<bool> action)) return;
            Debug.Log($"Firing event: {id}");
            action(status);
        }

        private void OnDoneWithCode(string key)
        {
            Debug.Log($"Checking for {key} notification");
            if (_infos.TryGetValue(key, out PopUpInfo info) && !string.IsNullOrEmpty(info.id))
                _toBeInvoked.Add(info);
            if (_gfInfos.TryGetValue(key, out PopUpInfo gfInfo) && !string.IsNullOrEmpty(gfInfo.id))
                _toBeInvoked.Add(gfInfo);
        }
    }
}