using Inventor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using stdole;
using System.IO;
using System.Windows.Forms;


namespace VaultifyYourInventorAddin
{
    internal static class Utilities
    {
        public static Inventor.ButtonDefinition CreateButtonDef(string DisplayName,
           string InternalName,
           string ToolTip = "",
           Image SmallIcon = null,
           Image LargeIcon = null)
        {
            Inventor.ButtonDefinition? existingDef = null;

            try
            {
                existingDef = Globals.InvApp.CommandManager.ControlDefinitions[InternalName] as ButtonDefinition;
            }
            catch (Exception)
            {
                // Optionally log exception
            }

            if (existingDef != null)
            {
                // Optionally show a message to the user
                return existingDef;
            }

            IPictureDisp iPic16 = null;
            if (SmallIcon != null)
            {
                try
                {
                    iPic16 = ImageConverterUtilities.ToIPictureDisp(SmallIcon);
                }
                catch (Exception)
                {
                    // Optionally show a message to the user
                }
            }

            IPictureDisp iPic32 = null;
            if (LargeIcon != null)
            {
                try
                {
                    iPic32 = ImageConverterUtilities.ToIPictureDisp(LargeIcon);
                }
                catch (Exception)
                {
                    // Optionally show a message to the user
                }
            }

            try
            {
                Inventor.ControlDefinitions controlDefs = Globals.InvApp.CommandManager.ControlDefinitions;

                ButtonDefinition btnDef = controlDefs.AddButtonDefinition(DisplayName,
                                                                           InternalName,
                                                                           CommandTypesEnum.kShapeEditCmdType,
                                                                           Globals.InvAppGuidID,
                                                                           "",
                                                                           ToolTip,
                                                                           iPic16,
                                                                           iPic32);
                return btnDef;
            }
            catch (Exception)
            {
                return null;
            }
        }



        public static Boolean ForceCopyInventorAsset(string originalFile, string newFile)
        {

            try
            {
                if (!Directory.Exists(newFile.DirectoryName()))
                {
                    Directory.CreateDirectory(newFile.DirectoryName());
                }
                System.IO.File.Copy(originalFile, newFile, true);
                FileInfo newFileInfo = new FileInfo(newFile) { IsReadOnly = false };
                return true;

            }
            catch (Exception)
            {

                throw;
            }


            return false;
        }



        public static string DirectoryName(this string incString)
        {
            try
            {
                return System.IO.Path.GetDirectoryName(incString);
            }
            catch (Exception)
            {
                return null;
           
            }



        }


    }
}
