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

    public SkinnedMeshRenderer personRenderer;

    public Color[] shirtColors, pantsColors, shoeColors, skinColors;

    public Material[] startingMaterials;

    MaterialPropertyBlock block;

    public Animator animator;

    float walkingSpeed;

    public Animation animationComponent;
    public AnimationClip[] animations;

    float animationIndex;

    public float minWalkingSpeed, maxWalkingSpeed;

    private void Awake()
    {
        startingPosition = transform.position;
        personRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        //animationComponent = GetComponentInChildren<Animation>();

        // You can re-use this block between calls rather than constructing a new one each time.
        //block = new MaterialPropertyBlock();

        // You can cache a reference to the renderer to avoid searching for it.
        //personRenderer.SetPropertyBlock(block);
    }

    void OnEnable()
    {
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

        //animationIndex = 0;
        animator.SetTrigger(animationIndex.ToString());
        
        //animationComponent.Play(animationComponent.clip)

        mainCamera = Camera.main;
        Instance = this;
        EnableRagdoll(false, transform);

        startingMaterials = personRenderer.materials;

        ChangeMaterials();

    }
    //// Start is called before the first frame update
    //void Start()
    //{
    //    walkingSpeed = 1;
    //    //walkingSpeed = Random.Range(minWalkingSpeed, maxWalkingSpeed);
    //    animator.SetFloat("Speed", walkingSpeed);
    //    animator.SetFloat("AnimationIndex", 2);

    //    mainCamera = Camera.main;
    //    Instance = this;
    //    EnableRagdoll(false, transform);


    //    startingMaterials = personRenderer.materials;

    //    ChangeMaterials();


    //    //int randomColor = Random.Range(0, shirtColors.Length - 1);

    //    //// You can re-use this block between calls rather than constructing a new one each time.
    //    //var block = new MaterialPropertyBlock();

    //    //// You can look up the property by ID instead of the string to be more efficient.
    //    //block.SetColor("_BaseColor", shirtColors[randomColor]);

    //    //// You can cache a reference to the renderer to avoid searching for it.
    //    //GetComponentInChildren<SkinnedMeshRenderer>().SetPropertyBlock(block, 3);

    //}

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

                // You can cache a reference to the renderer to avoid searching for it.
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
        rb.velocity = transform.forward * 2;
    }
}
