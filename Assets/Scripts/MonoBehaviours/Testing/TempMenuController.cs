using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TempMenuController : MonoBehaviour
{
    [SerializeField, Scene] private string _targetScene;
    
    private void Start()
    {
        GameManager.ReleaseTheCursor();
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene(_targetScene);
    }
    
    public void ExitGame () 
    {
        GameManager.ExitGame();
    }
}
