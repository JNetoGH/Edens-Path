using UnityEngine;


public class TalkWithDevilWakeUpMachineBehaviour : StateMachineBehaviour
{
    
    [SerializeField] private float _cameraXInitialRotation = 50f;
    [SerializeField] private float _cameraRotationSpeed = 6f;
    [SerializeField] private float _delayToLookUpInSeconds = 6.5f; 
   
    private bool _canLookUp = false;
    private float _lookUpTimer = 0;
    private CameraController _playerCamera;
    private GameObject _devil;
    private Vector3 _animatedRotation;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _canLookUp = false;
        _lookUpTimer = 0;
        
        // Finds player's camera and the devil.
        _playerCamera = FindObjectOfType<CameraController>();
        _devil = SuperTag.GetFirstObjectWithSuperTag("Devil");
        
        if (_playerCamera is null)
        {
            Debug.LogWarning("FirstTalkWithDevilMachineBehaviour could not find Player's Camera");
            return;
        }
        if (_devil is null)
        {
            Debug.LogWarning("FirstTalkWithDevilMachineBehaviour could not find devil using the SuperTag system");
            return;
        }
        
        SyncYWithDevil();
        LookToTheGround();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_devil is null || _playerCamera is null)
        {
            Debug.LogWarning("FirstTalkWithDevilMachineBehaviour could not find devil or player's camera");
            return;
        }

        const float xTarget = 0;
        _lookUpTimer += Time.deltaTime;
        _canLookUp = _lookUpTimer >= _delayToLookUpInSeconds;
        _animatedRotation = _playerCamera.transform.rotation.eulerAngles;
        
        if (_canLookUp)
        {
            // updates the animation of looking upwards.
            _animatedRotation.x -= _cameraRotationSpeed * Time.deltaTime;
            if (_animatedRotation.x <= xTarget)
            {
                _animatedRotation.x = xTarget;
                _canLookUp = false;
            }
            _playerCamera.transform.rotation = Quaternion.Euler(_animatedRotation);
        }
        
        SyncYWithDevil();
    }
    
    /// <summary>
    /// Looks to the ground to set the animation.
    /// </summary>
    private void LookToTheGround()
    {
        Vector3 currentRotation = _playerCamera.transform.rotation.eulerAngles;
        currentRotation.x = _cameraXInitialRotation;
        _animatedRotation.x = currentRotation.x;
        _playerCamera.transform.rotation = Quaternion.Euler(currentRotation);
    }
    
    /// <summary>
    /// Syncs the Camera Controller with the new rotation in the y axis.
    /// </summary>
    private void SyncYWithDevil()
    {
        _playerCamera.transform.LookAt(_devil.transform);
        Vector3 currentCamRotation = _playerCamera.transform.rotation.eulerAngles;
        currentCamRotation.x = _animatedRotation.x;
        _playerCamera.transform.rotation = Quaternion.Euler(currentCamRotation);
    }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerCamera.OverrideRotationCache(_animatedRotation.x, _animatedRotation.y);
    }
    
}
