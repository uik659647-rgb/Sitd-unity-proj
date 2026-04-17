using System.Collections;
using UnityEngine;
using Photon.Pun;

public class ChangeScene : MonoBehaviourPunCallbacks
{
    public string SceneName;
    public string handTag = "HandTag";

    private bool isSwitching = false;

    void OnTriggerEnter(Collider other)
    {
        // Fix: Removed the extra semicolon that was causing instant-triggering
        if (other.CompareTag(handTag) && !isSwitching)
        {
            isSwitching = true;
            StartCoroutine(CleanNetworkSwitch());
        }
    }

    IEnumerator CleanNetworkSwitch()
    {
        // 1. Sync the scene for everyone (if you are the Master Client)
        // This keeps Photon connected during the transition
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(SceneName);
        }
        else if (!PhotonNetwork.InRoom)
        {
            // If you're offline/testing, just load it normally
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
        }

        yield return null;
    }
}