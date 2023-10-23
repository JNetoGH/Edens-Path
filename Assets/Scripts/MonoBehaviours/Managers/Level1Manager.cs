using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;


public class Level1Manager : MonoBehaviour, ILevelProgressValidator
{

    // Singleton Pattern
    public static Level1Manager Instance { get; private set; }

    [SerializeField] public GameObject talkwithDevilCutsceneContainer;

    [Header("Burning Tree Cutscene (will be passed to other scripts via Singleton)")]
    [SerializeField] public GameObject burningTreeCutsceneContainer;
    [SerializeField] public GameObject originalTreeModel;
    [SerializeField] public GameObject burntTreeModel;
    [SerializeField] public GameObject birdContainer;
    [SerializeField] public GameObject bird;
    [SerializeField] public GameObject vinylDisc;
    
    [Header("Bridge Cutscene")]
    [SerializeField] private GameObject _bridgeContainer;
    [SerializeField] private List<GameObject> _bridges;
    [SerializeField] private float _bridgeRisingSpeed = 2;
    [SerializeField] private float _bridgeAppearingSpeed = 3;
    
    // Bridge cutScene fields
    private bool _execBridgeAppearingAnimation = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Deactivates the bridge
        _bridgeContainer.SetActive(false);
        
        // Subscribes at the handler
        CallAtStartAndSubscribeToHandler(GetComponent<LevelProgressionHandler>());
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            TriggerTalkWithDevilCutscene();

        if (_execBridgeAppearingAnimation)
        {
            // Moves the bridge container towards the 
            Vector3 curPos = _bridgeContainer.transform.localPosition;
            _bridgeContainer.transform.localPosition = Vector3.MoveTowards(
                curPos, 
                new Vector3(curPos.x, -0.1f, curPos.z), 
                _bridgeRisingSpeed * Time.deltaTime);
        }
    }
    
    public void OnValidation(LevelProgressionHandler handler)
    {
        // handler.HasProgressed = true;
    }
    
    public void OnProgression()
    {
        Debug.Log("Level Succeed");
        _bridgeContainer.SetActive(true);
        _execBridgeAppearingAnimation = true;
    }

    public void CallAtStartAndSubscribeToHandler(LevelProgressionHandler handler)
    {
        if (handler == null)
            Debug.LogError($"Tried to subscribe a ILevelProgressValidator to a null LevelProgressionHandler");
        handler.iLevelProgressValidator = this;
    }

    public void TriggerTalkWithDevilCutscene()
    {
        talkwithDevilCutsceneContainer.GetComponent<Animator>().SetTrigger("StartSequence");
    }

    // Called by the BurningTreeColision via Singleton
    public void TriggerBurningTreeCutscene()
    {
        burningTreeCutsceneContainer.GetComponent<Animator>().SetTrigger("StartSequence");
    }
    
}