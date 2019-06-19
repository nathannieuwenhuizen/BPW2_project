using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    private ParticleSystem ps;
    private SpriteRenderer sr;
    private bool check;

    [SerializeField]
    private Sprite checkedSprite;
    [SerializeField]
    private Sprite uncheckedSprite;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        sr = GetComponent<SpriteRenderer>();
    }

    public bool Check
    {
        get { return check; }
        set {
            check = value;
            sr.sprite = check ? checkedSprite : uncheckedSprite;
            if (check)
            {
                ps.Play();
            }
        }
    }
}
