using UnityEngine;

public class AnimationSpeedController : MonoBehaviour
{
    public Transform objectTransform;

    private Vector3 previousPosition;

    public float maxSpeed = 10f;

    public Animator animator;

    private void Start()
    {
        previousPosition = objectTransform.position;
    }

    private void Update()
    {
        float speed = (objectTransform.position - previousPosition).magnitude / Time.deltaTime / maxSpeed;
        animator.speed = speed;
        previousPosition = objectTransform.position;
    }
}
