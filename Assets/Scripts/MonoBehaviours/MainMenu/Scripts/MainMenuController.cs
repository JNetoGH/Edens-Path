using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// This script manages the main menu interactions.
public class MainMenuController : MonoBehaviour 
{
    
    [SerializeField, Scene] private string _sceneToLoadOnPlay = "Name";
    
    private void Start()
    {
        SuperTag.GetFirstObjectWithSuperTag("PlayButton").GetComponent<Button>().onClick.AddListener(PlayGame);
        SuperTag.GetFirstObjectWithSuperTag("QuitButton").GetComponent<Button>().onClick.AddListener(QuitGame);
    }

    #region Main Menu Button Events

        private void PlayGame () {
            SceneManager.LoadScene(_sceneToLoadOnPlay);
        }
        
        private void QuitGame () {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
        
    #endregion
    
}