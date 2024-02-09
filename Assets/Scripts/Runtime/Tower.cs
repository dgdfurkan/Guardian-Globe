using UnityEngine;

public class Tower : MonoBehaviour
{
    private CircleCollider2D _rangeCollider;
    [SerializeField] private float range = 5f; // Tower'ın ateş etme menzili
    [SerializeField] private float fireRate = 1f; // Tower'ın ateş etme hızı (saniye cinsinden)
    [SerializeField] private int damage = 50; // Tower'ın her ateş ettiğinde verdiği zarar
    private Enemy _targetEnemy = null;

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        _rangeCollider = GetComponent<CircleCollider2D>();
        _rangeCollider.radius = range;
        _rangeCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eğer hedefimiz yoksa ve çarpışan obje bir Enemy ise, hedef olarak belirle
        if (_targetEnemy == null && other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _targetEnemy = enemy;
            // FireAtEnemy metodunu belirli bir süre aralığıyla çağır
            InvokeRepeating(nameof(FireAtEnemy), 0f, fireRate);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        // Eğer çarpışan obje hedefimiz olan Enemy ise, hedefi sıfırla
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy) && enemy == _targetEnemy)
        {
            _targetEnemy = null;
            // FireAtEnemy metodunun çağrılmasını durdur
            CancelInvoke(nameof(FireAtEnemy));
            // Range içindeki en yakın Enemy'yi bul ve hedef olarak belirle
            FindTargetEnemy();
        }
    }
    
    public void FindTargetEnemy()
    {
        _targetEnemy = null;
        CancelInvoke(nameof(FireAtEnemy));
        // Tüm Enemy objelerini bul
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        Enemy closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            // Eğer bu düşman menzil içindeyse ve daha yakınsa, hedef olarak belirle
            if (distanceToEnemy <= range && distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy is not null)
        {
            _targetEnemy = closestEnemy;
            // FireAtEnemy metodunu belirli bir süre aralığıyla çağır
            InvokeRepeating(nameof(FireAtEnemy), 0f, fireRate);
        }
    }

    private void FireAtEnemy()
    {
        // Tower'ın hedefine doğru dönmesini sağla
        Vector3 direction = _targetEnemy.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle - 90); // -90 derece eklenir çünkü 0 derece doğru yönü gösterir


        // Enemy'ye zarar ver
        _targetEnemy.TakeDamage(damage, this);
        
        // Burada düşmana ateş etme kodunu yazabilirsiniz
        //Debug.Log("Firing at enemy!");
    }
    
private void OnDrawGizmos()
    {
        // Menzil çemberini çiz
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
        
        // Hedefe bir çizgi çiz
        if (_targetEnemy != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _targetEnemy.transform.position);
        }
    }
}