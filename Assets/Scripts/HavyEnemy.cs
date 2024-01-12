using UnityEngine;

namespace Assets.Scripts
{
    public class HavyEnemy : Enemie
    {
        [SerializeField]
        private GameObject _enemyPrefab;
        [SerializeField]
        private int _enemyCount = 2;

        protected override void Update()
        {
            if (!isDead)
                if (Hp <= 0)
                    Die();

            base.Update();
        }
        protected override void Die() 
        {
            for (int i = 0; i < _enemyCount; i++)
                Instantiate(_enemyPrefab);

            base.Die();
        }
    }
}
