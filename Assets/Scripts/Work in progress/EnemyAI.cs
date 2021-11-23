using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    [Range(0, 20f)] public float hearRange = 20f;
    [Range(0, 5f)] public float combatRange = 5f;
    [Range(0, 1f)] public float health;
    Vector3 startingPosition;
    Vector3 lastSeenPosition;
    public float reactionTime = 3f;
    private Animator anim;
    public Transform playerTransform;
    Animator playerAnim;
    public GameObject deathCanvas;

    private float guardStamina = 1f;
    private float parryProbability = 0f;

    //private StateDisplayer stateDisplayer;
    //private EnemyHealthBarUpdater healthBar;
    //private StatsDisplayer statsBars;
    public ParticleSystem particles;
    
    public Collider swordCollider;
    public Collider shieldCollider;
    
    //FSM STATES
    private FSM fsm;
    private FSMState idleState;
    private FSMState chaseState;
    private FSMState searchState;
    private FSMState onGuardState;
    private FSMState attackState;
    private FSMState defendState;
    private FSMState deadState;

    //DEFEND DECISION TREE
    //private DecisionTree defendDT;


    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        //stateDisplayer = GameObject.FindObjectOfType<StateDisplayer>();
        //healthBar = GameObject.FindObjectOfType<EnemyHealthBarUpdater>();
        //statsBars = GameObject.FindObjectOfType<StatsDisplayer>();
        startingPosition = transform.position;
        swordCollider.enabled = false;
        shieldCollider.enabled = false;
        //healthBar.UpdateHealth(health);
        //statsBars.UpdateStats(guardStamina, parryProbability);


        anim = GetComponent<Animator>();

        //CREO GLI STATI
        idleState = new FSMState();
        idleState.stayActions.Add(IdleAction);
        chaseState = new FSMState();
        chaseState.stayActions.Add(ChaseAction);
        searchState = new FSMState();
        searchState.stayActions.Add(GoToLastSeenPositionAction);
        onGuardState = new FSMState();
        onGuardState.enterActions.Add(OnGuardAction);
        attackState = new FSMState();
        attackState.stayActions.Add(AttackAction);
        defendState = new FSMState();
        defendState.enterActions.Add(DefendAction);
        deadState = new FSMState();
        deadState.stayActions.Add(DeadAction);

        //DEFINISCO LE TRANSIZIONI
        FSMTransition playerSpotted = new FSMTransition(PlayerSpotted);
        FSMTransition noPlayerInRange = new FSMTransition(NoPlayerInRange);
        FSMTransition playerNotFound = new FSMTransition(PlayerNotFound);
        FSMTransition onGuardRange = new FSMTransition(OnGuardRange);
        FSMTransition notOnGuardRange = new FSMTransition(NotOnGuardRange);
        FSMTransition isEnemyDead = new FSMTransition(IsEnemyDead);
        FSMTransition playerIsAttacking = new FSMTransition(playerIsAttackingCondition);
        FSMTransition canAttack = new FSMTransition(canAttackCondition);

        //LINKO GLI STATI ALLE TRANSIZIONI
        idleState.AddTransition(playerSpotted, chaseState);

        chaseState.AddTransition(noPlayerInRange, searchState);
        chaseState.AddTransition(onGuardRange, onGuardState);

        searchState.AddTransition(playerNotFound, idleState);
        searchState.AddTransition(playerSpotted, chaseState);

        onGuardState.AddTransition(isEnemyDead, deadState);
        onGuardState.AddTransition(notOnGuardRange, chaseState);
        onGuardState.AddTransition(playerIsAttacking, defendState);
        onGuardState.AddTransition(canAttack, attackState);

        attackState.AddTransition(isEnemyDead, deadState);
        attackState.AddTransition(notOnGuardRange, onGuardState);
        attackState.AddTransition(playerIsAttacking, onGuardState);

        defendState.AddTransition(canAttack, onGuardState);



        //METTO LA FSM NEL SUO STATO INIZIALE
        fsm = new FSM(idleState);

        InitDefendDT();

        StartCoroutine(Guard());

    }

    private void InitDefendDT()
    {
        //ACTIONS
        //DTAction parryAction = new DTAction(Parry);
        //DTAction blockAction = new DTAction(Block);
        //DTAction dodgeAction = new DTAction(Dodge);
        //DTAction getHitAction = new DTAction(GetHit);

        //DECISIONS
        //DTDecision parryCondition = new DTDecision(canParry);
        //DTDecision blockCondition = new DTDecision(canBlock);
        //DTDecision dodgeCondition = new DTDecision(canDodge);

        //parryCondition.AddLink(true, parryAction);
        //parryCondition.AddLink(false, blockCondition);

        //blockCondition.AddLink(true, blockAction);
        //blockCondition.AddLink(false, dodgeCondition);

        //dodgeCondition.AddLink(true, dodgeAction);
        //dodgeCondition.AddLink(false, getHitAction);

        //defendDT = new DecisionTree(parryCondition);
    }

    void OnDrawGizmosSelected()
    {

        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, new Vector3(0, 1, 0), hearRange);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, new Vector3(0, 1, 0), combatRange);

        Handles.color = Color.green;
        Handles.DrawLine(transform.position, transform.forward * 40 + transform.position);

        Handles.color = Color.blue;
        Handles.DrawLine(transform.position, playerTransform.position);
    }

    void PrintState()
    {
        //if (fsm.current == idleState)
        //    stateDisplayer.UpdateState("IDLE", Color.white);
        //else if (fsm.current == chaseState)
        //    stateDisplayer.UpdateState("CHASING", Color.yellow);
        //else if (fsm.current == searchState)
        //    stateDisplayer.UpdateState("SEARCHING", Color.magenta);
        //else if (fsm.current == onGuardState)
        //    stateDisplayer.UpdateState("ON GUARD", Color.green);
        //else if (fsm.current == attackState)
        //    stateDisplayer.UpdateState("ATTACKING", Color.red);
        //else if (fsm.current == defendState)
        //    stateDisplayer.UpdateState("DEFENDING", Color.blue);
        //else if (fsm.current == deadState)
        //    stateDisplayer.UpdateState("DEAD", Color.black);

        //statsBars.UpdateStats(guardStamina, parryProbability);
    }

    void ColliderManager()
    {
        
        if(fsm.current == attackState)
        {
            swordCollider.enabled = true;
        } else 
        { 
            swordCollider.enabled = false; 
        }

        if (fsm.current == defendState)
        {
            shieldCollider.enabled = true;
        }
        else
        {
            shieldCollider.enabled = false;
        }
    }

    public IEnumerator Guard()
    {
        while (true){
            fsm.Update();
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        PrintState();
        ColliderManager();

        if(guardStamina <= 1f)
        {
            if (anim.GetBool("hasParried"))
            {
                guardStamina += 0.2f * Time.deltaTime;
            }
            guardStamina += 0.05f * Time.deltaTime;
        }

        if(parryProbability > 0)
        {
            parryProbability -= 0.01f * Time.deltaTime;
        }
    }

    private void ShowDeathScreen()
    {
        deathCanvas.SetActive(true);

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void LookAtPlayer()
    {
        Vector3 targetDirection = playerTransform.position - transform.position;
        float singleStep = 5.0f * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public bool PlayerInFieldOfView(float fov)
    {
        Vector3 lineOfSight = (transform.forward * 40 + transform.position) - transform.position;
        Vector3 dirToPlayer = playerTransform.position - transform.position;
        float angle = Vector3.Angle(dirToPlayer, lineOfSight);

        if (angle < fov)
            return true;
        else
            return false;
    }

    public bool PlayerHiddenByObstacles()
    {

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, playerTransform.position - transform.position, distanceToPlayer);
        Debug.DrawRay(transform.position, playerTransform.position - transform.position, Color.blue); 
        List<float> distances = new List<float>();

        foreach (RaycastHit hit in hits)
        {
            Debug.Log(hit.transform.tag);
            if (hit.transform.tag == "Environment")
            {
                return true;
            }
        }

        return false;

    }

    public bool PlayerInHearingRange()
    {
        float distance = (playerTransform.position - transform.position).magnitude;

        if (distance <= hearRange)
        {
            return true;
        }

        return false;
    }

    //CONDITIONS
    public bool PlayerSpotted()
    {
        if (PlayerInHearingRange() && !playerAnim.GetBool("isDead"))
        {
            return true;
        }
        else if (PlayerInFieldOfView(65) && !playerAnim.GetBool("isDead"))
        {
                return !PlayerHiddenByObstacles();
        }

        return false;
    }

    public bool NoPlayerInRange()
    {
        return !PlayerSpotted();
    }

    public bool PlayerNotFound()
    {
        if ((lastSeenPosition - transform.position).magnitude <= 2f)
            return true;
        else
            return false;
    }

    public bool OnGuardRange()
    {
        if ((playerTransform.position - transform.position).magnitude <= combatRange)
        {
            return true;
        }

        return false;
    }

    public bool NotOnGuardRange()
    {
        return !OnGuardRange();
    }

    public bool playerIsAttackingCondition()
    {
        if (playerAnim.GetBool("isAttacking"))
            return true;

        return false;
    }

    public bool canAttackCondition()
    {
        
            return !playerIsAttackingCondition();

    }

    public bool IsEnemyDead()
    {
        if (health <= 0)
            return true;

        return false;
    }

    //ACTIONS
    public void IdleAction()
    {
        anim.SetBool("isMoving", false);

        if ((startingPosition - transform.position).magnitude > 2f)
        {
            anim.SetBool("isMoving", true);
            GetComponent<NavMeshAgent>().destination = startingPosition;
        }
    }

    public void ChaseAction()
    {
        LookAtPlayer();
        anim.SetBool("onGuard", false);
        anim.SetBool("isMoving", true);
        lastSeenPosition = playerTransform.position;
        GetComponent<NavMeshAgent>().destination = playerTransform.position;
    }

    public void GoToLastSeenPositionAction()
    {
        GetComponent<NavMeshAgent>().destination = lastSeenPosition;
    }

    public void OnGuardAction()
    {
        LookAtPlayer();
        anim.SetBool("onGuard", true);
    }

    public void AttackAction()
    {
        anim.SetTrigger("Attack");

    }

    public void DefendAction()
    {
        LookAtPlayer();
        //defendDT.walk();
    }

    public void DeadAction()
    {
        anim.SetBool("isDead", true);
        ShowDeathScreen();
    }

    //DT DECISIONS
    public object canBlock(object o)
    {
        if (guardStamina >= 0.3f)
        {
            if (PlayerInFieldOfView(30) && playerAnim.GetInteger("AttackType") == 0)
            {
                return true;
            }
        }

        return false;
    }

    public object canParry(object o)
    {

        float attempt = Random.value;
        if (attempt <= parryProbability && PlayerInFieldOfView(20))
        {
            return true;
        }
        return false;
    }

    public object canDodge(object o)
    {
        if (guardStamina >= 0.3f)
        {
            if (playerAnim.GetInteger("AttackType") == 1 || !PlayerInFieldOfView(20))
                return true;
        }
        return false;
    }
}
