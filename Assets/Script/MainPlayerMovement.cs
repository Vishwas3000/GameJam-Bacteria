using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerMovement : MonoBehaviour
{
    public static MainPlayerMovement MainPlayer;

    //public HealthBar healthBar;

    [Header("General Player Detail")]

    [SerializeField] private float verticalAcceleration = 5.0f;
    [SerializeField] private float horizontalAcceleration = 20.0f;
    [SerializeField] private float alphaRotation = 10.0f;
    [SerializeField] private float maxSpeed = 10.0f;
    [SerializeField] private float maxRotationSpeed = 10.0f;
    [SerializeField] private float velocityDrag = 1;
    [SerializeField] private float rotationDrag = 1;
    [SerializeField] private float rigid_body_angulerDrag = 2f;
    [SerializeField] private int playerMaxHealth = 100;

    public int playerScore =0 ;

    [Header ("Dash Info")]
    public bool multiDashing = false;
    public float multiDashTimeLimit = 1f;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashTimeLimit;
    [SerializeField] private float dashRegainTime;
    [SerializeField] private float damageOnDashing = 10.0f;
    [SerializeField] private float p_knockBackOnDashing = 2.0f;
    [SerializeField] private float knockOutTime = 0.5f;
    private TrailRenderer trail;


    [SerializeField]
    private GameObject dasheffect;

    [Header("Bullet Capacity")]
    public int bulletCapacity = 6;
    public int currentBullet = 0;
    public float bulletAutoRefillTime = 15f;
    public bool isBulletFull = false;


    private Rigidbody2D rb;
    private int playerCurrentHealth;

    private Vector3 velocity;
    private float zRotationVelocity;
    private Quaternion rotationValue;

    private float timeSince;
    private float timeStamp;

    public bool dashingNow = false;
    private float dashTimeStart = 0.0f;
    private bool canDash = true;
    private bool knockBack = false;

    [SerializeField] private GameObject onDamagedEffect;

    public void Awake()
    {
        MainPlayer = this;
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponentInChildren<TrailRenderer>();

        ParticleSystem ps = onDamagedEffect.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule psMain = ps.main;
        psMain.startColor = gameObject.GetComponentInChildren<SpriteRenderer>().color;

        Color color = gameObject.GetComponentInChildren<SpriteRenderer>().color;
        Debug.Log(color);
        Debug.Log(psMain.startColor);
        
    }

    public void Start()
    {
        playerCurrentHealth = playerMaxHealth;
        HealthBar.s_healthBar.SetMaxHealth(playerMaxHealth);

        timeSince = Time.timeSinceLevelLoad;
        
    }
    public void SetMovement()
    {
        float dashingTimeInterval;
        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (multiDashing)
            {
                Quaternion playerRot = transform.rotation;

                dashTimeStart = Time.time;
                dashingNow = true;
                velocity = dashSpeed * transform.up;

                CreateDashSmoke(playerRot);
                trail.enabled = true;
            }

            else
            {
                if (!dashingNow && canDash)
                {
                    Quaternion playerRot = transform.rotation ;

                    AudioManager.instance.Play("PlayerDashing");

                    dashTimeStart = Time.time;
                    dashingNow = true;
                    velocity = dashSpeed * transform.up;

                    CreateDashSmoke(playerRot);
                    trail.enabled = true;
                }

            }



        }

        dashingTimeInterval = Time.time - dashTimeStart;

        if (dashingTimeInterval > dashTimeLimit)
        {
            canDash = false;
            dashingNow = false;

            trail.enabled = false;
        }
        

        if (dashingTimeInterval >= dashRegainTime)
        {
            canDash = true;
        }

        if(multiDashing)
        {
            DashBar.s_dashBar.SetFill(1);
        }

        else
            DashBar.s_dashBar.SetFill(dashingTimeInterval / dashRegainTime);            //setting the dash fill

        //if (!dashingNow || !knockBack)
        {
            Vector3 acceleration = Input.GetAxis("Vertical") * verticalAcceleration * transform.up
                                    + Input.GetAxis("Horizontal") * horizontalAcceleration * transform.right;

            velocity += acceleration * Time.deltaTime;
        }                                                                                                    

    }

    void SetRotation()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0.0f;
        rotationValue = Quaternion.LookRotation(transform.forward,mousePos - transform.position);
        zRotationVelocity += alphaRotation * Time.deltaTime;

    }
    void Update()
    {
        SetMovement();
        SetRotation();

        timeStamp = Time.timeSinceLevelLoad - timeSince;

        if(timeStamp>bulletAutoRefillTime)                                          //Auto bullet over some time
        {
            UpdateBullet(1);

            timeSince = Time.timeSinceLevelLoad;
        }
    }

    private void FixedUpdate()
    {
        velocity = velocity * (1 - Time.deltaTime * velocityDrag);

        if (!dashingNow && !knockBack)
        {
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        }
        else
        {
            if(knockBack==true)
            {
                StartCoroutine(time());
            }
        }

        zRotationVelocity = zRotationVelocity * (1 - Time.deltaTime * rotationDrag);
        

        zRotationVelocity = Mathf.Clamp(zRotationVelocity, -maxRotationSpeed, maxRotationSpeed);

        rb.velocity = velocity ;
        rb.angularDrag = rigid_body_angulerDrag;

        transform.rotation = Quaternion.Lerp(transform.rotation, rotationValue, zRotationVelocity);
    }

    IEnumerator time()
    {
        yield return new WaitForSeconds(knockOutTime);
        knockBack = false;
    }


    public void CreateDashSmoke(Quaternion playerRotation)
    {
        GameObject createdParticle;
        createdParticle = Instantiate(dasheffect, transform.position, playerRotation);

        Destroy(createdParticle, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
        Vector3 enemydir = (transform.position - enemy.transform.position).normalized;

        if(dashingNow && enemy!=null)
        {
            enemy.OnTakeDamage((int)damageOnDashing, transform.up, p_knockBackOnDashing);
        }

        else
        {
            PlayerKnockBack(enemy.damagePoint, enemydir, enemy.e_knockBack);
        }
    }

    public void PlayerKnockBack(int damage, Vector3 direction, float knockBack_Amount)
    {
        Instantiate(onDamagedEffect, transform.position, transform.rotation);
        UpdateHealth(-damage);
        knockBack = true;
        AudioManager.instance.Play("PlayerGetDamaged");

        velocity += direction.normalized * knockBack_Amount;
    }


    public void UpdateHealth(int heal)
    {
        if(heal>0)
        {
            StartCoroutine(TriggerHealAnimation.instance.StartAnimationOnce());
        }

        playerCurrentHealth += heal;
        HealthBar.s_healthBar.SetHealth(playerCurrentHealth);

        if(playerCurrentHealth<=0)
        {
            GameManager.gameManager.EndGame();
        }

    }    

    public void UpdateBullet(int bulletPoint)
    {
        currentBullet += bulletPoint;
        if (currentBullet >= bulletCapacity)
        {
            currentBullet = bulletCapacity;
            isBulletFull = true;
        }
        else
            isBulletFull = false;

        BulletUI.instance.UpdateBullet(currentBullet, bulletCapacity);
    }

    public void UpdatePlayerScore(int score)
    {
        playerScore += score;
        ScoreDisplay.instance.UpdateScore(playerScore);
    }
}
