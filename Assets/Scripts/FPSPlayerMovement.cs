using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class FPSPlayerMovement : NetworkBehaviour
{
    public float speed = 5;
    public CharacterController controller;
    float gravity = -15f;

    Vector3 velocity;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    public GameObject Camera;

    [SyncVar(hook = nameof(PlayerNameChange))]//update to server
    public string playerName;
    [SyncVar(hook = nameof(PlayerHealthChange))]//update to server
    public int playerHealth;

    public TMP_Text nameText;
    public TMP_Text healthText;

    public GameObject bulletPrefab;
    public GameObject bulletSpawnLocation;

    public MouseLook mouseLookScript;

    [SyncVar(hook = nameof(PlayerDamgeChange))]
    public int playerGunDamage = 1;

    [SyncVar] 
    public string playerNumber;

    public BasicNetworkManager myNetworkManager;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        myNetworkManager = GameObject.Find("BasicNetworkManager").GetComponent<BasicNetworkManager>();

        if (isLocalPlayer) { Camera.SetActive(true); } //set the camera to active only on the local client
        if (isLocalPlayer) { bulletSpawnLocation.SetActive(true); } //so bullets will spawn on each players
        //Reset all variables here so it updates on the server
        mouseLookScript.enabled = true;
        CmdInitVariables();
    }

    void Update()
    {
        if (!isLocalPlayer) return;
        if (!hasAuthority) return;

        if (Input.GetKeyDown(KeyCode.R))
        { UpdatePlayerDamage(); }

        PlayerMovement();
        if (!Input.GetKeyDown(KeyCode.Space)) { return; } 
        SpawnBullet();
    }

    //Server SyncVars Updates
    public void PlayerNameChange(string oldValue, string newValue) { Debug.Log($"Player{playerNumber} => The old name was: {oldValue}, then updated name is: {newValue}"); nameText.text = playerName; }
    public void PlayerHealthChange(int oldValue, int newValue) { Debug.Log($"Player{playerNumber} => health was: {oldValue}, then updated health is: {newValue}"); healthText.text = playerHealth.ToString(); }
    public void PlayerDamgeChange(int oldValue, int newValue) { Debug.Log($"Player{playerNumber} => damage was: {oldValue}, then updated damage is: {newValue}"); playerGunDamage = newValue; }
    
    [Command]
    public void CmdInitVariables()
    {
        playerNumber = myNetworkManager.numPlayers.ToString();
        playerName = $"Player{playerNumber}";
        playerHealth = 5;
    }

    public void PlayerMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move* speed * Time.deltaTime);

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * groundDistance, Color.blue);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, groundDistance, groundMask)) 
        {
            velocity.y = 0f;
        }
        else
        {
            //adding gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }//This would be a command but the networkTransform compoent does that for me

    [Command]
    public void UpdatePlayerDamage()
    {
        playerGunDamage += 1;
    }

    [Command]
    public void SpawnBullet()
    {
        //bulletPrefab
        GameObject bulletInstance = Instantiate(bulletPrefab, bulletSpawnLocation.transform.position, Quaternion.identity);
        
        //Assign bullinstance logic here
        BulletScript bulletScriptInstance = bulletInstance.GetComponent<BulletScript>();
        bulletScriptInstance.creatorName = playerName;
        bulletScriptInstance.damage = playerGunDamage;
        bulletScriptInstance.playerTransform = gameObject.transform;
        //Debug.Log($"Before Spawn On server CN:{playerName}, D:{playerGunDamage}, PT:{gameObject.transform}");
        NetworkServer.Spawn(bulletInstance, connectionToClient); //connectionToClient
    }

    public void PlayeDeath()
    {
        //deactivate
        //set them to spawn point

    }
    
    [ServerCallback] //Server only
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "bullet") { return; }

        BulletScript bullet = other.gameObject.GetComponent<BulletScript>();
        if (bullet.creatorName != playerName)
        {
            playerHealth -= bullet.damage; //this is a sync var so it updates on server with out the [Command] 
        }
        NetworkServer.Destroy(other.gameObject);
    }
}
