using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// This script manages the main menu interactions.
public class MainMenuController : MonoBehaviour 
{
    
    [SerializeField, Scene] private string _sceneToLoadOnPlay = "Name";
    
    #region Main Menu Button Events

        public void PlayGame () {
            SceneManager.LoadScene(_sceneToLoadOnPlay);
        }
        
        public void QuitGame () {
            GameManager.ExitGame();
        }
        
    #endregion
    
}