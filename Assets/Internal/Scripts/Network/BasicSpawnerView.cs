using UnityEngine;
using Core;
using System.Collections;
using System.Collections.Generic;
using Fusion.Sockets;
using Fusion;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using General;

namespace Network
{
	public class BasicSpawnerView: MonoBehaviour,IView, INetworkRunnerCallbacks
    {

        ///  INSPECTOR VARIABLES       ///
        [SerializeField] private NetworkPrefabRef _playerPrefabVR;
        [SerializeField] private GameObject _dispalyView;
        [SerializeField] private GameObject _vrView;

        [SerializeField] private Canvas _hostCanvas;
        [SerializeField] private Canvas _loadingCanvas;

        ///  PRIVATE VARIABLES         ///
        BasicSpawnerMediator _mediator;
        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
        private bool _hasHost;
        ///  PRIVATE METHODS           ///

        ///  PUBLIC API                ///
        public void Init(BasicSpawnerMediator mediator)
        { 
            _mediator = mediator;
            _hostCanvas.enabled = false;
        }
        ///network
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                if (_hasHost)
                {
                    // Create a unique position for the player
                    Vector3 spawnPosition = new Vector3(0, 0, -2.5f);
                    Quaternion spawnRotation = (Quaternion.identity);
                    NetworkObject networkPlayerObject = runner.Spawn(_playerPrefabVR, spawnPosition, spawnRotation, player);
                    // Keep track of the player avatars so we can remove it when they disconnect
                    _spawnedCharacters.Add(player, networkPlayerObject);
                }
                else
                {
                    _hasHost = true;
                    _hostCanvas.enabled = true;
                    _vrView.SetActive(false); 
                    _loadingCanvas.enabled = false;
                    /*
                    // Create a unique position for the player
                    Vector3 spawnPosition = ( new Vector3(3.61f, 11.62f, -7.68f));
                    Quaternion spawnRotation = ( Quaternion.Euler(61.196f, 0, 0));
                    NetworkObject networkPlayerObject = runner.Spawn(( _playerPrefab), spawnPosition, spawnRotation, player);
                    // Keep track of the player avatars so we can remove it when they disconnect
                    _spawnedCharacters.Add(player, networkPlayerObject);
                    _hasHost = true;
                    */
                }
            }
            if (runner.IsClient)
            {
                _loadingCanvas.enabled = false;

                _vrView.SetActive(true);

                _dispalyView.SetActive(false);

            }

        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            // Find and remove the players avatar
            if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
            {
                runner.Despawn(networkObject);
                _spawnedCharacters.Remove(player);
            }
        }
        public void OnInput(NetworkRunner runner, NetworkInput input) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnDisconnectedFromServer(NetworkRunner runner) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) {
            Debug.Log(sessionList.Count);
            foreach (var session in sessionList) { if (session.Name == "TestRoom") { _hasHost = true; } }
        }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }

       

        private NetworkRunner _runner;

        private void OnGUI()
        {
            if (_runner == null)
            {


                if (_mediator.IsVR())
                {
                    _hasHost = true;
                    _vrView.SetActive(true);
                    _dispalyView.SetActive(false);
                    StartGame(GameMode.Client);

                }
                else
                {
                    _vrView.SetActive(false);

                    if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
                    {
                        StartGame(GameMode.Host);
                    }
                    if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
                    {
                        _hasHost = true;

                        StartGame(GameMode.Client);
                    }

                }
            }
        }

        async void StartGame(GameMode mode)
        {
            
            
            // Create the Fusion runner and let it know that we will be providing user input
            _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;
            _runner.name = "NetworkRunner";
            // Start or join (depends on gamemode) a session with a specific name
            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "TestRoom",
                Scene = SceneManager.GetActiveScene().buildIndex,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }

        





    }
}
