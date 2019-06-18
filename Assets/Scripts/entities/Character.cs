using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("movement")]
    [Range(1, 20f)]
    [SerializeField]
    private float walkSpeed = 5f;
    [Range(5, 30f)]
    [SerializeField]
    private float jumpForce = 10f;
    [Range(1, 10)]
    [SerializeField]
    private float gravityScale = 1f;

    [Space]
    [Header("hook mechanic")]
    [Range(10, 100)]
    [SerializeField]
    private float rayCastDistance = 20f;
    [Range(1, 50)]
    [SerializeField]
    private float launchSpeed = 10f;
    [SerializeField]
    private Transform hookTransform;

    [Space]
    [Header("other objects")]
    [SerializeField]
    private Transform aimPivot;
    [SerializeField]
    private Transform castPoint;
    [SerializeField]
    private Transform hitPoint;

    private Collision2D colobject;

    private bool launching = false;
    private bool dead = false;
    private bool fromLaunch = false;

    //overige
    private Rigidbody2D rb;
    private LineRenderer lr;
    private bool canClimb = false;
    private bool inAir = true;

    private Vector3 spawnPos;

    private CameraFade cameraFade;
    private CameraShake cameraShake;

    void Start()
    {
        cameraFade = Transform.FindObjectOfType<CameraFade>();
        cameraShake = cameraFade.GetComponent<CameraShake>();

        //declaring the rigidbody
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        spawnPos = transform.position;

        DisPatchHook();

    }

    void Update()
    {
        if (Time.timeScale == 0) { return; }

        if (Input.GetButtonUp("Fire1"))
        {
            DisPatchHook();
        }

        //if it launches forward, ignore all other input / should probably be implemented inside the functions.
        if (launching || dead) { return; }


        //inputs, need to be from a seperate input handeler.
        int h_input = 0;
        if (Input.GetKey(KeyCode.A))
        {
            h_input = -1;
        } else if(Input.GetKey(KeyCode.D))
        {
            h_input = 1;
        }
        Walking(h_input);
        //Walking(Input.GetAxis("Horizontal"));


        //jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelJump();
        }
        RotateToMouse();
        CastingRay();
        if (Input.GetButtonDown("Fire1"))
        {
            LaungeToHook();
        }

        //pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.Pause(true);
        }
    }

    //when collision stays
    void OnCollisionStay2D(Collision2D collision)
    {
        //if its a wall
        if (collision.gameObject.tag == "wall")
        {

            canClimb = true;
            //there is no gravity
            rb.gravityScale = 0;

            //this prevents launching upwards from direct below.
            if (fromLaunch)
            {
                CancelJump(false);
            }
        }

        //needs to be aplied later not earlier.
        fromLaunch = false;
        inAir = false;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "wall")
        {
            CancelJump(false);
        } else if (collision.gameObject.tag == "spike")
        {
            Death();
        } 
        colobject = collision;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        canClimb = false;
        rb.gravityScale = gravityScale;
        inAir = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "checkpoint")
        {
            spawnPos = collision.transform.position;
        }
    }

    /// <summary>
    /// Moves the rigidbody on a x-plane if it isntf from a launch
    /// </summary>
    /// <param name="h_input"></param>
    public void Walking(float h_input)
    {
        //rb.x gets updated.
        Vector3 tempRb = rb.velocity;
        float friction = inAir ? 0.4f : .8f;
        tempRb.x += h_input * ( 2* friction);

        if (!fromLaunch)
        {
            tempRb.x = Mathf.Min(Mathf.Max(tempRb.x, -walkSpeed), walkSpeed);
        }

        if (h_input == 0)
        {
            if (tempRb.x > 0)
            {
                tempRb.x -= friction;
            }
            else
            {
                tempRb.x += friction;
            }
                
        }

        if (Mathf.Abs(tempRb.x) < friction * 2)
        {
            fromLaunch = false;
            tempRb.x = 0;
        }
        rb.velocity = tempRb;
    }

    /// <summary>
    /// Launches the rigidbody upward if it isn't in the air.
    /// </summary>
    public void Jump()
    {
        if (inAir) { return; }

        Vector3 tempRb = rb.velocity;
        tempRb.y = jumpForce;
        if (canClimb)
        {
            tempRb.x = walkSpeed;

            if (colobject.transform.position.x > transform.position.x)
            {
                tempRb.x *= -1;
            } 
        }
        rb.velocity = tempRb;
    }

    /// <summary>
    /// Cancels the upwards jump (or even below) force.
    /// Handy for more precise controls
    /// </summary>
    /// <param name="OnlyWhenUp"></param>
    public void CancelJump(bool OnlyWhenUp = true)
    {
        if (rb.velocity.y > 0 || !OnlyWhenUp)
        {
            Vector3 tempRb = rb.velocity;
            tempRb.y = 0;
            rb.velocity = tempRb;
        }
    }

    /// <summary>
    /// Rotates the aimpivot towards the camera.
    /// </summary>
    public void RotateToMouse()
    {
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        aimPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    /// <summary>
    /// Casts a ray.
    /// </summary>
    public void CastingRay()
    {
        //casts a ray from the castpoint position.
        RaycastHit2D hit = Physics2D.Raycast(castPoint.position, castPoint.right, rayCastDistance);

        //probably needs a tweak that it doesnt get spikes or other things.
        if (hit.collider != null)
        {
            hitPoint.gameObject.SetActive(true);
            hitPoint.position = hit.point;
        } else
        {
            hitPoint.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Launches the actual hook to pull the character
    /// </summary>
    public void LaungeToHook()
    {
        //if it already launches and the hitobject is active, meaning there is a point to go to.
        if (launching || !hitPoint.gameObject.activeSelf) { return; }

        launching = true;
        ParticleManager.instance.SpawnParticle(ParticleManager.instance.landImpactParticle, hitPoint.position, hitPoint.rotation);
        StartCoroutine(LaunchingToHook());
    }
    /// <summary>
    /// CHanges the force of the rigidbody to move towards the hitpoint location. 
    /// Until then launching stays true.
    /// </summary>
    /// <returns></returns>
    IEnumerator LaunchingToHook()
    {
        lr.enabled = true;
        while (Vector3.Distance(hitPoint.position, castPoint.position) > .5f && launching)
        {
            hookTransform.position = hitPoint.position - castPoint.position.normalized * 0.2f;

            lr.SetPosition(0, castPoint.position);
            lr.SetPosition(1, hitPoint.position);

            rb.gravityScale = 0;
            rb.velocity = Vector3.Normalize(hitPoint.position - transform.position) * launchSpeed;
            yield return new WaitForFixedUpdate();
        }
        DisPatchHook();
    }
    private void DisPatchHook()
    {
        StopCoroutine(LaunchingToHook());

        hookTransform.position = castPoint.position;

        lr.enabled = false;

        launching = false;
        fromLaunch = true;
        rb.gravityScale = gravityScale;
    }
    public void Death()
    {
        ParticleManager.instance.SpawnParticle(ParticleManager.instance.deathParticle, transform.position, transform.rotation);

        dead = true;
        GameManager.instance.RespawnPlayerAfterTime(0.5f);
        gameObject.SetActive(false);

        //StartCoroutine(Respawning());
    }
    //public IEnumerator Respawning()
    //{
    //    yield return new WaitForSeconds(0.3f);
    //    Respawn();
    //}
    public void Respawn()
    {
        dead = false;

        this.transform.position = spawnPos;
        cameraFade.fadingOut = false;

    }
}
