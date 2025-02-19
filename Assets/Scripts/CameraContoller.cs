using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new(0, 2, -5);

    public float minZoom = 2f;
    public float maxZoom = 20f;
    public float zoomSpeed = 4f;
    public float rotateSpeed = 180f;

    private float currentZoom = 5f;
    private float rotateInputX = 0f;
    private float rotateInputY = 0.5f;

    public float minRotateY = -0.8f;
    public float maxRotateY = 0.8f;

    public float verticalOffset = 0f;
    public float verticalSpeed = 5f;
    public float maxVerticalOffset = 10f;
    public float minVerticalOffset = -5f;
    public float lookUpAmount = 1f;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        transform.position = target.position - (offset * currentZoom) + Vector3.up * verticalOffset;
    }

    void Update()
    {
        rotateInputX -= Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;
        rotateInputY -= Input.GetAxis("Vertical") * rotateSpeed * Time.deltaTime;

        //Middle Mouse Affecting Rotation Variables
        if (Input.GetMouseButton(2))
        {
            rotateInputX += Input.GetAxisRaw("Mouse X") * rotateSpeed * Time.deltaTime;
            rotateInputY += Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
        }

        rotateInputY = Mathf.Clamp(rotateInputY, minRotateY, maxRotateY);

        //Scrollwheel zoom in/out
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        verticalOffset = Mathf.Clamp(verticalOffset, minVerticalOffset, maxVerticalOffset);

        //Camera positioning
        transform.position = target.position - (offset * currentZoom) + Vector3.up * verticalOffset;
        transform.LookAt(target.position + (Vector3.up * lookUpAmount));

        //Set panning values to camera
        transform.RotateAround(target.position, Vector3.up, rotateInputX);
        transform.RotateAround(target.position, transform.right, -rotateInputY);
    }
}