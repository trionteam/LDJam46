using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Sprite pressedSprite;
    public Sprite depressedSprite;

    public bool IsPressed
    {
        get; private set;
    }

    private Collider2D plateCollider;
    new private SpriteRenderer renderer;

    private void Awake()
    {
        plateCollider = GetComponent<Collider2D>();
        Debug.Assert(plateCollider != null);

        renderer = GetComponent<SpriteRenderer>();
        Debug.Assert(renderer != null);

        Debug.Assert(pressedSprite != null);
        Debug.Assert(depressedSprite != null);
    }

    private void Start()
    {
        IsPressed = false;
    }

    void Update()
    {
        var candidates = new Collider2D[1];
        var filter = new ContactFilter2D();
        // We're looking for contacts only in the same collision layer as the pressure
        // plate (which is by default the ground collisions layer). Any colliders in this
        // layer are assumed to be moving on the ground, and should thus be able to
        // press the plate.
        filter.SetLayerMask(1 << gameObject.layer);
        var numCandidates = plateCollider.OverlapCollider(filter, candidates);
        IsPressed = numCandidates > 0;
        renderer.sprite = IsPressed ? pressedSprite : depressedSprite;
    }
}
