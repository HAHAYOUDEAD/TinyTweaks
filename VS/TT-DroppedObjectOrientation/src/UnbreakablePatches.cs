using UnityEngine;

namespace TinyTweaks
{
    public class RotateStuff : MelonMod
    {
        [HarmonyPatch(typeof(GameManager), nameof(GameManager.Start))]
        public static class RotateStuffPatch
        {
            public static void Postfix()
            {

                GameObject rifle = GearItem.LoadGearItemPrefab("GEAR_Rifle").gameObject;
                GameObject rifle1 = GearItem.LoadGearItemPrefab("GEAR_Rifle_Barbs").gameObject;
                GameObject rifle2 = GearItem.LoadGearItemPrefab("GEAR_Rifle_Curators").gameObject;
                GameObject rifle3 = GearItem.LoadGearItemPrefab("GEAR_Rifle_Vaughns").gameObject;
                GameObject rifle4 = GearItem.LoadGearItemPrefab("GEAR_Rifle_Trader").gameObject;
                GameObject matches = GearItem.LoadGearItemPrefab("GEAR_WoodMatches").gameObject;

                if (rifle)
                {
                    GameObject posDummy = rifle.transform.FindChild("DropDummy").gameObject;
                    posDummy.transform.eulerAngles = new Vector3(0f, 0f, 270f);
                }                
                if (rifle1)
                {
                    GameObject posDummy = rifle1.transform.FindChild("DropDummy").gameObject;
                    posDummy.transform.eulerAngles = new Vector3(0f, 0f, 270f);
                }                
                if (rifle2)
                {
                    GameObject posDummy = rifle2.transform.FindChild("DropDummy").gameObject;
                    posDummy.transform.eulerAngles = new Vector3(0f, 0f, 270f);
                }               
                if (rifle3)
                {
                    GameObject posDummy = rifle3.transform.FindChild("DropDummy").gameObject;
                    posDummy.transform.eulerAngles = new Vector3(0f, 0f, 270f);
                }                
                if (rifle4)
                {
                    GameObject posDummy = rifle4.transform.FindChild("DropDummy").gameObject;
                    posDummy.transform.eulerAngles = new Vector3(0f, 0f, 270f);
                }

                if (matches)
                { 
                    Transform LOD0 = matches.transform.FindChild("OBJ_WoodMatches_LOD0");
                    Transform LOD1 = matches.transform.FindChild("OBJ_WoodMatches_LOD1");

                    if (LOD0)
                    {
                        matches.GetComponent<Inspect>().m_Angles = new Vector3(-50f, 20f, 0f);
                        LOD0.localEulerAngles = new Vector3(0f, 180f, 0f);
                        LOD0.localPosition = new Vector3(0f, 0f, 0f);
                    }
                    if (LOD1)
                    {
                        LOD1.localEulerAngles = new Vector3(0f, 180f, 0f);
                        LOD1.localPosition = new Vector3(0f, 0f, 0f);
                    }

                    
                }
            }
        }
    }
}



