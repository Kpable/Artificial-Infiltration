using UnityEngine;

public class JumpMovement : MonoBehaviour {

    [Tooltip("Sets the maximum jump height for the player")]
    public float jump = 1;			//How fast the player moves upwards when jumping 

    private Rigidbody body;    //Reference to the rigid body component on parent object

    [Tooltip("If selected the player stacks momentum while jumping")]
    public bool conserveMomentum;   //Determines if momentum is removed or conserved during secondary jumps
    private int numberOfJumps;      //Tracks the current number of times the player has jumped in a row
    [Tooltip("Sets the total number of jumps before the player must land")]
    public int totalJumps;          //Sets the maximum number if jumps the player can have in a row

    private AudioSource audioSource;
                                    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody>();      //Component reference for RigidBody

        numberOfJumps = 0;                          //Hasn't jumped yet when starting
        conserveMomentum = false;                   //Sets boolean at start for bug prevention

        // Get or add the audio for the sound effect.
        audioSource = GetComponentInChildren<AudioSource>();
        if (!audioSource)
        {
            Debug.LogWarning(name + " Cannot find audio source to play sound effects. Adding it.");
            transform.Find("SFX").gameObject.AddComponent<AudioSource>();
            audioSource = GetComponentInChildren<AudioSource>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        //TODO May want to seperate these out in the futures
        if (Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (numberOfJumps < totalJumps)                                 //Stops jumping if player exceeds maximun
        {
            if (conserveMomentum)                                       //Adds vertical force of jump onto current upwards force 
            {
                body.AddForce(transform.up * jump);
                numberOfJumps++;
            }
            else                                                        //Stops all vertical movement before adding more vertical force 
            {
                body.velocity = new Vector3(body.velocity.x, 0, body.velocity.z);
                body.AddForce(transform.up * jump);
                numberOfJumps++;
            }
            
            //  Play Jump sound effect
            AudioClip clip = SoundManager.instance.GetSoundEffect("Jump");
            if (clip != null)
                audioSource.PlayOneShot(clip);
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Ground"))            //Detects when hitting the ground
        {
            //Debug.Log(name + " Hit Ground");
            if (numberOfJumps > 0)
            {
                //Debug.Log(" Just hit ground");
                AudioClip clip = SoundManager.instance.GetSoundEffect("Ground");
                if (clip != null)
                    audioSource.PlayOneShot(clip);
            }

            numberOfJumps = 0;


        }
    }
}
