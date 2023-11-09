using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class CubeEditor : MonoBehaviour
{
    [SerializeField] GameObject cube;
    //GameObject[,] cubeList = new GameObject[5,5];
    [SerializeField] List<GameObject> cubeList = new List<GameObject>();

    public GameObject UpdateCubeList(Vector2 pos, bool isActive)
    {
        if (isActive)
        {
            if (cubeList.Count > (int)pos.x && cubeList[(int)pos.x] != null) return null;
            else
            {
                var go = Instantiate(cube, new Vector3(pos.x, 0, pos.y), transform.rotation);
                cubeList.Add(go);
                return go;
            }
        }
        else 
        {
            if (cubeList.Count > (int)pos.x && cubeList[(int)pos.x] != null) return null;
            else
            {
                DestroyImmediate(cubeList[(int)pos.x]);
                //cubeList.Remove(cubeList[(int)pos.x]);
            }

        }
        return null;

        /*Debug.Log($"x : {(int)pos.x}, y : {(int)pos.y} = {cubeList[(int)pos.x][(int)pos.y]}");
        if (isActive)
        {
            if (cubeList[(int)pos.x][(int)pos.y] != null) return;
            else
            {
                cubeList[(int)pos.x][(int)pos.y] = Instantiate(cube, new Vector3(pos.x, 0, pos.y), transform.rotation);
                //Debug.Log($"x : {(int)pos.x}, y : {(int)pos.y} = {cubeList[(int)pos.x, (int)pos.y]}");
            }
        }
        else 
        {
            if (cubeList[(int)pos.x][(int)pos.y] != null) return;
            else
            {
                //Debug.Log($"x : {(int)pos.x}, y : {(int)pos.y} = {cubeList[(int)pos.x, (int)pos.y]}");
                DestroyImmediate(cubeList[(int)pos.x][(int)pos.y]);
                cubeList[(int)pos.x][(int)pos.y] = null;
            }

        }*/
    }
}
