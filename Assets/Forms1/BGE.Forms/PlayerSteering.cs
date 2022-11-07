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

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + force * 50);
    }

    public void activateThrusters(bool b)
    {
        left.readInput = b;
        right.readInput = b;
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

    float l;
    float r;
    

    public override void Update()
    {
        base.Update();
        Vector3[] points = new Vector3[2];
        points[0] = transform.position;
        points[1] = transform.position + force * 50;
        LineRenderer lr = GetComponent<LineRenderer>();
        if (lr == null)
        {
            lr = gameObject.AddComponent<LineRenderer>();
        }
        lr.SetPositions(points);
        //Debug.DrawLine(transform.position, transform.position + force * 50);
        average = Quaternion.Slerp(left.transform.parent.rotation
    , right.transform.parent.rotation, 0.5f);

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

        l = left.input.action.ReadValue<float>();
        r = right.input.action.ReadValue<float>();


        hSpeed = Mathf.Lerp(hSpeed
            , Utilities.Map(l + r, 0, 1, 0.1f, 0.8f)
            , 2.0f * Time.deltaTime
            );


        harmonic.multiplier = l + r;
        boid.speed = boid.maxSpeed * Mathf.Max(l, r);
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
            //force = (boid.right * rightForce * power)
            //    + (boid.up * upForce * power);
            force = average * Vector3.forward * power;            
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
