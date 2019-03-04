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

        private async Task<string> GetCSProj(string DirectoryPath)
        {
            return await GetSpecificFile(DirectoryPath, ".csproj");
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
        public async Task CreateNugetPackage()
        {
            UpdateProgress("Starting The Processes For Creation Of Nuget Packages");
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
                UpdateProgress($"Version Found Is {ThisProj.LastVersion}");
                return ThisProj;
            });
            await SaveData();
            await SavedList.ForEachAsync(async ThisItem =>
            {
                await ProcessProject(ThisItem);
            });

        }

        private async Task ProcessProject(VSProject ThisProj)
        {
            //at first, will only create package but not upload it.
            UpdateProgress($"Processing {ThisProj.ProjectDirectory}");
            var ThisFile = await GetFileAsync(ThisProj.DLLPath);
            switch (ThisProj.Status)
            {
                case EnumStatus.None:
                    //has to figure out whether it needs to create package or not.
                    DateTime LastSavedDate = ThisProj.LastModified;
                   
                    string VSVersion = await GetVersionNumber(ThisProj.CSPath);
                    if (VSVersion != ThisProj.LastVersion)
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
                    await UploadPackageAsync(ThisProj);
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
                ThisProj.LastVersion = LastVersion;
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
            await WriteAllTextAsync(BatPath, "dotnet pack -c release --no-build");
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
                ThisProj.Status = EnumStatus.NeedsToUpload;
                await SaveData();
                UpdateProgress("Successfully Created Nuget Package");
            }
            else
                throw new BasicBlankException($"Failed To Create Nuget Package Because No File At {ThisProj.NugetPath}.nupkg Exist");
            await UploadPackageAsync(ThisProj);
        }
        private async Task UploadPackageAsync(VSProject ThisProj)
        {
            UpdateProgress($"Uploading Package {ThisProj.NugetPath}");
            string Key = await ThisSetting.GetKey();
            string FilePath = await GetSpecificFile(ThisProj.NugetPath, ".nupkg");
            string TempName = FullFile(FilePath);
            await WriteAllTextAsync(BatPath, $"dotnet nuget push {TempName} -k {Key} -s https://api.nuget.org/v3/index.json");
            ProcessStartInfo psi = new ProcessStartInfo(BatPath);
            psi.WorkingDirectory = ThisProj.ProjectDirectory;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = true;
            Process process = Process.Start(psi);
            bool rets = process.WaitForExit(200000);
            if (rets == false)
                throw new BasicBlankException("Failed To Upload Nuget Package");
            else
            {
                ThisProj.Status = EnumStatus.None;
                await SaveData();            
                UpdateProgress($"Successfully Uploaded Nuget Package For {ThisProj.ProjectDirectory}");
            }
        }
    }
}