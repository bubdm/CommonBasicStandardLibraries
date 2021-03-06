#region Using Statements
using CommonBasicStandardLibraries.CollectionClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions.Strings;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.FileFunctions.FileFunctions;
using static CommonBasicStandardLibraries.CopyVS.CopyHelper;
using vb = CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.VBCompat;
using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
#endregion

namespace CommonBasicStandardLibraries.CopyVS
{
    public delegate Task FinishFileProcesses(CustomBasicList<string> files, string oldName, string newName, string newProjectFile);
    public class DesktopApps
    {
        #region Public Properties

        public string ProjectPath { get; set; } = "";
        public string NewPath { get; set; } = "";
        public string OldName { get; set; } = "";
        public string NewName { get; set; } = "";
        /// <summary>
        /// the purpose of this would be sometimes one project happens to be webstuff.
        /// </summary>
        public bool IncludeWebFiles { get; set; }
        /// <summary>
        /// sometimes blazor is needed.  the extension is .razor.
        /// </summary>
        public bool IncludeRazorFiles { get; set; }


        public FinishFileProcesses? FinishRenaming { get; set; }

        #endregion

        #region Public Methods

        public async Task CopyAsync() //this will now be async.
        {
            string parentPath = GetParentPath(ProjectPath);
            if (FileExists($@"{parentPath}\{OldName}.sln") == true)
            {
                if (FileExists($@"{NewPath}\{NewName}.sln") == true)
                    await DeleteFileAsync($@"{NewPath}\{NewName}.sln");
                await FileCopyAsync($@"{parentPath}\{OldName}.sln", $@"{parentPath}\{NewName}.sln");
                string thisText = await AllTextAsync(NewPath);
                thisText = thisText.Replace(OldName, NewName);
                await WriteTextAsync(NewPath, thisText, false);
            }
            if (DirectoryExists(ProjectPath) == false)
            {
                UIPlatform.ShowError($"'{ProjectPath}': Path doesn't exist");
                return;
            }
            if (FileExists($@"{ProjectPath}\{OldName}.csproj") == false)
            {
                UIPlatform.ShowError($"'{OldName}': does not exist");
                return;
            }
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
            if (FinishRenaming != null)
            {
                string newProjectPath = $@"{NewPath}\{NewName}\{NewName}.csproj";
                if (FileExists(newProjectPath) == false)
                {
                    UIPlatform.ShowError($"Path of {newProjectPath} does not exist.  Rethink");
                    return;
                }
                string checkPath = $@"{NewPath}\{NewName}";
                Console.WriteLine($"Checking path {checkPath}");
                var list = GetSeveralSpecificFiles(checkPath, "cs", SearchOption.AllDirectories);

                CustomBasicList<string> extensions = new CustomBasicList<string>();
                if (IncludeWebFiles)
                {
                    extensions = new CustomBasicList<string>()
                    {
                        "css", "js", "ts", "html"
                    };
                }
                if (IncludeRazorFiles)
                {
                    extensions.Add("razor");
                    //var nexts = GetSeveralSpecificFiles(checkPath, "razor", SearchOption.AllDirectories);
                    //list.AddRange(nexts);
                }
                extensions.ForEach(e =>
                {
                    var nexts = GetSeveralSpecificFiles(checkPath, e, SearchOption.AllDirectories);
                    list.AddRange(nexts);
                });

                await FinishRenaming.Invoke(list, OldName, NewName, newProjectPath);
            }


        }
        #endregion

        #region Private Methods
        private async Task<bool> ClearFolderAsync(string pPath)
        {
            try
            {
                await Task.Run(() =>
                {
                    if (Directory.Exists(pPath))
                    {
                        Directory.Delete(pPath, true);
                    }
                });
                return true;
            }
            catch (Exception ex)
            {
                UIPlatform.ShowError(ex.Message);
                return false;
            }
        }
        private async Task CopyFolderAsync(DirectoryInfo sourceDirectory, DirectoryInfo destinationDirectory) //done i think
        {
            DirectoryInfo[]? SourceSubDirectories = null;
            FileInfo[]? SourceFiles = null;
            await Task.Run(() =>
            {
                SourceFiles = sourceDirectory.GetFiles();
                SourceSubDirectories = sourceDirectory.GetDirectories();
                if (destinationDirectory.Exists == false)
                    destinationDirectory.Create();
            });
            foreach (DirectoryInfo sourceSubDirectory in SourceSubDirectories!)
            {
                await CopyFolderAsync(sourceSubDirectory, new DirectoryInfo($@"{destinationDirectory.FullName}\{sourceSubDirectory.Name}"));
            }
            foreach (FileInfo sourceFile in SourceFiles!)
            {
                await Task.Run(() => sourceFile.CopyTo($@"{destinationDirectory.FullName}\{Path.GetFileName(sourceFile.Name)}"));
            }
        }


