#region Using Statements
using System;
using System.IO;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions.Strings;
using static CommonBasicStandardLibraries.CopyVS.CopyHelper;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.FileFunctions.FileFunctions;
using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using System.Threading.Tasks;
using System.Text;
using CommonBasicStandardLibraries.CollectionClasses;
using vb = CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.VBCompat;
using System.Collections.Generic;
#endregion

namespace CommonBasicStandardLibraries.CopyVS
{
    public class DesktopApps
    {
        #region Public Properties

        public string ProjectPath { get; set; }
        public string NewPath { get; set; }
        public string OldName { get; set; }
        public string NewName { get; set; }
        public ISimpleUI ThisMessage { get; set; }//if you want to show messagebox, needs to do through here.
        #endregion

        #region Public Methods

        public async Task CopyAsync() //this will now be async.
        {
            if (ThisMessage == null)
                throw new ArgumentNullException(nameof(ThisMessage)); //this should speak for itself
            string ParentPath = GetParentPath(ProjectPath);
            if (FileExists($@"{ParentPath}\{OldName}.sln") ==true)
            {
                if (FileExists($@"{NewPath}\{NewName}.sln") == true)
                    await DeleteFileAsync($@"{NewPath}\{NewName}.sln");
                await FileCopyAsync($@"{ParentPath}\{OldName}.sln", $@"{ParentPath}\{NewName}.sln");
                string TempPath = $@"{NewPath}\{NewName}.sln";
                string ThisText = await AllTextAsync(NewPath);
                ThisText = ThisText.Replace(OldName, NewName);
                await WriteTextAsync(NewPath, ThisText, false);
            }
            if (DirectoryExists(ProjectPath) == false)
                ThisMessage.ShowError($"'{ProjectPath}': Path doesn't exist");

            if (FileExists($@"{ProjectPath}\{OldName}.vbproj") == false)
                ThisMessage.ShowError($"'{OldName}': does not exist");

            if (DirectoryExists($@"{NewPath}\{NewName}") == true)
            {
                if (await ClearFolderAsync($@"{NewPath}\{NewName}") == true)
                {
                    if (await ClearFolderAsync($@"{NewPath}\{NewName}") == true)
                    {
                        await CopyFolderAsync(new DirectoryInfo(ProjectPath), new DirectoryInfo($@"{NewPath}\{NewName}"));
                        await ProcessRenamingAsync();
                    }
                }
            }
            else
            {
                if (await ClearFolderAsync($@"{NewPath}\{NewName}") == true)
                {
                    await CopyFolderAsync(new DirectoryInfo(ProjectPath), new DirectoryInfo($@"{NewPath}\{NewName}"));
                    await ProcessRenamingAsync();
                }
            }
        }

        #endregion

        #region Private Methods
        private async  Task<bool> ClearFolderAsync(string pPath)
        {
            try
            {

                await Task.Run(() =>
                {
                    Directory.Delete(pPath, true);
                });
                
                
                return true;
            }

            catch(Exception ex)
            {
                ThisMessage.ShowError(ex.Message);
                return false;
            }
        }

        private async  Task<bool> CreateFolderAsync(string pPath)
        {
            try
            {
                await Task.Run(() => Directory.CreateDirectory(pPath));
                return true;
            }
            catch (Exception ex)
            {
                ThisMessage.ShowError(ex.Message);
                return false;
            }
        }

        private async Task CopyFolderAsync(DirectoryInfo SourceDirectory, DirectoryInfo DestinationDirectory) //done i think
        {
            DirectoryInfo[] SourceSubDirectories=null;
            FileInfo[] SourceFiles=null;
            await Task.Run(() =>
            {
                SourceFiles = SourceDirectory.GetFiles();
                SourceSubDirectories = SourceDirectory.GetDirectories();
                if (DestinationDirectory.Exists == false)
                    DestinationDirectory.Create();
            });
            foreach (DirectoryInfo SourceSubDirectory in SourceSubDirectories)
            {
                await CopyFolderAsync(SourceSubDirectory, new DirectoryInfo($@"{DestinationDirectory.FullName}\{SourceSubDirectory.Name}"));
            }
            foreach (FileInfo SourceFile in SourceFiles)
            {
                await Task.Run(() => SourceFile.CopyTo($@"{DestinationDirectory.FullName}\{Path.GetFileName(SourceFile.Name)}"));
            }
        }

