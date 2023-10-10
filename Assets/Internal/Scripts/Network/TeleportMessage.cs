using Fusion;
using General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportMessage : NetworkBehaviour
{
    public void Teleport(int index) { Debug.Log(index); RPC_SendMessage(index.ToString()); }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendMessage(string message, RpcInfo info = default)
    {
        Debug.Log(message);
        RecenterHandler[] handlers = GameObject.FindObjectsOfType<RecenterHandler>();


        if (info.IsInvokeLocal)
        {
            foreach (RecenterHandler handler in handlers)
            {
                Debug.Log("sent: " + message);
                handler.RecenterToPostition(int.Parse(message));
            }
        }
        else
        {
            foreach (RecenterHandler handler in handlers)
            {
                Debug.Log("recieved: "+message);
                handler.RecenterToPostition(int.Parse(message));
            }
        }
        
        

    }
}
