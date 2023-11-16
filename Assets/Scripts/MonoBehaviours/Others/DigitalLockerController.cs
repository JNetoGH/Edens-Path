using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;


public class DigitalLockerController : MonoBehaviour
{
    
    [Header("Password")] 
    [SerializeField] private string _password;
    [SerializeField, ReadOnly] private int _curDigitIndex = 0;
  
    [Header("Digits Text")]
    [SerializeField] private List<TextMeshProUGUI> _displayTexts;
    
    // Start is called before the first frame update
    private void Start()
    {
        _curDigitIndex = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_curDigitIndex >= 3)
        {
            bool correct = CheckPassword();
            Debug.Log($"Password is {correct}");
            ResetDisplay();
        }
    }

    #region Called Via Buttons Events
    
        public void ResetDisplay()
        {
            foreach (TextMeshProUGUI textMeshProUGUI in _displayTexts)
                textMeshProUGUI.text = "-";
            _curDigitIndex = 0;
        }
        
        private bool CheckPassword()
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
    
    #endregion

}
