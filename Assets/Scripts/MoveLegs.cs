using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLegs : MonoBehaviour
{

    public float speed;
    public Rigidbody rb;
    Vector3 startingPosition;
    public BoxCollider megTrigger;

    Camera mainCamera;

    private void Awake()
    {
        startingPosition = transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        
    }

    private void LateUpdate()
    {
        //Vector2 viewportPoint = mainCamera.WorldToViewportPoint(transform.position);
        // if object is outside of the camera view.
        //if (viewportPoint.x > 1.1f 
        //    || viewportPoint.x < -1.1f
        //        || viewportPoint.y > 1.1f
        //            || viewportPoint.y < -1.1f)
        //{
        //    megTrigger.enabled = true;
        //    transform.position = startingPosition;
        //}
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.right * speed;
    }
}
