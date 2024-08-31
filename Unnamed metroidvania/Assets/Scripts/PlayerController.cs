using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[SelectionBase]

public class PlayerController : MonoBehaviour
{
    [Header("Upgrades")]
    public static bool canDash = false;
    public static bool canWallJump = false;
    //public static bool canDoubleJump = false;
    public static bool canParaglide = false;
    public static bool canGrapple = false;

    [Header("Camera")]
    public Camera camera;

    [Header("Movement")]
    public float speed;
    public float jumpForce;
    public float wallJumpForceHorizontal;
    public float wallJumpForceVertical;
    public float slideSpeed;
    public bool isGrounded;
    public Vector3 lastGroundedPosition;
    public Vector3 secondLastGroundedPosition;
    public float sampleFrequency;
    public float sampleTimer;
    public IEnumerator returnCoroutine;
    public float iHateSlopes;
    public float airTime;
    public float maxAirTime;

    [Header("Wall jump")]
    public bool hasWallRight;
    public bool hasWallLeft;
    public float smoothDampTime;

    [Header("Dash")]
    public float dashStrength;
    public float dashTimer;
    public float dashCooldown;
    public float dashDuration;
    public bool isDashing;

    [Header("Damage")]
    public int maxHealth;
    public int currentHealth;
    public float invincibilityTimer;
    public float invincibilityDuration;

    [Header("Energy")]
    public float maxEnergy;
    public float currentEnergy;
    public float energyChargeSpeed;
    public float focusTimer;
    public float focusDelay;

    [Header("Spells")]
    public Transform fireballSpawn;
    public static List<SpellData> eruditeSpells = new List<SpellData>();
    public float spellCooldownTimer;

    [Header("Force")]
    public Vector2 phonyForce; //NEVER USE phonyForce.y IT IS STRICTLY PROHIBITED IF YOU USE THIS YOU ARE A TERRIBLE PERSON AND YOU DESERVE TO BE LAUNCHED INTO THE DEAD OF SPACE NEVER TO BE SEEN OR HEARD AGAIN BY ANYONE AND YOU WILL DIE ALONE KNOWING YOUR MISTAKES
    protected Vector2 forceReduction;
    public float buggyProblematicKnockbackMultiplier;

    [Header("Jump")]
    public static int jumpMax = 1;
    public int jumpCurrent;

    [Header("Death")]
    public float deathBarrier;

    [Header("Combat")]
    public GameObject meleeThingy;
    public float meleeDuration;
    public float meleeCooldown;
    public float meleeTimer;
    public int meleeDamage;
    public bool isSlashing;
    public int combo;

    [Header("Healing")]
    public int maxFlasks;
    public int flasks;
    public int killstreak;
    public int killQuota;
    public float flaskUseTime;
    public float healFraction;
    public float healTimer;

    [Header("Paraglider")]
    public float paraglideFallSpeed;

    [Header("Grapple")]
    public LineRenderer lineRenderer;
    public DistanceJoint2D distanceJoint;
    public GrappleObject grappleObject;
    public float maxGrappleDistance;
    public bool isGrappling;
    public float grappleSpeed;
    public float grappleFallSpeed;
    public float grappleSpringConstant;

    [Header("Filters")]
    public ContactFilter2D groundFilter;
    public ContactFilter2D wallFilterRight;
    public ContactFilter2D wallFilterLeft;
    //public LayerMask layerMask;

    [Header("Checkpoints")]
    public Vector3 lastSafePoint;
    //CHANGE BACK TO GAMEOBJECT LATER
    public Vector3 lastBench;

    [Header("Economy")]
    public int wealth;

    [Header("Dialogue")]
    [SerializeField] private DialogueController dialogueController;
    public DialogueController DialogueController => dialogueController;
    public NPCController NPC { get; set; }

    [Header("Animation")]
    public bool isMoving;
    public bool isJumping;
    public bool isFalling;

    [Header("Misc")]
    public Rigidbody2D rigidbody;
    public int level_number;
    public Manager manager;
    public float waterSlowdownFactor;
    public bool wet;

