using System;
using System.Text;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using System.Linq;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using CommonBasicStandardLibraries.CollectionClasses;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.NuGetHelpers.NuGetGlobals;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.FileFunctions.FileFunctions;
using js = CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.FileHelpers;
using System.Xml.Linq;
using System.Diagnostics;
//i think this is the most common things i like to do
namespace CommonBasicStandardLibraries.NuGetHelpers
{
    internal class NugetBusiness
    {
        //all business logic here.  i encourage others to use the view model to interact with this.
        private CustomBasicList<string> ProjectList;
        private string DataPath;
        private string BatPath;
        private string DefaultPath;
        private CustomBasicList<VSProject> SavedList;
        private bool CheckedForNuget;
        private bool PrivateHas;
        private const string NugetExt = ".nupkg";

        private async Task<string> GetCSProj(string DirectoryPath)
        {
            return await GetSpecificFile(DirectoryPath, ".csproj");
        }

        public async Task TestTraditionalNet()
        {
            //this is only used temporarily.
            //just until i can get it working for the sample.
            //has to hard code what i am using.   will use console only for showing messages
            Console.WriteLine("Trying To Create Nuget Packages For Traditional .Net Class Libraries");
            //step 1 is generating the nuget xml format
            BatPath = @"C:\TempFiles\E.bat"; //this will be the bat path.
            await WriteAllTextAsync(BatPath, "nuget spec ClassLibrary1.csproj");
            
            ProcessStartInfo psi = new ProcessStartInfo(BatPath);
            psi.WorkingDirectory = @"C:\VS\DesktopDlls4.7.1\Tests\ClassLibrary1";
            await DeleteSeveralFiles(psi.WorkingDirectory, ".nuspec");
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = true;
            Process process = Process.Start(psi);
            bool rets = process.WaitForExit(5000);
            if (rets == true)
            {
                if (await NewFileCreated(psi.WorkingDirectory, ".nuspec") == false)
                    throw new Exception("No File Created");
                string NuPath = await GetSpecificFile(psi.WorkingDirectory, ".nuspec");
                XElement ThisElement = XElement.Load(NuPath);
                XElement TempElement = ThisElement.Elements("metadata").Single();
                TempElement.Elements("licenseUrl").Remove();
                TempElement.Elements("projectUrl").Remove();
                TempElement.Elements("iconUrl").Remove();
                TempElement.Elements("releaseNotes").Remove();
                TempElement.Elements("copyright").Remove();
                TempElement.Elements("tags").Remove();
                ThisElement.Save(NuPath);
                //so far so good.

                //for testing, use shortcut
                await WriteAllTextAsync(BatPath, @"nuget pack -Properties Configuration=Release -OutputDirectory bin\Release -Version 2.0.0");
                string NewPath = $@"{psi.WorkingDirectory}\bin\Release";
                await DeleteSeveralFiles(NewPath, ".nupkg");
                process = Process.Start(psi);
                rets = process.WaitForExit(5000);
                if (rets == true)
                {
                    if (await NewFileCreated(NewPath, ".nupkg") == false)
                        throw new Exception("Rethink");
                    await DeleteSeveralFiles(psi.WorkingDirectory, ".nuspec");
                    Console.WriteLine("I think nuget package was created successfully.  Please Check");
                }
            }
            else
            {
                Console.WriteLine("Rethink");
            }
        }

        private void IncrementVersion(ref string StrVersion) //was going to do as extension but did not work out.  so was forced to just send in for this case.
        {
            var TempList = StrVersion.Split(".").ToCustomBasicList();
            int Version = int.Parse(TempList.Last());
            Version++;
            TempList[TempList.Count - 1] = Version.ToString();
            StrVersion = (string)string.Join(".", TempList);

            Console.WriteLine(StrVersion);
        }

        private async Task SaveUpdatedVersion(string CSFile, string StrVersion) //this will save the project with the updated version.
        {
            await Task.Run(() =>
            {
                XElement ThisXML = XElement.Load(CSFile);
                if (ThisXML.Elements("PropertyGroup").Elements("Version").Count() == 0)
                {
                    XElement NewElement = new XElement("Version", StrVersion);
                    XElement Temps = ThisXML.Elements("PropertyGroup").Single();
                    Temps.Add(NewElement);
                }
                else
                    ThisXML.Elements("PropertyGroup").Elements("Version").Single().Value = StrVersion;
                ThisXML.Save(CSFile);
            });
        }

