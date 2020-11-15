using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class PlayerChatSystem : NetworkBehaviour
{
    //chatbox text 
    public TMP_Text chatboxText;
    //message from input 
    public TMP_InputField sendableMessageText;
    //button to send
    public Button sendButton;
    //ChatCanvas to activate one for each person so no over lapping
    public GameObject CanvasChat;

    public string userMessage;

    //when i connect to server set canvas active
    public override void OnStartClient()
    {
        CanvasChat.SetActive(true);
    }

    //when I disconnect set canvas to false
    public override void OnStopClient() { CanvasChat.SetActive(false); base.OnStopClient(); }

    private void Update()
    {
        //SendMessageToServer();

        //if (!isLocalPlayer) { return; } //checks the player thats using it each player has the same instance so they can all use it but it updates locally until send to server
        //if (!Input.GetKeyDown(KeyCode.X)) { return; } 
        //print(sendableMessageText.text);
    }

    public void SendMessageToServer()
    {
        //if (!isLocalPlayer) { return; } //checks the player thats using it
        //if (!Input.GetKeyDown(KeyCode.X)) { return; }
        CmdSendMessage();
        //MyClientRPC(); //update ever one else on this variable
    }

    [Command] //runs this function from the client on the server
    public void CmdSendMessage()
    {
        //send variable to Server
        userMessage = sendableMessageText.text; //updates this locally and send that info to the server
        Debug.Log($"Server has received Text from the client: {userMessage}");
        //sendableMessageText.text
    }

    [ClientRpc] //sends to all clients from Server 
    private void MyClientRPC()
    {
        //userMessage = "newText";
        Debug.Log($"Client has received a message:{userMessage}");
    }
}
