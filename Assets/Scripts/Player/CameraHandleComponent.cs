using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandleComponent : MonoBehaviour
{

    [Header("Debug")]
    [SerializeField] private bool drawRays;

    [Header("Camera Attributes")]
    [SerializeField] private Vector3 cameraDefaultPos;
    [SerializeField] private LayerMask cameraObstructLayers;



    [Header("Camera Components")]
    [SerializeField] private Transform cameraHolderT;
    [SerializeField] private Transform cameraT;

    private void DrawRay(Vector3 a, Vector3 b, Color col)
    {
        if (drawRays)
            Debug.DrawRay(a, b, col, 0.1f);
    }

    private RaycastHit HandleRaycast(Vector3 a, Vector3 b, float distance, LayerMask layers)
    {
        RaycastHit hit;
        DrawRay(a, b * distance, Color.yellow);
        if(Physics.Raycast(a, b, out hit, distance, layers))
        {
            Debug.Log(hit.collider.gameObject);
        }
        return hit;
    }

    void Awake()
    {
        cameraDefaultPos = cameraT.position;
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        DrawRay(cameraT.position, cameraT.forward * Mathf.Abs(cameraDefaultPos.z), Color.yellow);
        if(Physics.Raycast(cameraT.position, cameraT.forward, out hit, Mathf.Abs(cameraDefaultPos.z), cameraObstructLayers))
        {
            Debug.Log(hit.collider.gameObject);
        }

        HandleRaycast(cameraT.position, cameraT.right, 1, cameraObstructLayers);
        HandleRaycast(cameraT.position, -cameraT.right, 1, cameraObstructLayers);
        HandleRaycast(cameraT.position, -cameraT.forward, 1, cameraObstructLayers);
    }


    

}
