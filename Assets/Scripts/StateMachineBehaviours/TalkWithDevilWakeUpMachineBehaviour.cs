using UnityEngine;


public class TalkWithDevilWakeUpMachineBehaviour : StateMachineBehaviour
{
    
    [SerializeField] private float _cameraXInitialRotation = 50f;
    [SerializeField] private float _cameraRotationSpeed = 6f;
    [SerializeField] private float _delayToLookUpInSeconds = 6.5f; 
    private bool _canLookUp = false;
    private float _lookUpTimer = 0;
    private GameObject _playerCamera;
    private GameObject _devil;
    private Vector3 _animatedRotation;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _canLookUp = false;
        _lookUpTimer = 0;
        
        // Finds player's camera.
        _playerCamera = FindObjectOfType<CameraController>().gameObject;
        if (_playerCamera is null)
        {
            Debug.LogWarning("FirstTalkWithDevilMachineBehaviour could not find Player's Camera");
            return;
        }

        // Looks to devil in order to sync the axis.
        _devil = SuperTag.GetFirstObjectWithSuperTag("Devil");
        if (_devil is null)
        {
            Debug.LogWarning("FirstTalkWithDevilMachineBehaviour could not find devil using the SuperTag system");
            return;
        }
          
        // Looks to the ground to set the animation.
        _playerCamera.transform.LookAt(_devil.transform);  
        Vector3 currentRotation = _playerCamera.transform.rotation.eulerAngles;
        currentRotation.x = _cameraXInitialRotation;
        _playerCamera.transform.rotation = Quaternion.Euler(currentRotation);
        
        // Syncs the Camera Controller with the new rotation
        CameraController cameraController = FindObjectOfType<CameraController>();
        cameraController.RotationY = currentRotation.y;
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
            if (_animatedRotation.x <=  xTarget)
            {
                _animatedRotation.x = xTarget;
                _canLookUp = false;
            }
            _playerCamera.transform.rotation = Quaternion.Euler(_animatedRotation);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerCamera.GetComponent<CameraController>().OverrideRotationCache(_animatedRotation.x, _animatedRotation.y);
    }
}
