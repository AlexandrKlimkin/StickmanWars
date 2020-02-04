using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class GameEndPanel : UIPanel
    {
        public Button RestartPanel;

        private void Start()
        {
            RestartPanel.onClick.AddListener(Restart);
        }

        private void Restart()
        {
            GameManager.Instance.RestartLevel();
        }

    }
}
