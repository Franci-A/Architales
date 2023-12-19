using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    public Rigidbody GetRigidBody => rb;
    [SerializeField] private ResidentHandler residentHandler;
    public ResidentHandler GetResident => residentHandler;
    [SerializeField] private BlockSocketHandler socketHandler;
    public BlockSocketHandler GetBlockSocketHandler => socketHandler;
    [SerializeField] private AudioPlayCollision collisionAudioPlayer;
    public AudioPlayCollision GetAudioCollision => collisionAudioPlayer;
}
