using UnityEngine;

public class PlayerGraphicsController : MonoBehaviour
{
    public Transform follow;
    public float smoothTime = 0.2f;
    public Animator animator;
    public bool ShowHpBar = true;
    private Vector3 velocity;
    PlayerController playerController = null;

    void Update()
    {
        if (playerController == null)
        {
            playerController = FindAnyObjectByType<PlayerController>();
            return;
        }

        float movementSpeed = velocity.magnitude;
        animator.SetFloat("MovementSpeed", movementSpeed);
        animator.SetBool("Run", playerController.Run);

        Vector3 targetPosition = follow.transform.position;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10f);
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

            GUI.color = Color.red;
            GUI.DrawTexture(new Rect(hpBarPosition.x - hpBarWidth / 2, Screen.height - hpBarPosition.y - hpBarHeight / 2, hpBarWidth, hpBarHeight), Texture2D.whiteTexture);
            GUI.color = Color.green;
            GUI.DrawTexture(new Rect(hpBarPosition.x - hpBarWidth / 2, Screen.height - hpBarPosition.y - hpBarHeight / 2, hpBarWidth * (playerController.currentHp / playerController.maxHp), hpBarHeight), Texture2D.whiteTexture);
        }
    }
}
