using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public CinemachineVirtualCamera FollowCamera;

    public float MinFOV;
    public float MaxFOV;
    float m_zoom = 1f;
    public void RegisterSimulation(SimulatedObject simulated)
    {
        FollowCamera.m_LookAt = simulated.transform;
    }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        FollowCamera.m_Lens.FieldOfView = MinFOV;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            m_zoom += Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            m_zoom -= Time.deltaTime;
        }
       
        m_zoom = Mathf.Clamp01(m_zoom);
        FollowCamera.m_Lens.FieldOfView = Mathf.Lerp(MinFOV,MaxFOV,m_zoom);
    }
}
