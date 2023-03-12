using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using MelonLoader;

namespace TinyTweaks
{
    class MapTextOutline : MelonMod
    {
        public override void OnInitializeMelon()
        {
            Settings.OnLoad();
        }

        [HarmonyPatch(typeof(Panel_Map), "CreateObjectPools")] 
        public class EditTextVisual
        {
            public static void Postfix(Panel_Map __instance)
            {
                for (int i = 0; i < __instance.m_TextPoolParent.childCount; i++)
                {
                    UILabel label = __instance.m_TextPoolParent.GetChild(i).GetComponent<UILabel>();

                    float alpha = 0.7f;

                    switch (Settings.options.outlineColor)
                    {
                        case 0: // Dark text w/ beige outline
                            label.color = new Color(0.125f, 0.094f, 0.094f, 1f); // HL dark color
                            label.effectStyle = UILabel.Effect.Outline;
                            label.effectColor = new Color(0.5808823f, 0.5514917f, 0.4954584f, alpha); // HL beige color
                            label.effectDistance = new Vector2(1.2f, 0.5f);
                            break;
                        case 1: // Dark text w/ white outline
                            label.color = new Color(0.125f, 0.094f, 0.094f, 1f); // HL dark color
                            label.effectStyle = UILabel.Effect.Outline;
                            label.effectColor = new Color(1f, 1f, 1f, alpha); 
                            label.effectDistance = new Vector2(1.2f, 0.5f);
                            break;
                        case 2: // Bright text w/ dark outline
                            label.color = new Color(0.88f, 0.83f, 0.735f, 1f); // my bright color
                            label.effectStyle = UILabel.Effect.Outline;
                            label.effectColor = new Color(0.125f, 0.094f, 0.094f, alpha); // HL dark color
                            label.effectDistance = new Vector2(1.2f, 0.5f);
                            break;
                        case 3: // White text w/ dark outline
                            label.color = new Color(1f, 1f, 1f, 1f); // my bright color
                            label.effectStyle = UILabel.Effect.Outline;
                            label.effectColor = new Color(0.125f, 0.094f, 0.094f, alpha); // HL dark color
                            label.effectDistance = new Vector2(1.2f, 0.5f);
                            break;

                    }
                }
            }
        }
    }
}
