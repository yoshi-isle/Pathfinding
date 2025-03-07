using UnityEngine;

public class PlayerGraphicsController : MonoBehaviour
{
    public Transform follow;
    Vector3 velocity;
    public float smoothTime = 0.2f;
    public Animator animator;
    public bool ShowHpBar = true;

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

    void OnGUI()
    {
        if (ShowHpBar)
        {
            Camera camera = FindAnyObjectByType<Camera>();
            Vector3 hpBarPosition = camera.WorldToScreenPoint(transform.position + new Vector3(0, 1.5f, 0));
            float hpBarWidth = 30;
            float hpBarHeight = 5;
            float currentHp = 80;
            float maxHp = 100;

            // Draw background (depleted health)
            GUI.color = Color.red;
            GUI.DrawTexture(new Rect(hpBarPosition.x - hpBarWidth / 2, Screen.height - hpBarPosition.y - hpBarHeight / 2, hpBarWidth, hpBarHeight), Texture2D.whiteTexture);

            // Draw foreground (current health)
            GUI.color = Color.green;
            GUI.DrawTexture(new Rect(hpBarPosition.x - hpBarWidth / 2, Screen.height - hpBarPosition.y - hpBarHeight / 2, hpBarWidth * (currentHp / maxHp), hpBarHeight), Texture2D.whiteTexture);
        }
    }
}
