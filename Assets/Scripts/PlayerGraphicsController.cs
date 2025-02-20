using UnityEngine;

public class PlayerGraphicsController : MonoBehaviour
{
    public Transform follow;
    Vector3 velocity;
    public float smoothTime = 0.2f;
    public Animator animator;

    void Update()
    {
        float movementSpeed = velocity.magnitude / Time.deltaTime;
        animator.SetFloat("MovementSpeed", movementSpeed);
        animator.SetBool("Run", FindAnyObjectByType<PlayerController>().Run);
        transform.position = Vector3.SmoothDamp(transform.position, follow.transform.position, ref velocity, smoothTime);

        // If the vector difference is still a "considerable" distance still rotate
        if (Vector3.Distance(velocity, Vector3.zero) > 0.1f)
        {
            Vector3 direction = new Vector3(velocity.x, 0, velocity.z);
            transform.rotation = Quaternion.LookRotation(direction);
        }
        // Otherwise we will snap towards it
        else
        {
            velocity = Vector3.zero;
        }
    }
}
