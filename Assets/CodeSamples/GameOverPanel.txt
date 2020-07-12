using UnityEngine;

namespace Coderman
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        
        #region Unity Methods

        private void OnEnable()
        {
            Events.Instance.gameOver += GameOver;
        }

        private void OnDisable()
        {
            if(ApplicationStatus.IsQuitting) return;
            Events.Instance.gameOver -= GameOver;
        }

        #endregion

        private void GameOver()
        {
            canvas.enabled = true;
        }
    }
}
