using TMPro;
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

        private string _currentString;
        private int _index = 0;
        private int _lineCount = 2;
        private int _codeSampleIndex = 0;
        private string _codeSampleName = "";

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
            return codeSamples[sampleCount].text;
        }

        private void GotKey(KeyCode key)
        {
            if (ApplicationStatus.IsPaused || ApplicationStatus.IsPopUpActive) return;
            if (_index == 0) terminal.text = "";
            int newPos = Random.Range(minTypeSpeed,
                Mathf.Clamp(Mathf.RoundToInt(maxTypeSpeed * ApplicationStatus.Effectiveness), minTypeSpeed + 1,
                    maxTypeSpeed));
            if (_currentString.Length <= newPos + _index)
                newPos = _currentString.Length - _index - 1;

            string newString = "";
            if (_index == 0)
                newString = "1. ";
            for (int i = _index; i < _index + newPos; i++)
            {
                if (i >= _currentString.Length) break;
                char chr = _currentString[i];
                newString = $"{newString}{chr.ToString()}";
                if (chr.Equals(' ')) newPos++;
                else if (chr.Equals('\n')) newString = $"{newString}{(++_lineCount).ToString()}. ";
            }

            terminal.text += newString;
            _index += newPos;

            if (_index < _currentString.Length - 1) return;
            _index = 0;
            _lineCount = 2;
            Events.Instance.doneWithCode?.Invoke(_codeSampleName);
            _currentString = GetASample(ApplicationStatus.IsCareerActive ? ++_codeSampleIndex : -1);
        }
    }
}