        private async Task ChangeProjFileAsync() //done
        {
            await Task.Run(() =>
            {
                string PPath;
                string SPath;
                PPath = $@"{NewPath}\{NewName}\{NewName}.csproj"; //its going to be c# now.  no more vb.net anymore.
                SPath = $@"{NewPath}\{NewName}\{NewName}.sln";
                StreamReader rdr = new StreamReader(PPath, Encoding.Default);
                string allFile = rdr.ReadToEnd();
                rdr.Close();
                StreamWriter wri = new StreamWriter(PPath, false, Encoding.Default);
                allFile = allFile.Replace($"<AssemblyName>{OldName}</AssemblyName>", $"<AssemblyName>{NewName}</AssemblyName>");
                wri.Write(allFile);
                wri.Close();
                rdr = new StreamReader(SPath, Encoding.Default);
                allFile = rdr.ReadToEnd();
                rdr.Close();
                wri = new StreamWriter(SPath, false, Encoding.Default);
                allFile = allFile.Replace(OldName, NewName);
                allFile = GetCleanText(allFile);
                wri.Write(allFile.Trim());
                wri.Close();

            });
        }

        private async Task ProcessRenamingAsync()
        {
            await Task.Run(() =>
            {
            StringBuilder TempOld = new StringBuilder(OldName);
            StringBuilder TempNew = new StringBuilder(NewName);
            TempOld.Replace(" ", "_");

            CustomBasicList<string> NewList = new CustomBasicList<string> { " ", "(", ")", ",", "-" };
            NewList.ForEach(a => TempNew.Replace(a, "_"));
            string FirstDigit = TempNew.ToString().Substring(0, 1);
            bool rets = FirstDigit.IsNumeric();

            if (rets == true)

                TempNew.Insert(0, "_");

            string Path = $@"{NewPath}\{NewName}";
            string NewString = TempNew.ToString();
            string OldString = TempOld.ToString();
            string _FileData;
            StreamReader __StreamReader;
            StreamWriter _StreamWriter;

            if (File.Exists($@"{Path}\{OldName}.csproj") == true)
            {
                __StreamReader = new StreamReader($@"{Path}\{OldName}.csproj", Encoding.Default);
                _FileData = __StreamReader.ReadToEnd();
                __StreamReader.Close();
                _StreamWriter = new StreamWriter($@"{Path}\{OldName}.csproj", false, Encoding.Default);
                StringBuilder TempMiddle = new StringBuilder(_FileData);
                TempMiddle.Replace($"<AssemblyName>{OldName}</AssemblyName>", $"<AssemblyName>{NewName}</AssemblyName>");
                TempMiddle.Replace($"<DocumentationFile>{OldName}.xml</DocumentationFile>", $"<DocumentationFile>{NewName}.xml</DocumentationFile>");
                TempMiddle.Replace($"<RootNamespace>{OldString}</RootNamespace>", $"<RootNamespace>{NewString}</RootNamespace>");
                TempMiddle.Replace($"<StartupObject>{OldString}.My.MyApplication</StartupObject>", $"<StartupObject>{NewString}.My.MyApplication</StartupObject>");
                _StreamWriter.Write(TempMiddle.ToString());
                _StreamWriter.Close();
                File.Move($@"{Path}\{OldName}.csproj", $@"{Path}\{NewName}.csproj"); //has to be this way because the other function is asyn and this is already async
            }

            if (File.Exists($@"{Path}\{OldName}.sln") == true)
            {
                __StreamReader = new StreamReader($@"{Path}\{OldName}.sln", Encoding.Default);
                _FileData = __StreamReader.ReadToEnd();
                __StreamReader.Close();
                _StreamWriter = new StreamWriter($@"{Path}\{OldName}.sln", false, Encoding.Default);
                _FileData = _FileData.Replace(OldName, NewName);
                _FileData = GetCleanText(_FileData);
                _StreamWriter.Write(_FileData.Trim());
                _StreamWriter.Close();
                File.Move($@"{Path}\{OldName}.sln", $@"{Path}\{NewName}.sln");
            }
            if (File.Exists($@"{Path}\{OldName}.suo") == true)
                File.Move($@"{Path}\{OldName}.suo", $@"{Path}\{NewName}.suo");

            if (File.Exists($@"{Path}\{OldName}.csproj.user") == true)
                File.Move($@"{Path}\{OldName}.csproj.user", $@"{Path}\{NewName}.csproj.user");

            if (Directory.Exists($@"{Path}\My Project") == true)
            {
                if (File.Exists($@"{Path}\My Project\Application.Designer.cs") == true)
                {
                    __StreamReader = new StreamReader($@"{Path}\My Project\Application.Designer.cs", Encoding.Default);
                    _FileData = __StreamReader.ReadToEnd();
                    __StreamReader.Close();
                    _StreamWriter = new StreamWriter($@"{Path}\My Project\Application.Designer.cs", false, Encoding.Default);
                    _FileData = _FileData.Replace($"Global.{OldString}", $"Global.{NewString}");
                    _StreamWriter.Write(_FileData);
                    _StreamWriter.Close();
                }

                if (File.Exists($@"{Path}\My Project\Settings.Designer.cs") == true)
                {
                    __StreamReader = new StreamReader($@"{Path}\My Project\Settings.Designer.cs", Encoding.Default);
                    _FileData = __StreamReader.ReadToEnd();
                    __StreamReader.Close();
                    _StreamWriter = new StreamWriter($@"{Path}\My Project\Settings.Designer.cs", false, Encoding.Default);
                    _FileData = _FileData.Replace($"Global.{OldString}", $"Global.{NewString}");
                    _StreamWriter.Write(_FileData);
                    _StreamWriter.Close();
                }

                if (File.Exists($@"{Path}\My Project\AssemblyInfo.cs") == true)
                {
                    __StreamReader = new StreamReader($@"{Path}\My Project\AssemblyInfo.cs", Encoding.Default);
                    _FileData = __StreamReader.ReadToEnd();
                    __StreamReader.Close();
                    _StreamWriter = new StreamWriter($@"{Path}\My Project\AssemblyInfo.cs", false, Encoding.Default);
                    _FileData = _FileData.Replace($"<Assembly: AssemblyTitle({vb.ChrW(34)}{OldName}{vb.ChrW(34)})>", $"<Assembly: AssemblyTitle({vb.ChrW(34)}{OldName}{vb.ChrW(34)})>");
                    _FileData = _FileData.Replace($"<Assembly: AssemblyProduct({vb.ChrW(34)}{OldName}{vb.ChrW(34)})>", $"<Assembly: AssemblyProduct({vb.ChrW(34)}{OldName}{vb.ChrW(34)})>");
                    _StreamWriter.Write(_FileData);
                    _StreamWriter.Close();
                }
                    CustomBasicList<string> TempList;
                    CustomBasicList<string> SearchList = new CustomBasicList<string>() { $@"{Path}\bin\Debug", $@"{Path}\bin\Release", $@"{Path}\obj", $@"{Path}\obj\Debug", $@"{Path}\obj\Release" };
                    SearchList.ForEach(a =>
                    {
                        if (DirectoryExists(a) == true)
                        {
                            TempList = GetFileList(a);
                            TempList.ForEach(b =>
                            {
                                string TempOldName = FullFile(b);
                                if (TempOldName.ToLower().Contains(OldName.ToLower()) == true)
                                {
                                    string TempNewName = TempOldName.Replace(OldName, NewName);
                                    File.Move(b, $@"{a}\{TempNewName}");
                                }
                            });
                        }
                    });
                }
            });
        }

        
        private CustomBasicList<string> GetFileList(string Path)
        {
            CustomBasicList<string> tFileList = new CustomBasicList<string>();
            IEnumerable<string> TempList;
            TempList = Directory.EnumerateDirectories(Path, "*", SearchOption.TopDirectoryOnly);
            foreach (var ThisItem in TempList)
                tFileList.Add(ThisItem);
            return tFileList;

        }



        #endregion


    }
}
