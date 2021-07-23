using System.Collections;

namespace AGM130
{
    public class Main : VTOLMOD
    {
        public override void ModLoaded()
        {
            VTOLAPI.SceneLoaded += SceneLoaded;
            base.ModLoaded();
        }

        private void SceneLoaded(VTOLScenes scene)
        {
            switch (scene)
            {
                case VTOLScenes.ReadyRoom:
                    break;
                case VTOLScenes.LoadingScene:
                    break;
                case VTOLScenes.CustomMapBase:
                case VTOLScenes.Akutan:
                    Log("Trying to start mod");
                    StartCoroutine("StartMod");
                    break;
            }
        }
        private IEnumerator StartMod()
        {
            while (VTMapManager.fetch == null || !VTMapManager.fetch.scenarioReady || FlightSceneManager.instance.switchingScene)
            {
                yield return null;
            }
            if (VTOLAPI.GetPlayersVehicleEnum() != VTOLVehicles.FA26B) { yield break; }


            CustomMFDPage custoMfd = new RadarPower();

            yield break;
        }
    }
}