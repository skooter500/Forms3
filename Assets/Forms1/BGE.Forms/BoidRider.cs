using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

namespace BGE.Forms
{
    public class BoidRider : MonoBehaviour
    {        
        bool vrMode = false;

        [HideInInspector]
        PlayerSteering ps;

        bool attached = false;

        public InputActionProperty input;

        // Use this for initialization
        void Start()
        {
            //vrMode = UnityEngine.XR.XRDevice.isPresent;
        }


        Quaternion targetQuaternion = Quaternion.identity;

        public void Detatch()
        {
            Debug.Log("Detatch");
            if (attached)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                attached = false;

                Boid boid = Utilities.FindBoidInHierarchy(this.gameObject);
                //other.transform.parent = this.transform.parent;

                player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                player.GetComponent<Rigidbody>().isKinematic = false;
                ps = boid.GetComponent<PlayerSteering>();
                ps.activateThrusters(true);
                ps.SetActive(false);
                
                if (boid.GetComponent<NoiseWander>() != null)
                {
                    boid.GetComponent<NoiseWander>().SetActive(true);
                }

                if (boid.GetComponent<Harmonic>() != null)
                {
                    boid.GetComponent<Harmonic>().SetActive(true);
                    boid.GetComponent<Harmonic>().auto = true;
                }


                boid.maxSpeed = boid.GetComponent<PlayerSteering>().maxSpeed;


                

                VaryTenticles vt = boid.transform.parent.GetComponent<VaryTenticles>();
                if (vt != null)
                {
                    vt.Vary();
                }

                RotateMe[] r = FindObjectsOfType<RotateMe>();
                foreach (RotateMe rm in r)
                {
                    rm.speed = 0.1f;
                }
            }
        }

        public void OnTriggerEnter(Collider c)
        {
            GameObject other = c.gameObject;
            if (other.tag == "Player" && !dontAttach)
            {
                attached = true;
                Boid boid = Utilities.FindBoidInHierarchy(this.gameObject);
                //other.transform.parent = this.transform.parent;
                
                other.GetComponent<Rigidbody>().velocity = Vector3.zero;
                other.GetComponent<Rigidbody>().isKinematic = true;
                ps = boid.GetComponent<PlayerSteering>();
                ps.SetActive(true);
                ps.activateThrusters(false);
                ps.hSpeed = 1.0f;
                //boid.GetComponent<Harmonic>().SetActive(true);
                
                HarmonicController hc = boid.GetComponent<HarmonicController>();
                if (boid.GetComponent<HarmonicController>() != null)
                {
                    hc.enabled = false;
                    //boid.GetComponent<Harmonic>().amplitude = hc.initialAmplitude;
                    //boid.GetComponent<Harmonic>().speed = hc.initialSpeed;
                }
                
                VaryTenticles vt = boid.transform.parent.GetComponent<VaryTenticles>();
                if (vt != null)
                {
                    vt.UnVary();
                }
                
                Constrain con = boid.GetComponent<Constrain>();
                if (con != null)
                {
                    con.SetActive(false);
                }

                if (boid.GetComponent<NoiseWander>() != null)
                {
                    boid.GetComponent<NoiseWander>().SetActive(false);
                }

                if (boid.GetComponent<JitterWander>() != null)
                {
                    boid.GetComponent<JitterWander>().SetActive(false);
                }
                RotateMe r = GetComponent<RotateMe>();
                if (r != null)
                {
                    r.speed = 0;
                }
                //boid.damping = 0.01f;
                Debug.Log(boid);
                dontAttach = true;
                Invoke("Attach", 5);
            }
        }

        bool dontAttach = false;

        void Attach()
        {
            dontAttach = false;
        }

        void OnTriggerStay(Collider c)
        {
            GameObject other = c.gameObject;
            // iF its a player and still attached
            if (other.tag == "Player" && attached)
            {
                other.transform.position = Vector3.Lerp(other.transform.position, this.transform.position, Time.deltaTime * 5.0f);
                // Dont do this in VR
                /*if (!vrMode)
                {
                    ForceController fc = other.GetComponent<ForceController>();
                    if (fc.cameraType == ForceController.CameraType.forward)
                    {
                        Transform parent = transform.parent;                         
                        if (!fc.rotating)
                        {
                            fc.desiredRotation = Quaternion.Slerp(fc.desiredRotation, parent.rotation, Time.deltaTime * 1.5f);
                        }
                    }
                } 
                */
                Debug.Log(input.action.ReadValue<float>());
                if (input.action.ReadValue<float>() == 1)
                {
                    Detatch();
                }
            }
        }
    }
}
