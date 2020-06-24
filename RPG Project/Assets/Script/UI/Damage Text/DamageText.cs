using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] Text text;
        public void Setvalue(float amount)
        {
            text.text = string.Format("{0:0}", amount);
        }
    }
}
