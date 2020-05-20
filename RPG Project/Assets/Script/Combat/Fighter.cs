using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 5f;
        
        Transform target;
        Mover mover;
        ActionScheduler actionScheduler;
        Animator animator;

        float timeSinceLastAttack = 0;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            MoveToEnemyToAttack();
        }

        private void MoveToEnemyToAttack()
        {
            if (target == null) { return; }

            if (target != null && GetDistanceToTarget() > weaponRange)
            {
                mover.MoveTo(target.position);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if(timeSinceLastAttack < timeBetweenAttacks) { return; }
            animator.SetTrigger("attack"); //This will trigger the Hit() event.
            timeSinceLastAttack = 0;
        }

        void Hit() //Animation Event
        {
            target.GetComponent<Health>().TakeDamage(weaponDamage);
        }

        private float GetDistanceToTarget()
        {
            return Vector3.Distance(transform.position, target.position);
        }

        public void Attack(CombatTarget combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.transform;

        }

        public void Cancel()
        {
            target = null;
        }
    }
}