        private async Task<string> GetVersionNumber(string CSFile)
        {
            string Results = default;
            await Task.Run(() =>
            {
                XElement ThisXML = XElement.Load(CSFile);
                if (ThisXML.Elements("PropertyGroup").Elements("Version").Count() == 0)
                    Results = "1.0.0";
                else
                    Results = ThisXML.Elements("PropertyGroup").Elements("Version").Single().Value;
            });
            return Results;
        }

        private void UpdateProgress(string Text)
        {
            ThisMod.Progress = Text;
            Console.WriteLine(Text);
        }

        public async Task UploadAlone()
        {
            UpdateProgress("Uploading Packages Alone.  Will not even reconcile the list.");
            //this assumes that its already ran once but somehow did not show up so you have to try again.
            await LoadSettings();
            if (SavedList.Count == 0)
                throw new BasicBlankException("You never tried to upload to nuget before");
            await SavedList.ForEachAsync(async ThisItem =>
            {
                await UploadPackageAsync(ThisItem, true);
            });
        }

        private async Task LoadSettings()
        {
            DefaultPath = await ThisSetting.GetDataPath(); //just in case the interface does something complex.
            ProjectList = await ThisSetting.GetProjectLists();
            //key will only be used at the end.
            DataPath = $@"{DefaultPath}\data.json";
            BatPath = $@"{DefaultPath}\nuget.bat";
            if (FileExists(DataPath) == true)
            {
                SavedList = await js.RetrieveSavedObjectAsync<CustomBasicList<VSProject>>(DataPath);
            }
            else
            {
                SavedList = new CustomBasicList<VSProject>();
            }
        }
        public async Task CreateNugetPackage()
        {
            UpdateProgress("Starting The Processes For Creation Of Nuget Packages");
            await LoadSettings();
            CheckedForNuget = false;
            //first check to see if there is anything to add to the saved list.
            UpdateProgress("Start Reconciling To Make Sure The Saved Lists Shows Current Items Needed");
            await ProjectList.ReconcileStrings<VSProject>(SavedList, Items => Items.ProjectDirectory, async Items =>
            {
                string CSProj = await GetCSProj(Items);
                UpdateProgress($"Adding {Items} To The List For Nuget Packages");
                //var ThisFile = await GetFileAsync(CSProj);
                string TempName = $"{FileName(CSProj)}.dll";
                string DllPath = await SearchForFileName($@"{Items}\bin\Release", TempName);
                var ThisFile = await GetFileAsync(DllPath);
                //Microsoft.NET.Sdk
                //bool IsTrad;
                string TempText = await AllTextAsync(CSProj);
                
                VSProject ThisProj = new VSProject()
                {
                    ProjectDirectory = Items,
                    Status = EnumStatus.New, //needs to know its brand new.
                    LastModified = ThisFile.DateModified, //i think
                    LastVersion = await GetVersionNumber(CSProj),
                    CSPath = CSProj,
                    DLLPath = DllPath,
                    NugetPath = $@"{Items}\bin\Release"
                };
                if (TempText.Contains("Microsoft.NET.Sdk") == true)
                {
                    ThisProj.LastVersion = await GetVersionNumber(CSProj);
                    ThisProj.NetVersion = EnumNet.CoreStandard;
                }
                else
                {
                    ThisProj.LastVersion = "1.0.0";
                    ThisProj.NetVersion = EnumNet.Traditional;
                }
                UpdateProgress($"Version Found Is {ThisProj.LastVersion}");
                return ThisProj;
            });

            SavedList.ForConditionalItems(Items => Items.NetVersion == EnumNet.None,
                Items =>
                {
                    Items.NetVersion = EnumNet.CoreStandard;
                }); //tried an experiment where it updated it and it still worked.
            await SaveData();
            await SavedList.ForEachAsync(async ThisItem =>
            {
                await ProcessProject(ThisItem);
            });

        }

