using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.FileFunctions.FileFunctions;
using static CommonBasicStandardLibraries.NuGetHelpers.NuGetGlobals;
using js = CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.FileHelpers;
namespace CommonBasicStandardLibraries.NuGetHelpers
{
    internal class NugetBusiness
    {
        //all business logic here.  i encourage others to use the view model to interact with this.
        private CustomBasicList<string>? _projectList;
        private string _dataPath = "";
        private string _batPath = "";
        private string _defaultPath = "";
        private CustomBasicList<VSProject>? _savedList;
        private bool _checkedForNuget;
        private bool _privateHas;
        private const string _nugetExt = ".nupkg";
        private async Task<string> GetCSProj(string directoryPath)
        {
            return await GetSpecificFileAsync(directoryPath, ".csproj");
        }
        //risk not having the test anymore.

        //public async Task TestTraditionalNet()
        //{
        //    //this is only used temporarily.
        //    //just until i can get it working for the sample.
        //    //has to hard code what i am using.   will use console only for showing messages
        //    Console.WriteLine("Trying To Create Nuget Packages For Traditional .Net Class Libraries");
        //    //step 1 is generating the nuget xml format
        //    _batPath = @"C:\TempFiles\E.bat"; //this will be the bat path.
        //    await WriteAllTextAsync(_batPath, "nuget spec ClassLibrary1.csproj");

        //    ProcessStartInfo psi = new ProcessStartInfo(_batPath);
        //    psi.WorkingDirectory = @"C:\VS\DesktopDlls4.7.1\Tests\ClassLibrary1";
        //    await DeleteSeveralFiles(psi.WorkingDirectory, ".nuspec");
        //    psi.CreateNoWindow = true;
        //    psi.WindowStyle = ProcessWindowStyle.Hidden;
        //    psi.UseShellExecute = true;
        //    Process process = Process.Start(psi);
        //    bool rets = process.WaitForExit(5000);
        //    if (rets == true)
        //    {
        //        if (await NewFileCreated(psi.WorkingDirectory, ".nuspec") == false)
        //            throw new Exception("No File Created");
        //        string NuPath = await GetSpecificFile(psi.WorkingDirectory, ".nuspec");
        //        XElement ThisElement = XElement.Load(NuPath);
        //        XElement TempElement = ThisElement.Elements("metadata").Single();
        //        TempElement.Elements("licenseUrl").Remove();
        //        TempElement.Elements("projectUrl").Remove();
        //        TempElement.Elements("iconUrl").Remove();
        //        TempElement.Elements("releaseNotes").Remove();
        //        TempElement.Elements("copyright").Remove();
        //        TempElement.Elements("tags").Remove();
        //        ThisElement.Save(NuPath);
        //        //so far so good.

        //        //for testing, use shortcut
        //        await WriteAllTextAsync(_batPath, @"nuget pack -Properties Configuration=Release -OutputDirectory bin\Release -Version 2.0.0");
        //        string NewPath = $@"{psi.WorkingDirectory}\bin\Release";
        //        await DeleteSeveralFiles(NewPath, ".nupkg");
        //        process = Process.Start(psi);
        //        rets = process.WaitForExit(5000);
        //        if (rets == true)
        //        {
        //            if (await NewFileCreated(NewPath, ".nupkg") == false)
        //                throw new Exception("Rethink");
        //            await DeleteSeveralFiles(psi.WorkingDirectory, ".nuspec");
        //            Console.WriteLine("I think nuget package was created successfully.  Please Check");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("Rethink");
        //    }
        //}

