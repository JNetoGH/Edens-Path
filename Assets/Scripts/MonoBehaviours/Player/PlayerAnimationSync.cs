using NaughtyAttributes;
using UnityEngine;

public class PlayerAnimationSync : MonoBehaviour
{

    // Serialized
    [SerializeField, Required] private Animator _animator;
   
    // Components
    private PlayerController _playerController;
    private GroundDetector _groundDetector;
    private PickupSystem _pickupSystem;

    // Animator Parameters
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int IsPickingUp = Animator.StringToHash("isPickingUp");
    private static readonly int IsOutlining = Animator.StringToHash("isOutlining");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
    private static readonly int Jump = Animator.StringToHash("jump");

    // Start is called before the first frame update
    void Start()
    {
        _pickupSystem = FindObjectOfType<PickupSystem>();
        _playerController = FindObjectOfType<PlayerController>();
        _groundDetector = FindObjectOfType<GroundDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_animator is null)
        {
            Debug.LogWarning($"{nameof(PlayerController)} has no animator set");
            return;
        } 
        _animator.SetBool(IsOutlining, _pickupSystem.IsOutlining);
        _animator.SetBool(IsPickingUp, _pickupSystem.IsPickingUp);
        _animator.SetBool(IsGrounded, _groundDetector.IsGrounded && _playerController.VerticalVelocity < 0.1f);
        _animator.SetBool(IsWalking, _playerController.IsMoving && _groundDetector.IsGrounded);
        if (_playerController.JumpedThisFrame)
            _animator.SetTrigger(Jump);
    }
    
}