        private async Task PrepNugetTraditional(VSProject ThisProj)
        {
            if (CheckedForNuget == true &&  PrivateHas == false)
            {
                UpdateProgress("Previously checked for nuget but don't have it");
                return;
            }
            UpdateProgress("Starting To Prep Traditional .net Framework");
            await DeleteSeveralFiles(ThisProj.ProjectDirectory, NugetExt);
            await WriteAllTextAsync(BatPath, $@"nuget spec {FullFile(ThisProj.CSPath)}");
            ProcessStartInfo psi = new ProcessStartInfo(BatPath);
            psi.WorkingDirectory = ThisProj.ProjectDirectory;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = true;
            Process process = Process.Start(psi);
            string XMLExt = ".nuspec";
            process.WaitForExit(5000);
            CheckedForNuget = true;
            //i think it can ignore this
            if (await NewFileCreated(ThisProj.ProjectDirectory, XMLExt) == false)
            {
                UpdateProgress("Did not detect nuget was running");
                return;
            }
            UpdateProgress($"Checking Working Folder {psi.WorkingDirectory} for extension of {XMLExt}");
            string NuPath = await GetSpecificFile(psi.WorkingDirectory, XMLExt);
            XElement ThisElement = XElement.Load(NuPath);
            XElement TempElement = ThisElement.Elements("metadata").Single();
            TempElement.Elements("licenseUrl").Remove();
            TempElement.Elements("projectUrl").Remove();
            TempElement.Elements("iconUrl").Remove();
            TempElement.Elements("releaseNotes").Remove();
            TempElement.Elements("copyright").Remove();
            TempElement.Elements("tags").Remove();
            ThisElement.Save(NuPath);
            //await DeleteSeveralFiles(psi.WorkingDirectory, XMLExt);
            PrivateHas = true;
            UpdateProgress("Finished Prepping .net framework");
        }

        private async Task ProcessProject(VSProject ThisProj)
        {
            //at first, will only create package but not upload it.
            if (ThisProj.NetVersion == EnumNet.Traditional)
            {
                await PrepNugetTraditional(ThisProj);
                if (PrivateHas == false)
                {
                    UpdateProgress("Since you don't have nuget running, can't resume to create the traditional .net class library to nuget");
                    return;
                }
            }
            UpdateProgress($"Processing {ThisProj.ProjectDirectory}");
            var ThisFile = await GetFileAsync(ThisProj.DLLPath);
            switch (ThisProj.Status)
            {
                case EnumStatus.None:
                    //has to figure out whether it needs to create package or not.
                    DateTime LastSavedDate = ThisProj.LastModified;
                    string VSVersion;
                    if (ThisProj.NetVersion == EnumNet.CoreStandard)
                        VSVersion = await GetVersionNumber(ThisProj.CSPath);
                    else
                        VSVersion = ""; //we will not have the ability to change it if the traditional .net framework
                    if (VSVersion != ThisProj.LastVersion && ThisProj.NetVersion == EnumNet.CoreStandard)
                    {
                        UpdateProgress($"Updating Version To Match Visual Studios Of {VSVersion}");
                        ThisProj.LastVersion = VSVersion; //needs to make sure you match it again since somebody decided to manually change it.
                        await SaveData(); //you need to save it as well.
                        await CreatePackageAsync(ThisProj, false, ThisFile.DateModified); //if you decided to manually change it in visual studio, then use it.
                    }
                    else if (ThisFile.DateModified > LastSavedDate)
                        await CreatePackageAsync(ThisProj, true, ThisFile.DateModified); //since you edited that file, 
                    else if (ThisFile.DateModified != LastSavedDate)
                        throw new BasicBlankException($"The Saved Date Of {ThisFile.DateModified} Has To Be Greater Than The Saved Date Of {ThisProj.LastModified}");
                    UpdateProgress($"No Changes Found From Dll {ThisProj.DLLPath}");
                    break;
                case EnumStatus.NeedCreate:
                    //hopefully can go right to below.
                case EnumStatus.New:
                    await CreatePackageAsync(ThisProj, false, ThisFile.DateModified);
                    break;
                case EnumStatus.NeedsToUpload:
                    await UploadPackageAsync(ThisProj, false);
                    break;
                default:
                    throw new BasicBlankException("Not Supported Currently");
            }
        }

        private async Task SaveData()
        {
            await js.SaveObjectAsync(DataPath, SavedList);
        }

