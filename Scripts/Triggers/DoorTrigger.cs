using UnityEngine;

public class DoorTrigger : MonoBehaviour {

    [Tooltip("The door/object you wish to open")]
    public GameObject doorObject;                   // The door object to open. 
    [Tooltip("The direction in which to move the door")]
    public Direction direction = Direction.Back;      // The direction to move the door in. 
    [Tooltip("How long should the door stay open (in seconds)")]
    public float durationBeforeClosing = 3f;                     // How long, in seconds, should the door stay "open"
    [Tooltip("Should the door close after the above duration.")]
    public bool closeAfterOpening = true;               // Whether the door should close after a set time
    [Tooltip("Is the door locked, requiring a key?")]
    public bool isLocked = false;                   // Whether the door is locked. 
    [Tooltip("The door ID, used to match with a key with the same ID")]
    public int doorID = -1;
    [Tooltip("The locked and unlocked lights.")]
    public GameObject redLight, greenLight; // ...1 2 3. 

    private float destinationAngle;                    // Object destination. 

    [SerializeField]
    private float moveRate = 2f;                  // Rate at which the object moves.

    private bool opening = false;                      // Whether the door is opening.
    private bool closing = false;                      // Whether the door is closing.

    private bool doorOpen = false;
    
    void Awake()
    {
        if (doorID == -1 && isLocked )
        {
            Debug.LogWarning(name + ": This door isnt assigned a doorID. Position: " + transform.position.ToString());
        }
    }

    // Use this for initialization
    void Start ()
    {
        // Set the red light on and the green light off
        LightsChange();

        switch (direction)
        {
            // The positives
            case Direction.Up:
            case Direction.Right:
            case Direction.Forward:
                destinationAngle = doorObject.transform.rotation.eulerAngles.y + 90f;
                break;
            // The negatives
            case Direction.Left:
            case Direction.Down:
            case Direction.Back:
                destinationAngle = doorObject.transform.rotation.eulerAngles.y - 90f;
                break;
            default:
                destinationAngle = 0;
                break;
        }
    }

    // Using Fixed update for any physics based actions. 
    void FixedUpdate () {
        if (opening)               // If we are opening the door...
        {
            OpenDoor();             //  ...do so.

            if (doorObject.transform.rotation.eulerAngles.y == destinationAngle || 
                doorObject.transform.rotation.eulerAngles.y == 360 + destinationAngle)       // If we've reached our destination! 
            {
                opening = false;        // Stop opening already

                // Precautionary so the player doesnt get stuck after the door is open
                doorObject.GetComponentInChildren<Collider>().isTrigger = true;

                if (closeAfterOpening)            // (reset) If you are moving back dont move back again. 
                {
                    Invoke("CloseDoor", durationBeforeClosing);    // Return to start position after duration.
                }
            }
        }
        else if (closing)       // If we are closing the door...
        {
            CloseDoor();        // ...do the thing

            if (doorObject.transform.rotation.eulerAngles.y == 0)       // If we've reached our destination! 
            {
                closing = false;
                doorOpen = false;
                LightsChange();
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log(name + " was triggered by: " + col.gameObject.name);

        if (col.CompareTag("Player"))           // If the player entered the trigger
        {
            if (doorObject != null)             // If we have a door object to move
            {
                if ( isLocked )                 // If the door is locked. 
                {   
                    if( col.gameObject.GetComponent<Keys>().HasKey(doorID) )     // Check if player has the right key
                    {
                        //col.gameObject.GetComponent<Keys>().UseKey();   // Use that key
                        isLocked = false;                               // Unlock the door permanently
                        opening = true;                                    // Begin moving to destination

                        doorOpen = true;

                        // Set the red light on and the green light off
                        LightsChange();

                        if (closeAfterOpening)                              // If door was unlocked its not returning to start
                        {
                            Debug.LogWarning("Locked doors do not close after they open.");       // Annoying reminder
                            closeAfterOpening = false;                                          // Fixing 
                        }
                    }
                }
                else
                {
                    opening = true;                // If not locked just move right along... nothing to see here
                    doorOpen = true;
                    LightsChange();
                }
            }
        }
    }

    void OpenDoor()
    {
        doorObject.transform.rotation = Quaternion.RotateTowards(doorObject.transform.rotation, Quaternion.AngleAxis(destinationAngle, Vector3.up), moveRate);         
    }

    public void CloseDoor()
    {
        if (!closing)
        {
            closing = true;                           // Get a move on.
            doorObject.GetComponentInChildren<Collider>().isTrigger = false;  // make a solid object again as its closing.
        }

        doorObject.transform.rotation = Quaternion.RotateTowards(doorObject.transform.rotation, Quaternion.AngleAxis(0, Vector3.up), moveRate);
    }

    void LightsChange()
    {
        if (redLight) redLight.SetActive(!doorOpen);
        if (greenLight) greenLight.SetActive(doorOpen);
    }
}
