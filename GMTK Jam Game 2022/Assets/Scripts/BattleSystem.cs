using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    UnitInfo playerUnitInfo;
    UnitInfo enemyUnitInfo;

    public TextMeshProUGUI dialogueText;
    
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
        bool isDead = enemyUnitInfo.TakeDamage(playerUnitInfo.damage);
        
        enemyHUD.SetHP(enemyUnitInfo.currentHP);
        dialogueText.text = "The attack hits!";

        yield return new WaitForSeconds(3f);

        //check if dead
        if(isDead)
        {
            //end battle
            state = BattleState.WON;
            EndBattle();
        } else
        {
            StartCoroutine(EnemyTurn());
        }

        //change state
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

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        state = BattleState.ENEMYTURN;
        StartCoroutine(PlayerAttack());
    }
}
