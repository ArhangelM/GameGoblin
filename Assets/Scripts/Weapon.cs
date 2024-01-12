using Assets.Scripts.Interfaces;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private string _enemyTag;
    [SerializeField] private float _damage = 10;

    public float Damage { get => _damage; set => _damage = value; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_enemyTag))
        {
            var enemy = other.GetComponent<IStats>();
            enemy.Hp -= _damage;
            Debug.Log("Damage");
        }
    }


}
