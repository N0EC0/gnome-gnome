using UnityEngine;

public class Shroom : MonoBehaviour
{

    public enum ShroomState
    {
        Growing,
        Mature
    }

    [Header("Growth")]
    [SerializeField] private float growthTime = 8f;
    [SerializeField] private Vector3 startScale = new Vector3(0.2f, 0.2f, 0.2f);
    [SerializeField] private Vector3 matureScale = new Vector3(0.8f, 1.2f, 0.8f);

    [SerializeField] private Transform shroomVisual;

    [SerializeField] private Renderer shroomRenderer;

    [SerializeField] private Color growingColor = Color.white;
    [SerializeField] private Color matureColor = Color.red;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip matureSound;
    private float growthTimer =0f;

    private ShroomState currentState = ShroomState.Growing;

    public ShroomState CurrentState => currentState;
    public bool IsMature => currentState == ShroomState.Mature;

    private bool isFertilized = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shroomVisual.localScale = startScale;
        if (shroomRenderer != null)
        {
            shroomRenderer.material.color = growingColor;
        }
        CheckForFertilizer();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == ShroomState.Mature)
        {
            return;
        }

        growthTimer += Time.deltaTime;

        float growthProgress = Mathf.Clamp01(growthTimer / growthTime);

        shroomVisual.localScale = Vector3.Lerp(startScale, matureScale, growthProgress);

        if (growthTimer >= growthTime)
        {
            currentState = ShroomState.Mature;

            shroomVisual.localScale = matureScale;

            if (shroomRenderer != null)
            {
                shroomRenderer.material.color = matureColor;
            }

            if (audioSource != null && matureSound != null)
            {
                audioSource.PlayOneShot(matureSound);
            }

            Debug.Log(gameObject.name + " is mature!");
        }
    }


    public void Eat()
    {
       Debug.Log(gameObject.name + " was eaten!"); 
       Destroy(gameObject);
    }


    public void Fertilize()
    {
        if (isFertilized)
        {
            return;
        }

        if (IsMature)
        {
            return;
        }

        isFertilized = true;

        // Instant growth by 50%
        growthTimer += growthTime * 0.5f;

        Debug.Log(gameObject.name + " has been fertilized!");
    }


    void CheckForFertilizer()
    {
        Fertilizer[] fertilizers = FindObjectsByType<Fertilizer>();

        foreach (Fertilizer fertilizer in fertilizers)
        {
            float distance = Vector3.Distance(transform.position, fertilizer.transform.position);

            if (distance <= fertilizer.FertilizeRadius)
            {
                Fertilize();
                return;
            }
        }
    }
}