        private void IncrementVersion(ref string strVersion) //was going to do as extension but did not work out.  so was forced to just send in for this case.
        {
            var tempList = strVersion.Split(".").ToCustomBasicList();
            int version = int.Parse(tempList.Last());
            version++;
            tempList[tempList.Count - 1] = version.ToString();
            strVersion = string.Join(".", tempList);
            Console.WriteLine(strVersion);
        }
        private async Task SaveUpdatedVersionAsync(string csFile, string strVersion) //this will save the project with the updated version.
        {
            await Task.Run(() =>
            {
                XElement thisXML = XElement.Load(csFile);
                if (thisXML.Elements("PropertyGroup").Elements("Version").Count() == 0)
                {
                    XElement newElement = new XElement("Version", strVersion);
                    XElement temps = thisXML.Elements("PropertyGroup").Single();
                    temps.Add(newElement);
                }
                else
                {
                    thisXML.Elements("PropertyGroup").Elements("Version").Single().Value = strVersion;
                }

                thisXML.Save(csFile);
            });
        }
        private async Task<string> GetVersionNumberAsync(string csFile)
        {
            string results = "";
            await Task.Run(() =>
            {
                XElement thisXML = XElement.Load(csFile);
                if (thisXML.Elements("PropertyGroup").Elements("Version").Count() == 0)
                    results = "1.0.0";
                else
                    results = thisXML.Elements("PropertyGroup").Elements("Version").Single().Value;
            });
            return results;
        }
        private void UpdateProgress(string text)
        {
            ThisMod!.Progress = text;
            Console.WriteLine(text);
        }
        public async Task UploadAloneAsync()
        {
            UpdateProgress("Uploading Packages Alone.  Will not even reconcile the list.");
            //this assumes that its already ran once but somehow did not show up so you have to try again.
            await LoadSettings();
            if (_savedList!.Count == 0)
                throw new BasicBlankException("You never tried to upload to nuget before");
            await _savedList.ForEachAsync(async thisItem =>
            {
                await UploadPackageAsync(thisItem, true);
            });
        }

        private async Task LoadSettings()
        {
            _defaultPath = await ThisSetting!.GetDataPathAsync(); //just in case the interface does something complex.
            _projectList = await ThisSetting.GetProjectListsAsync();
            //key will only be used at the end.
            _dataPath = $@"{_defaultPath}\data.json";
            _batPath = $@"{_defaultPath}\nuget.bat";
            if (FileExists(_dataPath) == true)
            {
                _savedList = await js.RetrieveSavedObjectAsync<CustomBasicList<VSProject>>(_dataPath);
            }
            else
            {
                _savedList = new CustomBasicList<VSProject>();
            }
        }
        public async Task CreateNugetPackage()
        {
            UpdateProgress("Starting The Processes For Creation Of Nuget Packages");
            await LoadSettings();
            _checkedForNuget = false;
            //first check to see if there is anything to add to the saved list.
            UpdateProgress("Start Reconciling To Make Sure The Saved Lists Shows Current Items Needed");
            await _projectList!.ReconcileStrings<VSProject>(_savedList!, items => items.ProjectDirectory, async Items =>
            {
                string csProj = await GetCSProj(Items);
                UpdateProgress($"Adding {Items} To The List For Nuget Packages");
                string tempName = $"{FileName(csProj)}.dll";
                string dllPath = await SearchForFileNameAsync($@"{Items}\bin\Release", tempName);
                var thisFile = await GetFileAsync(dllPath);
                string tempText = await AllTextAsync(csProj);
                VSProject thisProj = new VSProject()
                {
                    ProjectDirectory = Items,
                    Status = EnumStatus.New, //needs to know its brand new.
                    LastModified = thisFile!.DateModified, //i think
                    LastVersion = await GetVersionNumberAsync(csProj),
                    CSPath = csProj,
                    DLLPath = dllPath,
                    NugetPath = $@"{Items}\bin\Release"
                };
                if (tempText.Contains("Microsoft.NET.Sdk") == true)
                {
                    thisProj.LastVersion = await GetVersionNumberAsync(csProj);
                    thisProj.NetVersion = EnumNet.CoreStandard;
                }
                else
                {
                    thisProj.LastVersion = "1.0.0";
                    thisProj.NetVersion = EnumNet.Traditional;
                }
                UpdateProgress($"Version Found Is {thisProj.LastVersion}");
                return thisProj;
            });

            _savedList!.ForConditionalItems(items => items.NetVersion == EnumNet.None,
                Items =>
                {
                    Items.NetVersion = EnumNet.CoreStandard;
                }); //tried an experiment where it updated it and it still worked.
            await SaveDataAsync();
            await _savedList.ForEachAsync(async ThisItem =>
            {
                await ProcessProject(ThisItem);
            });

        }

