using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DungeonKIT
{
    [Serializable]
    public class DamageEffect
    {
        //Parametrs
        public Color hitColor, normalColor;

        public IEnumerator Damage(SpriteRenderer sprite)
        {
            float time = 0;

            while (time < 0.1f)
            {
                sprite.color = Color.Lerp(sprite.color, hitColor, 0.4f); //Smooth change sprite color to damage
                time += Time.deltaTime;
                yield return null;
            }

            time = 0;

            while (time < 0.1f)
            {
                sprite.color = Color.Lerp(sprite.color, normalColor, 0.4f); //Smooth change sprite color to normal
                time += Time.deltaTime;
                yield return null;
            }

        }
    }
}
