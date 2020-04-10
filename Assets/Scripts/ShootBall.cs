﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBall : MonoBehaviour
{

    public float maxBallSpeed;
    public GameObject ballPrefab;
    Rigidbody ballRb;
    public Transform placementPoint;
    public Transform startPoint;

    GameObject placedBall;
    bool ballIsReady = false;
    public float placementForce = 50;

    Vector2 touch;
    float firstTouchTime;

    public Camera mainCamera;

    public LayerMask aimableLayer;

    float ballLimitTimer = 1;

    Vector3 firstTouchPos;

    float maxDistForSwipe = 500;

    float minDistForSwipe = 10;

    public AnimationCurve forceCurve;

    // Start is called before the first frame update
    void Start()
    {
        maxDistForSwipe = (Screen.height / 8 * 5);
        minDistForSwipe = Screen.height / 20;

        PlaceBall();
    }

    void EnableBallPhysics(bool enable)
    {
        Rigidbody ballRb = placedBall.GetComponent<Rigidbody>();
        placedBall.GetComponent<SphereCollider>().enabled = enable;

        ballRb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        ballRb.isKinematic = !enable;
        
        ballRb.useGravity = enable;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ballIsReady)
        {
            ballLimitTimer -= Time.deltaTime;
            if (ballLimitTimer <= 0)
            {
                //placedBall.transform.position = Vector3.Lerp(placedBall.transform.position, placementPoint.position, (placementPoint.position - placedBall.transform.position).magnitude / Time.deltaTime);

                Vector3 velocity = Vector3.zero;
                placedBall.transform.position = Vector3.SmoothDamp(placedBall.transform.position, placementPoint.transform.position, ref velocity, 0.01f);
                EnableBallPhysics(false);
            }


            // if the ball is close enough to the placement position, teleport it to the right place
            if (placedBall.transform.position.z >= placementPoint.position.z)
            {
                placedBall.transform.position = new Vector3(placementPoint.position.x, placedBall.transform.position.y, placementPoint.position.z);
                Rigidbody ballRb = placedBall.GetComponent<Rigidbody>();

                EnableBallPhysics(false);

                ballRb.velocity = Vector3.zero;
                ballRb.Sleep();
                ballIsReady = true;
                ballLimitTimer = 1;
            }
        }

        // if clicked for first time
        if (Input.GetMouseButtonDown(0))
        {
            firstTouchTime = Time.timeSinceLevelLoad;
            firstTouchPos = Input.mousePosition;
        }

        // If click released, shoot ball 
        if (Input.GetMouseButtonUp(0) && ballIsReady)
        {
            float swipeDist = (Input.mousePosition - firstTouchPos).magnitude;

            // if swipe is too short, return
            if (swipeDist < minDistForSwipe)
            {
                return;
            }

            Vector3 direction = (Input.mousePosition - firstTouchPos).normalized;
            Vector3 flatDirection = new Vector3(direction.x, 0, direction.y);

            RaycastHit hit = new RaycastHit();

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            Vector3 hitPosition = Vector3.zero;

            if (Physics.Raycast(ray, out hit, 100, aimableLayer)) {
                hitPosition = hit.point;
            }

            // point without 
            Vector3 modifiedWorldPoint = new Vector3(hitPosition.x, placedBall.transform.position.y, hitPosition.z);
            Vector3 ballDirection = -(placedBall.transform.position - modifiedWorldPoint).normalized;

            EnableBallPhysics(true);

            ballRb = placedBall.GetComponent<Rigidbody>();

            float normalizedDist = swipeDist / maxDistForSwipe;

            normalizedDist = Mathf.Clamp(normalizedDist, 0.01f, 0.9f);

            normalizedDist = forceCurve.Evaluate(normalizedDist);

            float normalizedTime = (0.25f - (Time.timeSinceLevelLoad - firstTouchTime)) * 0.1f;
            normalizedTime = Mathf.Clamp(normalizedTime, 0.01f, 0.2f);

            float normalizedForce = normalizedTime + normalizedDist;

            Vector3 force = flatDirection * maxBallSpeed * normalizedForce;

            ballRb.AddForce(force, ForceMode.VelocityChange);

            ballRb.GetComponent<Ball>().Instance.WasKicked();

            firstTouchTime = -1;
            // Get another ball ready
            ballIsReady = false;
            PlaceBall();
        }
    }

    void PlaceBall() {

        placedBall = ObjectPooler.Instance.EnableBall();
        //TrailRenderer trail = placedBall.GetComponentInChildren<TrailRenderer>();

        //trail.enabled = false;

        placedBall.transform.rotation = 
            Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
        Rigidbody rb = placedBall.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
        placedBall.transform.position = startPoint.position;
        placedBall.GetComponent<Rigidbody>().AddForce(-(placedBall.transform.position - placementPoint.position).normalized * placementForce, ForceMode.VelocityChange);
        //trail.enabled = true;
    }
}
