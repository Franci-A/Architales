using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Bricks")]
public class PieceSO : ScriptableObject
{
    public List<Cube> cubes;
    public Resident resident;
}
