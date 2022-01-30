using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EnemyType
{
    Normal,
    Shooter
}

public class EnemyAI : MonoBehaviour
{
    public static List<Rigidbody2D> EnemyRbs;

    [Header("Enemy inputs")]
    public int health;
    public float speed = 20.0f;

    public int damagePoint = 5;
    public float e_knockBack = 5.0f;

    public float holdOnSpawned = 2f;
    [SerializeField] private bool isKnockBack = false;
    [SerializeField] private float knockOutTime = 0.5f;
    [SerializeField] private GameObject onDamaged;

    [SerializeField] private EnemyType enemyType;
    [SerializeField] private int enemyPoint;

    [Header("Collectables On Death")]
    public GameObject collectable;

    private float timeSince;
    private float timeSinceInstantiate;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private float alphaAcceleration,
                    velocityDrag = 1.0f, rotationDrag = 1.0f,
                    maxRotationSpeed = 10.0f,
                    repelRange = 5.0f, repelAmount = 20.0f;

    [SerializeField] private float maxDistanceFromPlayer = 50f;

    private Vector2 velocity;
    private Rigidbody2D rb;

    private Quaternion rotation;
    private float zRotationVelocity;

    [Header("Slider")]
    [SerializeField] private float sliderApprochDistance = 5f;
    [SerializeField] private float slideSpeed = 10f;

    public bool slide = false;


    private void Start()
    {
        timeSince = Time.timeSinceLevelLoad;

        rb = GetComponent<Rigidbody2D>();

        if (EnemyRbs == null)
            EnemyRbs = new List<Rigidbody2D>();

        EnemyRbs.Add(rb);
        velocity = Vector2.zero;

        ParticleSystem ps1 = onDamaged.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule ps1Main = ps1.main;
        ps1Main.startColor = gameObject.GetComponent<SpriteRenderer>().color;

        ParticleSystem ps2 = deathEffect.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule ps2Main = ps2.main;
        ps2Main.startColor = gameObject.GetComponent<SpriteRenderer>().color;

    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        timeSinceInstantiate = Time.timeSinceLevelLoad - timeSince;

        if(timeSinceInstantiate > holdOnSpawned)
        {
            //velocity = rb.velocity;
            EnemyMovement();

        }

        CheckDistance();


        if(timeSinceInstantiate > holdOnSpawned)
        {

            if(slide)
            {
                Vector3 tangentialDir = Vector3.Cross(MainPlayerMovement.MainPlayer.transform.position - gameObject.transform.position, new Vector3(0, 0, 1));
                tangentialDir = (Vector2)tangentialDir.normalized;
                velocity += slideSpeed * (Vector2)tangentialDir ;

                velocity = Vector3.ClampMagnitude(velocity, speed);


            }


            velocity = velocity * (1 - Time.deltaTime* velocityDrag);
            //velocity = Vector3.ClampMagnitude(velocity, speed);
            rb.velocity = velocity;

            zRotationVelocity = zRotationVelocity * (1 - Time.deltaTime * rotationDrag);
            zRotationVelocity = Mathf.Clamp(zRotationVelocity, -maxRotationSpeed, maxRotationSpeed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, zRotationVelocity);
        }
    }

    public void EnemyMovement()
    {


        Vector3 playerPos = MainPlayerMovement.MainPlayer.transform.position;
        Vector3 moveDir = playerPos - transform.position;


        switch (enemyType)
        {
            case EnemyType.Normal:

                if(!isKnockBack)
                {
                    velocity = speed * (Vector2)moveDir.normalized;
                }
                else
                    StartCoroutine( KnockBack());

                break;

            case EnemyType.Shooter:

                if((rb.position - (Vector2)MainPlayerMovement.MainPlayer.transform.position).magnitude <= sliderApprochDistance)
                {
                    slide = true;
                }

                else
                {
                    slide = false;

                    if (!isKnockBack)
                    {
                        velocity = speed * (Vector2)moveDir.normalized;
                    }
                    else
                        StartCoroutine(KnockBack());

                }

                break;

        }


        rotation = Quaternion.LookRotation(transform.forward, moveDir);
        zRotationVelocity += alphaAcceleration * Time.deltaTime;
        rb.angularDrag = rotationDrag;

        Vector2 repelForce = Vector2.zero;
        foreach(Rigidbody2D enemy in EnemyRbs)
        {
            if (enemy == rb)
                continue;
            if (Vector2.Distance(enemy.position, rb.position)<=repelRange)
            {
                Vector2 repelDir = (rb.position - enemy.position).normalized;
                repelForce += repelDir;
            }
        }

        velocity += repelForce * Time.fixedDeltaTime * repelAmount;


        if(!MainPlayerMovement.MainPlayer.isPlayerAlive)
        {
            velocity = Vector2.zero;
        }
    }

    IEnumerator KnockBack()
    {
        yield return new WaitForSeconds(knockOutTime);
        isKnockBack = false;

    }

    public void CheckDistance()
    {
        float distance= (rb.position - (Vector2)MainPlayerMovement.MainPlayer.transform.position).magnitude; 
        if(distance >= maxDistanceFromPlayer)
        {
            EnemySpawner.instance.UpdateEnemyPool(gameObject);
            EnemyRbs.Remove(rb);
            Destroy(gameObject);
        }
    }

    public void OnTakeDamage(int damage, Vector2 bulletDirection, float knockBack)
    {
        Instantiate(onDamaged, transform.position, transform.rotation);
        AudioManager.instance.Play("EnemyGettingDamage");

        bulletDirection = bulletDirection.normalized;
        isKnockBack = true;
        velocity += bulletDirection * knockBack;

        health -= damage;

        if (health <= 0)
        {
            Dead();
            MainPlayerMovement.MainPlayer.UpdatePlayerScore(enemyPoint);
        }
                
    }

    private void Dead()
    {

        if (deathEffect != null)
        {
            GameObject deathEffectObject;
            deathEffectObject = Instantiate(deathEffect, transform.position, Quaternion.identity);
        }


        EnemyManager.enemyManager.EnemyGotKiller();

        EnemySpawner.instance.EnemyKilled(gameObject);
        Instantiate(collectable, transform.position, Quaternion.identity);
        
        if(MainPlayerMovement.MainPlayer.dashingNow)
        {
            TimeManager.instance.DoSlowMotion();
        }

        EnemyRbs.Remove(rb);
        Destroy(gameObject);
    }

}







   

