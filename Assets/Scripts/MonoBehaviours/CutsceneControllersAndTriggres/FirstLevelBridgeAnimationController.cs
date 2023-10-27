using System.Collections;
using Cinemachine;
using NaughtyAttributes;
using UnityEngine;


/// <summary>
/// This Cutscene doesn't use the animator because it's too simple.
/// So this animation basically works with COROUTINES.
/// </summary>
public class FirstLevelBridgeAnimationController : ACutsceneController
{

    private const string G1 = "REQUIRED REFENCES";
    [HorizontalLine]
    [BoxGroup(G1), Required, SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [BoxGroup(G1), Required, SerializeField] private GameObject _bridgeContainer;
    
    private const string G2 = "CUTSCENE PARAMETERS";
    [HorizontalLine]
    [BoxGroup(G2), SerializeField] private float _bridgeRisingSpeed = 2;
    [BoxGroup(G2), SerializeField] private float _risingDelay = 1;
    [BoxGroup(G2), SerializeField] private float _exitDelay = 1;
    
    // Bridge cutscene fields
    private bool _triggerCutscene = false;
    private bool _hasFinishedAnimation = false;
    
    private void Start()
    {
        // Deactivates the bridge
        _bridgeContainer.SetActive(false);
        
        // In case the cutscene has been already watched,the bridge will be teleported to the target.
        if (hasBeenAlreadyWatched)
        {
            _bridgeContainer.SetActive(true);
            Vector3 curPos = _bridgeContainer.transform.localPosition;
            Vector3 targetPos = new Vector3(curPos.x,-0.1f, curPos.z);
            _bridgeContainer.transform.localPosition = targetPos;
        }
    }
    
    private void Update()
    {
        if (_triggerCutscene && !hasBeenAlreadyWatched)
            StartCoroutine(UpdateCutscene());
    }
    
    /// <summary>
    /// Coroutine that updates the bridge rising animation during the cutscene.
    /// </summary>
    private IEnumerator UpdateCutscene()
    {
        yield return new WaitForSeconds(_risingDelay);
        
        // Moves the bridge container towards the 
        Vector3 curPos = _bridgeContainer.transform.localPosition;
        Vector3 targetPos = new Vector3(curPos.x, -0.1f, curPos.z);
        _bridgeContainer.transform.localPosition = Vector3.MoveTowards(curPos, targetPos, _bridgeRisingSpeed * Time.deltaTime);
        
        // Coroutines transition
        _hasFinishedAnimation = curPos == targetPos;
        if (_hasFinishedAnimation)
        {
            StopCoroutine(UpdateCutscene());
            StartCoroutine(StopCutscene());
        }
    }

    /// <summary>
    /// Coroutine that stops the cutscene after it finishes.
    /// </summary>
    private IEnumerator StopCutscene()
    {
        yield return new WaitForSeconds(_exitDelay);
        
        GameManager.EnterGameplayMode();
        FindObjectOfType<CinemachineBrain>(includeInactive: true).gameObject.SetActive(false);
        _virtualCamera.enabled = false;
        _triggerCutscene = false;
        hasBeenAlreadyWatched = true;
        StopAllCoroutines();
    }
    
    /// <summary>
    /// Initiates and plays the cutscene animation.
    /// </summary>
    public override void PlayCutscene()
    {
        GameManager.EnterCutsceneMode();
       
        FindObjectOfType<CinemachineBrain>(includeInactive: true).gameObject.SetActive(true);
        _bridgeContainer.SetActive(true);
        _virtualCamera.enabled = true;
        _triggerCutscene = true;
    }
    
}
