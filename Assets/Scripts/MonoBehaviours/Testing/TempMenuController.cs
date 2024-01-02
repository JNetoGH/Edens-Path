using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TempMenuController : MonoBehaviour
{
    [SerializeField, Scene] private string _targetScene;

    public void GoBackToMenu()
    {
        SceneManager.LoadScene(_targetScene);
    }
    
    public void ExitGame () {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
