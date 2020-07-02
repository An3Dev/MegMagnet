using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCameraMovement : MonoBehaviour
{

    public float speed = 5;

    Vector2 previousMousePos;
    bool mouseButtonUp = true;

    public float inertiaValue = 5;
    float startInertiaValue;
    float inertiaTimer;
    float maxInertiaTime = 2;

    float maxDistance;

    float direction;
    // Start is called before the first frame update
    void Start()
    {
        maxDistance = Screen.width / 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (inertiaTimer >= maxInertiaTime)
            {
                inertiaTimer = 0;
            }
            //lerpTimer += Time.deltaTime;
            if (mouseButtonUp)
            {
                previousMousePos.x = Input.mousePosition.x;
            }

            mouseButtonUp = false;
            float distance = Input.mousePosition.x - previousMousePos.x;
            distance = Mathf.Clamp(distance, -maxDistance, maxDistance);
            direction = Mathf.Clamp(distance, -1, 1);
            transform.position -= transform.right * (distance * speed);

            previousMousePos = Input.mousePosition;
        } else
        {
            if (inertiaTimer >= maxInertiaTime)
            {
                return;
            }
            inertiaTimer += Time.deltaTime;

            transform.position -= transform.right * (direction * speed * (maxInertiaTime - inertiaTimer) * inertiaValue);
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseButtonUp = true;
        }
    }
}
