using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    
    private void Start()
    {
        InvokeRepeating(nameof(CreateEnemy), 1f, 1f);
    }

    void CreateEnemy()
    {
        var enemy = Instantiate(_enemyPrefab, _startPoint.position, Quaternion.identity);
        enemy.GetComponent<Enemy>().Initialize(_endPoint.position);
    }
}
