using Photon.Pun;
using Photon.VR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineRig : MonoBehaviour
{
    public SkinnedMeshRenderer[] OfflineRigParts;

    void Update()
    {
        if (!PhotonNetwork.InRoom)
        {
            SetOfflineRigsActive(true);
        }
        else
        {
            SetOfflineRigsActive(false);
        }
    }

    void SetOfflineRigsActive(bool active)
    {
        // Iterate through the array and set the active state for each GameObject
        foreach (SkinnedMeshRenderer rig in OfflineRigParts)
        {
            if (rig != null)
            {
                rig.gameObject.SetActive(active);
            }
        }
    }
}
