using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandleComponent : MonoBehaviour
{

    [Header("Debug")]
    [SerializeField] private bool drawRays;

    [Header("Camera Attributes")]
    [SerializeField] private Vector3 cameraDefaultDir;
    [SerializeField] private float cameraDefaultDist;
    [SerializeField] private LayerMask cameraObstructLayers;
    [SerializeField] private bool obstructHandleRotation = false;
    [SerializeField] private float obstructOffset = 0.1f;

    [Header("Camera Components")]
    [SerializeField] private Transform cameraRotationT;
    [SerializeField] private Transform cameraHolderT;
    [SerializeField] private Transform cameraT;

    [Header("Camera Obstructions")]
    [SerializeField] private List<GameObject> obstructions = new List<GameObject>();
    [SerializeField] private float CameraSpring = 1;

    private void DrawRay(Vector3 a, Vector3 b, Color col)
    {
        if (drawRays)
            Debug.DrawRay(a, b, col, 0.1f);
    }

    private RaycastHit[] HandleRaycasts(Vector3 a, Vector3 b, float distance, LayerMask layers)
    {
        RaycastHit[] hits;
        DrawRay(a, b * distance, Color.yellow);
        hits = Physics.RaycastAll(a, b, distance, layers);
        return hits;
    }

    private void ChangeObjectAlpha(ref GameObject obj, float alpha)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        Color c = renderer.material.color;
        renderer.material.color = new Color(c.r, c.g, c.b, alpha);
    }

    private void HandleWorldObjets()
    {
        RaycastHit[] hits;
        var heading = cameraHolderT.position - cameraRotationT.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        hits = HandleRaycasts(cameraRotationT.position, direction, distance, cameraObstructLayers);

        List<GameObject> localObs = new List<GameObject>();

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag != "WorldObject")
                continue;
            GameObject obj = hit.collider.gameObject;
            ChangeObjectAlpha(ref obj, 0.3f);
            localObs.Add(obj);
        }

        foreach (GameObject obs in obstructions)
        {
            GameObject obj = obs;
            if (localObs.Contains(obj))
                continue;
            ChangeObjectAlpha(ref obj, 1.0f);
        }
        obstructions = localObs;
    }


    private void HandleWorldBorder()
    {
        var heading = cameraHolderT.position - cameraRotationT.position;
        var dir = heading / cameraDefaultDist;
        RaycastHit[] hits;
        hits = HandleRaycasts(cameraRotationT.position, dir, cameraDefaultDist * (1 + obstructOffset), cameraObstructLayers);
        if (hits.Length == 0)
            CameraSpring = 1;


        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag != "WorldBorder")
                continue;
            CameraSpring = (hit.distance / cameraDefaultDist) - obstructOffset;
        }

        Vector3 newCameraPos = Vector3.Lerp(cameraRotationT.position, cameraHolderT.position, CameraSpring);
        cameraT.position = newCameraPos;
        if (obstructHandleRotation)
        {
            Vector3 newCameraRotation = Vector3.Lerp(cameraRotationT.eulerAngles, cameraHolderT.eulerAngles, CameraSpring);
            cameraT.eulerAngles = newCameraRotation;
        }

    }

    void Awake()
    {
        var heading = cameraT.position - cameraRotationT.position;
        cameraDefaultDist = heading.magnitude;
    }

    void FixedUpdate()
    {
        HandleWorldObjets();
        HandleWorldBorder();
        /*
        if (cameraSpring < 1)
            cameraSpring += cameraSpringStep * Time.deltaTime;
        if (cameraSpring > 1)
            cameraSpring = 1;
            */
    }

    
}
