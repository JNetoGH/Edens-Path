using System.Collections.Generic;
using Cinemachine;
using NaughtyAttributes;
using TMPro;
using UnityEngine;


public class DigitalLockerController : MonoBehaviour
{
    
    [Header("Password")] 
    [SerializeField] private string _password;
    [SerializeField, ReadOnly] private int _curDigitIndex = 0;
  
    [Header("Digits Text")]
    [SerializeField, Required] private List<TextMeshProUGUI> _displayTexts;

    [Header("Door")] 
    [SerializeField] private float _doorAnimationDuration = 2;
    [SerializeField, Required] private Animator _doorAnimator;
    [SerializeField, Required] private CinemachineVirtualCamera _doorVirtualCamera;
    
    private CinemachineBrain _cinemachineBrain;
    
    // Start is called before the first frame update
    private void Start()
    {
        _curDigitIndex = 0;
        _cinemachineBrain = FindObjectOfType<CinemachineBrain>(includeInactive: true);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_curDigitIndex >= 3)
        {
            bool correct = CheckPassword();
            Debug.Log($"Password is {correct}");
            if (correct)
            {
                TempEnableDorCam(_doorAnimationDuration);
                _doorAnimator.SetTrigger("open");
            }
            ResetDisplay();
        }
    }

    #region Called Via Buttons and Other Events
    
        public void ResetDisplay()
        {
            foreach (TextMeshProUGUI textMeshProUGUI in _displayTexts)
                textMeshProUGUI.text = "-";
            _curDigitIndex = 0;
        }
        
        public bool CheckPassword()
        {
            for (int i = 0; i < _password.Length; i++)
                if (_password[i] != _displayTexts[i].text[0])
                    return false;
            return true;
        }
        
        public void SetCurrentDigit(int digit)
        {
            if (_curDigitIndex >= 3)
                return;
            _displayTexts[_curDigitIndex].text = digit.ToString();
            _curDigitIndex++;
        }
    
        private void TempEnableDorCam(float howLong)
        {
            GameManager.EnterCutsceneMode();
            // Activate CinemachineBrain object to enable virtual camera control during the cutscene.
            _cinemachineBrain.gameObject.SetActive(true);
            _doorVirtualCamera.enabled = true;
            Invoke(nameof(DisableDorCam), howLong);
           
        }
        
        private void DisableDorCam()
        {
            GameManager.EnterGameplayMode();
            // Activate CinemachineBrain object to enable virtual camera control during the cutscene.
            _cinemachineBrain.gameObject.SetActive(false);
            _doorVirtualCamera.enabled = false;
        }
        
    #endregion

}
