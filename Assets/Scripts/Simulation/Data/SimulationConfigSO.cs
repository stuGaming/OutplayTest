using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
[CreateAssetMenu(fileName ="SimulationConfig_",menuName ="Data/Simulation Config")]
public class SimulationConfigSO : ScriptableObject
{
    
    public float MoveSpeed = 10f;
    public Vector3 StartPoint;
    public List<Vector3> Targets = new List<Vector3>();
    public bool ShouldLoop = false;
    public bool RestartOnDeath = false;

    public SimulatedObject SimulatedObject;


    public int ObstacleCount = 100;
    public List<GameObject> Obstacles = new List<GameObject>();
    public float ObstacleSafeSpace = 10;
    public float ObstaleSpawnSpace = 100;

    public GameObject CollisionEffect;
    public AudioSource SoundEffect;


}
