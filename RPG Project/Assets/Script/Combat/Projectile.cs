using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 3f;
        [SerializeField] bool isHomingProjectile = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10f;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 1f;
        [SerializeField] UnityEvent Impact;

        Health target = null;
        GameObject instigator = null;
        float damage = 0;


        void Update()
        {
            if(target == null) { return; }
            if(isHomingProjectile && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
            transform.LookAt(GetAimLocation());
            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if(targetCapsule == null)
            {
                return target.transform.position;
            }

            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            Health otherHealth = other.GetComponent<Health>();

            if(otherHealth != target) { return; }

            Impact.Invoke();

            speed = 0;

            target.TakeDamage(instigator, damage);
            if (!otherHealth.IsDead())
            {
                HitEffect();

                DestroyOnImpact();

                Destroy(gameObject, lifeAfterImpact);
            }
            if (otherHealth.IsDead())
            {
                DestroyOnImpact();
                HitEffect();
            }
        }

        private void HitEffect()
        {
            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }
        }

        private void DestroyOnImpact()
        {
            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
        }
    }
}


