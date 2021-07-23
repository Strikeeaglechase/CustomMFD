using Harmony;
using System.Collections.Generic;
using UnityEngine;


namespace AGM130
{
    abstract class CustomMFDPage
    {
        protected MFDPage page;
        protected MFDManager manager;
        protected string mfdHomeName;
        protected string mfdName;
        protected GameObject aircraft;
        public CustomMFDPage(string mfdHomeName, string mfdName, MFD.MFDButtons slot)
        {
            this.mfdHomeName = mfdHomeName;
            this.mfdName = mfdName;

            aircraft = VTOLAPI.GetPlayersVehicleGameObject();
            MFDManager[] managers = aircraft.GetComponentsInChildren<MFDManager>();
            foreach (MFDManager manager in managers)
            {
                if (manager.name == "MFDManager")
                {
                    this.manager = manager;
                }
            }
            // MFDFlightLog flightLogPage = manager.GetComponentInChildren<MFDFlightLog>(true);
            Traverse traverse = Traverse.Create(manager.mfds[0]);
            MFDPage homePage = (MFDPage)traverse.Field("homePage").GetValue();
            page = GameObject.Instantiate(homePage);

            Setup(slot);
        }
        private void Setup(MFD.MFDButtons slot)
        {
            page.buttons = this.GetButtons();
            page.canSOI = false;
            page.pageName = mfdName;
            page.transform.SetParent(manager.transform);
            page.manager = manager;

            manager.mfdPages.Add(page);
            Traverse managerTraverse = Traverse.Create(manager);
            Dictionary<string, MFDPage> pagesDic = (Dictionary<string, MFDPage>)managerTraverse.Field("pagesDic").GetValue();
            pagesDic.Add(page.pageName, page);

            foreach (MFD mfd in manager.mfds)
            {
                AddHomeButton(mfd, mfdHomeName, slot);
            }
            // TODO: delete this
            // manager.EnsureReady();

        }
        private void AddHomeButton(MFD mfd, string name, MFD.MFDButtons slot)
        {
            List<MFDPage.MFDButtonInfo> newButtons = new List<MFDPage.MFDButtonInfo>();

            MFDPage.MFDButtonInfo ourButton = MakeButton(name, slot, () =>
            {
                mfd.OpenPage(mfdName);
            });
            Traverse traverse = Traverse.Create(mfd);
            MFDPage homePage = (MFDPage)traverse.Field("homePage").GetValue();
            foreach (MFDPage.MFDButtonInfo button in homePage.buttons)
            {
                newButtons.Add(button);
            }
            newButtons.Add(ourButton);
            homePage.buttons = newButtons.ToArray();
        }
        abstract protected MFDPage.MFDButtonInfo[] GetButtons();
        private MFDPage.MFDButtonInfo GetButtonInfo(string name, MFD.MFDButtons slot)
        {
            MFDPage.MFDButtonInfo button = new MFDPage.MFDButtonInfo
            {
                label = name,
                toolTip = name,
                button = slot
            };
            return button;
        }
        protected MFDPage.MFDButtonInfo MakeButton(string name, MFD.MFDButtons slot, UnityEngine.Events.UnityAction handle)
        {
            MFDPage.MFDButtonInfo button = GetButtonInfo(name, slot);
            button.OnPress.AddListener(handle);
            return button;
        }
    }
}
