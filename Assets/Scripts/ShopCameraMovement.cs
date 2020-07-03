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
    float distance;
    public ShopScript shopScript;

    public Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        maxDistance = Screen.width / 7;
        startPos = transform.position;
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
            distance = Input.mousePosition.x - previousMousePos.x;
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

            transform.position -= (transform.right * (distance * speed)) * (1 - inertiaTimer / maxInertiaTime);
            //transform.position -= (transform.right * (direction * speed * 1 + (distance / maxDistance) * inertiaValue)) * (maxInertiaTime - inertiaTimer);
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, startPos.x, startPos.x + (shopScript.ballsList.Count - 3) * 2), transform.position.y, transform.position.z);
        if (Input.GetMouseButtonUp(0))
        {
            mouseButtonUp = true;
        }
    }
}
