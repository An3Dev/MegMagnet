using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{

    public float speed;
    public Rigidbody rb;
    Vector3 startingPosition;
    public BoxCollider megStart;

    Camera mainCamera;

    public Person Instance;

    public Transform leftFoot, rightFoot;

    private void Awake()
    {
        startingPosition = transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        Instance = this;
        EnableRagdoll(false, transform);
    }

    public void EnableRagdoll(bool enable, Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            

            if (child.GetComponent<Rigidbody>() != null)
            {
                Rigidbody childRb = child.GetComponent<Rigidbody>();
                childRb.isKinematic = !enable;
                childRb.useGravity = enable;
            }

            

            if (child.childCount != 0)
            {
                EnableRagdoll(enable, child);
            }
        }
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
        rb.velocity = transform.forward * speed;
    }
}