        private async Task CreatePackageAsync(VSProject ThisProj, bool AutoIncrement, DateTime NewDate)
        {
            if (AutoIncrement == true)
            {
                UpdateProgress($"Updating Version For Dll {ThisProj.DLLPath}");
                if (ThisProj.Status == EnumStatus.NeedCreate)
                    throw new BasicBlankException("You cannot increment if you needed to create the nuget package");
                string LastVersion = ThisProj.LastVersion;
                IncrementVersion(ref LastVersion);
                if (LastVersion == "")
                    throw new BasicBlankException("Last Version Can't Be Blank Before Saving");
                ThisProj.LastVersion = LastVersion;
                if (ThisProj.NetVersion == EnumNet.CoreStandard)
                    await SaveUpdatedVersion(ThisProj.CSPath, ThisProj.LastVersion);
                UpdateProgress($"New Version Of Product Is {ThisProj.LastVersion}");
                await SaveData();
            }
            if (ThisProj.Status != EnumStatus.NeedCreate)
            {
                UpdateProgress("Updating Date Modified For New Package");
                ThisProj.LastModified = NewDate;
                ThisProj.Status = EnumStatus.NeedCreate; //you now need to create.
                await SaveData(); //to record the newest version
            }
            UpdateProgress($"Creating Package For {ThisProj.ProjectDirectory}");
            await DeleteSeveralFiles(ThisProj.NugetPath, ".nupkg");

            

            if (ThisProj.NetVersion == EnumNet.CoreStandard)
                await WriteAllTextAsync(BatPath, "dotnet pack -c release --no-build");
            else
            {
                if (ThisProj.LastVersion == "")
                {
                    throw new BasicBlankException("Last Version Can't Be Blank Before Creating Package");
                }
                await WriteAllTextAsync(BatPath, $@"nuget pack -Properties Configuration=Release -OutputDirectory bin\Release -Version {ThisProj.LastVersion}");
            }
               
            ProcessStartInfo psi = new ProcessStartInfo(BatPath);
            psi.WorkingDirectory = ThisProj.ProjectDirectory;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = true;
            Process process = Process.Start(psi);
            bool rets = process.WaitForExit(5000);
            if (rets == false)
                throw new BasicBlankException("Failed To Create Nuget Package");
            rets = await NewFileCreated(ThisProj.NugetPath, ".nupkg");
            if (rets == true)
            {
                if (ThisProj.NetVersion == EnumNet.Traditional)
                    await DeleteSeveralFiles(ThisProj.ProjectDirectory, ".nupkg");
                ThisProj.Status = EnumStatus.NeedsToUpload;
                await SaveData();
                UpdateProgress("Successfully Created Nuget Package");
            }
            else
                throw new BasicBlankException($"Failed To Create Nuget Package Because No File At {ThisProj.NugetPath}.nupkg Exist");
            await UploadPackageAsync(ThisProj, false);
        }

        private async Task UploadPackageAsync(VSProject ThisProj, bool ShowHints)
        {
            UpdateProgress($"Uploading Package {ThisProj.NugetPath}");
            string Key = await ThisSetting.GetKey();
            string FilePath = await GetSpecificFile(ThisProj.NugetPath, ".nupkg");
            string TempName = FullFile(FilePath);
            string TextForBat;
            if (ThisProj.NetVersion == EnumNet.CoreStandard)
                TextForBat = $"dotnet nuget push {TempName} -k {Key} -s https://api.nuget.org/v3/index.json";
            else
                TextForBat = $"nuget push {TempName} {Key} -source https://api.nuget.org/v3/index.json";
            if (ShowHints == true)
                TextForBat = TextForBat + Constants.vbLf + "pause";
            //if uploading alone, putting pause so a person can double check.
            await WriteAllTextAsync(BatPath, TextForBat);
            ProcessStartInfo psi = new ProcessStartInfo(BatPath);
            psi.WorkingDirectory = ThisProj.NugetPath;
            if (ShowHints == false)
            {
                psi.CreateNoWindow = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.UseShellExecute = true;
            }
            else
            {
                psi.WindowStyle = ProcessWindowStyle.Normal;
                psi.CreateNoWindow = false;
                psi.UseShellExecute = true;
            }
            Process process = Process.Start(psi);
            bool rets = process.WaitForExit(200000);
            if (rets == false)
                throw new BasicBlankException("Failed To Upload Nuget Package");
            else
            {
                if (ShowHints == false)
                {
                    ThisProj.Status = EnumStatus.None;
                    await SaveData();
                }
                            
                UpdateProgress($"Successfully Uploaded Nuget Package For {ThisProj.ProjectDirectory}");
            }
        }
    }
}