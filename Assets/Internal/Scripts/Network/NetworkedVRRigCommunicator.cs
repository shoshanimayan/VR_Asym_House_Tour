using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class NetworkedVRRigCommunicator : NetworkBehaviour
{

    [SerializeField] private NetworkTransform _headNetwork;
    [SerializeField] private NetworkTransform _leftArmNetwork;
    [SerializeField] private NetworkTransform _rightArmNetwork;

    [SerializeField] private Transform _head;
    [SerializeField] private Transform _leftArm;
    [SerializeField] private Transform _rightArm;

    
    private NetworkTransform _networkTransform;
    


    public bool IsLocalNetworkRig => Object.HasInputAuthority;

    // Start is called before the first frame update
    private void Awake()
    {
        _networkTransform = GetComponent<NetworkTransform>();
    }

    


    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        // update the rig at each network tick
        if (GetInput<RigInput>(out var input) && !IsLocalNetworkRig)
        {
            transform.position = input.playAreaPosition;
            transform.rotation = input.playAreaRotation;

            _head.position = input.headsetPosition;
            _head.rotation = input.headsetRotation;
            _leftArm.position = input.leftHandPosition;
            _leftArm.rotation = input.leftHandRotation;
            _rightArm.position = input.rightHandPosition;
            _rightArm.rotation = input.rightHandRotation;
        }
    }

    public override void Render()
    {
        base.Render();
        if (!IsLocalNetworkRig)
        {
            
            _networkTransform.InterpolationTarget.position = transform.position;
            _networkTransform.InterpolationTarget.rotation = transform.rotation;
            _leftArmNetwork.InterpolationTarget.position = _leftArm.transform.position;
            _leftArmNetwork.InterpolationTarget.rotation = _leftArm.transform.rotation;
            _rightArmNetwork.InterpolationTarget.position = _rightArm.transform.position;
            _rightArmNetwork.InterpolationTarget.rotation = _rightArm.transform.rotation;
            _headNetwork.InterpolationTarget.position = _head.transform.position;
            _headNetwork.InterpolationTarget.rotation = _head.transform.rotation;
        }
    }
}