    private void Awake()
    {
        //currentHealth = maxHealth;
        //currentEnergy = 0;
        manager = FindObjectOfType<Manager>();
        //transform.position = manager.coords;
        if (manager.transitionedByDying)
        {
            transform.position = manager.spawnPoint;
        }
        else
        {
            transform.position = manager.coords;
            Debug.Log(manager.coords);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        meleeThingy.SetActive(false);
        distanceJoint.enabled = false;

        //lastSafePoint = transform.position;

        //SaveData.LoadAllData();
        //SaveData.ReadPlayerData(this);

        manager = FindObjectOfType<Manager>();
        level_number = SceneManager.GetActiveScene().buildIndex;

        //manager.health = currentHealth;
        //manager.energy = currentEnergy;
        camera.transform.SetParent(null);

        if(manager.health == 0)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth = manager.health;
        }
        currentEnergy = manager.energy;

        //PLACEHOLDER CODE DELET LATER
        //lastBench = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        manager = FindObjectOfType<Manager>();

        //camera.transform.position = new Vector3(transform.position.x, transform.position.y + 1, camera.transform.position.z);

        bool wasGrounded = isGrounded;
        bool hadWallRight = hasWallRight;
        bool hadWallLeft = hasWallLeft;
        isGrounded = rigidbody.IsTouching(groundFilter);
        hasWallRight = rigidbody.IsTouching(wallFilterRight);
        hasWallLeft = rigidbody.IsTouching(wallFilterLeft);

        if (Time.timeScale <= 0 || dialogueController.isTalking)
        {
            return;
        }

        if (isGrounded)
        {
            if (sampleTimer >= sampleFrequency)
            {
                secondLastGroundedPosition = lastGroundedPosition;
                lastGroundedPosition = transform.position;
                sampleTimer = 0;
            }
            sampleTimer += Time.deltaTime;
        }

        if (isGrounded || hasWallLeft || hasWallRight)
        {
            jumpCurrent = jumpMax;
            airTime = 0;
        }
        else if (wasGrounded || hadWallLeft || hadWallRight)
        {
            jumpCurrent = jumpMax - 1;
            airTime += Time.deltaTime;
        }

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), 0);
        rigidbody.velocity = input * speed + Vector2.up * rigidbody.velocity.y;
        if (input.x == 0)
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            isMoving = false;
        }

        if(input.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isMoving = true;
        }
        else if(input.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isMoving = true;
        }

        if (jumpCurrent > 0 && Input.GetKeyDown(KeyCode.Space) && !((hasWallLeft || hasWallRight) && !isGrounded))
        {
            //rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpForce, 0);
            //isGrounded = false;
            jumpCurrent--;
        }
        else if (hasWallRight && Input.GetKeyDown(KeyCode.Space) && canWallJump)
        {
            phonyForce = Vector2.left * wallJumpForceHorizontal;
            //rigidbody.AddForce(Vector2.up * wallJumpForceVertical, ForceMode2D.Impulse);
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpForce, 0);
            /*phonyForce = new Vector2(-1, 0.1f).normalized * wallJumpForce;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);*/
        }
        else if (hasWallLeft && Input.GetKeyDown(KeyCode.Space) && canWallJump)
        {
            phonyForce = Vector2.right * wallJumpForceHorizontal;
            //rigidbody.AddForce(Vector2.up * wallJumpForceVertical, ForceMode2D.Impulse);
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpForce, 0);
        }
        else if((hasWallLeft || hasWallRight) && canWallJump)
        {
            if(rigidbody.velocity.y <= slideSpeed)
            {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, slideSpeed, 0);
            }
        }

        if (isDashing)
        {
            //rigidbody.velocity += input * dashStrength;

            //Experimental code
            //ok this code actually works nvm guess it's not experimental now
            rigidbody.velocity = new Vector3(dashStrength * transform.localScale.x, 0, 0);

            if (dashTimer >= dashDuration)
            {
                isDashing = false;
                dashTimer = 0;
            }
        }

        else if (Input.GetKeyDown(KeyCode.C) && dashTimer >= dashCooldown && canDash)
        {
            //rigidbody.AddForce(Vector2.right * dashStrength, ForceMode2D.Impulse);
            isDashing = true;
            dashTimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            focusTimer = 0;
        }

        if (Input.GetKey(KeyCode.F) && currentEnergy <= maxEnergy && isGrounded)
        {
            rigidbody.velocity = Vector3.zero;
            focusTimer += Time.deltaTime;

            if(focusTimer >= focusDelay)
            {
                currentEnergy += energyChargeSpeed * Time.deltaTime;
            }
        }

        dashTimer += Time.deltaTime;

        if (isSlashing)
        {
            rigidbody.velocity = Vector2.zero;
            if (meleeTimer >= meleeDuration)
            {

                isSlashing = false;
                meleeTimer = 0;
                meleeThingy.SetActive(false);
                if(combo > 2)
                {
                    combo = 0;
                }
            }
        }

        else if (Input.GetKeyDown(KeyCode.Mouse0) && (meleeTimer >= meleeCooldown || combo <= 2))
        {
            combo++;
            //rigidbody.AddForce(Vector2.right * dashStrength, ForceMode2D.Impulse);
            isSlashing = true;
            meleeTimer = 0;
            meleeThingy.SetActive(true);

            if(combo > 2)
            {
                combo = 0;
            }
            else
            {
                //combo++;
            }
        }

        else if(meleeTimer > meleeDuration)
        {
            combo = 0;
        }

        if (Input.GetKeyDown(KeyCode.Z) && eruditeSpells.Count > 0)
        {
            if(currentEnergy >= eruditeSpells[0].energyCost && spellCooldownTimer >= eruditeSpells[0].cooldown)
            {
                GameObject projectile = Instantiate(eruditeSpells[0].spellPrefab, fireballSpawn.position, fireballSpawn.rotation);
                SpellProjectile spellProjectile = projectile.GetComponent<SpellProjectile>();
                spellProjectile.spellData = eruditeSpells[0];
                spellProjectile.rigidbody.velocity = new Vector2(transform.localScale.x * eruditeSpells[0].speed, 0);
                currentEnergy -= eruditeSpells[0].energyCost;
                spellCooldownTimer = 0;
            }
        }

        if (Input.GetKey(KeyCode.V) && !isGrounded && canParaglide)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, -paraglideFallSpeed);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            NPC?.Interact(this);
        }

        Vector2 mousePos = (Vector2)camera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.R) && canGrapple && grappleObject.canGrapple && CheckRaycast(mousePos))
        {
            lineRenderer.SetPosition(0, mousePos);
            lineRenderer.SetPosition(1, transform.position);
            distanceJoint.connectedAnchor = mousePos;

            distanceJoint.enabled = true;
            lineRenderer.enabled = true;
            isGrappling = true;
        }
        else if (Input.GetKeyUp(KeyCode.R) && isGrappling)
        {
            distanceJoint.enabled = false;
            lineRenderer.enabled = false;
            isGrappling = false;

            rigidbody.AddForce(grappleSpringConstant * Vector2.up, ForceMode2D.Impulse);
        }
        if (distanceJoint.enabled)
        {
            lineRenderer.SetPosition(1, transform.position);
        }

        if (isGrappling)
        {
            float xdir = Input.GetAxisRaw("Horizontal");
            Vector2 movedir = Vector2.right;
            rigidbody.AddForce(xdir * movedir * grappleSpeed, ForceMode2D.Force);

            rigidbody.AddForce(grappleFallSpeed * Vector2.down);
        }

        if (Input.GetKeyUp(KeyCode.J))
        {
            StopCoroutine("HealTimer");
            healTimer = 0;
        }

        if (flasks > 0)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                StartCoroutine("HealTimer");
            }

            if(healTimer >= flaskUseTime)
            {
                currentHealth += Mathf.RoundToInt(maxHealth * healFraction);
                flasks--;
                healTimer = 0;
            }
        }

        if(killstreak >= killQuota)
        {
            flasks++;
            killstreak = 0;
        }

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if(currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
        if(flasks > maxFlasks)
        {
            flasks = maxFlasks;
        }

        meleeTimer += Time.deltaTime;
        invincibilityTimer += Time.deltaTime;
        spellCooldownTimer += Time.deltaTime;

        // Death barrier
        if (transform.position.y < deathBarrier)
        {
            if(returnCoroutine == null)
            {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                EngageReturnToSafeGround();
                TakeDamage(10, Vector2.zero);
            }
        }

        if(isGrounded && rigidbody.velocity.y != 0)
        {
            rigidbody.gravityScale = 0;
        }
        else
        {
            rigidbody.gravityScale = 1;
        }

        if(!isGrounded && rigidbody.velocity.y > 0)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }

        if(!isGrounded && rigidbody.velocity.y < 0)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }

        //isJumping = !isGrounded && rigidbody.velocity.y > 0;

        rigidbody.velocity += new Vector2(phonyForce.x, 0);
        //transform.position += new Vector3(0, phonyForce.y * Time.deltaTime, 0);
        //phonyForce = Vector2.SmoothDamp(phonyForce, Vector2.zero, ref forceReduction, smoothDampTime);
        phonyForce.x = Mathf.SmoothDamp(phonyForce.x, 0, ref forceReduction.x, smoothDampTime);
        //phonyForce.y = Mathf.SmoothDamp(phonyForce.y, 0, ref forceReduction.x, smoothDampTime);

        // Debug self-destruct
        if(Input.GetKey(KeyCode.Semicolon) && Input.GetKey(KeyCode.BackQuote)) // unity hates tilde
        {
            currentHealth = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //collision.contacts[0].normal.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "h2o")
        {
            speed *= waterSlowdownFactor;
            wet = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "h2o")
        {
            speed /= waterSlowdownFactor;
            wet = false;
        }
    }

    public void TakeDamage(int damage, Vector2 distance)
    {
        if(invincibilityTimer < invincibilityDuration)
        {
            return;
        }
        currentHealth -= damage;
        phonyForce.x += distance.x;
        rigidbody.velocity = new Vector2(0, distance.y * buggyProblematicKnockbackMultiplier);
        invincibilityTimer = 0;

        // dead of death
        if(currentHealth <= 0)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            // below line is placeholder
            // replace this with go back to save function when I make it
            //EngageReturnToSafeGround();
            // ^ no longer placeholder code :)

            //transform.position = lastBench;
            //currentHealth = maxHealth;
            // ^ this is placeholder code again
            manager.transitionedByDying = true;
            manager.PlayerDeath();

        }
    }

    public void EngageReturnToSafeGround()
    {
        if(returnCoroutine != null)
        {
            return;
        }
        returnCoroutine = ReturnToSafeGround();
        StartCoroutine(returnCoroutine);
    }

    IEnumerator ReturnToSafeGround()
    {
        //yield return new WaitForSeconds(0.3f);
        yield return FindObjectOfType<BlackRectangle>().Fade();

        rigidbody.velocity = Vector2.zero;
        //transform.position = secondLastGroundedPosition;
        transform.position = lastSafePoint;
        returnCoroutine = null;

        yield return FindObjectOfType<BlackRectangle>().Unfade();
    }

    public void UpdateSafePoint(Vector3 newSafePoint)
    {
        lastSafePoint = newSafePoint;
    }

    public bool CheckRaycast(Vector3 mousePos)
    {
        Vector3 dir = Vector3.MoveTowards(transform.position, mousePos, 1) - transform.position;
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position + dir * 3, dir, maxGrappleDistance);
        if (raycastHit.collider)
        {
            if (raycastHit.transform.gameObject == grappleObject.gameObject)
            {
                return true;
            }
        }
        return false;
    }

    public IEnumerator HealTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            healTimer += 1;
        }
    }
}
