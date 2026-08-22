using Inventor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;

namespace VaultifyYourInventorAddin
{
    public class CustomRibbon
    {
        private ButtonDefinition _tempButton { get; set; }

        private RibbonTab _tempRibbonTab;

        private RibbonPanel _tempRibbonPanel;

        private System.Drawing.Image _16x16;
        private System.Drawing.Image _32x32;



        public CustomRibbon()
        {
            _tempButton = Utilities.CreateButtonDef("Run", "_Run", "", Resources._16x16, Resources._32x32);
            _tempButton.OnExecute += Run_OnExecute;


            // Add to specific ribbons
            Ribbon partRibbon = Globals.InvApp.UserInterfaceManager.Ribbons["Part"];
            Ribbon assemblyRibbon = Globals.InvApp.UserInterfaceManager.Ribbons["Assembly"];
            Ribbon drawingRibbon = Globals.InvApp.UserInterfaceManager.Ribbons["Drawing"];
            Ribbon zeroDocRibbon = Globals.InvApp.UserInterfaceManager.Ribbons["ZeroDoc"];

            AddToRibbon(partRibbon);
            AddToRibbon(assemblyRibbon);
            AddToRibbon(drawingRibbon);
            AddToRibbon(zeroDocRibbon);
        }


        public void AddToRibbon(Ribbon targetRibbon)
        {
            // Create and add ribbon tabs and panels
            _tempRibbonTab = targetRibbon.RibbonTabs.Add("Vaultify Your Inventor Add-In", "id_TheAU2026-MFG1906_VaultifyYourInventorAddin" + targetRibbon.InternalName, Globals.InvAppGuidID);

            _tempRibbonPanel = _tempRibbonTab.RibbonPanels.Add("Utilities", "id_Utilities" + targetRibbon.InternalName, Globals.InvAppGuidID);

            // Add command controls to ribbon panels
            _tempRibbonPanel.CommandControls.AddButton(_tempButton);


        }


        #region UI Events


        private void Run_OnExecute(NameValueMap context)
        {

            var newForm = new BoxForm();

            var helper = new WindowInteropHelper(newForm);
            helper.Owner = (IntPtr)Globals.InvApp.MainFrameHWND;
            newForm.Show();

        }

        #endregion


    }
}
