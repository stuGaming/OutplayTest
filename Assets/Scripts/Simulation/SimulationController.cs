using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Simulation that is run on start
    /// </summary>
    public SimulationConfigSO CurrentSimulation;

    /// <summary>
    /// Used to override values in the SimulationConfig do not use.
    /// </summary>
    public List<Vector3> Targets = new List<Vector3>();
    public List<GameObject> Obstacles = new List<GameObject>();
    public SimulatedObject SimulatedObject;
    private SimulatedObject _simulatedObject;
    public GameObject CollisionEffect;
    public AudioSource SoundEffect;
    #endregion
    #region UNITY METHODS
    private void Start()
    {
        SetupObstacles();
        SetupSimulatedObject();
    }
    #endregion
    #region Simulation Logic
    /// <summary>
    /// Instantiate moving object
    /// </summary>
    private void SetupSimulatedObject()
    {
        _simulatedObject = Instantiate(CurrentSimulation.SimulatedObject, CurrentSimulation.StartPoint,Quaternion.identity, this.transform);
        StartCoroutine(iTravel(_simulatedObject));
        _simulatedObject.OnCollision = CollisionOccured;
    }
    /// <summary>
    /// Move simulated object
    /// </summary>
    /// <param name="simulatedObject"></param>
    /// <returns></returns>
    private IEnumerator iTravel(SimulatedObject simulatedObject)
    {
        int targetIndex = 0;

        List<Vector3> targets = new List<Vector3>();
        targets.AddRange(CurrentSimulation.Targets);
        if (CurrentSimulation.ShouldLoop)
        {
            targets.Add(CurrentSimulation.StartPoint);
        }


        while (targetIndex < targets.Count)
        {
            Vector3 currentTarget = targets[targetIndex];
            // If next movement will overshoot the target skip it
            while((simulatedObject.transform.position - currentTarget).magnitude>Time.deltaTime * CurrentSimulation.MoveSpeed)
            {
                Vector3 velocity= (currentTarget - simulatedObject.thisRigid.position).normalized * Time.deltaTime * CurrentSimulation.MoveSpeed;

                simulatedObject.thisRigid.MovePosition(simulatedObject.thisRigid.position+velocity);
                yield return null;
            }
            simulatedObject.thisRigid.MovePosition(currentTarget);
            yield return null;
            targetIndex++;
            // Loop through targets infinitely
            if (CurrentSimulation.ShouldLoop && targetIndex>= targets.Count)
            {
                targetIndex = 0;
            }

        }

        PlayDestroyEffect(_simulatedObject.transform.position);
        DestroySimulatedObject();

        
    }

    public void SetupObstacles()
    {
        for(int i = 0; i < CurrentSimulation.ObstacleCount; i++)
        {
            var ChosenObstacle = CurrentSimulation.Obstacles.GetRandom();
            SpawnObstacle(ChosenObstacle);
        }
    }
    /// <summary>
    /// Spanws obstacle in random position in the specified area
    /// </summary>
    /// <param name="chosenObstacle"></param>
    private void SpawnObstacle(GameObject chosenObstacle)
    {
        var obstacle = Instantiate(chosenObstacle, this.transform).gameObject;

        Vector3 direction = UnityEngine.Random.onUnitSphere;
        direction *= UnityEngine.Random.Range(CurrentSimulation.ObstacleSafeSpace, CurrentSimulation.ObstaleSpawnSpace);

        obstacle.transform.position =CurrentSimulation.StartPoint +  direction;
    }

    public void PlayDestroyEffect(Vector3 pos)
    {
        var effect = Instantiate(CurrentSimulation.CollisionEffect, pos, Quaternion.identity, this.transform).gameObject;
        Destroy(effect, 5f);
        var soundEffect = Instantiate(CurrentSimulation.SoundEffect, pos, Quaternion.identity, this.transform).gameObject;
        Destroy(soundEffect, 5f);
    }

    public void DestroySimulatedObject()
    {

     
        if (CurrentSimulation.ShouldLoop)
        {
            _simulatedObject.gameObject.SetActive(true);
            _simulatedObject.transform.position = CurrentSimulation.StartPoint;
            StartCoroutine(iTravel(_simulatedObject));
        }
        else
        {
            Destroy(_simulatedObject.gameObject);
        }
    }
    /// <summary>
    /// Playe collision effects then after delay destroy simulated object for camera movement
    /// </summary>
    /// <param name="obstacle"></param>
    public async void CollisionOccured(GameObject obstacle)
    {

        PlayDestroyEffect(obstacle.transform.position);
        StopAllCoroutines();
        Destroy(obstacle);
        _simulatedObject.gameObject.SetActive(false);
        await UsefulExtensions.WaitSeconds(1f);
        DestroySimulatedObject();

    }

    #endregion
    #region GIZMOS
    public bool ShowOrigin = true;
    public bool ShowSafe = true;
    public bool ShowObstacleSpawn = true;
    public bool ShowPath = true;

    private void OnDrawGizmosSelected()
    {
        if (CurrentSimulation == null)
            return;
        Color col = Color.blue;

        if (ShowOrigin)
        {
            col.a = 0.2f;
            Gizmos.color = col;
            Gizmos.DrawSphere(CurrentSimulation.StartPoint, 5f);

        }
        if (ShowSafe)
        {
            col = Color.green;
            col.a = 0.2f;
            Gizmos.color = col;
            Gizmos.DrawSphere(CurrentSimulation.StartPoint, CurrentSimulation.ObstacleSafeSpace);
        }
        if (ShowObstacleSpawn)
        {
            col = Color.red;
            col.a = 0.2f;
            Gizmos.color = col;
            Gizmos.DrawSphere(CurrentSimulation.StartPoint, CurrentSimulation.ObstaleSpawnSpace);
        }
        if (ShowPath)
        {
            Vector3 currentPosition = CurrentSimulation.StartPoint;
            DrawPositionGizmo(currentPosition);
            for(int i = 0; i < CurrentSimulation.Targets.Count; i++)
            {
                DrawLineGizmo(currentPosition, CurrentSimulation.Targets[i]);
                DrawPositionGizmo(CurrentSimulation.Targets[i]);
                currentPosition = CurrentSimulation.Targets[i];
            }
            if (CurrentSimulation.ShouldLoop)
            {
                DrawLineGizmo(currentPosition, CurrentSimulation.StartPoint);

            }
        }

    }

    private void DrawLineGizmo(Vector3 currentPosition, Vector3 nextPosition)
    {
        Color lineColor = Color.white;
        lineColor.a = 0.2f;
        Gizmos.color = lineColor;
        Gizmos.DrawLine(currentPosition, nextPosition);
    }

    private void DrawPositionGizmo(Vector3 currentPosition)
    {

        Color pointColor = Color.yellow;
        pointColor.a = 0.2f;
        Gizmos.color = pointColor;
        Gizmos.DrawSphere(currentPosition, 2f);

    }
    #endregion
}
