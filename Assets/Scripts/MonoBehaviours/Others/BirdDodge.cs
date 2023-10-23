using System;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class BirdDodge : MonoBehaviour
{

    // Singleton to be accessed by the state machine behaviour.
    public static BirdDodge Instance { get; private set; }
    public bool hasFinishedPreviousDodge;
    
    [SerializeField] private float _distanceFromPlayerToDodge = 1f;
    private Animator _animator;
    private GameObject _player;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        Instance = this;
        hasFinishedPreviousDodge = true;
    }

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>().gameObject;
    }

    void Update()
    {
        float distanceFromPlayer = (_player.transform.position - this.transform.position).magnitude;
        if (distanceFromPlayer <= _distanceFromPlayerToDodge)
            if (hasFinishedPreviousDodge)
                _animator.SetTrigger("DodgePlayer");
    }
    
}
