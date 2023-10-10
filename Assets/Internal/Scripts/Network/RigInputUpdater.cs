using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


public struct RigInput : INetworkInput
{
    public Vector3 playAreaPosition;
    public Quaternion playAreaRotation;
    public Vector3 leftHandPosition;
    public Quaternion leftHandRotation;
    public Vector3 rightHandPosition;
    public Quaternion rightHandRotation;
    public Vector3 headsetPosition;
    public Quaternion headsetRotation;

}

public class RigInputUpdater : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _leftArm;
    [SerializeField] private Transform _rightArm;

    private RigInput _rigInput;

    public NetworkRunner runner;

    public enum RunnerExpectations
    {
        NoRunner, // For offline usages
        PresetRunner,
        DetectRunner // should not be used in multipeer scenario
    }
    public RunnerExpectations runnerExpectations = RunnerExpectations.DetectRunner;

    bool searchingForRunner = false;

    public async Task<NetworkRunner> FindRunner()
    {
        while (searchingForRunner) await Task.Delay(10);
        searchingForRunner = true;
        if (runner == null && runnerExpectations != RunnerExpectations.NoRunner)
        {
            if (runnerExpectations == RunnerExpectations.PresetRunner || NetworkProjectConfig.Global.PeerMode == NetworkProjectConfig.PeerModes.Multiple)
            {
                Debug.LogWarning("Runner has to be set in the inspector to forward the input");
            }
            else
            {
                // Try to detect the runner
                runner = FindObjectOfType<NetworkRunner>(true);
                var searchStart = Time.time;
                while (searchingForRunner && runner == null)
                {
                    if (NetworkRunner.Instances.Count > 0)
                    {
                        runner = NetworkRunner.Instances[0];
                    }
                    if (runner == null)
                    {
                        await System.Threading.Tasks.Task.Delay(10);
                    }
                }
            }
        }
        searchingForRunner = false;
        return runner;
    }

    protected virtual async void Start()
    {
        await FindRunner();
        if (runner)
        {
            runner.AddCallbacks(this);
        }
    }

    private void OnDestroy()
    {
        if (searchingForRunner) Debug.LogError("Cancel searching for runner in HardwareRig");
        searchingForRunner = false;
        if (runner) runner.RemoveCallbacks(this);
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        Debug.Log("Input");
        _rigInput = new RigInput();
        _rigInput.playAreaPosition = transform.position;
        _rigInput.playAreaRotation = transform.rotation;
        _rigInput.headsetPosition = _head.position;
        _rigInput.headsetRotation = _head.rotation;
        _rigInput.leftHandPosition = _leftArm.position;
        _rigInput.leftHandRotation = _leftArm.rotation;
        _rigInput.rightHandPosition = _rightArm.position;
        _rigInput.rightHandRotation = _rightArm.rotation;

        input.Set(_rigInput);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }
}
