using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class PlayerInteraction : NetworkBehaviour
{
    [SyncVar(hook = nameof(PlayerNameChange))]//update to server
    string playerName;
    [SyncVar(hook = nameof(PlayerHealthChange))]//update to server
    int playerHealth;

    public TMP_Text nameText;
    public TMP_Text healthText;

    public GameObject bulletPrefab;
    public Transform bulletSpawnLocation;
    private void Start()
    {
        //Reset all variables here so it updates on the server
        playerName = "Player";
        playerHealth = 5;
    }


    //Server SyncVars Updates
    public void PlayerNameChange(string oldValue, string newValue) { Debug.Log($"The old name was: {oldValue}, then updated name is: {newValue}"); nameText.text = playerName; }
    public void PlayerHealthChange(int oldValue, int newValue) { Debug.Log($"The health was: {oldValue}, then updated health is: {newValue}"); healthText.text = playerHealth.ToString();}

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) { return; } //checks that its a local player
        if (!Input.GetKeyDown(KeyCode.Space)) { return; }
        SpawnBullet();
        //Spawnbullet the player that spawned it or just the team
        //if you collide with bullet destroy it from all clients
        //and take damage
    }

    [Command]
    public void SpawnBullet()
    {
        //bulletPrefab
        GameObject bulletInstance = Instantiate(bulletPrefab, bulletSpawnLocation.position, transform.rotation);
        //Asign bullinstance logic here
        
        NetworkServer.Spawn(bulletInstance/* connectionToClient*/);
    }

    [Command]
    void TakeDamage() // when you collide with a bullet
    {
        playerHealth -= 1; 
    }

    [ClientRpc] //sends to all clients from Server 
    private void MyClientRPC()
    {
        Debug.Log("Client has received message");
    }
}
