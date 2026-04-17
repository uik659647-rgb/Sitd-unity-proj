using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class aidoesntworkwhenviewismine : MonoBehaviour
{
	public PhotonView photonView;

	public NavMeshAgent navMeshAgent;

	private void Start()
	{
		if (photonView.IsMine)
		{
			navMeshAgent.enabled = true;
		}
		else
		{
			navMeshAgent.enabled = false;
		}
	}

	private void Update()
	{
		if (photonView.IsMine)
		{
			navMeshAgent.enabled = true;
		}
		else
		{
			navMeshAgent.enabled = false;
		}
	}
}
