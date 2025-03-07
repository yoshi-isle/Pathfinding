using UnityEngine;
public class Enemy : Interactable
{
    public bool AggroTowardsPlayer = false;
    GameObject player;
    public bool ShowHpBar = false;

    public void Start()
    {
        //TODO
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public override void Interact()
    {
        SetAggressive();
        if (AggroTowardsPlayer)
        {
            if (player != null)
            {
                Vector3 direction = (player.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
    }

    public void SetAggressive()
    {
        ShowHpBar = true;
        AggroTowardsPlayer = true;
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.red;
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
            float currentHp = 100;
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