using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfController : VirtualController
{
    public float SlerpTime = 0.25f;
    public NavMeshAgent navMeshAgent;
    public GameObject NavTarget;
    [Range(0,200f)]
    public int UpdateInterval;
    private int interval;
    private bool doUpdate;

    public Vector3 relativeVelocity;
    private Rigidbody rigidbody;
    private Vector3 SpawnPoint;
    private Vector2 currentMovementInput;
    void Awake() {
        SpawnPoint = transform.position;
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos() {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (Reset) {
        //     // navMeshAgent.enabled = false;
        //     navMeshAgent.Warp(navMeshAgent.transform.InverseTransformPoint(Vector3.zero));
        //     // transform.position = Vector3.zero;
        //     // navMeshAgent.enabled = true;
        //     Reset = false;
        //     navMeshAgent.stoppingDistance = StoppingDistance;
        //     // Debug.Break();
        //     // Reset = false;
        // }
    }

    private void FixedUpdate() {
        // interval++;
        // doUpdate = (interval % UpdateInterval) == 0;
        // if (doUpdate) {
            navMeshAgent.SetDestination(NavTarget.transform.position);
            relativeVelocity =  transform.InverseTransformVector(navMeshAgent.velocity);
            // var slerpedVelocity = Vector3.Slerp(rigidbody.velocity.normalized, relativeVelocity.normalized, SlerpTime);
            // currentMovementInput = new Vector2(
            //     slerpedVelocity.x,
            //     slerpedVelocity.z);
            currentMovementInput = new Vector2(
                relativeVelocity.x,
                relativeVelocity.z);

            base.onMovementInput(currentMovementInput);
            // if (Normalized) {
            //     base.onMovementInput(currentMovementInput.normalized);
            // } else if (Softened) {
            //     base.onMovementInput(currentMovementInput*Softener);
            // }
        // }    
    }
}
