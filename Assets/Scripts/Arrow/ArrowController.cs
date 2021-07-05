using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public Vector3 InitialSpawnPositionOffset;
    public Vector3 InitialSpawnRotation;

    public Vector3 BackBoneOffset;
    public Vector3 ArrowOffset;
    public Vector3 FrontBoneOffset;
    public float ArrowVelocity = 4f;
    public Vector3 ArrowRotationalVelocity = new Vector3(0,0,4);
    public GameObject BackBone;
    public GameObject ForwardBone;
    
    bool isBalancing = false;
    bool isFlying = false;
    private Vector3 arrowDestination;

    // Start is called before the first frame update
    public void BeginFlight(Vector3 destination) {
        isFlying = true;
        arrowDestination = destination;
    }

    public void BeginBalancing() {
        isBalancing = true;
    }
    public void DisableBalancing() {
        isBalancing = false;
    }

    private void OnDrawGizmos() {
        if (isBalancing) {
            Gizmos.color = new Color(1,0,0,0.5f);
            var correctedOffset = ForwardBone.transform.TransformPoint(FrontBoneOffset);
            var offset = ForwardBone.transform.TransformVector(FrontBoneOffset);
            Gizmos.DrawCube(ForwardBone.transform.position + offset, new Vector3(.1f,.1f,.1f));
        }
    }

    private void Start() {
        // transform.localPosition = InitialSpawnPositionOffset;
        // transform.localEulerAngles = InitialSpawnRotation;
        // transform.SetParent(BackBone.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlying) {
            transform.position += transform.forward * ArrowVelocity * Time.deltaTime;
            transform.localEulerAngles += ArrowRotationalVelocity * Time.deltaTime;
        } else if (isBalancing) {
            var midPoint = Utilities.MidPoint(
                BackBone.transform.position + BackBoneOffset,
                ForwardBone.transform.position + FrontBoneOffset);

            transform.position = midPoint + ArrowOffset;
            var correctedOffset =ForwardBone.transform.TransformVector(FrontBoneOffset);
            transform.LookAt(ForwardBone.transform.position + correctedOffset);
        }
    }

    private void FixedUpdate() {
        var ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, transform.forward, out hit, 1f)){
            ArrowVelocity = 0;
            ArrowRotationalVelocity = Vector3.zero;
        }
    }
    public IEnumerator RotateToRest(float duration) {
        var offsetToBackBone = transform.localPosition;
        transform.SetParent(null);

        float t = 0f;
        while (t < duration) {
            Quaternion targetRotation = Quaternion.LookRotation(ForwardBone.transform.position - transform.position);
            t += Time.deltaTime;
            float factor = t / duration;

            var midPoint = Utilities.MidPoint(
                BackBone.transform.position + BackBoneOffset,
                ForwardBone.transform.position + FrontBoneOffset);

            // transform.position = midPoint + ArrowOffset;
            transform.position = BackBone.transform.position + offsetToBackBone;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, factor);
            Debug.Log("still rotating!");
            yield return null;
        }
        BeginBalancing();
    }
}

