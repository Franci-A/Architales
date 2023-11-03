using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Bricks")]
public class BrickSO : ScriptableObject
{
    public List<Block> Blocks;
}
