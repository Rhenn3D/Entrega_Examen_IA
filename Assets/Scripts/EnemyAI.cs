using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent _enemyAgent;
    public enum EnemyState
    {
        Patrullar,
        Perseguir,
        Atacar,
    }

    [SerializeField] private Transform[] _patrollPoints;

    private EnemyState _currentState;

    [SerializeField] private float _detectionRange = 7f;
    [SerializeField] private float _attackRange = 3f;
    [SerializeField] private float _attackTimer;
    [SerializeField] private float _attackDelay = 3f;

    private Transform _player;

    void Awake()
    {
        _enemyAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player").transform;
    }
        
    void Start()
    {
        _currentState = EnemyState.Patrullar;
        _attackTimer = _attackDelay;
        _enemyAgent.SetDestination(_player.position);
        PatrollingPoints();
    }

    void Update()
    {
        switch(_currentState)
        {
            case EnemyState.Patrullar:
            Patrullando();
            break;

            case EnemyState.Perseguir:
            Persiguiendo();
            break;

            case EnemyState.Atacar:
            Atacando();
            break;

            default:
            Patrullando();
            break;
        }

        
    }


    void Patrullando()
    {
        if(OnRange(_detectionRange))
        {
            _currentState = EnemyState.Perseguir;
        }

        if(_enemyAgent.remainingDistance <= 0.5f)
        {
            PatrollingPoints();
        }
        
    }

    void Persiguiendo()
    {
        if(!OnRange(_detectionRange))
        {
            _currentState = EnemyState.Patrullar;
        }
        if(OnRange(_attackRange))
        {
            _currentState = EnemyState.Atacar;
        }

        _enemyAgent.SetDestination(_player.position);
    }

    void Atacando()
    {
        if(!OnRange(_attackRange))
        {
            _currentState = EnemyState.Perseguir;
        }
        _attackTimer += Time.deltaTime;
        if(_attackTimer >= _attackDelay)
        {
            Debug.Log("Ataque");
            _attackTimer = 0;
        }
    }


    void PatrollingPoints()
    {
        _enemyAgent.SetDestination(_patrollPoints[Random.Range(0, _patrollPoints.Length)].position);

    }

    public bool OnRange(float distance)
    {
        float DistanceToPlayer = Vector3.Distance(transform.position, _player.position);
        if(DistanceToPlayer <= distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }

    
}
