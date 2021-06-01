using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class antController : MonoBehaviour
{
    public float maxSpeed = 2;
    public float steerStrength = 2;
    public float wanderStrength = 0.5F;
    public float checkRate = 3;
    public float detectDistance = 3;
    public float castDistance = 10;

    Vector3 position;
    Vector3 velocity;
    Vector3 desiredDirection;
    float checkTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        checkTimer = checkRate;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void FixedUpdate()
    {

        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, castDistance, layerMask))
        {
            
            //ebug.DrawLine(this.transform.position, hit.point);
            if (hit.distance < detectDistance)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);

                if (checkTimer <= 0)
                {
                    var right45 = (transform.forward + transform.right).normalized;
                    RaycastHit rhit;
                    // since transform.left doesn't exist, you can use -transform.right instead
                    var left45 = (transform.forward - transform.right).normalized;
                    RaycastHit lhit;

                    Vector3 newDir = transform.forward;
                    
                    //Check if left raycast hits
                    if (Physics.Raycast(transform.position, left45, out lhit, castDistance, layerMask))
                    {
                        // Check if hit is near-distance
                        if (lhit.distance < detectDistance)
                        {
                            newDir = (newDir + transform.right * 5).normalized;
                            Debug.DrawRay(transform.position, left45 * lhit.distance, Color.red);
                        }
                        else
                        {
                            Debug.DrawRay(transform.position, left45 * lhit.distance, Color.yellow);
                        }
                        
                    }
                    else
                    {
                        Debug.DrawRay(transform.position, left45 * castDistance, Color.white);

                        // Check right
                        if (Physics.Raycast(transform.position, right45, out rhit, castDistance, layerMask))
                        {
                            if (rhit.distance < detectDistance)
                            {
                                newDir = (newDir - transform.right * 5).normalized;
                                Debug.DrawRay(transform.position, right45 * rhit.distance, Color.red);
                            }
                            else
                            {
                                Debug.DrawRay(transform.position, right45 * rhit.distance, Color.yellow);
                            }

                        }
                        else
                        {
                            Debug.DrawRay(transform.position, right45 * castDistance, Color.white);
                        }
                    }


                    //transform.Rotate(Vector3.up * 180);
                    float randomness = 3;
                    newDir.x += Random.Range(-randomness, randomness);
                    newDir.y += Random.Range(-randomness, randomness);

                    setDesiredDirection(newDir * 20, maxSpeed * 0.5f, steerStrength * 10);

                    checkTimer = checkRate;
                }
                else
                {
                    checkTimer -= 1;
                }
                
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Vector3 xyDirection = Random.insideUnitCircle * wanderStrength;

                setDesiredDirection(xyDirection, maxSpeed, steerStrength);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10, Color.white);
            //Debug.DrawLine(transform.position, hit.point);

            // Update Desired Direction
            Vector3 xyDirection = Random.insideUnitCircle * wanderStrength;

            setDesiredDirection(xyDirection, maxSpeed, steerStrength);
        }
    }

    void setDesiredDirection( Vector3 xyDirection, float speed, float steer)
    {
        desiredDirection.x = desiredDirection.x + xyDirection.x;
        desiredDirection.y = 0;
        desiredDirection.z = desiredDirection.z + xyDirection.y;
        desiredDirection = (desiredDirection).normalized;

        Vector3 desiredVelocity = desiredDirection * speed;
        Vector3 desiredSteeringForce = (desiredVelocity - velocity) * steer;
        Vector3 acceleration = Vector3.ClampMagnitude(desiredSteeringForce, steer) / 1;

        velocity = Vector3.ClampMagnitude(velocity + acceleration * Time.deltaTime, speed);
        position += velocity * Time.deltaTime;

        float angle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
        transform.SetPositionAndRotation(position, Quaternion.Euler(0, angle, 0));
    }

}
