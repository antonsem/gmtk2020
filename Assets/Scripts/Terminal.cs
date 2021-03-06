﻿using TMPro;
using UnityEngine;

namespace Coderman
{
    public class Terminal : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI terminal;
        [SerializeField] private TextAsset[] codeSamples;
        [SerializeField] private TextAsset[] randomSamples;
        [SerializeField] private int minTypeSpeed = 1;
        [SerializeField] private int maxTypeSpeed = 5;
        [SerializeField] private KeyCode submit = KeyCode.Return;

        private string _currentString;
        private int _index = 0;
        private int _lineCount = 2;
        private int _codeSampleIndex = 0;
        private string _codeSampleName = "";
        private bool _requireSubmit = false;

        #region Unity Methods

        private void OnEnable()
        {
            Events.Instance.pressedKeyboardKey += GotKey;
            Events.Instance.startCareer += StartCareer;
        }

        private void OnDisable()
        {
            if (ApplicationStatus.IsQuitting) return;
            Events.Instance.pressedKeyboardKey -= GotKey;
            Events.Instance.startCareer -= StartCareer;
        }

        private void Start()
        {
            _currentString = GetASample(0);
        }

        #endregion

        private void StartCareer(bool val)
        {
            if (val) _currentString = GetASample(1);
        }

        private string GetASample(int sampleCount)
        {
            _codeSampleIndex = sampleCount;
            if (sampleCount < 0)
            {
                _codeSampleName = "";
                return randomSamples[Random.Range(0, randomSamples.Length - 1)].text;
            }

            if (sampleCount >= codeSamples.Length)
            {
                Debug.LogError("You are the best!");
                Events.Instance.beatTheGame?.Invoke();
                return "";
            }

            _index = 0;
            _codeSampleName = codeSamples[sampleCount].name;
            return codeSamples[sampleCount].text.Replace('\r', ' ');
        }

        private void GotKey(KeyCode key)
        {
            if (_requireSubmit)
            {
                if (key != submit) return;
                _index = 0;
                _lineCount = 2;
                Events.Instance.doneWithCode?.Invoke(_codeSampleName);
                _currentString = GetASample(ApplicationStatus.IsCareerActive ? ++_codeSampleIndex : -1);
                _requireSubmit = false;
            }

            if (ApplicationStatus.IsPaused || ApplicationStatus.IsPopUpActive) return;
            if (_index == 0) terminal.text = "_";
            int newPos = Random.Range(minTypeSpeed,
                Mathf.Clamp(Mathf.RoundToInt(maxTypeSpeed * ApplicationStatus.Effectiveness), minTypeSpeed + 1,
                    maxTypeSpeed));
            if (_currentString.Length <= newPos + _index)
                newPos = 1;

            string newString = "";
            if (_index == 0)
                newString = "   1. ";
            for (int i = _index; i < _index + newPos; i++)
            {
                if (i >= _currentString.Length) break;
                char chr = _currentString[i];
                newString = $"{newString}{chr.ToString()}";
                if (chr.Equals(' ')) newPos++;
                else if (chr.Equals('\n'))
                {
                    _lineCount++;
                    for (int j = 0; j < 4 - _lineCount.ToString().Length; j++)
                        newString += " ";
                    newString = $"{newString}{(_lineCount).ToString()}. ";
                }
            }

            terminal.text = $"{terminal.text.Remove(terminal.text.Length - 1)}{newString}_";
            _index += newPos;

            if (terminal.text.Length > 1000)
                terminal.text = terminal.text.Remove(0, 500);

            if (_index < _currentString.Length) return;
            terminal.text = $"{terminal.text.Remove(terminal.text.Length - 2)}{newString} <sprite=12 color=#00BF08>";
            _requireSubmit = true;
        }
    }
}