using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;


public class Level1Manager : MonoBehaviour, ILevelProgress
{
    
    [Header("Burning Tree Cutscene (will be passed to the state machine)")]
    [SerializeField] private GameObject _burningTreeContainer;
    [SerializeField] private GameObject _bird;
    [SerializeField] private GameObject _vinylDisc;
    [SerializeField] private CinemachineVirtualCamera _burningTreeSeqCam1;
    [SerializeField] private CinemachineVirtualCamera _burningTreeSeqCam2;
    [SerializeField] private CinemachineVirtualCamera _burningTreeSeqCam3;
    
    [Header("Bridge Cutscene")]
    [SerializeField] private GameObject _bridge;
    [SerializeField] private float _bridgeRisingSpeed = 2;
    [SerializeField] private float _bridgeAppearingSpeed = 3;
    
    // Shall be removed soon
    private List<LitableStick> _sticks;
    
    // Burning tree cutscene control variable
    private bool _execBurningTreeCutscene = false;
    
    // Bridge cutScene fields
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
    
    public void OnProgression()
    {
        Debug.Log("Level Succeed");
        _bridge.SetActive(true);
        _execBridgeAppearingAnimation = true;
    }
    
    public void TriggerBurningTreeCutscene()
    {
        // Called Once at method's call
        _execBurningTreeCutscene = true;
        FindObjectOfType<PickupSystem>().ReleaseCurrentObject();
    }
    
    private void Update()
    {
        if (_execBurningTreeCutscene)
        {
            
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