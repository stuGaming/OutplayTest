using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatedObject : MonoBehaviour
{
    private Rigidbody _thisRigid;
    public Rigidbody thisRigid
    {
        get
        {
            if (_thisRigid == null)
            {
                _thisRigid = this.GetComponent<Rigidbody>();
            }
            return _thisRigid;
        }
    }

    public Action<GameObject> OnCollision;

    // Start is called before the first frame update
    void Start()
    {
        CameraManager.Instance.RegisterSimulation(this);
    }
    private void OnTriggerEnter(Collider other)
    {
        OnCollision?.Invoke(other.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
