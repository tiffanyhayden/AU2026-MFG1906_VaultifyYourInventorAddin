using System;
using System.Runtime.InteropServices;
using Inventor;
using Microsoft.Win32;

namespace VaultifyYourInventorAddin
{
    /// <summary>
    /// This is the primary AddIn Server class that implements the ApplicationAddInServer interface
    /// that all Inventor AddIns are required to implement. The communication between Inventor and
    /// the AddIn is via the methods on this interface.
    /// </summary>
    [GuidAttribute("E4B8221D-4004-4150-BBDA-25237FC27143")]
    public class StandardAddInServer : Inventor.ApplicationAddInServer
    {

        // Inventor application object.
        private Inventor.Application m_inventorApplication;
        private UserInterfaceEvents m_uiEvents;
        //56F7F41D-1C10-4D28-AFB4-D6A9CC715338 old guid

        public StandardAddInServer()
        {

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        }

        #region ApplicationAddInServer Members

        public void Activate(Inventor.ApplicationAddInSite addInSiteObject, bool firstTime)
        {

            // Initialize AddIn members.
            Globals.InvApp = addInSiteObject.Application;
            Globals.InvAppEvents = new InventorEventHandler();
            Globals.InvAppRibbon = new CustomRibbon();

            m_uiEvents = Globals.InvApp.UserInterfaceManager.UserInterfaceEvents;

        }

        public void Deactivate()
        {
            m_uiEvents =  null;
            Globals.InvApp = null;


            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public object Automation
        {
            // This property is provided to allow the AddIn to expose an API 
            // of its own to other programs. Typically, this  would be done by
            // implementing the AddIn's API interface in a class and returning 
            // that class object through this property.

            get
            {
                // TODO: Add ApplicationAddInServer.Automation getter implementation
                return null;
            }
        }

        void ApplicationAddInServer.ExecuteCommand(int commandID)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
