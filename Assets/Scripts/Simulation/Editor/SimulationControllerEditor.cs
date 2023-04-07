using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(SimulationController))]
public class SimulationControllerEditor : Editor
{
    private SerializedProperty currentSimulation;
    private SerializedProperty showOrigin;
    private SerializedProperty showSafe;
    private SerializedProperty showPath;

    private SerializedProperty showObstacleSpawn;
    private SerializedProperty simulationTargets;
    private SerializedProperty simulationObstacles;
    private SerializedProperty simulationObject;
    private SerializedProperty collisionEffects;
    private SerializedProperty collisionSound;
    private void OnEnable()
    {
        currentSimulation = serializedObject.FindProperty("CurrentSimulation");
        showOrigin = serializedObject.FindProperty("ShowOrigin");
        showSafe = serializedObject.FindProperty("ShowSafe");
        showPath = serializedObject.FindProperty("ShowPath");
        showObstacleSpawn = serializedObject.FindProperty("ShowObstacleSpawn");
        simulationTargets = serializedObject.FindProperty("Targets");
        simulationObstacles = serializedObject.FindProperty("Obstacles");
        simulationObject = serializedObject.FindProperty("SimulatedObject");
        collisionEffects = serializedObject.FindProperty("CollisionEffect");
        collisionSound = serializedObject.FindProperty("SoundEffect");
    }
    bool ShowSimulationSettings = false;
    bool ShowDebugSettings = false;
    bool isFirstLoad => lastConfigLoaded != ((SimulationController)target).CurrentSimulation;
    SimulationConfigSO lastConfigLoaded;
    public override void OnInspectorGUI()
    {
        SimulationController thisController = (SimulationController)target;
        serializedObject.Update();
        EditorGUILayout.PropertyField(currentSimulation);

        

        if (thisController.CurrentSimulation != null)
        {
            thisController.Targets = thisController.CurrentSimulation.Targets;
            thisController.Obstacles = thisController.CurrentSimulation.Obstacles;
            if (isFirstLoad)
            {
                thisController.CollisionEffect = thisController.CurrentSimulation.CollisionEffect;
                thisController.SoundEffect = thisController.CurrentSimulation.SoundEffect;
                thisController.SimulatedObject = thisController.CurrentSimulation.SimulatedObject;

            }
            else
            {
                thisController.CurrentSimulation.CollisionEffect=thisController.CollisionEffect;
                thisController.CurrentSimulation.SoundEffect=thisController.SoundEffect;
                 thisController.CurrentSimulation.SimulatedObject=thisController.SimulatedObject;

            }

            //}
            //if (thisController.CollisionEffect == null && !isFirstLoad)
            //{
            //    thisController.CurrentSimulation.CollisionEffect = null;
            //}
            //else if (thisController.CurrentSimulation.CollisionEffect!=null)
            //    thisController.CollisionEffect = thisController.CurrentSimulation.CollisionEffect;
            //if (thisController.SoundEffect == null && !isFirstLoad)
            //{
            //    thisController.CurrentSimulation.SoundEffect = null;
            //}
            //else if (thisController.CurrentSimulation.SoundEffect != null)
            //    thisController.SoundEffect = thisController.CurrentSimulation.SoundEffect;
            //if (thisController.SimulatedObject == null && !isFirstLoad)
            //{
            //    thisController.CurrentSimulation.SimulatedObject = null;
            //}
            //else if (thisController.CurrentSimulation.SimulatedObject != null)
            //    thisController.SimulatedObject = thisController.CurrentSimulation.SimulatedObject;
           
            ShowSimulationSettings = EditorGUILayout.BeginFoldoutHeaderGroup(ShowSimulationSettings,"Simulation Settings");
            
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (ShowSimulationSettings)
            {
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Simulation General Properties");

                thisController.CurrentSimulation.ShouldLoop = EditorGUILayout.Toggle("Should Loop", thisController.CurrentSimulation.ShouldLoop);


                GUILayout.Space(10);
                EditorGUILayout.LabelField("Simulated Object Properties");

                EditorGUILayout.PropertyField(simulationObject);
             
                if(thisController.SimulatedObject == null)
                    Debug.LogError("Simulated object destroyed");
                thisController.CurrentSimulation.StartPoint = EditorGUILayout.Vector3Field("Spawn Position", thisController.CurrentSimulation.StartPoint);

                thisController.CurrentSimulation.MoveSpeed = EditorGUILayout.FloatField("Move Speed", thisController.CurrentSimulation.MoveSpeed);
               
                EditorGUILayout.PropertyField(simulationTargets);
                thisController.CurrentSimulation.Targets = thisController.Targets;
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Simulated Obstacle Properties");
                thisController.CurrentSimulation.ObstacleCount = EditorGUILayout.IntField("Obstacle Count", thisController.CurrentSimulation.ObstacleCount);

                EditorGUILayout.PropertyField(simulationObstacles);
                thisController.CurrentSimulation.Obstacles = thisController.Obstacles;
                thisController.CurrentSimulation.ObstacleSafeSpace = EditorGUILayout.FloatField("Obstacle Safe Space", thisController.CurrentSimulation.ObstacleSafeSpace);

                thisController.CurrentSimulation.ObstaleSpawnSpace = EditorGUILayout.FloatField("Obstacle Spawn Space", thisController.CurrentSimulation.ObstaleSpawnSpace);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Simulation CollisionEffects");

                EditorGUILayout.PropertyField(collisionEffects);
                EditorGUILayout.PropertyField(collisionSound);
            
                GUILayout.Space(10);
                
                EditorUtility.SetDirty(thisController.CurrentSimulation);
            }
          
            ShowDebugSettings = EditorGUILayout.BeginFoldoutHeaderGroup(ShowDebugSettings, "Debug Settings");
            if (ShowDebugSettings)
            {
                GUILayout.Space(10);

                EditorGUILayout.PropertyField(showOrigin);
                EditorGUILayout.PropertyField(showSafe);
                EditorGUILayout.PropertyField(showObstacleSpawn);
                EditorGUILayout.PropertyField(showPath);

                EditorGUILayout.EndFoldoutHeaderGroup();
            }

        }
        else
        {

            thisController.CollisionEffect = null;
            thisController.SoundEffect = null;
            thisController.SimulatedObject = null;

        }
        lastConfigLoaded = thisController.CurrentSimulation;
        serializedObject.ApplyModifiedProperties();
    }
}
