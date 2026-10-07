using UnityEngine;

public class PlantingSpot : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private Shroom currentShroom;

    public bool IsAvailable => currentShroom == null;

    public Vector3 PlantPosition
    {
        get
        {
            if (spawnPoint != null)
                return spawnPoint.position;

            return transform.position;
        }
    }

    public void RegisterShroom(Shroom shroom)
    {
        currentShroom = shroom;
    }
}