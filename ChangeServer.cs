using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeServer : MonoBehaviour
{
    public string HandTag = "HandTag";

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == HandTag)
        {
        	PhotonNetwork.JoinRandomRoom();
        }
    }
}