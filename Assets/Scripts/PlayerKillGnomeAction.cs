using UnityEngine;

public class PlayerScareGnomeAction : MonoBehaviour
{
    [SerializeField] private float killRange = 1.5f;


    void Update()
    {
        if (GameManager.Instance == null)
            return;

        if (!GameManager.Instance.GameStarted ||
            GameManager.Instance.GameEnded)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            ScareGnome();
        }
    }

    void ScareGnome()
    {
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, killRange);

        foreach (Collider nearbyObject in nearbyObjects)
        {
            Gnome gnome = nearbyObject.GetComponent<Gnome>();

            if (gnome != null)
            {
                Debug.Log("Gnome removed!");
                gnome.Scare(transform);
                return;
            }
        }
    }
}
