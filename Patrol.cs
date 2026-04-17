using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;

public class Patrol : MonoBehaviourPunCallbacks
{
    public Transform[] points;
    private int currentPointIndex = 0;
    private NavMeshAgent agent;
    private PhotonView pv;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        agent = GetComponent<NavMeshAgent>();
        
        if (pv.IsMine || !PhotonNetwork.InRoom)
        {
            SetNextDestination();
        }
    }

    private void Update()
    {
        if (!pv.IsMine) return;

        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            SetNextDestination();
        }
    }

    private void SetNextDestination()
    {
        if (points.Length == 0)
            return;

        currentPointIndex = (currentPointIndex + 1) % points.Length;
        agent.destination = points[currentPointIndex].position;
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer == newMasterClient)
        {
            pv.RequestOwnership();
        }
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            pv.RequestOwnership();
        }
    }
}