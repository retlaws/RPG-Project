using UnityEngine;
using RPG.Attributes;
using RPG.Control;
using System;


namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRayCast(PlayerController callingController)
        {
            Fighter fighter = callingController.GetComponent<Fighter>();
            
            if (!fighter.CanAttack(gameObject))
            {
                return false;
            }

            if (Input.GetMouseButton(0))
            {
            fighter.Attack(gameObject);
            }
            return true;
        }
      
    }
}

