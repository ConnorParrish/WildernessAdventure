using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfController : VirtualController
{
    public Vector3 relativeVelocity;
    public float ExtraRotationSpeed = 7f;
    [Range(0,1f)]
    public float VelocityDampener = 1f;
    public GameObject NavTarget;
    private NavMeshAgent navMeshAgent;
    private Vector2 currentMovementInput;
    void Awake() {
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        if (NavTarget) {
        }
    }

    private void OnDrawGizmos() {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate() {

        if (NavTarget) {
            navMeshAgent.SetDestination(NavTarget.transform.position);
            // if (!navMeshAgent.destination){
                // navMeshAgent.SetDestination(NavTarget.transform.position);
            // }
            Vector3 lookRotation = navMeshAgent.steeringTarget - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookRotation), ExtraRotationSpeed*Time.deltaTime);


            relativeVelocity =  transform.InverseTransformVector(navMeshAgent.velocity);
            // relativeVelocity += transform.InverseTransformVector(lookRotation);
            // relativeVelocity *= VelocityDampener;
            relativeVelocity /= navMeshAgent.speed *.5f;
            currentMovementInput = new Vector2(
                relativeVelocity.x,//*ExtraRotationSpeed,
                relativeVelocity.z);//*ExtraRotationSpeed);

            base.onMovementInput(currentMovementInput);
        }
    }
}
