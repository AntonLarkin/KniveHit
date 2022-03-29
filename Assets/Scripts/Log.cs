using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float speedRotation;
    [SerializeField] private float minTimeOfRotation;
    [SerializeField] private float maxTimeOfRotation;
    [SerializeField] private float durationOfChangingSpeed;
    
    [SerializeField] private Sprite[] logStages;
    [SerializeField] private ParticleSystem explosionParticleEffect;

    private float currentSpeed;
    private bool isGoingLeft;
    private bool isGoingRight;
    private bool isSpeeding;
    private bool isSlowing;
    private bool isExhist;
    private SpriteRenderer spriteRenderer;
    private int currentStage;

    private void Awake()
    {
        currentStage = 0;
        isExhist = true;
        currentSpeed = speedRotation;
    }

    private void OnEnable()
    {
        LevelManager.Instance.OnLogDestroyed += DestroyLog;
    }

    private void OnDisable()
    {
        LevelManager.Instance.OnLogDestroyed -= DestroyLog;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Vibration.Init();
        StartCoroutine(OnChangeDirection());
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Knive"))
        {
            ChangeStage();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Knive")|| collision.gameObject.CompareTag("StuckKnive"))
        {
            collision.gameObject.GetComponent<Knive>().FreeKnives();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Apple"))
        {
            collision.gameObject.GetComponent<Apple>().FreeApple();
        }
        if (collision.gameObject.CompareTag("StuckKnive"))
        {
            collision.gameObject.GetComponent<Knive>().FreeKnives();
        }
    }

    private void Update()
    {
        if (isExhist && GameManager.Instance.IsGameLaunched)
        {
            if (isSlowing)
            {
                SlowLog();
                RotateLog(currentSpeed);
                return;
            }
            if (isSpeeding)
            {
                BoostLog();
                RotateLog(currentSpeed);
                return;
            }
            spriteRenderer.sprite = logStages[currentStage];
            RotateLog(speedRotation);
        }
        if (isExhist && !GameManager.Instance.IsGameLaunched)
        {
            DestroyLog();
        }
        
    }

    public void ChangeStage()
    {
        if (currentStage < logStages.Length)
        {
            currentStage++;
        }
    }

    private void RotateLog(float direction)
    {
        transform.Rotate(0, 0, direction*Time.deltaTime, Space.World);
    }

    private void DestroyLog()
    {
        if (!GameManager.Instance.IsGameLaunched)
        {
            Destroy(gameObject);
            return;
        }

        isExhist = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        explosionParticleEffect.Play();
        GetComponent<Rigidbody2D>().isKinematic = false;

        Vibration.Vibrate();

        Destroy(gameObject, explosionParticleEffect.main.duration);
    }

    private void ChangeDirection()
    {
        speedRotation = -speedRotation;
        StartCoroutine(OnChangeDirection());
    }

    private void SlowLog()
    {
        if (currentSpeed < -5)
        {
            currentSpeed += durationOfChangingSpeed;
            return;
        }
        else if (currentSpeed > 5)
        {
            currentSpeed -= durationOfChangingSpeed;
            return;
        }
        if (currentSpeed >0 || currentSpeed <0)
        {
            isSlowing = false;
            isSpeeding = true;
            if (currentSpeed > 0)
            {
                isGoingLeft = true;
            }
            else
            {
                isGoingRight = true;
            }
        }
    }

    private void BoostLog()
    {
        if (isGoingRight && Mathf.Abs(currentSpeed) < Mathf.Abs(speedRotation))
        {
            currentSpeed += durationOfChangingSpeed;
            return;
        }
        else if (isGoingLeft && Mathf.Abs(currentSpeed) <  Mathf.Abs(speedRotation))
        {
            currentSpeed -= durationOfChangingSpeed;
            return;
        }

        isSpeeding = false;
        ChangeDirection();
        isGoingLeft = false;
        isGoingRight = false;

    }

    private IEnumerator OnChangeDirection()
    {
        yield return new WaitForSeconds(Random.Range(minTimeOfRotation, maxTimeOfRotation));

        isSlowing = true;
    }
}
