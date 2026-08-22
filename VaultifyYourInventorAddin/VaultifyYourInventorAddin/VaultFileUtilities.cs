using Autodesk.Connectivity.WebServices;
using Autodesk.DataManagement.Client.Framework.Currency;
using Autodesk.DataManagement.Client.Framework.Vault.Currency.Connections;
using Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities;
using Autodesk.DataManagement.Client.Framework.Vault.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Versioning;
using System.Text;
using ACW = Autodesk.Connectivity.WebServices;
using VDF = Autodesk.DataManagement.Client.Framework;

namespace VaultifyYourInventorAddin
{
    [SupportedOSPlatform("windows7.0")]
    internal class VaultFileUtilities
    {

        public static ACW.File FindFileFromProps(string length, string width, string depth, string searchInFolder)
        {
            if(searchInFolder == "")
            {
                searchInFolder = "$/";
            }

            try
            {
                if(VaultConn.GetActiveConnection == null)
                {
                    throw new InvalidOperationException("Vault connection is not active.");
                }


                ACW.DocumentService docService = VaultConn.GetActiveConnection().WebServiceManager.DocumentService;
                ACW.PropDef[] propDefs = VaultConn.GetActiveConnection().WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");
                List<ACW.SrchCond> searches = new List<ACW.SrchCond>();

                ACW.PropDef lengthPropDef = propDefs.SingleOrDefault(pd => pd.DispName == "LENGTH");
                ACW.PropDef widthPropDef = propDefs.SingleOrDefault(pd => pd.DispName == "WIDTH");
                ACW.PropDef depthPropDef = propDefs.SingleOrDefault(pd => pd.DispName == "DEPTH");
                ACW.PropDef fileExtPropDef = propDefs.SingleOrDefault(pd => pd.DispName == "File Extension");

                ACW.SrchCond lengthSrch;
                ACW.SrchCond widthSrch;
                ACW.SrchCond depthSrch;
                ACW.SrchCond fileExtSrch;

                if (lengthPropDef != null)
                {
                    lengthSrch = new ACW.SrchCond
                    {
                        PropDefId = lengthPropDef.Id,
                        PropTyp = PropertySearchType.SingleProperty,
                        SrchOper = 3,
                        SrchRule = ACW.SearchRuleType.Must,
                        SrchTxt = length
                    };

                    if (lengthSrch != null)
                    {
                        searches.Add(lengthSrch);

                    }

                }


                if (widthPropDef != null)
                {
                    widthSrch = new ACW.SrchCond
                    {
                        PropDefId = widthPropDef.Id,
                        PropTyp = PropertySearchType.SingleProperty,
                        SrchOper = 3,
                        SrchRule = ACW.SearchRuleType.Must,
                        SrchTxt = width
                    };

                    if (widthSrch != null)
                    {
                        searches.Add(widthSrch);

                    }

                }

                if (depthPropDef != null)
                {
                    depthSrch = new ACW.SrchCond
                    {
                        PropDefId = depthPropDef.Id,
                        PropTyp = PropertySearchType.SingleProperty,
                        SrchOper = 3,
                        SrchRule = ACW.SearchRuleType.Must,
                        SrchTxt = depth
                    };

                    if (depthSrch != null)
                    {
                        searches.Add(depthSrch);

                    }
                }

                if (fileExtPropDef != null)
                {
                    fileExtSrch = new ACW.SrchCond
                    {
                        PropDefId = fileExtPropDef.Id,
                        PropTyp = PropertySearchType.SingleProperty,
                        SrchOper = 3,
                        SrchRule = ACW.SearchRuleType.Must,
                        SrchTxt = "ipt"
                    };

                    if (fileExtSrch != null)
                    {
                        searches.Add(fileExtSrch);

                    }
                }

                ACW.Folder[] folders = null;

                folders = docService.FindFoldersByPaths(new[] { searchInFolder });

                if( folders != null)
                {
                    long[] folderIds = folders.Where(pd => pd.Id != -1).Select(pd => pd.Id).ToArray();

                    if (folderIds != null)
                    {
                        string bookmark = string.Empty;
                        ACW.SrchStatus status = null;

                        ACW.File[] files = null;
                        if (searches.Count > 0)
                        {
                            files = docService.FindFilesBySearchConditions(searches.ToArray(), null, folderIds, true, true, ref bookmark, out status);

                            if (files != null)
                            {
                                return files[0];

                            }
                        }

                    }
                }    







            }
            catch (Exception)
            {

                throw;
            }




            return null;
        }


