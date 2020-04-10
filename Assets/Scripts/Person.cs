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

    public SkinnedMeshRenderer renderer;

    public Color[] shirtColors, pantsColors, shoeColors, skinColor;

    public Material[] startingMaterials;

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

        startingMaterials = renderer.materials;

        ChangeMaterials();
    }

    void ChangeMaterials()
    {
        for(int i = 0; i < startingMaterials.Length; i++)
        {
            
            renderer.materials[i] = Instantiate(startingMaterials[i]) as Material;
            Material newMaterial = startingMaterials[i];
            Debug.Log(newMaterial.color);

            if (i == 0)
            {
                int randomColor = Random.Range(0, shirtColors.Length - 1);
                newMaterial.color = shirtColors[randomColor];
                Debug.Log("New: " + newMaterial.color);
            }
            if (i == 1)
            {
                int randomColor = Random.Range(0, pantsColors.Length - 1);
                newMaterial.color = pantsColors[randomColor];
            }
            if (i == 2)
            {
                int randomColor = Random.Range(0, shoeColors.Length - 1);
                newMaterial.color = shoeColors[randomColor];
            }
            if (i == 3)
            {
                int randomColor = Random.Range(0, skinColor.Length - 1);
                newMaterial.color = skinColor[randomColor];
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
        rb.velocity = transform.forward * speed;
    }
}
