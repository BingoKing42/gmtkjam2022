using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public GameObject playerSprite;
    public Sprite oldPlayerSprite;
    public Sprite newPlayerSprite;

    public GameObject enemySprite;
    public Sprite passiveElement0;
    public Sprite passiveElement1;
    public Sprite passiveElement2;
    public Sprite passiveElement3;
    public Sprite passiveElement4;
    public Sprite passiveElement5;
    public Sprite attackingElement0;
    public Sprite attackingElement1;
    public Sprite attackingElement2;
    public Sprite attackingElement3;
    public Sprite attackingElement4;
    public Sprite attackingElement5;

    private SpriteRenderer playerSpriteRenderer;
    private SpriteRenderer enemySpriteRenderer;

    void Start()
    {
        playerSpriteRenderer = playerSprite.GetComponent<SpriteRenderer>();
        enemySpriteRenderer = enemySprite.GetComponent<SpriteRenderer>();

        playerSpriteRenderer.sprite = oldPlayerSprite;
        enemySpriteRenderer.sprite = passiveElement0;
    }

    public void PlayerAttackAnimation()
    {
        StartCoroutine(PlayerAnimRoutine(2.0f));
    }

    public void EnemyAttackAnimation(int tempElementNum, int elementNum)
    {
        Sprite attackingSprite;
        switch (elementNum)
        {
            case 0:
                attackingSprite = attackingElement0;
                break;
            case 1:
                attackingSprite = attackingElement1;
                break;
            case 2:
                attackingSprite = attackingElement2;
                break;
            case 3:
                attackingSprite = attackingElement3;
                break;
            case 4:
                attackingSprite = attackingElement4;
                break;
            case 5:
                attackingSprite = attackingElement5;
                break;
            default:
                attackingSprite = attackingElement0;
                break;
        }
        Sprite passiveSprite;
        switch (tempElementNum)
        {
            case 0:
                passiveSprite = passiveElement0;
                break;
            case 1:
                passiveSprite = passiveElement1;
                break;
            case 2:
                passiveSprite = passiveElement2;
                break;
            case 3:
                passiveSprite = passiveElement3;
                break;
            case 4:
                passiveSprite = passiveElement4;
                break;
            case 5:
                passiveSprite = passiveElement5;
                break;
            default:
                passiveSprite = passiveElement0;
                break;
        }
        StartCoroutine(EnemyAnimRoutine(2.0f, passiveSprite, attackingSprite));
    }

    IEnumerator PlayerAnimRoutine(float seconds)
    {
        playerSpriteRenderer.sprite = newPlayerSprite;
        yield return new WaitForSeconds(seconds);
        playerSpriteRenderer.sprite = oldPlayerSprite;
    }

    IEnumerator EnemyAnimRoutine(float seconds, Sprite oldSprite, Sprite newSprite)
    {
        enemySpriteRenderer.sprite = newSprite;
        yield return new WaitForSeconds(seconds);
        enemySpriteRenderer.sprite = oldSprite;
    }
}
