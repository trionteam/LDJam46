using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public SpriteRenderer sprite;

    private float previousX;

    private void Awake()
    {
        if (rigidBody == null)
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }
        Debug.Assert(rigidBody != null);

        if (sprite == null)
        {
            sprite = GetComponent<SpriteRenderer>();
        }
        Debug.Assert(sprite != null);
    }

    private void Start()
    {
        previousX = transform.position.x;
    }

    void Update()
    {
        var delta = transform.position.x - previousX;
        if (delta < 0.0f)
        {
            sprite.flipX = false;
        }
        else if (delta > 0.0f)
        {
            sprite.flipX = true;
        }
        previousX = transform.position.x;
    }
}
