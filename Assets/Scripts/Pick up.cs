using Unity.VisualScripting;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    
    [SerializeField] private LayerMask ItemMask;
    [SerializeField] private Transform cam;

    [SerializeField] private Artefactcolected artefactcolected;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float pickupdistance= 2f;
            if(Physics.Raycast(cam.position, cam.forward, out RaycastHit raycasthit,pickupdistance, ItemMask))
            {
                artefactcolected.Oncollected();
            }
        }

       
    }
}
