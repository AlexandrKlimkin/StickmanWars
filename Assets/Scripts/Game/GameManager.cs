using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UI.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    public event Action GameEnd;

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void EndGame()
    {
        UIManager.Instance.SetActivePanel<GameEndPanel>();
        Time.timeScale = 0;
    }

}
