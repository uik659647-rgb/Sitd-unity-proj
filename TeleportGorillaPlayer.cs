using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TeleportGorillaPlayer : MonoBehaviour
{
    public Transform GorillaPlayer;
    public GameObject[] ObjectsToDisable;
    public Transform TeleportLocation;
    public float WaitTime;
    public GameObject TeleportOverlay;
    public AudioSource TeleportSound;
    public Renderer[] ObjectsToDisableRENDERER;
    public bool usesrenderer = false;

    private LayerMask defaultLayers;

    public string objectToSpawn;

    public Transform whereDoesItSpawn;

    public void Start()
    {
        defaultLayers = GorillaLocomotion.Player.Instance.locomotionEnabledLayers;
    }

    void OnTriggerEnter(Collider Other)
    {
        if (Other.gameObject.CompareTag("MainCamera"))
        {
            SpawnObject();
            TeleportOverlay.SetActive(true);
            TeleportSound.Play();
            foreach (GameObject OTD in ObjectsToDisable)
            {
                OTD.SetActive(false);
            }
            if (usesrenderer)
            {
                foreach (Renderer OTD2 in ObjectsToDisableRENDERER)
                {
                    OTD2.enabled = false;
                }
            }
            StartCoroutine(TPWD());
        }
   
    }

    [PunRPC]
    private void SpawnObject()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (whereDoesItSpawn != null)
            {
                PhotonNetwork.Instantiate(objectToSpawn, whereDoesItSpawn.position, Quaternion.identity);
            }
        }
    }

    IEnumerator TPWD()
    {
        yield return new WaitForSeconds(WaitTime);
        //No collisions
        GorillaLocomotion.Player.Instance.locomotionEnabledLayers = default;
        GorillaLocomotion.Player.Instance.headCollider.enabled = false;
        GorillaLocomotion.Player.Instance.bodyCollider.enabled = false;
        GorillaPlayer.position = TeleportLocation.position;
        GorillaPlayer.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSeconds(WaitTime);
        //Enable collisions again
        GorillaLocomotion.Player.Instance.locomotionEnabledLayers = defaultLayers;
        GorillaLocomotion.Player.Instance.headCollider.enabled = true;
        GorillaLocomotion.Player.Instance.bodyCollider.enabled = true;
        foreach (GameObject OTD in ObjectsToDisable)
        {
            OTD.SetActive(true);
        }
        if (usesrenderer)
        {
            foreach (Renderer OTD2 in ObjectsToDisableRENDERER)
            {
                OTD2.enabled = true;
            }
        }
        TeleportOverlay.SetActive(false);
    }
}
