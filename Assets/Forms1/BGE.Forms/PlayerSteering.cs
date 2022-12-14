using UnityEngine;
using System.Collections;
using System;
using BGE.Forms;

public class PlayerSteering : SteeringBehaviour
{
    public float power = 500.0f;

    public float upForce;
    public float rightForce;

    private ViveController viveController;
    private OculusController oculusController;
    bool vrMode = false;
    private Vector3 viveForce;

    private Quaternion average;

    private bool pickedUp = false;

    public enum ControlType { Ride, Tenticle, TenticleFlipped, JellyTenticle };

    public ControlType controlType = ControlType.Ride;

    Harmonic harmonic;

    [HideInInspector]
    public float maxSpeed = 0;

    [HideInInspector]
    public bool controlSpeed = true;

    public void Start()
    {
        viveController = FindObjectOfType<ViveController>();
        oculusController = FindObjectOfType<OculusController>();
        harmonic = GetComponent<Harmonic>();
        vrMode = true; //  UnityEngine.XR.XRDevice.isPresent;
        maxSpeed = boid.maxSpeed;
    }
    

    public override void Update()
    {
        base.Update();
        upForce = -Input.GetAxis("Vertical");
        rightForce = Input.GetAxis("Horizontal") * 0.6f;
        CreatureManager.Log("Player force: " + force);
        CreatureManager.Log("RightForce: " + rightForce);

        /*

        // Control the boid
        if (viveController != null)
        {
            if (viveController.leftTrackedObject != null && viveController.rightTrackedObject != null  && viveController.leftTrackedObject.isActiveAndEnabled)
            {

                average = Quaternion.Slerp(viveController.leftTrackedObject.transform.rotation
                    , viveController.rightTrackedObject.transform.rotation, 0.5f);

                if (controlType == ControlType.Tenticle)
                {
                    Vector3 xyz = average.eulerAngles;
                    harmonic.theta = Mathf.Deg2Rad * (xyz.x + 180);
                }
                if (controlType == ControlType.TenticleFlipped)
                {
                    Vector3 xyz = average.eulerAngles;
                    harmonic.theta = Mathf.Deg2Rad * (xyz.x);
                }


            }
        }

        if (oculusController != null && oculusController.isActiveAndEnabled)
        {
            if (OVRInput.GetControllerPositionTracked(OVRInput.Controller.LTouch) || OVRInput.GetControllerPositionTracked(OVRInput.Controller.RTouch))
            {

                average = Quaternion.Slerp(oculusController.leftHand.rotation
                    , oculusController.rightHand.rotation, 0.5f);

                if (controlType == ControlType.Tenticle)
                {
                    Vector3 xyz = average.eulerAngles;
                    harmonic.theta = Mathf.Deg2Rad * (xyz.x + 180);
                }
                if (controlType == ControlType.TenticleFlipped)
                {
                    Vector3 xyz = average.eulerAngles;
                    harmonic.theta = Mathf.Deg2Rad * (xyz.x);
                    
                }
            }
        }
        */

        /*
              hSpeed = Mathf.Lerp(hSpeed
            ,Utilities.Map(Input.GetAxis("LeftTrigger") + Input.GetAxis("RightTrigger"), 0, 2, 0.0f, 0.01f)
            , 2.0f * Time.deltaTime
            );

        harmonic.theta += hSpeed * Time.deltaTime * harmonic.speed;
        */

        hSpeed = Mathf.Lerp(hSpeed
            ,Utilities.Map(Input.GetAxis("LeftTrigger") + Input.GetAxis("RightTrigger"), 0, 1, 0.1f, 0.8f)
            , 2.0f * Time.deltaTime
            );

        harmonic.theta += hSpeed * Time.deltaTime;
        if (controlSpeed && controlType == ControlType.Ride || controlType == ControlType.JellyTenticle)
        {
            boid.maxSpeed = maxSpeed * hSpeed;
        }
        //Debug.Log("Cont: " + contWalk);
    }

    [HideInInspector]
    public float hSpeed = 0;

    public override Vector3 Calculate()
    {
        if (controlType == ControlType.Ride)
        {
            force = (boid.right * rightForce * power)
                + (boid.up * upForce * power);
            if (vrMode)
            {
                force += average * Vector3.forward * power;
            }
        }
        else if (controlType == ControlType.JellyTenticle)
        {
            force = (boid.right * rightForce * power)
                            + (boid.up * upForce * power);

            if (vrMode)
            {
                force += average * Vector3.forward * power;
            }
        }
        else
        {
            force = Vector3.zero;
        }
        return force;
    }
}
