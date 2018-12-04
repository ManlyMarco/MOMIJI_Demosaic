using System.Collections;
using BepInEx;
using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MOMIJI_Demosaic
{
    [BepInPlugin("MOMIJI_Demosaic", "MOMIJI Demosaic", "1.0")]
    public class MOMIJI_Demosaic : BaseUnityPlugin
    {
        void Start()
        {
            HarmonyInstance.Create("MOMIJI_Demosaic").PatchAll(typeof(MOMIJI_Demosaic));
            
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            StartCoroutine(DemosaicTitle());
        }

        private IEnumerator DemosaicTitle()
        {
            for (int i = 0; i < 10; i++)
                yield return null;
            DisableMosaicObjects();
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            DisableMosaicObjects();
        }

        private static void DisableMosaicObjects()
        {
            foreach (var go in GameObject.FindObjectsOfType<GameObject>())
            {
                if (go.name.Contains("モザイク"))
                    go.SetActive(false);
            }
        }

        [HarmonyPatch(typeof(mosaic_PositionFixed), "Start")]
        [HarmonyPostfix]
        public static void MosaicDisable(mosaic_PositionFixed __instance)
        {
            __instance.gameObject.SetActive(false);
        }

        [HarmonyPatch(typeof(mosaic_PositionFixed), "Update")]
        [HarmonyPrefix]
        public static bool MosaicDisable2(mosaic_PositionFixed __instance)
        {
            return false;
        }
        
        [HarmonyPatch(typeof(controller_guide_togle), "Update")]
        [HarmonyPrefix]
        public static bool DesktopFix(controller_guide_togle __instance)
        {
            return __instance.righthand_controllerevent != null;
        }
        [HarmonyPatch(typeof(plau_heni), "Update")]
        [HarmonyPrefix]
        public static bool DesktopFix2(plau_heni __instance)
        {
            return __instance.lefthand_controllerevent != null;
        }
    }
}
