using UnityEngine;

public class Fertilizer : MonoBehaviour
{

    [SerializeField] private float fertilizeRadius = 2f;

    [SerializeField] private float lifetime = 12f;

    public float FertilizeRadius => fertilizeRadius;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FertilizeNearbyShrooms();

        Destroy(gameObject, lifetime);
    }

    void FertilizeNearbyShrooms()
    {
        Shroom[] shrooms = FindObjectsByType<Shroom>();

        foreach (Shroom shroom in shrooms)
        {
            float distance = Vector3.Distance(transform.position, shroom.transform.position);

            if (distance <= fertilizeRadius)
            {
                shroom.Fertilize();
            }
        }
    }
}
