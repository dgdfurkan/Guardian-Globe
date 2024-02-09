using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Vector3 _endPosition;
    [SerializeField] private int health = 100; // Enemy'nin canı
    private Tower _tower;

    private void Awake()
    {   
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    
    private void Start()
    {
        UpdateNavMeshSettings();
    }
    
    public void Initialize(Vector3 endPosition)
    {
        _endPosition = endPosition;
    }

    private void UpdateNavMeshSettings()
    {
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _navMeshAgent.SetDestination(_endPosition);
    }
    
    public void TakeDamage(int damage, Tower tower)
    {
        _tower = tower;
        // Enemy'nin canını azalt
        health -= damage;
        print("Enemy'nin canı: " + health);

        // Eğer Enemy'nin canı 0'a düşerse, Enemy'yi yok et
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    void CancelInvokeTower(Tower tower)
    {
        tower.FindTargetEnemy();
    }

    private void OnDestroy()
    {
        CancelInvokeTower(_tower);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Finish"))
        {
            print("Game Over!");
            Destroy(gameObject);
        }
    }
}