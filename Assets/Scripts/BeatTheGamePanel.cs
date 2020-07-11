using UnityEngine;

namespace Coderman
{
    public class BeatTheGamePanel : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        
        #region Unity Methods

        private void OnEnable()
        {
            Events.Instance.beatTheGame += BeatTheGame;
        }

        private void OnDisable()
        {
            if(ApplicationStatus.IsQuitting) return;
            Events.Instance.beatTheGame -= BeatTheGame;
        }

        #endregion

        private void BeatTheGame()
        {
            canvas.enabled = true;
        }
    }
}
