using System.Threading;
using UnityEngine;

public class EnemyBehhaviour : MonoBehaviour
{
    public AIMove _AIMove;
    public Monster monster;
    public float attackDistance = 1.5f;
    public Transform flowerTransform;
    public ParticleSystem hitParticle;

    private void Start()
    {
        monster = GetComponent<Monster>();
        monster.target = GameManager.instance.playerController.transform;
        flowerTransform = GameManager.instance.flowerTransform;
        _AIMove = GetComponent<AIMove>();
        GetComponent<EnemyBehhaviour>().ChangeState(EnemyBehhaviour.EnemyState.Idle);
    }

    private void Update()
    {
        monster.UpdateDirection();

        float distanceToPlayer = Vector2.Distance(monster.transform.position, GameManager.instance.playerController.transform.position);
        float distanceToFlower = Vector2.Distance(monster.transform.position, flowerTransform.position);
        if(monster.target == null )
        {
            ChangeState(EnemyState.Idle);
        }

        ChangeState(EnemyState.FindFlower);
        if (distanceToFlower <= attackDistance)
        {
            ChangeState(EnemyState.AttackBase);
        }
        if (GameManager.instance.playerCharacter.isPlayerDead)
        {
            ChangeState(EnemyState.FindFlower);
        }
        else if (distanceToPlayer <= monster.detectionRange && distanceToFlower >= distanceToPlayer)
        {

            ChangeState(EnemyState.ChasePlayer);
            if (distanceToPlayer <= attackDistance)
            {

                ChangeState(EnemyState.Attack);

            }
            else
            {
                ChangeState(EnemyState.ChasePlayer);

            }
        }
        
    }
    public enum EnemyState
    {
        Idle, ChasePlayer ,FindFlower , Attack ,AttackBase, Die , TakeDamage
    }
    public EnemyState enemyState = EnemyState.Idle;

    public void ChangeState(EnemyState state)
    {
        enemyState = state;
        switch (state)
        {
            case EnemyState.Idle:
                monster.animator.SetBool("isWalk", false);
                break;
            case EnemyState.ChasePlayer:
                monster.SetTarget(GameManager.instance.playerController.transform);
                _AIMove.target = monster.target;
                _AIMove.enabled = true; // เริ่มการเคลื่อนที่
                monster.animator.SetBool("isWalk", true);
                break;
            case EnemyState.FindFlower:
                monster.SetTarget(GameManager.instance.flowerTransform);
                _AIMove.target = monster.target;
                _AIMove.enabled = true;
                monster.animator.SetBool("isWalk", true);
                break;
            case EnemyState.Attack:
                monster.animator.SetTrigger("Attack");
                _AIMove.enabled = false; // หยุดการเคลื่อนที่
                monster.animator.SetBool("isWalk", false);
                monster.Attack(); 
                break;
            case EnemyState.AttackBase:
                monster.animator.SetTrigger("Attack");
                _AIMove.enabled = false; // หยุดการเคลื่อนที่
                monster.animator.SetBool("isWalk", false);
                monster.AttackBase();
                break;
            case EnemyState.TakeDamage:
                hitParticle.Play();
                monster.GetKnockBack(1.5f);
                break;
            case EnemyState.Die:
                _AIMove.enabled = false; // หยุดการเคลื่อนที่
                monster.animator.SetBool("isWalk", false);
                break;
            default:
                break;
        }

    }
}