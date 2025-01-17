﻿using UnityEngine;

public class Person : MonoBehaviour
{
    float speed = 2;
    public Rigidbody rb;
    Vector3 startingPosition;
    public BoxCollider megStart;

    Camera mainCamera;

    public Person Instance;

    public Transform leftFoot, rightFoot;

    public SkinnedMeshRenderer personRenderer;

    public Color[] shirtColors, pantsColors, shoeColors, skinColors;

    public Material[] startingMaterials;

    MaterialPropertyBlock block;

    public Animator animator;

    float walkingSpeed;

    public Animation animationComponent;
    public AnimationClip[] animations;

    float animationIndex;

    public bool involvedInDoubleMeg = false;

    public GameObject doubleMegObject;

    float percentChanceOfAttacking = 0.5f;

    bool attackWhenHit = false;

    MyGameManager manager;

    bool slapping = false;
    private void Awake()
    {
        startingPosition = transform.position;
        personRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        manager = FindObjectOfType<MyGameManager>();
        //animationComponent = GetComponentInChildren<Animation>();

        // You can re-use this block between calls rather than constructing a new one each time.
        block = new MaterialPropertyBlock();

        personRenderer.SetPropertyBlock(block);
    }

    public void ForceAnimation(int index)
    {
        animator.SetTrigger(index.ToString());
        animator.SetTrigger(index.ToString());
        animator.SetTrigger(index.ToString());
        animator.SetTrigger(index.ToString());

        animationIndex = index;
        walkingSpeed = 1;
    }

    public void SetPerson()
    {
        speed = 2;
        mainCamera = Camera.main;
        Instance = this;
        EnableRagdoll(false, transform);

        attackWhenHit = Random.Range(0f, 100f) <= percentChanceOfAttacking;
        startingMaterials = personRenderer.materials;

        ChangeMaterials();

        if (involvedInDoubleMeg)
        {
            doubleMegObject.SetActive(true);
            return;
        }

        doubleMegObject.SetActive(false);

        walkingSpeed = 1;
        //walkingSpeed = Random.Range(minWalkingSpeed, maxWalkingSpeed);
        //animator.SetFloat("Speed", walkingSpeed);
        int percent = Random.Range(0, 99);

        animationIndex = 0;
        if (percent < 15) // 15 percent chance for this walk
        {
            // Regular walk
            animationIndex = 0;
            walkingSpeed = 2f;
        }
        else if (percent >= 15 && percent < 30) // 15 percent chance
        {
            // alternate regular walk
            animationIndex = 3;
        }
        else if (percent >= 30 && percent < 45) // 15 percent change
        {
            // alternate 2 regular walk. Regular arm length
            animationIndex = 4;
        }
        else if (percent >= 45 && percent < 55) // ten percent change
        {
            // alternate 2 regular walk. short arm length
            animationIndex = 4;
            //walkingSpeed = 1.05f;
        }
        else if (percent >= 55 && percent < 65) // ten percent change
        {
            // other walk
            animationIndex = 2;
        }
        else if (percent >= 65 && percent < 75) // ten percent change
        {
            // calm walk
            //animationIndex = 7;
            animationIndex = 0;
        }
        else if (percent >= 75 && percent < 85) // ten percent change
        {
            // other walk
            animationIndex = 8;
        }
        else if (percent >= 85 && percent < 93) // 8 percent change
        {
            // swag walk
            animationIndex = 1;
            walkingSpeed = 0.4f;
        }
        else if (percent >= 93 && percent < 97) // 5 percent change
        {
            // gangster walk
            animationIndex = 6;
            walkingSpeed = 0.5f;
        }
        else if (percent >= 97 && percent < 99) // 2 percent change
        {
            // weird walk
            animationIndex = 9;
            walkingSpeed = 0.8f;
        }
        else if (percent >= 99) // one percent change
        {
            // wide weird walk
            animationIndex = 10;
            walkingSpeed = 0.75f;
        }

        animator.SetTrigger(animationIndex.ToString());

    }

    private void Update()
    {
        if(!involvedInDoubleMeg && doubleMegObject.activeInHierarchy)
        {
            doubleMegObject.SetActive(false);
        }
    }

    void ChangeMaterials()
    {
        for(int i = 0; i < startingMaterials.Length; i++)
        {
            // order of materials is shirt, pants, shoes, skin

            Material newMaterial = startingMaterials[i];

            if (i == 0)
            {
                int randomColor = Random.Range(0, shirtColors.Length - 1);

                // You can re-use this block between calls rather than constructing a new one each time.
                block = new MaterialPropertyBlock();

                // You can look up the property by ID instead of the string to be more efficient.
                block.SetColor("_BaseColor", shirtColors[randomColor]);

                GetComponentInChildren<SkinnedMeshRenderer>().SetPropertyBlock(block, i);

                //Debug.Log("New: " + newMaterial.color);
            }
            if (i == 1)
            {
                int randomColor = Random.Range(0, pantsColors.Length - 1);

                // You can re-use this block between calls rather than constructing a new one each time.
                block = new MaterialPropertyBlock();

                // You can look up the property by ID instead of the string to be more efficient.
                block.SetColor("_BaseColor", pantsColors[randomColor]);

                // You can cache a reference to the renderer to avoid searching for it.
                GetComponentInChildren<SkinnedMeshRenderer>().SetPropertyBlock(block, i);
            }
            if (i == 2)
            {
                int randomColor = Random.Range(0, shoeColors.Length - 1);

                // You can re-use this block between calls rather than constructing a new one each time.
                block = new MaterialPropertyBlock();

                // You can look up the property by ID instead of the string to be more efficient.
                block.SetColor("_BaseColor", shoeColors[randomColor]);

                // You can cache a reference to the renderer to avoid searching for it.
                GetComponentInChildren<SkinnedMeshRenderer>().SetPropertyBlock(block, i);
            }
            if (i == 3)
            {
                int randomColor = Random.Range(0, skinColors.Length - 1);

                
                block = new MaterialPropertyBlock();

                block.SetColor("_BaseColor", skinColors[randomColor]);
                
                GetComponentInChildren<SkinnedMeshRenderer>().SetPropertyBlock(block, i);
            }
        }
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
                childRb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            }          

            if (child.childCount != 0)
            {
                EnableRagdoll(enable, child);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (attackWhenHit && collision.collider.CompareTag("Ball"))
        {
            //speed = 0;
            Vector3 offset = (mainCamera.transform.position - transform.position);
            offset.y = transform.position.y;
            transform.forward = (mainCamera.transform.position - transform.position).normalized;
            slapping = true;
        }
    }

    void Slap()
    {
        animator.SetTrigger("Slap");
        Debug.Log("Slap");

        Invoke("EndGame", 0.8f);
    }

    void EndGame()
    {
        manager.gameOver = true;
        slapping = false;
    }

    private void FixedUpdate()
    {
        if (slapping && (transform.position - mainCamera.transform.position).sqrMagnitude < 6)
        {
            Slap();
        }
        rb.velocity = transform.forward * speed;
    }
}
