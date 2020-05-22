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
        IsPressed = false;
        var candidates = new Collider2D[64];
        var filter = new ContactFilter2D();
        filter.SetLayerMask(1 << gameObject.layer);
        var numCandidates = plateCollider.OverlapCollider(filter, candidates);
        for (int i = 0; i < numCandidates; ++i)
        {
            // TODO(ondrasej): We need a better component for characters that can press the plate.
            var candidate = candidates[i].GetComponentInParent<ZombieController>();
            if (candidate != null)
            {
                IsPressed = true;
                break;
            }
        }
        renderer.sprite = IsPressed ? pressedSprite : depressedSprite;
    }
}
