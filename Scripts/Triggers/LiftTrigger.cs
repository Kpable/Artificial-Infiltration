using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LiftTrigger : MonoBehaviour {

    [Tooltip("Sets the force to apply while the object is within the trigger.")]
    public float liftForce = 20;        //The lift force to apply to the object that enters the trigger
    [Tooltip("Sets the direction of which the above force is applied.")]
    public Direction direction = Direction.Up;  //The Direction in which to apply that force. 
    [Tooltip("Sets the whether the force is applied as a one time cannon shot")]
    public bool singleShot = false;   // Whether to shoot the force one time. 

    List<Rigidbody> liftTargets;    

    void Awake()
    {
        liftTargets = new List<Rigidbody>();                    //Strangle prevention comment!
    }

    void FixedUpdate()
    {
        if( liftTargets.Count > 0 )
        {
            //Continuously add liftForce to the rigid bodies in the trigger
            if (!singleShot)
                liftTargets.ForEach(body => { body.AddForce(direction.Trans(body.transform) * liftForce); }); // Get handsy with that body

        }
    }

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log(name + " was triggered by: " + col.gameObject.name);
        Rigidbody body = col.GetComponent<Rigidbody>();             //Grab the Rigidbody
        if ( body != null)                                          //Check if its present
        {
            if (singleShot)            
                body.AddForce(direction.Trans(body.transform) * liftForce);         // Just once.. just once   
            else
                liftTargets.Add(body);                                  //Add to list of bodies to get handsy with
            
        }
    }

    void OnTriggerExit(Collider col)
    {
        //Debug.Log(name + " trigger exited by: " + col.gameObject.name);
        Rigidbody body = col.GetComponent<Rigidbody>();        //Grab the Rigidbody
        if ( body != null )                                         //Check if its present
        {
            if ( liftTargets.Contains(body) )                       //Check if it's currently in the list of bodies
            {
                bool success = liftTargets.Remove(body);            //Remove them if they are
                if (!success) Debug.LogWarning("Failed to remove body: " + body.gameObject.name);
            }
        }
    }
}
