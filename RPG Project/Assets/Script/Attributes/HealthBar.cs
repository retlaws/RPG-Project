using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Image foreground;
        [SerializeField] Canvas canvas;

        Health health;
        float healthFraction = 1f;


        private void Awake()
        {
            health = GetComponentInParent<Health>();
            canvas.enabled = false;
        }
        void Update()
        {
            healthFraction = health.GetHealthPoints() / health.GetMaxHealthPoints();
            
            EnableAndDisableCanvas();

            foreground.rectTransform.localScale = new Vector3(healthFraction, 1, 1);
        }

        private void EnableAndDisableCanvas()
        {
            if (Mathf.Approximately(healthFraction, 1)) { return; }
            if (healthFraction <= 0) { canvas.enabled = false; }
            else if (healthFraction < 1) { canvas.enabled = true; }
        }
    }

}