        public static List<ACW.File> FindFilesFromProps(string length = null, string width = null, string depth = null, string searchInFolder = "")
        {
            if (searchInFolder == "")
            {
                searchInFolder = "$/";
            }

            try
            {
                if (VaultConn.GetActiveConnection == null)
                {
                    throw new InvalidOperationException("Vault connection is not active.");
                }


                ACW.DocumentService docService = VaultConn.GetActiveConnection().WebServiceManager.DocumentService;
                ACW.PropDef[] propDefs = VaultConn.GetActiveConnection().WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");
                List<ACW.SrchCond> searches = new List<ACW.SrchCond>();

                ACW.PropDef lengthPropDef = length != null ? propDefs.SingleOrDefault(pd => pd.DispName == "LENGTH") : null;
                ACW.PropDef widthPropDef = width != null ? propDefs.SingleOrDefault(pd => pd.DispName == "WIDTH") : null;
                ACW.PropDef depthPropDef = depth!= null ? propDefs.SingleOrDefault(pd => pd.DispName == "DEPTH"): null;
                ACW.PropDef fileExtPropDef = propDefs.SingleOrDefault(pd => pd.DispName == "File Extension");

                ACW.SrchCond lengthSrch;
                ACW.SrchCond widthSrch;
                ACW.SrchCond depthSrch;
                ACW.SrchCond fileExtSrch;

                if (lengthPropDef != null)
                {
                    lengthSrch = new ACW.SrchCond
                    {
                        PropDefId = lengthPropDef.Id,
                        PropTyp = PropertySearchType.SingleProperty,
                        SrchOper = 3,
                        SrchRule = ACW.SearchRuleType.Must,
                        SrchTxt = length
                    };

                    if (lengthSrch != null)
                    {
                        searches.Add(lengthSrch);

                    }

                }


                if (widthPropDef != null)
                {
                    widthSrch = new ACW.SrchCond
                    {
                        PropDefId = widthPropDef.Id,
                        PropTyp = PropertySearchType.SingleProperty,
                        SrchOper = 3,
                        SrchRule = ACW.SearchRuleType.Must,
                        SrchTxt = width
                    };

                    if (widthSrch != null)
                    {
                        searches.Add(widthSrch);

                    }

                }

                if (depthPropDef != null)
                {
                    depthSrch = new ACW.SrchCond
                    {
                        PropDefId = depthPropDef.Id,
                        PropTyp = PropertySearchType.SingleProperty,
                        SrchOper = 3,
                        SrchRule = ACW.SearchRuleType.Must,
                        SrchTxt = depth
                    };

                    if (depthSrch != null)
                    {
                        searches.Add(depthSrch);

                    }
                }

                if (fileExtPropDef != null)
                {
                    fileExtSrch = new ACW.SrchCond
                    {
                        PropDefId = fileExtPropDef.Id,
                        PropTyp = PropertySearchType.SingleProperty,
                        SrchOper = 3,
                        SrchRule = ACW.SearchRuleType.Must,
                        SrchTxt = "ipt"
                    };

                    if (fileExtSrch != null)
                    {
                        searches.Add(fileExtSrch);

                    }
                }

                ACW.Folder[] folders = null;

                folders = docService.FindFoldersByPaths(new[] { searchInFolder });
                ACW.File[] files = null;
                if (folders != null)
                {
                    long[] folderIds = folders.Where(pd => pd.Id != -1).Select(pd => pd.Id).ToArray();

                    if (folderIds != null)
                    {
                        string bookmark = string.Empty;
                        ACW.SrchStatus status = null;

                        
                        if (searches.Count > 0)
                        {
                            files = docService.FindFilesBySearchConditions(searches.ToArray(), null, folderIds, true, true, ref bookmark, out status);
                        }

                    }
                }


                if (files != null)
                {
                    return files.ToList();
                }




            }
            catch (Exception)
            {

                throw;
            }




            return null;
        }


        public static string AcquireFile(ACW.File file, Boolean checkout)
        {
            string filename = "";
            if (file == null) throw new ArgumentNullException(nameof(file));

            Connection activeConnection = VaultConn.GetActiveConnection();
            if (activeConnection == null) throw new InvalidOperationException("No active Vault connection.");

            VDF.Vault.Services.Connection.IWorkingFoldersManager services = activeConnection.WorkingFoldersManager;

            if (services == null) throw new InvalidOperationException("Working folders manager is unavailable.");

            FileIteration fileIteration = new FileIteration(activeConnection, file);
            Boolean downloadSuccess = true;

            try
            {
                AcquireFilesSettings fileSettings = new AcquireFilesSettings(activeConnection);
                fileSettings.AddFileToAcquire(fileIteration, AcquireFilesSettings.AcquisitionOption.Download);
                fileSettings.OptionsRelationshipGathering.FileRelationshipSettings.IncludeChildren = false;
                fileSettings.OptionsRelationshipGathering.FileRelationshipSettings.IncludeAttachments = false;
                fileSettings.OptionsResolution.OverwriteOption = VDF.Vault.Settings.AcquireFilesSettings.AcquireFileResolutionOptions.OverwriteOptions.ForceOverwriteAll;

                VDF.Vault.Results.AcquireFilesResults results = activeConnection.FileManager.AcquireFiles(fileSettings);
                foreach (VDF.Vault.Results.FileAcquisitionResult result in results.FileResults)
                {
                    if(result.Status != VDF.Vault.Results.FileAcquisitionResult.AcquisitionStatus.Success)
                    {
                        downloadSuccess = false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            if (!downloadSuccess)
            {
                filename = null;

            }

            FilePathAbsolute localPath = services.GetPathOfFileInWorkingFolder(fileIteration);
            return localPath?.FullPath;
        }

        public static string GetNextPartNumber(string scheme)
        {

            ACW.NumberingService numSerice= VaultConn.GetActiveConnection().WebServiceManager.NumberingService;

            ACW.NumSchm[] numSchemes = numSerice.GetNumberingSchemes("FILE", ACW.NumSchmType.Activated);
            ACW.NumSchm numScheme = numSchemes.FirstOrDefault(s => s.Name == scheme);

            if (numScheme == null)
            {
                throw new InvalidOperationException("Numbering Scheme " + scheme + " not found.");
            }

            string[] inputs = { null };
            string newPartNumber = numSerice.GenerateNumberBySchemeId(numScheme.SchmID, inputs);

            return newPartNumber;



        }


    }
}