        private async Task PrepNugetTraditionalAsync(VSProject thisProj)
        {
            if (_checkedForNuget == true && _privateHas == false)
            {
                UpdateProgress("Previously checked for nuget but don't have it");
                return;
            }
            UpdateProgress("Starting To Prep Traditional .net Framework");
            await DeleteSeveralFilesAsync(thisProj.ProjectDirectory, _nugetExt);
            await WriteAllTextAsync(_batPath, $@"nuget spec {FullFile(thisProj.CSPath)}");
            ProcessStartInfo psi = new ProcessStartInfo(_batPath);
            psi.WorkingDirectory = thisProj.ProjectDirectory;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = true;
            Process process = Process.Start(psi);
            string xmlExt = ".nuspec";
            process.WaitForExit(5000);
            _checkedForNuget = true;
            if (await NewFileCreatedAsync(thisProj.ProjectDirectory, xmlExt) == false)
            {
                UpdateProgress("Did not detect nuget was running");
                return;
            }
            UpdateProgress($"Checking Working Folder {psi.WorkingDirectory} for extension of {xmlExt}");
            string nuPath = await GetSpecificFileAsync(psi.WorkingDirectory, xmlExt);
            XElement thisElement = XElement.Load(nuPath);
            XElement tempElement = thisElement.Elements("metadata").Single();
            tempElement.Elements("licenseUrl").Remove();
            tempElement.Elements("projectUrl").Remove();
            tempElement.Elements("iconUrl").Remove();
            tempElement.Elements("releaseNotes").Remove();
            tempElement.Elements("copyright").Remove();
            tempElement.Elements("tags").Remove();
            thisElement.Save(nuPath);
            _privateHas = true;
            UpdateProgress("Finished Prepping .net framework");
        }
        private async Task ProcessProject(VSProject thisProj)
        {
            //at first, will only create package but not upload it.
            if (thisProj.NetVersion == EnumNet.Traditional)
            {
                await PrepNugetTraditionalAsync(thisProj);
                if (_privateHas == false)
                {
                    UpdateProgress("Since you don't have nuget running, can't resume to create the traditional .net class library to nuget");
                    return;
                }
            }
            UpdateProgress($"Processing {thisProj.ProjectDirectory}");
            var thisFile = await GetFileAsync(thisProj.DLLPath);
            switch (thisProj.Status)
            {
                case EnumStatus.None:
                    //has to figure out whether it needs to create package or not.
                    DateTime? lastSavedDate = thisProj.LastModified!.Value;
                    string vsVersion;
                    if (thisProj.NetVersion == EnumNet.CoreStandard)
                        vsVersion = await GetVersionNumberAsync(thisProj.CSPath);
                    else
                        vsVersion = ""; //we will not have the ability to change it if the traditional .net framework
                    if (vsVersion != thisProj.LastVersion && thisProj.NetVersion == EnumNet.CoreStandard)
                    {
                        UpdateProgress($"Updating Version To Match Visual Studios Of {vsVersion}");
                        thisProj.LastVersion = vsVersion; //needs to make sure you match it again since somebody decided to manually change it.
                        await SaveDataAsync(); //you need to save it as well.
                        await CreatePackageAsync(thisProj, false, thisFile!.DateModified); //if you decided to manually change it in visual studio, then use it.
                    }
                    else if (thisFile!.DateModified > lastSavedDate)
                    {
                        await CreatePackageAsync(thisProj, true, thisFile.DateModified); //since you edited that file, 
                    }
                    else if (thisFile.DateModified != lastSavedDate)
                    {
                        throw new BasicBlankException($"The Saved Date Of {thisFile.DateModified} Has To Be Greater Than The Saved Date Of {thisProj.LastModified}");
                    }

                    UpdateProgress($"No Changes Found From Dll {thisProj.DLLPath}");
                    break;
                case EnumStatus.NeedCreate:
                //hopefully can go right to below.
                case EnumStatus.New:
                    await CreatePackageAsync(thisProj, false, thisFile!.DateModified);
                    break;
                case EnumStatus.NeedsToUpload:
                    await UploadPackageAsync(thisProj, false);
                    break;
                default:
                    throw new BasicBlankException("Not Supported Currently");
            }
        }
        private async Task SaveDataAsync()
        {
            await js.SaveObjectAsync(_dataPath, _savedList!);
        }

