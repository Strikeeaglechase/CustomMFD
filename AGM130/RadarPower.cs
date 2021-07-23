namespace AGM130
{
    class RadarPower : CustomMFDPage
    {
        public RadarPower() : base("RDR PWR", "radarPower", MFD.MFDButtons.T2)
        {
        }
        protected override MFDPage.MFDButtonInfo[] GetButtons()
        {
            MFDRadarUI radarUI = aircraft.GetComponentInChildren<MFDRadarUI>();
            MFDPage.MFDButtonInfo[] buttons = {
                MakeButton("Power On", MFD.MFDButtons.L2, () => radarUI.SetRadarPower(1)),
                MakeButton("Power Off", MFD.MFDButtons.L3, () => radarUI.SetRadarPower(0))
            };
            return buttons;
        }
    }
}
