using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : NetworkBehaviour
{
    [SerializeField] [SyncVar(hook = (nameof(CreatorNameUpdate)))] public string creatorName;
    [SerializeField] [SyncVar(hook = (nameof(DamageUpdate)))] public int damage;
    [SerializeField] [SyncVar(hook = (nameof(PlayerTransformUpdate)))] public Transform playerTransform;
    //store team here instead of Creator

    private void Start()
    {
        //GetComponent<Rigidbody>().AddForce(transform.forward * 30f);
        Debug.Log($"CN:{creatorName}, D:{damage}, PT:{playerTransform}"); 

        GetComponent<Rigidbody>().AddForce(playerTransform.forward * 12f, ForceMode.Impulse);

        StartCoroutine(WaitAndPrint(1f));
    }

    public void CreatorNameUpdate(string _oldValue, string _newValue){ Debug.Log($"Old:{_oldValue}, New:{_newValue}"); }
    public void DamageUpdate(int _oldValue, int _newValue) { Debug.Log($"Old:{_oldValue}, New:{_newValue}"); }
    public void PlayerTransformUpdate(Transform _oldValue, Transform _newValue) { Debug.Log($"Old:{_oldValue}, New:{_newValue}"); }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        print("Coroutine ended: " + Time.time + " seconds");
        NetworkServer.Destroy(gameObject);
    }
}
