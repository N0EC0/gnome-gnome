using UnityEngine;

public class Gnome : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 360f;

    [Header("Eating")]
    [SerializeField] private float eatDistance = 0.8f;

    [Header("Pooping")]
    [SerializeField] private GameObject poopPrefab;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip scareSound;
    [SerializeField] private AudioClip poopSound;
    [SerializeField] private AudioClip slapSound;

    [Header("Scared")]
    [SerializeField] private float scaredRunDuration = 2f;
    [SerializeField] private float scaredWaitDuration = 3f;
    [SerializeField] private float scaredMoveSpeed = 4f;

    private bool isScared = false;
    private bool isRunningAway = false;

    private float scaredTimer = 0f;
    private Transform scareSource;

    private Rigidbody rb;

    private Shroom targetShroom;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isScared)
        {
            scaredTimer -= Time.deltaTime;

            if (scaredTimer <= 0f)
            {
                if (isRunningAway)
                {
                    // Finished running. Now stand still.
                    isRunningAway = false;
                    scaredTimer = scaredWaitDuration;

                    Debug.Log(gameObject.name + " stopped running and is scared!");
                }
                else
                {
                    // Finished waiting. Resume normal behavior.
                    isScared = false;
                    scareSource = null;

                    Debug.Log(gameObject.name + " is no longer scared!");
                }
            }

            return;
        }

        if (targetShroom == null)
        {
            FindNearestMatureShroom();
        }
    }

    void FixedUpdate()
    {
        if (isScared)
        {
            if (isRunningAway)
            {
                RunAway();
            }

            // If scared but not running, just stand there.
            return;
        }

        if (targetShroom == null)
            return;

        MoveTowardShroom();
    }

    void FindNearestMatureShroom()
    {
        Shroom[] allShrooms = FindObjectsByType<Shroom>();

        Shroom closestShroom = null;

        float closestDistance = Mathf.Infinity;

        foreach (Shroom shroom in allShrooms)
        {
            if (!shroom.IsMature)
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, shroom.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestShroom = shroom;
            }
        }

        targetShroom = closestShroom;
    }

    void MoveTowardShroom()
    {
        if (targetShroom == null)
        {
            return;
        }

        Vector3 targetPosition = targetShroom.transform.position;

        // Keep gnome at its current heigt
        targetPosition.y = rb.position.y;

        // Direction toward the shroom
        Vector3 direction = targetPosition - rb.position;
        direction.y = 0f;

        // Face the direction we're moving
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);

            Quaternion newRotation = Quaternion.RotateTowards(
                    rb.rotation,
                    targetRotation,
                    rotationSpeed * Time.fixedDeltaTime
                );

            rb.MoveRotation(newRotation);
        }

        Vector3 newPosition = Vector3.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);

        rb.MovePosition(newPosition);

        float distance = Vector3.Distance(rb.position, targetPosition);

        if (distance <= eatDistance)
        {
            EatShroom();
        }
    }

    void EatShroom()
    {
        if (targetShroom == null)
        {
            return;
        }

        Debug.Log(gameObject.name + " is eating " + targetShroom.gameObject.name);

        targetShroom.Eat();

        Poop();

        targetShroom = null;
    }


    void Poop()
    {
        if (poopPrefab == null)
        {
            return;
        }

        Vector3 poopPosition = transform.position - transform.forward * 0.8f;

        poopPosition.y = 0.1f;

        Instantiate(poopPrefab, poopPosition, Quaternion.identity);

        if (audioSource != null && poopSound != null)
        {
            audioSource.PlayOneShot(poopSound);
        }

        Debug.Log(gameObject.name + " pooped!");
    }


    public void Scare(Transform player)
    {
        isScared = true;
        isRunningAway = true;

        scaredTimer = scaredRunDuration;
        scareSource = player;

        // Stop chasing the current shroom
        targetShroom = null;

        if (audioSource != null)
        {
            if (slapSound != null)
                audioSource.PlayOneShot(slapSound, 1f);

            if (scareSound != null)
                audioSource.PlayOneShot(scareSound, 0.6f);
        }

        Debug.Log(gameObject.name + " got scared!");
    }


    void RunAway()
    {
        if (scareSource == null)
            return;

        Vector3 direction =
            rb.position - scareSource.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        direction.Normalize();

        // Face away from player
        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        Quaternion newRotation =
            Quaternion.RotateTowards(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );

        rb.MoveRotation(newRotation);

        // Run away
        Vector3 newPosition =
            rb.position +
            direction *
            scaredMoveSpeed *
            Time.fixedDeltaTime;

        rb.MovePosition(newPosition);
    }
}
