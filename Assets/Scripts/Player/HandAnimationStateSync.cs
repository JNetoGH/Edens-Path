using UnityEngine;

public class HandAnimationStateSync : MonoBehaviour
{

    [SerializeField] private PickupSystem _pickupSytem;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("isPickingUp", _pickupSytem.IsPickingUp);
    }
}
