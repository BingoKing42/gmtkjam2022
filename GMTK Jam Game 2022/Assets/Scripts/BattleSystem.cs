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
    public TextMeshProUGUI d4Text;
    public TextMeshProUGUI d8Text;
    public TextMeshProUGUI d10Text;
    public TextMeshProUGUI d12Text;
    public TextMeshProUGUI d20Text;


    public Button d10Button;
    public Button d8Button;
    
    public BattleHUDScript playerHUD;
    public BattleHUDScript enemyHUD;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    //Sets up battle, HUD UI, dialogue text, dicepool
    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnitInfo = playerGO.GetComponent<UnitInfo>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnitInfo = enemyGO.GetComponent<UnitInfo>();

        //introduction dialogue
        dialogueText.text = "The roaring " + enemyUnitInfo.unitName + " swoops in!";
    
        //sets up HUD info (names and health)
        playerHUD.SetHUD(playerUnitInfo);
        enemyHUD.SetHUD(enemyUnitInfo);

        //sets player dice to starting amount
        playerUnitInfo.setUpDice();
        setDiceAmount();

        yield return new WaitForSeconds(3f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    //function to update dicepool text
    void setDiceAmount()
    {
        d4Text.text = "x" + playerUnitInfo.d4s;
        d8Text.text = "x" + playerUnitInfo.d8s;
        d10Text.text = "x" + playerUnitInfo.d10s;
        d12Text.text = "x" + playerUnitInfo.d12s;
        d20Text.text = "x" + playerUnitInfo.d20s;
    }
    
    //function to deal damage (using d10)
    IEnumerator PlayerAttack()
    {
        //damage, random number 1-10
        int d10Damage = Random.Range(1, 10);
        bool isDead = enemyUnitInfo.TakeDamage(d10Damage);
        
        //changes enemy HP and updates dialogue text
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
            StartCoroutine(PlayerDraft());
        }

    }

    //heals player using d8
    IEnumerator PlayerHeal()
    {
        int d8Heal = Random.Range(1, 8);
        playerUnitInfo.Heal(d8Heal);

        //updates player HUD and dialogue text
        playerHUD.SetHP(playerUnitInfo.currentHP);
        dialogueText.text = "You healed " + d8Heal + " points of health";

        yield return new WaitForSeconds(4f);

        StartCoroutine(PlayerDraft());
    }

    //adds dice to the players pool
    IEnumerator PlayerDraft()
    {
        dialogueText.text = "Add a die to your collection";

        int dieOptions = Random.Range(1, 6);

        yield return new WaitForSeconds(2f);

        //takes the value from the d6 to decide what to add to dice pool
        if (dieOptions == 1)
        {
            playerUnitInfo.d10s += 1;
            d10Button.enabled = true;

        } else if (dieOptions == 2) {
            playerUnitInfo.d8s += 1;
            d8Button.enabled = true;
        } else {
            playerUnitInfo.d10s +=1;
            playerUnitInfo.d8s +=1;
            d10Button.enabled = true;
            d8Button.enabled = true;
        }
        setDiceAmount();
        dialogueText.text = "Aw yeah, new dice!";

        yield return new WaitForSeconds(2f);

        StartCoroutine(EnemyTurn());
    }

    //during player turn, wait until button is clicked. buttons for dice not in the pool are disabled
    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";

        if (playerUnitInfo.d10s == 0)
        {
            d10Button.enabled = false;
        }

        if (playerUnitInfo.d8s == 0)
        {
            d8Button.enabled = false;
        }
        
    }

    //when d10 button clicked, subtract one from pool and attack
    public void OnD10Button()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        playerUnitInfo.d10s -= 1;

        setDiceAmount();

        state = BattleState.ENEMYTURN;
        StartCoroutine(PlayerAttack());
    }

    //when d8 button clicked, subtract one from pool and heal
    public void OnD8Button()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        playerUnitInfo.d8s -= 1;

        setDiceAmount();

        state = BattleState.ENEMYTURN;
        StartCoroutine(PlayerHeal());
    }

    //enemy does flat damage, changes player HP, checks if game is over
    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(2f);

        int dwagonDamage = Random.Range(1, 6);
        bool isDead = playerUnitInfo.TakeDamage(dwagonDamage);

        dialogueText.text = enemyUnitInfo.unitName + " attacks for " + dwagonDamage + " damage!";

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

    //gives final message when battle is over, should transition to a final scene
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
