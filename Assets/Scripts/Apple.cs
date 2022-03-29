using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    [SerializeField] private AppearableObjectData appleData;
    [SerializeField] private ParticleSystem objectParticle;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Knive"))
        {
            spriteRenderer.enabled = false;
            boxCollider.enabled = false;

            gameObject.transform.parent = null;

            GameManager.Instance.AddAppleToACount();
            objectParticle.Play();
            Destroy(gameObject, objectParticle.main.duration);
        }

        if (collision.CompareTag("StuckKnive"))
        {
            Destroy(gameObject);
        }
    }

    public void FreeApple()
    {
        rb.isKinematic = false;
        GetComponent<BoxCollider2D>().enabled = false;
        rb.gravityScale = 1;
    }

}

