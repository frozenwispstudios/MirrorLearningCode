using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//This Class is used like a document not a Useable Class
//DO NOT USE

public class MirrorLearningScript : NetworkManager
{
    //In Order of runtime
    #region Server
    //Start Events
    public override void OnStartServer(){base.OnStartServer();}
    public void OnServerSceneChanged() { }

    //Client Connect
    public override void OnServerConnect(NetworkConnection conn) {base.OnServerConnect(conn);}
    public override void OnServerReady(NetworkConnection conn){base.OnServerReady(conn);}
    public override void OnServerAddPlayer(NetworkConnection conn){base.OnServerAddPlayer(conn);}

    //Client Disconnect
    public override void OnServerDisconnect(NetworkConnection conn){base.OnServerDisconnect(conn);}

    //Stop
    public override void OnStopServer(){base.OnStopServer();}

    //Client only
    [Server]
    public void ClientFunction() { /*run from Server only*/}
    #endregion

    #region Client
    //Start
    public override void OnStartClient(){}
    public void OnClientConnect(){}
    public void OnClientChangeScene() { }
    public void OnClientSceneChanged() { }

    //Stop
    public override void OnStopClient(){base.OnStopClient();}
    public override void OnClientDisconnect(NetworkConnection conn){base.OnClientDisconnect(conn);}

    //Client only
    //[Client]
    //public void ClientOnlyFunction(){ /*run from client only*/}

    //[Command] 
    //public void CmdClientCommandFunction() { /*client to server only*/ }

    //[Command(ignoreAuthority = true)]
    //public void CmdClientCommandIgnoreAuthority() { /*sends to server but all players can interact with this command not just from one user*/ }
    #endregion


}
