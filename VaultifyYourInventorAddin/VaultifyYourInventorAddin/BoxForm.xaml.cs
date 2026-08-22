using Autodesk.Connectivity.WebServices;
using Autodesk.DataManagement.Client.Framework.Currency;
using Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities;
using Inventor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ACW = Autodesk.Connectivity.WebServices;
using VDF = Autodesk.DataManagement.Client.Framework;

namespace VaultifyYourInventorAddin
{


    /// <summary>
    /// Interaction logic for BoxForm.xaml
    /// </summary>
    [SupportedOSPlatform("windows7.0")]
    public partial class BoxForm : Window
    {
        private List<ACW.File> BoxFiles;
        private PropertyService PropService;
        private long[] BoxIds;
        private PropDef LengthDef;
        private PropDef WidthDef;
        private PropDef DepthDef;
        private ACW.PropInst[] BoxProps;
        private string VaultLocal;
        private ACW.File ChosenBox;
        private FileIteration ChosenBoxIteration;
        private VDF.Vault.Services.Connection.IWorkingFoldersManager FolderManager;
        private FilePathAbsolute ChosenBoxPath;
        private string Length;
        private string Width;
        private string Depth;

        public BoxForm()
        {
            InitializeComponent();
            LoadBoxDate();
            

        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {






            DialogResult = true;
            Close();
        }




        private void LoadBoxDate()
        {

            try
            {

                VaultLocal = "$/AU";
                BoxFiles = VaultFileUtilities.FindFilesFromProps(null, null, null, VaultLocal);

                BoxIds = BoxFiles.Select(f => f.Id).ToArray();

                FolderManager = VaultConn.GetActiveConnection().WorkingFoldersManager;
                PropService = VaultConn.GetActiveConnection().WebServiceManager.PropertyService;
                ACW.PropDef[] propDefs = PropService.GetPropertyDefinitionsByEntityClassId("FILE");


                LengthDef = FindPropDef(propDefs, "LENGTH");
                WidthDef = FindPropDef(propDefs, "WIDTH");
                DepthDef = FindPropDef(propDefs, "DEPTH"); 

                if (LengthDef == null || WidthDef == null || DepthDef == null)
                {
                    MessageBox.Show("One or more required properties (Length, Width, Depth) were not found.", 
                        "Missing Properties", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                BoxProps = PropService.GetPropertiesByEntityIds("FILE", BoxIds);

                List<string> ValuesFor(long defId) => BoxProps
                    .Where(pi => pi.PropDefId == defId)
                    .Select(pi => pi.Val?.ToString())
                    .Where(v => !string.IsNullOrEmpty(v))
                    .Distinct()
                    .OrderBy(v => v)
                    .ToList();


                LengthCombo.ItemsSource = ValuesFor(LengthDef.Id);
                WidthCombo.ItemsSource = ValuesFor(WidthDef.Id);
                DepthCombo.ItemsSource = ValuesFor(DepthDef.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to laod box data." + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }


        private static ACW.PropDef FindPropDef(ACW.PropDef[] propDefs, string dispName)
        {
            return propDefs.FirstOrDefault(pd => pd.DispName == dispName);
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            string asmTemplatePath = @"C:\AU2026\AU\Templates\Standard.iam";
            string newAsmAssetName = @"C:\AU2026\AU\" + "BX" + VaultFileUtilities.GetNextPartNumber("BoxAssy") + ".iam";
            
            if( ! Utilities.ForceCopyInventorAsset(asmTemplatePath, newAsmAssetName))
            {

                throw new Exception("Could not copy assembly asset to" + newAsmAssetName);

            }

            AssemblyDocument asmDocument = null;
            try
            {
                asmDocument = (AssemblyDocument)Globals.InvApp.Documents.Open(newAsmAssetName, true);
                asmDocument.Save();
            }
            catch (Exception)
            {

                throw;
            }

            if (asmDocument != null)
            {
                Inventor.Matrix matrix = Globals.InvApp.TransientGeometry.CreateMatrix();
                if (ChosenBox != null)
                {

                    
                    asmDocument.ComponentDefinition.Occurrences.Add(ChosenBoxPath.ToString(), matrix);

                }
                else
                {
                    string partTemplatePath = @"C:\AU2026\AU\Box Asset.ipt";
                    string newPartNumber = "BX" + VaultFileUtilities.GetNextPartNumber("BoxPart") +".ipt";
                    string newPartAssetName = @"C:\AU2026\AU\"  + newPartNumber ;

                    if (!Utilities.ForceCopyInventorAsset(partTemplatePath, newPartAssetName))
                    {

                        throw new Exception("Could not copy assembly asset to" + newPartAssetName);

                    }

                    ComponentOccurrence boxPart =  asmDocument.ComponentDefinition.Occurrences.Add(newPartAssetName, matrix);
                    Document boxPartDoc = (Document)boxPart.Definition.Document;

                    InventorUtilities.SetPropertyValue(boxPartDoc, "Part Number", newPartNumber) ;
                    InventorUtilities.SetPropertyValue(boxPartDoc, "LENGTH", Length);
                    InventorUtilities.SetPropertyValue(boxPartDoc, "WIDTH", Width);
                    InventorUtilities.SetPropertyValue(boxPartDoc, "DEPTH", Depth);

                    boxPart.Edit();
                    InventorUtilities.RuniLogicRule(boxPartDoc, "PushProperties");
                    boxPart.ExitEdit(ExitTypeEnum.kExitToTop);
                }
            }


            this.Close();

        }





        private void ValidatePart()
        {
            Length = LengthCombo.SelectedValue != null ? LengthCombo.SelectedValue.ToString() : "";
            Width = WidthCombo.SelectedValue != null ? WidthCombo.SelectedValue.ToString() : "";
             Depth = DepthCombo.SelectedValue != null? DepthCombo.SelectedValue.ToString() : "";

            if (Length != null & Width != null & Depth != null)
            {
                ChosenBox = VaultFileUtilities.FindFileFromProps(Length, Width, Depth, VaultLocal);

                if (ChosenBox != null)
                {
                    ChosenBoxIteration = new FileIteration(VaultConn.GetActiveConnection(), ChosenBox);
                    if (ChosenBoxIteration != null)
                    {
                        ChosenBoxPath = FolderManager.GetPathOfFileInWorkingFolder(ChosenBoxIteration);
                    }
                    
                }
                

            }

            if (ChosenBox != null)
            {
                ValidationText.Text = "Box exists!";
                ValidationText.Foreground = System.Windows.Media.Brushes.Green;
            }
            else
            {
                ValidationText.Text = "Box doesn't exit!";
                ValidationText.Foreground = System.Windows.Media.Brushes.Red;
            }

        }

        private void LengthCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidatePart();
        }

        private void WidthCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidatePart();
        }

        private void DepthCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidatePart();
        }
    }
}
