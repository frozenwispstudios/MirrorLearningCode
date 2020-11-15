using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    //This Shows a full loop on send to server from client
    //then from server to client and on only and all clients

    [SyncVar(hook = nameof(WhenThisVariableUpdates))] 
    public int testInt; //updated on server to clients but not to all other so you will have to send this to the server first to update all clients

    private void Update()
    {
        SendMessageToServer();//send message to server from client

        SendMessagefromServerToClients();//Send message to all clients from Server

        SendMessagefromServerToOneClientOnly();//Send Message to one Client only of choice from Server

        SendUpdatedVariable();
    }

    #region This updates local variables to the server for that player instance for all to see (IMPORTANT)
    //Hook Update function
    public void WhenThisVariableUpdates(int oldvalue, int newvalue ){ Debug.Log($"Before updated Value to Server:{oldvalue} after value is updated: {newvalue}"); }

    public void SendUpdatedVariable()
    {
        if (!isLocalPlayer) { return; } //checks the player thats using it
        if (!Input.GetKeyDown(KeyCode.V)) { return; }

        IncreaseValueAndSendToServer();

    }//updates variable called testInt

    [Command]
    public void IncreaseValueAndSendToServer()
    {
        testInt += 1;
    }

    #endregion 

    public void SendMessageToServer()
    {
        if (!isLocalPlayer) { return; } //checks the player thats using it
        if (!Input.GetKeyDown(KeyCode.X)) { return; }
        Debug.Log("Server has sent a Message to all clients");
        //above will happen on client and before server thus will not be sent to server
        CmdOutputText();
    }

    public void SendMessagefromServerToClients()
    {
        if (!Input.GetKeyDown(KeyCode.Z)) { return; }
        MyClientRPC();
    }

    public void SendMessagefromServerToOneClientOnly()
    {
        if (!Input.GetKeyDown(KeyCode.C)) { return; }
        MyTargetRPC();
    }

    [Command]
    private void CmdOutputText()//Send data to the Server from the client
    {
        Debug.Log("Server has received Text from the client");
    }

    [ClientRpc] //sends to all clients from Server 
    private void MyClientRPC()
    {
        Debug.Log("Client has received message");
    }

    [TargetRpc]
    private void MyTargetRPC()//sends to target client ONLY from Server ONLY
    {
        Debug.Log("Test");
    }
}
