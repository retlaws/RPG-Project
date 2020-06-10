using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Resources;
using System;
using RPG.Saving;
using RPG.Stats;
using System.Collections.Generic;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;

        
        Health target;
        Mover mover;
        ActionScheduler actionScheduler;
        Animator animator;
        Weapon currentWeapon = null;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(target == null || target.IsDead()) { return; }

            MoveToEnemyToAttack();
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return target;
        }

        private void MoveToEnemyToAttack()
        {
            if (target == null) { return; }
            if (target.IsDead()) { return; }

            if (!GetIsInRange())
            {
                mover.MoveTo(target.transform.position, 1f);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack < timeBetweenAttacks) { return; }
            animator.SetTrigger("attack"); //This will trigger the Hit() event.
            timeSinceLastAttack = 0;
        }

        void Hit() //Animation Event
        {
            if(target == null) { return; }

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }

        void Shoot() //Animation Event
        {
            Hit();
        }

        private bool GetIsInRange()
        {
            float distanceTotarget = Vector3.Distance(transform.position, target.transform.position);
            return distanceTotarget < currentWeapon.GetRange();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public bool CheckIfRangedWeapon()
        {
            return currentWeapon.HasProjectile();
        }

        public Weapon GetCurrentWeapon()
        {
            return currentWeapon;
        }
       
        public void Cancel()
        {
            target = null;
            animator.SetTrigger("stopAttack");
            mover.Cancel();
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeapon.GetDamage();
            }
        }
        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.GetPercentageBonus();
            }
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            String weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}

