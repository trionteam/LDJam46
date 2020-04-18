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
	private SpriteRenderer spriteRenderer;
	private float rot = 1;

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
		lifeLeft = LifeTime + Random.Range(-1.0f, 1.0f) * LifeLengthRandomness;
		spriteRenderer = GetComponent<SpriteRenderer>();

		// add a bit of variation
		float H, S, V;
		Color.RGBToHSV(spriteRenderer.color, out H, out S, out V);
		H += Random.Range(-0.1f, 0.1f);
		spriteRenderer.color = Color.HSVToRGB(H, S, V);

		transform.localScale *= Random.Range(0.8f, 1.2f);
		transform.localRotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
	}

    // Update is called once per frame
    void Update()
    {
		transform.position += velocity * Time.deltaTime;
		transform.Rotate(Vector3.forward, rot * Time.deltaTime);

		Color c = spriteRenderer.color;
		lifeLeft -= Time.deltaTime;
		if (lifeLeft <= 0)
		{
			c.a = (FadeLastSeconds + lifeLeft) / FadeLastSeconds;
			if (lifeLeft <= -FadeLastSeconds)
			{
				Destroy(gameObject);
				return;
			}
		}
		else
		{
			c.a = Mathf.Clamp(c.a + Time.deltaTime * FadeLastSeconds, 0, 1);
		}
		spriteRenderer.color = c;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
