using System.Collections.Generic;
using UnityEngine;


public class FirstLevelProgression : MonoBehaviour
{
    [SerializeField] private List<LitableStick> _sticks;
    [SerializeField] private GameObject _bridge;
    [SerializeField] private float _bridgeRisingSpeed = 2;
    [SerializeField] private float _bridgeAppearingSpeed = 3;

    private bool _execBridgeAppearingAnimation = false;
    private Renderer _bridgeRenderer;
    private Color _bridgeColor;

    private void Start()
    {
        _bridgeRenderer = _bridge.GetComponent<Renderer>();
        _bridgeColor = _bridgeRenderer.material.color;
        _bridgeColor.a = 0f; // Set initial alpha to 0 (fully transparent)
        _bridgeRenderer.material.color = _bridgeColor;
        
        _bridge.SetActive(false);
    }

    public void Validate(LevelProgressionHandler handler)
    {
        bool isThereFire = false;
        _sticks.ForEach(s =>
        {
            if (s.isLit)
                isThereFire = true;
        });
        if (isThereFire)
            handler.HasProgressed = true;
    }

    public void OnProgression()
    {
        Debug.Log("Level Succeed");
        _bridge.SetActive(true);
        _execBridgeAppearingAnimation = true;
    }

    private void Update()
    {
        if (_execBridgeAppearingAnimation)
        {
            // Moves the bridge towards the 
            Vector3 curPos = _bridge.transform.localPosition;
            _bridge.transform.localPosition = Vector3.MoveTowards(curPos, new Vector3(curPos.x, 0, curPos.z), _bridgeRisingSpeed * Time.deltaTime);

            // Gradually increase the alpha value
            _bridgeColor.a = Mathf.MoveTowards(_bridgeColor.a, 1f, _bridgeAppearingSpeed * Time.deltaTime);
            _bridgeRenderer.material.color = _bridgeColor;
        }
    }
}