﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    // config params
    [SerializeField] AudioClip breakSound;
    [SerializeField] GameObject blockSparklesVFX;
    [SerializeField] int maxHits;
    [SerializeField] Sprite[] hitSprites;

    // cached reference
    Level level;
    GameStatus gameStatus;

    // state variables
    [SerializeField] int timesHit; // TODO serialized for debug purposes

    private void Start() {
        gameStatus = FindObjectOfType<GameStatus>();
        CountBreakableBlocks();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (tag == "Breakable") {
            HandleHit();
        }
    }

    private void CountBreakableBlocks() {
        level = FindObjectOfType<Level>();
        if (tag == "Breakable") {
            level.CountBlocks();
        }
    }

    private void DestroyBlock() {
        PlayBlockDestroySFX();
        Destroy(gameObject);
        TriggerSparklesVFX();
        level.BlockDestroyed();
        gameStatus.IncreseScore();
    }

    private void HandleHit() {
        timesHit++;
        if (timesHit >= maxHits) {
            DestroyBlock();
        } else {
            ShowNextHitSprite();
        }
    }

    private void ShowNextHitSprite() {
        int spriteIndex = timesHit - 1;
        GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
    }

    private void PlayBlockDestroySFX() {
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
    }

    private void TriggerSparklesVFX() {
        GameObject sparkles = Instantiate(blockSparklesVFX, transform.position, transform.rotation);
        Destroy(sparkles, 1f);
    }
}