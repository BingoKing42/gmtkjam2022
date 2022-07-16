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

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }
    
    //function to deal damage (using d10)
    IEnumerator PlayerAttack()
    {
        //damage, random number 1-10
        int d10Damage = Random.Range(1, 10);
        bool isDead = enemyUnitInfo.TakeDamage(d10Damage);
        
        //changes enemy HP and updates dialogue text
        enemyHUD.SetHP(enemyUnitInfo.currentHP);

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

    }

    public void OnSpellButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        
    }

    //enemy does flat damage, changes player HP, checks if game is over
    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(2f);

        int dwagonDamage = Random.Range(1, 6);
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
