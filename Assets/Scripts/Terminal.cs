using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Coderman
{
    public class Terminal : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI terminal;
        [SerializeField] private TextAsset[] codeSamples;
        [SerializeField] private int minTypeSpeed = 1;
        [SerializeField] private int maxTypeSpeed = 5;

        private string _currentString;
        private bool _isQuitting = false;
        private int _index = 0;
        private int _lineCount = 2;

        #region Unity Methods

        private void OnEnable()
        {
            Events.Instance.pressedKeyboardKey += GotKey;
        }

        private void OnDisable()
        {
            if (_isQuitting) return;
            Events.Instance.pressedKeyboardKey -= GotKey;
        }

        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        private void Start()
        {
            _currentString = GetASample();
        }

        #endregion

        private string GetASample()
        {
            return codeSamples[Random.Range(0, codeSamples.Length)].text;
        }

        private void GotKey(KeyCode key)
        {
            if (_index == 0) terminal.text = "";
            int newPos = Random.Range(minTypeSpeed, maxTypeSpeed);
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
            _currentString = GetASample();
        }
    }
}