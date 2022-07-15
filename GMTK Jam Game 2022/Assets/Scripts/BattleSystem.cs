using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, DRAFT, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    UnitInfo playerUnitInfo;
    UnitInfo enemyUnitInfo;

    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI d10Button;
    public TextMeshProUGUI d8Button;
    
    public BattleHUDScript playerHUD;
    public BattleHUDScript enemyHUD;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnitInfo = playerGO.GetComponent<UnitInfo>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnitInfo = enemyGO.GetComponent<UnitInfo>();

        dialogueText.text = "The roaring " + enemyUnitInfo.unitName + " swoops in!";
    
        playerHUD.SetHUD(playerUnitInfo);
        enemyHUD.SetHUD(enemyUnitInfo);

        yield return new WaitForSeconds(3f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }
    
    IEnumerator PlayerAttack()
    {
        //damage
        int d10Damage = Random.Range(1, 10);
        bool isDead = enemyUnitInfo.TakeDamage(d10Damage);
        
        enemyHUD.SetHP(enemyUnitInfo.currentHP);
        dialogueText.text = "The attack hits for " + d10Damage + " damage!";

        yield return new WaitForSeconds(4f);

        //check if dead
        if(isDead)
        {
            //end battle
            state = BattleState.WON;
            EndBattle();
        } else
        {
            PlayerDraft();
        }

    }

    IEnumerator PlayerHeal()
    {
        int d8Heal = Random.Range(1, 8);
        playerUnitInfo.Heal(d8Heal);

        playerHUD.SetHP(playerUnitInfo.currentHP);
        dialogueText.text = "You healed " + d8Heal + " points of health";

        yield return new WaitForSeconds(4f);

        PlayerDraft();
    }

    void PlayerDraft()
    {
        dialogueText.text = "Add a die to your collection";

        int dieOptions = Random.Range(1, 6);

        StartCoroutine(EnemyTurn());
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";

        /*
        if (playerUnitInfo.d10s == 0)
        {
            d10Button.Interactable = false;
        }

        if (playerUnitInfo.d8s == 0)
        {
            d8Button.Interactable = false;
        }
        */
    }

    public void OnD10Button()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        playerUnitInfo.d10s -= 1;

        state = BattleState.ENEMYTURN;
        StartCoroutine(PlayerAttack());
    }

    public void OnD8Button()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        state = BattleState.ENEMYTURN;
        StartCoroutine(PlayerHeal());
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnitInfo.unitName + " attacks!";

        yield return new WaitForSeconds(2f);

        bool isDead = playerUnitInfo.TakeDamage(enemyUnitInfo.damage);

        playerHUD.SetHP(playerUnitInfo.currentHP);

        yield return new WaitForSeconds(1f);

        if(isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        } else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if(state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
        } else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
        }
    }

}
