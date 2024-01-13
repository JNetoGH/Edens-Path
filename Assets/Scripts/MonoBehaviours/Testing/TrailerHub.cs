using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using NaughtyAttributes;
using UnityEngine;

public class TrailerHub : MonoBehaviour
{

    [SerializeField, Required] private Camera _activeCamera; 
    private List<CinemachineBrain> _brains;
    private List<CinemachineVirtualCamera> _virtuals;
    private List<Camera> _cams;
    
    void Start()
    {
        _brains = FindObjectsOfType<CinemachineBrain>().ToList();
        _virtuals = FindObjectsOfType<CinemachineVirtualCamera>().ToList();
        _cams = FindObjectsOfType<Camera>().ToList();
    }
    
    void Update()
    {
        _brains.ForEach(b => b.gameObject.SetActive(false));
        _virtuals.ForEach(v => v.gameObject.SetActive(false));
        _cams.ForEach(c => c.gameObject.SetActive(false));
        _activeCamera.gameObject.SetActive(true);
    }
    
}
