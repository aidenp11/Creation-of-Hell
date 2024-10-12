using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Enemy Data")]
    private Rigidbody2D rb;
    [SerializeField] int Health;
    [SerializeField] int damage;
    [SerializeField] float speed;
    [SerializeField] float acceleration;
    [SerializeField] float drag;
    [SerializeField] float attackSpeed;
    [SerializeField] float fadeSeconds;
}
