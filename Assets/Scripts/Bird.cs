using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public event EventHandler onDied;
    public event EventHandler onStartPlaying;
    private static Bird instance;
    private State state;

    private enum State
    {
        WaitingToStart,
        Playing,
        Dead,
    }
    public static Bird GetInstance()
    {
        return instance;
    }
    private const float JUMP_AMOUNT = 10f;
    private Rigidbody2D birdRigidbody2d;
    private void Awake()
    {
        instance = this;
        birdRigidbody2d = GetComponent<Rigidbody2D>();
        birdRigidbody2d.bodyType = RigidbodyType2D.Static;
        state = State.WaitingToStart;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    state = State.Playing;
                    birdRigidbody2d.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    if (onStartPlaying != null) onStartPlaying(this, EventArgs.Empty);
                }
                break;
            case State.Playing:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    Jump();
                }
                break;
            case State.Dead:
                break;
        }
        
    }
    private void Jump()
    {
        birdRigidbody2d.velocity = Vector2.up * JUMP_AMOUNT;
        SoundManager.PlaySound(SoundManager.Sound.BirdJump);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        birdRigidbody2d.bodyType = RigidbodyType2D.Static;
        SoundManager.PlaySound(SoundManager.Sound.Lose);
        if (onDied != null) onDied(this, EventArgs.Empty);
    }
}
