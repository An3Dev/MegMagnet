using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBall : MonoBehaviour
{

    public float ballSpeed;
    public GameObject ballPrefab;
    Rigidbody ballRb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject ball = Instantiate(ballPrefab, transform.position + Vector3.forward, Quaternion.identity);
            ballRb = ball.GetComponent<Rigidbody>();
            ballRb.AddForce(Vector3.forward * ballSpeed);
        }
    }
}
