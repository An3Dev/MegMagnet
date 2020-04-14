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


    private void Awake()
    {
        startingPosition = transform.position;

        personRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        // You can re-use this block between calls rather than constructing a new one each time.
        block = new MaterialPropertyBlock();

        // You can cache a reference to the renderer to avoid searching for it.
        personRenderer.SetPropertyBlock(block);
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        Instance = this;
        EnableRagdoll(false, transform);


        startingMaterials = personRenderer.materials;

        ChangeMaterials();


        //int randomColor = Random.Range(0, shirtColors.Length - 1);

        //// You can re-use this block between calls rather than constructing a new one each time.
        //var block = new MaterialPropertyBlock();

        //// You can look up the property by ID instead of the string to be more efficient.
        //block.SetColor("_BaseColor", shirtColors[randomColor]);

        //// You can cache a reference to the renderer to avoid searching for it.
        //GetComponentInChildren<SkinnedMeshRenderer>().SetPropertyBlock(block, 3);

    }

    void ChangeMaterials()
    {
        //Debug.Log(startingMaterials.Length);

        for(int i = 0; i < startingMaterials.Length; i++)
        {
            // order of materials is shirt, pants, shoes, skin

            
            //renderer.materials[i] = Instantiate(startingMaterials[i]) as Material;
            Material newMaterial = startingMaterials[i];


            if (i == 0)
            {
                int randomColor = Random.Range(0, shirtColors.Length - 1);

                // You can re-use this block between calls rather than constructing a new one each time.
                var block = new MaterialPropertyBlock();

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
                var block = new MaterialPropertyBlock();

                // You can look up the property by ID instead of the string to be more efficient.
                block.SetColor("_BaseColor", pantsColors[randomColor]);

                // You can cache a reference to the renderer to avoid searching for it.
                GetComponentInChildren<SkinnedMeshRenderer>().SetPropertyBlock(block, i);
            }
            if (i == 2)
            {
                int randomColor = Random.Range(0, shoeColors.Length - 1);

                // You can re-use this block between calls rather than constructing a new one each time.
                var block = new MaterialPropertyBlock();

                // You can look up the property by ID instead of the string to be more efficient.
                block.SetColor("_BaseColor", shoeColors[randomColor]);

                // You can cache a reference to the renderer to avoid searching for it.
                GetComponentInChildren<SkinnedMeshRenderer>().SetPropertyBlock(block, i);
            }
            if (i == 3)
            {
                int randomColor = Random.Range(0, skinColors.Length - 1);

                // You can re-use this block between calls rather than constructing a new one each time.
                var block = new MaterialPropertyBlock();

                // You can look up the property by ID instead of the string to be more efficient.
                block.SetColor("_BaseColor", skinColors[randomColor]);

                // You can cache a reference to the renderer to avoid searching for it.
                GetComponentInChildren<SkinnedMeshRenderer>().SetPropertyBlock(block, i);
            }

            //personRenderer.SetPropertyBlock(block);  

            //renderer.materials[i] = newMaterial;
            //Debug.Log("Renderer" + personRenderer.materials[i].color);
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
        rb.velocity = transform.forward * speed;
    }
}
