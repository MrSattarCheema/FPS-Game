using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Camera camera;
    float startFOV, targetFOV;
    public float zoomSpeed = 1f;
    public static CameraController instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        startFOV = camera.fieldOfView;
        targetFOV = startFOV;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
    }
    public void ZoomIn(float amount)
    {
        targetFOV = amount;
    }
    public void ZoomOut()
    {
        targetFOV = startFOV;
    }
}
