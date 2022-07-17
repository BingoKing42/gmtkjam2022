using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public GameObject playerSprite;
    public Sprite oldPlayerSprite;
    public Sprite newPlayerSprite;

    public GameObject enemySprite;
    public Sprite oldEnemySprite;
    public Sprite newEnemySprite;

    private SpriteRenderer playerSpriteRenderer;
    private SpriteRenderer enemySpriteRenderer;

    void Start()
    {
        playerSpriteRenderer = playerSprite.GetComponent<SpriteRenderer>();
        enemySpriteRenderer = enemySprite.GetComponent<SpriteRenderer>();

        playerSpriteRenderer.sprite = oldPlayerSprite;
        enemySpriteRenderer.sprite = oldEnemySprite;
    }

    public void PlayerAttackAnimation()
    {
        StartCoroutine(PlayerAnimRoutine(2.0f));
    }

    public void EnemyAttackAnimation()
    {
        StartCoroutine(EnemyAnimRoutine(2.0f));
    } 

    IEnumerator PlayerAnimRoutine(float seconds)
    {
        playerSpriteRenderer.sprite = newPlayerSprite;
        yield return new WaitForSeconds(seconds);
        playerSpriteRenderer.sprite = oldPlayerSprite;
    }

    IEnumerator EnemyAnimRoutine(float seconds)
    {
        enemySpriteRenderer.sprite = newEnemySprite;
        yield return new WaitForSeconds(seconds);
        enemySpriteRenderer.sprite = oldEnemySprite;
    }
}
