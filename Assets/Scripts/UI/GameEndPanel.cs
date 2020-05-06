using System.Collections;
using System.Collections.Generic;
using Core.Services.Game;
using UnityDI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class GameEndPanel : UIPanel {
        [Dependency]
        private readonly GameManagerService _GameManagerService;

        public Button RestartPanel;

        private void Start()
        {
            RestartPanel.onClick.AddListener(Restart);
        }

        private void Restart()
        {
            ContainerHolder.Container.BuildUp(this);
            _GameManagerService.RestartGame();
        }
    }
}
