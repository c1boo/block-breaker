using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    //Config params
    [SerializeField] AudioClip breakSound;
    [SerializeField] GameObject breakingSparkleVFX;
    [SerializeField] Sprite[] hitSprites;

    //Chaced vars
    LevelController level;

    //State vars
    int maxHits;
    int timesHit = 0;

    private void Start() {
        CountBreakableBlocks();
    }

    private void CountBreakableBlocks() {
        level = FindObjectOfType<LevelController>();
        if (tag == "Breakable")
            level.CountBlock();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (tag == "Breakable") {
            HandleHit();
        }
    }

    private void HandleHit() {
        timesHit++;
        maxHits = hitSprites.Length + 1;

        if (timesHit >= maxHits) {
            DestroyBlock();
        } else {
            ShowNextSpire();
        }
    }

    private void ShowNextSpire() {
        int spriteIndex = timesHit - 1;
        GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
    }

    private void DestroyBlock() {
        level.DecreaseNumberOfBlocks();
        PlayCollisionSound();
        FindObjectOfType<GameState>().AddToScore(maxHits);
        PlayBreakingVFX();
        Destroy(gameObject);
    }

    private void PlayCollisionSound() {
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
    }

    private void PlayBreakingVFX() {
        GameObject sparklingVFX = Instantiate(breakingSparkleVFX, transform.position, transform.rotation);
        Destroy(sparklingVFX, 2f);
    }
}
