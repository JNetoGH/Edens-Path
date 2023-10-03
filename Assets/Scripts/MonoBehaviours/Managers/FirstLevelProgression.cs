using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class FirstLevelProgression : MonoBehaviour
{
    private List<LitableStick> _sticks;
    [SerializeField] private GameObject _bridge;
    [SerializeField] private float _bridgeRisingSpeed = 2;
    [SerializeField] private float _bridgeAppearingSpeed = 3;
    
    // Burning tree cutscene
    private bool _execBurningTreeCutscene = false;
    
    // Bridge cutScene
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

    public void ExecBurningTreeCutscene()
    {
        _execBurningTreeCutscene = true;
    }
    
    // Called by the _onLevelValidation UnityEvent at the LevelProgressionHandler
    public void OnValidation(LevelProgressionHandler handler)
    {
        // Updates the list of Litables Sticks to be verified every frame,
        // because they can be instantiated from the inventory.
        _sticks = FindObjectsOfType<LitableStick>().ToList<LitableStick>();

        // Then checks if any of them is on fire.
        bool isThereFire = false;
        _sticks.ForEach(s =>
        {
            if (s.isLit)
                isThereFire = true;
        });
        if (isThereFire)
            handler.HasProgressed = true;
    }

    // Called by the _onLevelProgress UnityEvent the LevelProgressionHandler
    public void OnProgression()
    {
        Debug.Log("Level Succeed");
        _bridge.SetActive(true);
        _execBridgeAppearingAnimation = true;
    }

    private void Update()
    {

        if (_execBurningTreeCutscene)
        {
            FindObjectOfType<PickupSystem>().ReleaseCurrentObject();
            _execBurningTreeCutscene = false;
        }
        
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