using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{ 
    public enum PickupType
{
    Life,
    PowerupJump,
    PowerupSpeed,
    Score
}

    [SerializeField] private PickupType type;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            PlayerController pc = collider.GetComponent<PlayerController>();

            switch(type)
            {
                case PickupType.Life:
                    pc.lives++;
                    break;
                case PickupType.PowerupJump:
                case PickupType.PowerupSpeed:
                    pc.PowerupValueChange(type);
                    break;
                case PickupType.Score:
                    break;
                 
            }

            Destroy(this.gameObject);
        }
    }
}
