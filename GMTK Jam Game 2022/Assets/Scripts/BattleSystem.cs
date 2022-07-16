using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, ROLLING, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public GameObject diceManager;
    DiceManager diceManagerScript;

    public Slider timer;
    Timer timerScript;

    public GameObject dieSlot1;
    public GameObject dieSlot2;
    public GameObject dieSlot3;
    DieSlot dieSlot1Script;
    DieSlot dieSlot2Script;
    DieSlot dieSlot3Script;

    public Button spellButton;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    UnitInfo playerUnitInfo;
    UnitInfo enemyUnitInfo;
    
    public BattleHUDScript playerHUD;
    public BattleHUDScript enemyHUD;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    //Sets up battle, HUD UI
    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation.transform);
        playerUnitInfo = playerGO.GetComponent<UnitInfo>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation.transform);
        enemyUnitInfo = enemyGO.GetComponent<UnitInfo>();

        diceManagerScript = diceManager.GetComponent<DiceManager>();

        timerScript = timer.GetComponent<Timer>();

        dieSlot1Script = dieSlot1.GetComponent<DieSlot>();
        dieSlot2Script = dieSlot2.GetComponent<DieSlot>();
        dieSlot3Script = dieSlot3.GetComponent<DieSlot>();
    
        //sets up HUD info (names and health)
        playerHUD.SetHUD(playerUnitInfo);
        enemyHUD.SetHUD(enemyUnitInfo);

        yield return new WaitForSeconds(3f);

        state = BattleState.PLAYERTURN;
        StartCoroutine(RollTheDice());
    }

    IEnumerator RollTheDice()
    {
        //rolls dice and determines numbers on each face

        int[] dieValues = diceManagerScript.RollDice();

        //do dice animation, just text cycling through numbers
        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }
    
    //function to deal damage (using d10)
    IEnumerator PlayerAttack()
    {
        int dmg;
        int heal;
        string effects;
        //IDK HOW TO RECEIVE THE VALUES
        diceManagerScript.ScorePicks(dieSlot1Script.slottedDie, dieSlot2Script.slottedDie, dieSlot3Script.slottedDie, out dmg, out heal, out effects);

        //INCLUDE function to return the three dice to their original spots

        //deal damage and heal
        bool isDead = enemyUnitInfo.TakeDamage(dmg);
        playerUnitInfo.Heal(heal);

        //SPACE FOR OTHER, NON DIRECT DAMAGE OR HEAL EFFECTS
        
        //changes enemy HP 
        enemyHUD.SetHP(enemyUnitInfo.currentHP);
        playerHUD.SetHP(playerUnitInfo.currentHP);

        yield return new WaitForSeconds(4f);

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

    }

    //during player turn, wait until button is clicked. buttons for dice not in the pool are disabled
    void PlayerTurn()
    {
        timerScript.BeginTimer();

        while (dieSlot1Script.isEmpty || dieSlot2Script.isEmpty || dieSlot3Script.isEmpty)
        {
            spellButton.enabled = false;
        }
        //

    }

    public void OnSpellButton()
    {
        //if not player turn, do nothing
        if (state != BattleState.PLAYERTURN)
            return;

        //if all slots filled, calcualte damage and effects
        if (!dieSlot1Script.isEmpty && !dieSlot2Script.isEmpty && !dieSlot3Script.isEmpty)
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(PlayerAttack());
        }
    }

    

    //enemy does flat damage, changes player HP, checks if game is over
    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(2f);

        int dwagonDamage = Random.Range(1, 7);
        bool isDead = playerUnitInfo.TakeDamage(dwagonDamage);

        playerHUD.SetHP(playerUnitInfo.currentHP);

        yield return new WaitForSeconds(1f);

        if(isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        } else
        {
            state = BattleState.ROLLING;
            StartCoroutine(RollTheDice());
        }
    }

    //gives final message when battle is over, should transition to a final scene
    void EndBattle()
    {
        
    }

}
