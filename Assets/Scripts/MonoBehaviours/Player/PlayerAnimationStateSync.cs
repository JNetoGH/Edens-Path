using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationStateSync : MonoBehaviour
{

    private Animator _animator;
    private PickupSystem _pickupSystem;
    private PlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _pickupSystem = FindObjectOfType<PickupSystem>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("isPickingUp", _pickupSystem.IsPickingUp);
        _animator.SetBool("isWalking", _playerController.IsMoving);
        _animator.SetBool( "isJumping", _playerController.VerticalVelocity > 0);
    }
    
}
