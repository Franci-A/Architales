using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grid3DManager : MonoBehaviour
{
    Dictionary<Vector3, GameObject> grid; // x = right; y = up; z = forward;

    [SerializeField] private float maxDistance = 10;
    [SerializeField] private LayerMask gridLayer;
    [SerializeField] private LayerMask blockLayer;
    [SerializeField] private Piece brick;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, maxDistance, blockLayer))
            {
                if (hit.normal != Vector3.up)
                {

                    PlaceBlock(hit.point + hit.normal / 2);
                }
                else
                    PlaceBlock(hit.point);
            }
        }
    }

    public void PlaceBlock(Vector3 position)
    {
        position = new Vector3(Mathf.Floor(position.x) + .5f, Mathf.Floor(position.y) + .5f, Mathf.Floor(position.z) + .5f);
        Instantiate(brick, position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).origin + Camera.main.ScreenPointToRay(Input.mousePosition).direction * maxDistance);
    }
}
