using UnityEngine;


public class BurningTreeCutsceneSequence : StateMachineBehaviour
{
   
    [SerializeField] private float _cam1ToCam2Wait = 2f;
    private float _cam1ToCam2Timer = 0;
    private bool _cam1ToCam2Done = false;
    
    [SerializeField] private float _cam2ToCam3Wait = 3f;
    private float _cam2ToCam3Timer = 0;
    private bool _cam2ToCam3Done = false;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Burning cutscene is being executed by State Machine Behaviour");
        TurnOnCinemachineBrain();    
        ChangeToCamera1();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        TurnOffCinemachineBrain();   
        Debug.Log("Burning cutscene is no longer being executed by State Machine Behaviour");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        // Cam1 to Cam2 control
        if (_cam1ToCam2Timer >= _cam1ToCam2Wait && !_cam1ToCam2Done)
        {
            ChangeToCamera2();
            _cam1ToCam2Done = true;
        }
        // Updates Timer (cam1 TO can 2)
        _cam1ToCam2Timer += Time.deltaTime;
        
        // Cam2 to Cam3 control
        if (_cam2ToCam3Timer >= _cam2ToCam3Wait && _cam1ToCam2Done && !_cam2ToCam3Done)
        {
            ChangeToCamera3();
            _cam2ToCam3Done = true;
        }
        // Updates Timer (cam2 TO can 3)
        if (_cam1ToCam2Done)
            _cam2ToCam3Timer += Time.deltaTime;
    }

    private void TurnOnCinemachineBrain()
    {
        Level1Manager.Instance.cinemachineBrain.SetActive(true);
    }
    
    private void TurnOffCinemachineBrain()
    {
        Level1Manager.Instance.cinemachineBrain.SetActive(false);
        SetVirtualCams(false, false, false);
    }
    
    private void ChangeToCamera1()
    {
        SetVirtualCams(true, false, false);
    }
    
    private void ChangeToCamera2()
    {
        SetVirtualCams(false, true, false);
    }
    
    private void ChangeToCamera3()
    {
        SetVirtualCams(false, false, true);
    }

    private void SetVirtualCams(bool cam1, bool cam2, bool cam3)
    {
        Level1Manager.Instance.burningTreeSeqCam1.SetActive(cam1);
        Level1Manager.Instance.burningTreeSeqCam2.SetActive(cam2);
        Level1Manager.Instance.burningTreeSeqCam3.SetActive(cam3);
    }
    
}
