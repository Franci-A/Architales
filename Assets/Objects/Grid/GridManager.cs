using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private float gridScale = 1;
    [SerializeField] private int gridSizeX = 2;
    [SerializeField] private int gridSizeY = 2;

    private List<List<GameObject>> grid;

    [SerializeField] private float maxDistance = 10;
    [SerializeField] private LayerMask gridLayer;
    [SerializeField] private LayerMask blockLayer;
    [SerializeField] private GameObject brick;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        grid = new List<List<GameObject>>(); 
        for (int i = 0; i < gridSizeY; i++)
        {
            grid.Add(new List<GameObject>());
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, maxDistance, blockLayer))
            {
                if(hit.normal != Vector3.up)
                {

                    PlaceBlock(hit.point + hit.normal/2);
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
