using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BGE.Forms;

public class OculusController : MonoBehaviour {
    public Transform leftHand;
    public Transform rightHand;

    GameObject leftEngine;
    GameObject rightEngine;

    private JetFire leftJet;
    private JetFire rightJet;
    private Rigidbody rb;

    public GameObject head;
    public float maxSpeed = 250.0f;
    public float power = 1000.0f;

    public bool haptics = false;

    public static OculusController Instance;

    void Awake()
    {
        Instance = this;
    }

    public Boid boid;

    /*

    // Use this for initialization
    void Start ()
    {
        leftEngine = leftHand.GetChild(0).gameObject;
        leftJet = leftEngine.GetComponentInChildren<JetFire>();

        rightEngine = rightHand.GetChild(0).gameObject;
        rightJet = rightEngine.GetComponentInChildren<JetFire>();

        rb = GetComponent<Rigidbody>();
	}

    

    public bool GetGrip()
    {
        if (isActiveAndEnabled)
        {
            return (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch) > 0.5f || OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch) > 0.5f);
        }
        else
        {
            return false;
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.H))
        {
            haptics = !haptics;
        }
        CreatureManager.Log("Haptics: " + haptics);


        if (OVRInput.GetControllerPositionTracked(OVRInput.Controller.LTouch))
        {
            leftEngine.SetActive(true);
            float leftTrig =  OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
            if (leftTrig > 0.02f)
            {
                if (boid == null)
                {
                    rb.AddForceAtPosition(leftHand.forward * power * leftTrig, leftEngine.transform.position);
                    leftJet.fire = leftTrig;
                    if (haptics)
                    {
                        OVRInput.SetControllerVibration(leftTrig * 0.5f, leftTrig * 0.5f, OVRInput.Controller.LTouch);
                    }
                }
                else
                {
                    boid.speed = boid.maxSpeed * leftTrig;
                    HarmonicController hc = boid.GetComponent<HarmonicController>();
                    if (hc != null)
                    {
                        boid.GetComponent<Harmonic>().speed = boid.GetComponent<HarmonicController>().initialSpeed * leftTrig;
                    }
                }
            }
            else
            {
                
                leftJet.fire = 0;
                if (haptics)
                {
                    OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
                }

            }
        }
        else
        {
            leftEngine.SetActive(false);
        }
        if (OVRInput.GetControllerPositionTracked(OVRInput.Controller.RTouch))
        {
            rightEngine.SetActive(true);

            float rightTrig = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
            if (rightTrig > 0.02f)
            {
                if (boid == null)
                {
                    rb.AddForceAtPosition(rightHand.forward * power * rightTrig, rightEngine.transform.position);
                    rightJet.fire = rightTrig;

                    if (haptics)
                    {
                        OVRInput.SetControllerVibration(rightTrig * 0.5f, rightTrig * 0.5f, OVRInput.Controller.RTouch);
                    }
                }
                else
                {
                    boid.speed = boid.maxSpeed * rightTrig;
                    HarmonicController hc = boid.GetComponent<HarmonicController>();
                    if (hc != null)
                    {
                        boid.GetComponent<Harmonic>().speed = boid.GetComponent<HarmonicController>().initialSpeed * rightTrig;
                    }
                }
            }
            else
            {
                rightJet.fire = 0;
                if (haptics)
                {
                    OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
                }
            }
        }
        else
        {
            rightEngine.SetActive(false);
        }       
    }
    */
}
