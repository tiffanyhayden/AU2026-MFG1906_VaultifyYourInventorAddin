using Autodesk.Connectivity.WebServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Inventor;
using Autodesk.DataManagement.Client.Framework.Internal.ExtensionMethods;

namespace VaultifyYourInventorAddin
{
    internal static class InventorUtilities
    {



      public static void SetPropertyValue(Document doc, string name, string val )
        {
            if (doc != null)
            {
                PropertySet sumProps = doc.PropertySets["Inventor Summary Information"];
                PropertySet designProps = doc.PropertySets["Design Tracking Properties"];
                PropertySet docSumProps = doc.PropertySets["Inventor Document Summary Information"];
                PropertySet customProps = doc.PropertySets["Inventor User Defined Properties"];


                foreach (Property prop in sumProps)
                {
                    if (prop.Name == name)
                    {
                        prop.Value = val;
                        return;
                    }
                }

                foreach (Property prop in designProps)
                {
                    if (prop.Name == name)
                    {
                        prop.Value = val;
                        return;
                    }
                }


                foreach (Property prop in docSumProps)
                {
                    if (prop.Name == name)
                    {
                        prop.Value = val;
                        return;
                    }
                }

                foreach (Property prop in customProps)
                {
                    if (prop.Name == name)
                    {
                        prop.Value = val;
                        return;
                    }
                }

            }
        }

        public static void RuniLogicRule(Document doc, string ruleName)
        {
            string iLogicGuid = "{3BDD8D79-2179-4B11-8A5A-257B1C0263AC}";
            ApplicationAddIn addin = null;

            try
            {
                addin = Globals.InvApp.ApplicationAddIns.ItemById[iLogicGuid];
            }
            catch (Exception)
            {

                throw;
            }

            if (addin != null)
            {
                try
                {
                    if (!addin.Activated)
                    {
                        addin.Activate();

                    }

                    dynamic iLogicAutomation = addin.Automation;
                    dynamic rule = iLogicAutomation.GetRule(doc, ruleName);

                    if (rule != null)
                    {
                        iLogicAutomation.RunRule(doc, ruleName);

                    }
                }
                catch (Exception)
                {
                }
            }
        }




    }
}
