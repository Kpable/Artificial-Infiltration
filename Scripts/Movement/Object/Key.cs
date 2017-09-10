using UnityEngine;

[RequireComponent (typeof (BoxCollider))]
public class Key : MonoBehaviour {

    public int keyID = -1;

    void Awake()
    {
        BoxCollider col = GetComponent<BoxCollider>();
        if (!col.isTrigger)
        {
            Debug.LogWarning("This Key needs to be a trigger. Making it a trigger");
            col.isTrigger = true;
        }

        if(keyID == -1)
        {
            Debug.LogWarning(name + ": This key isnt assigned a keyID. Position: " + transform.position.ToString());
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Keys>().GainKey(keyID);
            Destroy(gameObject);
        }
    }
}
