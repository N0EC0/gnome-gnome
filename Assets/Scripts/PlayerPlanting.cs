using UnityEngine;

public class PlayerPlanting : MonoBehaviour
{
    [SerializeField] private GameObject shroomPrefab;
    [SerializeField] private float plantingRange = 1.5f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip plantingSound;

    void Update()
    {
        if (GameManager.Instance == null)
            return;

        if (!GameManager.Instance.GameStarted ||
            GameManager.Instance.GameEnded)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPlant();
        }
    }

    void TryPlant()
    {
        Collider[] nearbyObjects = Physics.OverlapSphere(
            transform.position,
            plantingRange,
            ~0,
            QueryTriggerInteraction.Collide
        );

        PlantingSpot closestSpot = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider nearbyObject in nearbyObjects)
        {
            PlantingSpot spot =
                nearbyObject.GetComponentInParent<PlantingSpot>();

            if (spot == null)
                continue;

            if (!spot.IsAvailable)
                continue;

            float distance = Vector3.Distance(
                transform.position,
                spot.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestSpot = spot;
            }
        }

        if (closestSpot == null)
        {
            Debug.Log("No available planting spot nearby.");
            return;
        }

        GameObject newShroom = Instantiate(
            shroomPrefab,
            closestSpot.PlantPosition,
            Quaternion.identity
        );

        Shroom shroom = newShroom.GetComponent<Shroom>();

        closestSpot.RegisterShroom(shroom);

        if (audioSource != null && plantingSound != null)
        {
            audioSource.PlayOneShot(plantingSound);
        }

        Debug.Log("Shroom planted!");
    }
}


// using UnityEngine;

// public class PlayerPlanting : MonoBehaviour
// {

//     [SerializeField] private GameObject shroomPrefab;
//     [SerializeField] private Transform shroomPoint;
//     [SerializeField] private float plantingRadius = 0.5f;

//     // Update is called once per frame
//     void Update()
//     {
//         if (GameManager.Instance == null)
//             return;

//         if (!GameManager.Instance.GameStarted ||
//             GameManager.Instance.GameEnded)
//             return;

//         if (Input.GetKeyDown(KeyCode.E))
//         {
//             PlantSeed();
//         }
//     }


//     void PlantSeed()
//     {
//         Collider[] objectsNearby = Physics.OverlapSphere(shroomPoint.position, plantingRadius);
        
//         foreach (Collider objectNearby in objectsNearby)
//         {
//             if (objectNearby.CompareTag("Shroom"))
//             {
//                 Debug.Log("Too close to another shroom!");
//                 return;
//             }
//         }

//         Instantiate(shroomPrefab, shroomPoint.position,Quaternion.identity);
//         Debug.Log("Seed planted!");
//     }
// }
