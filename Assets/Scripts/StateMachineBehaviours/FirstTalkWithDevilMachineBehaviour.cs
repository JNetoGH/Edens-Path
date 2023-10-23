using UnityEngine;


public class FirstTalkWithDevilMachineBehaviour : StateMachineBehaviour
{
    
    [SerializeField] private float _cameraXInitialRotation = 50f;
    [SerializeField] private float _cameraRotationSpeed = 6f;
    [SerializeField] private float _delayToLookUpInSeconds = 6.5f; 
    private bool _canLookUp = false;
    private float _lookUpTimer = 0;
    private GameObject _playerCamera;
    private GameObject _devil;
    
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
        _playerCamera.transform.LookAt(_devil.transform);    
        
        // Looks to the ground to set the animation.
        Vector3 currentRotation = _playerCamera.transform.rotation.eulerAngles;
        currentRotation.x = _cameraXInitialRotation;
        _playerCamera.transform.rotation = Quaternion.Euler(currentRotation);
    }
    
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_devil is null || _playerCamera is null)
        {
            Debug.LogWarning("FirstTalkWithDevilMachineBehaviour could not find devil or player's camera");
            return;
        }

        _lookUpTimer += Time.deltaTime;
        _canLookUp = _lookUpTimer >= _delayToLookUpInSeconds;
        Vector3 animatedRotation = _playerCamera.transform.rotation.eulerAngles;
        float xTarget = 0;
        bool hasFinishedLookingUp = _playerCamera.transform.rotation.eulerAngles.x == xTarget;
        
        if (_canLookUp)
        {
            // updates the animation of looking upwards.
            animatedRotation.x -= _cameraRotationSpeed * Time.deltaTime;
            if (animatedRotation.x <=  xTarget)
            {
                animatedRotation.x =  xTarget;
                _canLookUp = false;
            }
            _playerCamera.transform.rotation = Quaternion.Euler(animatedRotation);
        }

        // Allows the player to look around when he finishes looking up.
        if (hasFinishedLookingUp)
        {
            GameManager.CanRotateCamera = true;
            _playerCamera.GetComponent<CameraController>().OverrideRotationCache(animatedRotation.x, animatedRotation.y);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
  
    }
}