        private async Task ProcessRenamingAsync()
        {
            await Task.Run(() =>
            {
                StringBuilder tempOld = new StringBuilder(OldName);
                StringBuilder tempNew = new StringBuilder(NewName);
                tempOld.Replace(" ", "_");

                CustomBasicList<string> newList = new CustomBasicList<string> { " ", "(", ")", ",", "-" };
                newList.ForEach(a => tempNew.Replace(a, "_"));
                string firstDigit = tempNew.ToString().Substring(0, 1);
                bool rets = firstDigit.IsNumeric();
                if (rets == true)

                    tempNew.Insert(0, "_");

                string path = $@"{NewPath}\{NewName}";
                string newString = tempNew.ToString();
                string oldString = tempOld.ToString();
                string fileData;
                StreamReader streamReader;
                StreamWriter streamWriter;
                if (File.Exists($@"{path}\{OldName}.csproj") == true)
                {
                    streamReader = new StreamReader($@"{path}\{OldName}.csproj", Encoding.Default);
                    fileData = streamReader.ReadToEnd();
                    streamReader.Close();
                    streamWriter = new StreamWriter($@"{path}\{OldName}.csproj", false, Encoding.Default);
                    StringBuilder tempMiddle = new StringBuilder(fileData);
                    tempMiddle.Replace($"<AssemblyName>{OldName}</AssemblyName>", $"<AssemblyName>{NewName}</AssemblyName>");
                    tempMiddle.Replace($"<DocumentationFile>{OldName}.xml</DocumentationFile>", $"<DocumentationFile>{NewName}.xml</DocumentationFile>");
                    tempMiddle.Replace($"<RootNamespace>{oldString}</RootNamespace>", $"<RootNamespace>{newString}</RootNamespace>");
                    tempMiddle.Replace($"<StartupObject>{oldString}.My.MyApplication</StartupObject>", $"<StartupObject>{newString}.My.MyApplication</StartupObject>");
                    streamWriter.Write(tempMiddle.ToString());
                    streamWriter.Close();
                    File.Move($@"{path}\{OldName}.csproj", $@"{path}\{NewName}.csproj"); //has to be this way because the other function is asyn and this is already async
                }
                if (File.Exists($@"{path}\{OldName}.sln") == true)
                {
                    streamReader = new StreamReader($@"{path}\{OldName}.sln", Encoding.Default);
                    fileData = streamReader.ReadToEnd();
                    streamReader.Close();
                    streamWriter = new StreamWriter($@"{path}\{OldName}.sln", false, Encoding.Default);
                    fileData = fileData.Replace(OldName, NewName);
                    fileData = GetCleanText(fileData);
                    streamWriter.Write(fileData.Trim());
                    streamWriter.Close();
                    File.Move($@"{path}\{OldName}.sln", $@"{path}\{NewName}.sln");
                }
                if (File.Exists($@"{path}\{OldName}.suo") == true)
                    File.Move($@"{path}\{OldName}.suo", $@"{path}\{NewName}.suo");

                if (File.Exists($@"{path}\{OldName}.csproj.user") == true)
                    File.Move($@"{path}\{OldName}.csproj.user", $@"{path}\{NewName}.csproj.user");

                if (Directory.Exists($@"{path}\My Project") == true)
                {
                    if (File.Exists($@"{path}\My Project\Application.Designer.cs") == true)
                    {
                        streamReader = new StreamReader($@"{path}\My Project\Application.Designer.cs", Encoding.Default);
                        fileData = streamReader.ReadToEnd();
                        streamReader.Close();
                        streamWriter = new StreamWriter($@"{path}\My Project\Application.Designer.cs", false, Encoding.Default);
                        fileData = fileData.Replace($"Global.{oldString}", $"Global.{newString}");
                        streamWriter.Write(fileData);
                        streamWriter.Close();
                    }
                    if (File.Exists($@"{path}\My Project\Settings.Designer.cs") == true)
                    {
                        streamReader = new StreamReader($@"{path}\My Project\Settings.Designer.cs", Encoding.Default);
                        fileData = streamReader.ReadToEnd();
                        streamReader.Close();
                        streamWriter = new StreamWriter($@"{path}\My Project\Settings.Designer.cs", false, Encoding.Default);
                        fileData = fileData.Replace($"Global.{oldString}", $"Global.{newString}");
                        streamWriter.Write(fileData);
                        streamWriter.Close();
                    }
                    if (File.Exists($@"{path}\My Project\AssemblyInfo.cs") == true)
                    {
                        streamReader = new StreamReader($@"{path}\My Project\AssemblyInfo.cs", Encoding.Default);
                        fileData = streamReader.ReadToEnd();
                        streamReader.Close();
                        streamWriter = new StreamWriter($@"{path}\My Project\AssemblyInfo.cs", false, Encoding.Default);
                        fileData = fileData.Replace($"<Assembly: AssemblyTitle({vb.ChrW(34)}{OldName}{vb.ChrW(34)})>", $"<Assembly: AssemblyTitle({vb.ChrW(34)}{OldName}{vb.ChrW(34)})>");
                        fileData = fileData.Replace($"<Assembly: AssemblyProduct({vb.ChrW(34)}{OldName}{vb.ChrW(34)})>", $"<Assembly: AssemblyProduct({vb.ChrW(34)}{OldName}{vb.ChrW(34)})>");
                        streamWriter.Write(fileData);
                        streamWriter.Close();
                    }
                    CustomBasicList<string> tempList;
                    CustomBasicList<string> searchList = new CustomBasicList<string>() { $@"{path}\bin\Debug", $@"{path}\bin\Release", $@"{path}\obj", $@"{path}\obj\Debug", $@"{path}\obj\Release" };
                    searchList.ForEach(a =>
                    {
                        if (DirectoryExists(a) == true)
                        {
                            tempList = GetFileList(a);
                            tempList.ForEach(b =>
                            {
                                string tempOldName = FullFile(b);
                                if (tempOldName.ToLower().Contains(OldName.ToLower()) == true)
                                {
                                    string tempNewName = tempOldName.Replace(OldName, NewName);
                                    File.Move(b, $@"{a}\{tempNewName}");
                                }
                            });
                        }
                    });
                }
            });



        }
        private CustomBasicList<string> GetFileList(string path)
        {
            CustomBasicList<string> tFileList = new CustomBasicList<string>();
            IEnumerable<string> tempList;
            tempList = Directory.EnumerateDirectories(path, "*", SearchOption.TopDirectoryOnly); //because we have multiple folders now.
            foreach (var thisItem in tempList)
                tFileList.Add(thisItem);
            return tFileList;

        }
        #endregion
    }
}