        private async Task CreatePackageAsync(VSProject thisProj, bool autoIncrement, DateTime newDate)
        {
            if (autoIncrement == true)
            {
                UpdateProgress($"Updating Version For Dll {thisProj.DLLPath}");
                if (thisProj.Status == EnumStatus.NeedCreate)
                    throw new BasicBlankException("You cannot increment if you needed to create the nuget package");
                string lastVersion = thisProj.LastVersion;
                IncrementVersion(ref lastVersion);
                if (lastVersion == "")
                    throw new BasicBlankException("Last Version Can't Be Blank Before Saving");
                thisProj.LastVersion = lastVersion;
                if (thisProj.NetVersion == EnumNet.CoreStandard)
                    await SaveUpdatedVersionAsync(thisProj.CSPath, thisProj.LastVersion);
                UpdateProgress($"New Version Of Product Is {thisProj.LastVersion}");
                await SaveDataAsync();
            }
            if (thisProj.Status != EnumStatus.NeedCreate)
            {
                UpdateProgress("Updating Date Modified For New Package");
                thisProj.LastModified = newDate;
                thisProj.Status = EnumStatus.NeedCreate; //you now need to create.
                await SaveDataAsync(); //to record the newest version
            }
            UpdateProgress($"Creating Package For {thisProj.ProjectDirectory}");
            await DeleteSeveralFilesAsync(thisProj.NugetPath, ".nupkg");
            if (thisProj.NetVersion == EnumNet.CoreStandard)
            {
                await WriteAllTextAsync(_batPath, "dotnet pack -c release --no-build");
            }
            else
            {
                if (thisProj.LastVersion == "")
                {
                    throw new BasicBlankException("Last Version Can't Be Blank Before Creating Package");
                }
                await WriteAllTextAsync(_batPath, $@"nuget pack -Properties Configuration=Release -OutputDirectory bin\Release -Version {thisProj.LastVersion}");
            }

            ProcessStartInfo psi = new ProcessStartInfo(_batPath);
            psi.WorkingDirectory = thisProj.ProjectDirectory;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = true;
            Process process = Process.Start(psi);
            bool rets = process.WaitForExit(5000);
            if (rets == false)
                throw new BasicBlankException("Failed To Create Nuget Package");
            rets = await NewFileCreatedAsync(thisProj.NugetPath, ".nupkg");
            if (rets == true)
            {
                if (thisProj.NetVersion == EnumNet.Traditional)
                    await DeleteSeveralFilesAsync(thisProj.ProjectDirectory, ".nupkg");
                thisProj.Status = EnumStatus.NeedsToUpload;
                await SaveDataAsync();
                UpdateProgress("Successfully Created Nuget Package");
            }
            else
            {
                throw new BasicBlankException($"Failed To Create Nuget Package Because No File At {thisProj.NugetPath}.nupkg Exist");
            }

            await UploadPackageAsync(thisProj, false);
        }

        private async Task UploadPackageAsync(VSProject thisProj, bool showHints)
        {
            UpdateProgress($"Uploading Package {thisProj.NugetPath}");
            string key = await ThisSetting!.GetKeyAsync();
            string filePath = await GetSpecificFileAsync(thisProj.NugetPath, ".nupkg");
            string tempName = FullFile(filePath);
            string textForBat;
            if (thisProj.NetVersion == EnumNet.CoreStandard)
                textForBat = $"dotnet nuget push {tempName} -k {key} -s https://api.nuget.org/v3/index.json";
            else
                textForBat = $"nuget push {tempName} {key} -source https://api.nuget.org/v3/index.json";
            if (showHints == true)
                textForBat = textForBat + Constants.vbLf + "pause";
            await WriteAllTextAsync(_batPath, textForBat);
            ProcessStartInfo psi = new ProcessStartInfo(_batPath);
            psi.WorkingDirectory = thisProj.NugetPath;
            if (showHints == false)
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
            {
                throw new BasicBlankException("Failed To Upload Nuget Package");
            }
            else
            {
                if (showHints == false)
                {
                    thisProj.Status = EnumStatus.None;
                    await SaveDataAsync();
                }

                UpdateProgress($"Successfully Uploaded Nuget Package For {thisProj.ProjectDirectory}");
            }
        }
    }
}