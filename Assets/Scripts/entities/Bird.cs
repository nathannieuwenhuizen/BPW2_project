using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public enum BirdState { Idle, Panic }
    public BirdState state;

    [SerializeField]
    private float panicRange = 5;
    [Range(0.01f, 0.5f)]
    [SerializeField]
    private float speed = .3f;
    private Transform player;
    private Vector3 panicDirection;

    private SpriteRenderer sprite;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        state = BirdState.Idle;
        player = Transform.FindObjectOfType<Character>().transform;
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        float randomIdleStart = Random.Range(0, anim.GetCurrentAnimatorStateInfo(0).length); //Set a random part of the animation to start from
        anim.Play("Idle", 0, randomIdleStart);
    }

    // Update is called once per frame
    void Update()
    {
        ExecuteState();
    }

    private void ExecuteState()
    {
        switch (state)
        {
            case BirdState.Panic:
                PanicState();
                break;
            case BirdState.Idle:
                IdleState();
                break;
        }
    }

    private bool CheckPlayerInRange(float range)
    {
        return Vector3.Distance(player.transform.position, transform.position) < range;
    }

    private void IdleState()
    {
        if (CheckPlayerInRange(panicRange))
        {
            panicDirection = transform.position - player.position;
            panicDirection.y = Mathf.Abs(panicDirection.y) + Random.value * 5f;
            panicDirection = panicDirection.normalized * speed;

            StartCoroutine(WaitForDisapear());
            state = BirdState.Panic;
            anim.SetBool("flying", true);
        }
    }
    private void PanicState()
    {
        transform.Translate(panicDirection);
        //if (!CheckPlayerInRange(panicRange * 1.5f))
        //{
        //    SwitchState(BirdState.Idle);
        //}
    }
    private IEnumerator WaitForDisapear ()
    {
        yield return new WaitForSeconds(1f);
        while (sprite.color.a != 0)
        {
            sprite.color = new Color(1f, 1f, 1f, sprite.color.a - 0.01f);
            yield return new WaitForSeconds(Time.deltaTime);

        }
        Disappear();
    }
    private void Disappear()
    {
        this.gameObject.SetActive(false);
    }

    public void SwitchState(BirdState newState)
    {

        state = newState;
    }
    private void OnDrawGizmos()
    {
        //UnityEditor.Handles.color = Color.yellow;
        //UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, panicRange);
    }
}
