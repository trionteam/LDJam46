using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    public Zombifiable.State CausedState = Zombifiable.State.Zombie;
    public List<Zombifiable.State> AcceptedStates = null;

    private GameObject source;

	public float LifeTime = 5;  // seconds
	public float LifeLengthRandomness = 0.25f;
	public float FadeLastSeconds = 0.5f;    // number of seconds to fade after it dies
	public float RotationSpeed = 100;
	private Vector3 velocity = Vector3.zero;

	private float lifeLeft = 0;
	private float actualLifeTime = 0;
	private SpriteRenderer spriteRenderer;
	private float rot = 1;
	private float scale = 1;

	public void Init(Vector2 vel)
	{
		velocity = vel;
		rot = Random.Range(0, 2) == 0 ? -1.0f : 1.0f;
		rot *= Random.Range(0.5f, 2.5f) * RotationSpeed;
	}

    public void SetSourceZombie(GameObject source)
    {
        this.source = source;
    }

    // Start is called before the first frame update
    void Start()
    {
		actualLifeTime = lifeLeft = LifeTime + Random.Range(-1.0f, 1.0f) * LifeLengthRandomness;
		spriteRenderer = GetComponent<SpriteRenderer>();

		// add a bit of variation
		float H, S, V;
		Color.RGBToHSV(spriteRenderer.color, out H, out S, out V);
		H += Random.Range(-0.1f, 0.1f);
		spriteRenderer.color = Color.HSVToRGB(H, S, V);

		scale = Random.Range(0.8f, 1.2f);
		transform.localRotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
		transform.localScale = Vector3.zero;
	}

    // Update is called once per frame
    void Update()
    {
		lifeLeft -= Time.deltaTime;

		transform.position += velocity * Time.deltaTime;
		transform.Rotate(Vector3.forward, rot * Time.deltaTime);

		float normalizedLifeTime = (actualLifeTime - lifeLeft) / actualLifeTime;

		Color c = spriteRenderer.color;
		
		if (lifeLeft <= 0)
		{
			Destroy(gameObject);
			return;
		}

		c.a = Curves.Instance?.CloudCurveAlpha.Evaluate(normalizedLifeTime) ?? 1.0f;
		spriteRenderer.color = c;

		float s = scale * (Curves.Instance?.CloudCurveSize.Evaluate(normalizedLifeTime) ?? 1.0f);
		transform.localScale = Vector3.one * s;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Destroy the cloud if it hit a barrier.
        if (collision.gameObject.layer == Layers.SprayBarrier)
        {
            Destroy(gameObject);
            return; 
        }

        var zombifiable = collision.GetComponentInChildren<Zombifiable>() ?? collision.GetComponentInParent<Zombifiable>();
        if (zombifiable != null && zombifiable.gameObject != source)
        {
            if (AcceptedStates.Contains(zombifiable.CurrentState))
            {
                zombifiable.CurrentState = CausedState;
            }
			// Remove itself on collision with a person.
			Destroy(gameObject);
        }
    }
}
