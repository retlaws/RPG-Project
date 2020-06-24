using RPG.Combat;
using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 5f;

        SphereCollider sphereCollider;

        private void Awake()
        {
            sphereCollider = GetComponent<SphereCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                Pickup(other.GetComponent<Fighter>());
            }
        }

        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);

        }

        private void ShowPickup(bool ShouldShowPickup)
        {
            sphereCollider.enabled = true;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(ShouldShowPickup);
            }

        }

        public bool HandleRayCast(PlayerController callingController)
        {
            if (Input.GetMouseButton(0))
            {
                callingController.GetComponent<Mover>().StartMoveAction(transform.position, 1f);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}


