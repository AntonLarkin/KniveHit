using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knive : MonoBehaviour
{
    public static Sprite equippedSkin;

    
    [SerializeField] private float speed;
    [SerializeField] private ParticleSystem hitParticleEffect;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vibration.Init();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Knive"))
        {
            Vibration.VibratePop();
            Destroy(gameObject);
            GameManager.Instance.GameOver();
            return;
        }

        KniveHit(collision);

        LevelManager.Instance.ResetActiveKnive();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("StuckKnive"))
        {
            Vibration.VibratePop();
            Destroy(gameObject);
            GameManager.Instance.GameOver();
            return;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.IsGameLaunched)
        {
            ThrowKnife();
        }
       
    }

    public void FreeKnives()
    {
        rb.isKinematic = false;
        GetComponent<BoxCollider2D>().enabled = false;
        rb.gravityScale = 1;
    }

    public void EquipSkin(SkinInfo skinInfo)
    {
        equippedSkin = skinInfo.SkinSprite;
        gameObject.GetComponentInChildren<SpriteRenderer>().sprite = equippedSkin;
    }

    private void ThrowKnife()
    {
        rb.AddForce(new Vector2(0, speed), ForceMode2D.Impulse);
    }

    private void KniveHit(Collision2D collision)
    {
        transform.SetParent(collision.transform);
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        hitParticleEffect.Play();
        Vibration.VibratePop();
        UIManager.Instance.HideUsedKnive();
        GameManager.Instance.AddKniveToACount();
    }

}
