using UnityEngine;
using System.Collections;
using System;
using BGE.Forms;
using UnityEngine.Windows;

public class PlayerSteering : SteeringBehaviour
{
    public float power = 500.0f;

    public float upForce;
    public float rightForce;

    //private ViveController viveController;
    //private OculusController oculusController;
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


    private Thruster left;
    private Thruster right;
    private GameObject player;

    public void activateThrusters(bool b)
    {
        left.enabled = b;
        right.enabled = b;
    }


    public void Start()
    {
        //viveController = FindObjectOfType<ViveController>();
        //oculusController = FindObjectOfType<OculusController>();
        harmonic = GetComponent<Harmonic>();
        //vrMode = UnityEngine.XR.XRDevice.isPresent;
        maxSpeed = boid.maxSpeed;

        player = GameObject.FindGameObjectWithTag("Player");
        Thruster[] controllers = player.GetComponentsInChildren<Thruster>();
        left = controllers[0];
        right = controllers[1];
    }
    

    public override void Update()
    {
        base.Update();        

        average = Quaternion.Slerp(left.transform.rotation
    , right.transform.rotation, 0.5f);

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

        float l = left.input.action.ReadValue<float>();
        float r = right.input.action.ReadValue<float>();


        hSpeed = Mathf.Lerp(hSpeed
            ,Utilities.Map(l + r, 0, 1, 0.1f, 0.8f)
            , 2.0f * Time.deltaTime
            );

        harmonic.theta += hSpeed * Time.deltaTime;
        if (controlSpeed && controlType == ControlType.Ride || controlType == ControlType.JellyTenticle)
        {
            boid.maxSpeed = maxSpeed * hSpeed;
        }
        CreatureManager.Log("hSpeed: " + hSpeed);
        CreatureManager.Log("h Direction: " + average * Vector3.forward);

        //Debug.Log("Cont: " + contWalk);
    }

    [HideInInspector]
    public float hSpeed = 0;

    public override Vector3 Calculate()
    {
        if (controlType == ControlType.Ride)
        {
            /*force = (boid.right * rightForce * power)
                + (boid.up * upForce * power);
            */
            //if (vrMode)
            {
                force += average * Vector3.forward * power;
            }
        }
        else if (controlType == ControlType.JellyTenticle)
        {
            force = (boid.right * rightForce * power)
                            + (boid.up * upForce * power);

            //if (vrMode)
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
