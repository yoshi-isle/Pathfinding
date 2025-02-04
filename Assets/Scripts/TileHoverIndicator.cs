using Unity.VisualScripting;
using UnityEngine;

public class TileHoverIndicator : MonoBehaviour
{
    void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            transform.position = new Vector3(Mathf.Round(hit.point.x), 0.1f, Mathf.Round(hit.point.z));
        }
    }
}