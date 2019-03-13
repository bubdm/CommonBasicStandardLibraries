using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonBasicStandardLibraries.Exceptions;
using System.IO;
using CommonBasicStandardLibraries.CollectionClasses;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions.ListsExtensions;
using System.Reflection;

namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.FileFunctions
{
    public static class FileFunctions
    {
        //i think that setting the path caused too many problems.  so that will no longer be static

        public static string GetApplicationDataForMobileDevices()
        {
            // Dim ThisPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            return System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // i think this is where it should be.
        }

        public static string GetSDCardReadingPathForAndroid()
        {
            if (DirectoryExists("/storage/sdcard1") == true)
                return "/storage/sdcard1"; // can't assume its always accessing the music folder
            else if (DirectoryExists("/storage/emulated/0") == true)
                return "/storage/emulated/0";
            else
                throw new Exception("Either an unusual situation or not even an android device");
        }

        public static string GetWriteLocationForExternalOnAndroid()
        {
            return "/storage/emulated/0"; // for writing, its always this location.  so for cristina, it will be internal.  for andy, external but the path is still the same
        }

        public static bool FileExists(string FilePath)
        {
            return System.IO.File.Exists(FilePath);
        }

        public static async  Task<bool> NewFileCreated(string DirectoryPath, string ExpectedExtension)
        {
            var FirstList = await FileListAsync(DirectoryPath);
            string Results = FirstList.Find(Items =>
            {
                return Items.ToLower().EndsWith(ExpectedExtension.ToLower());
            }
            );
            if (string.IsNullOrWhiteSpace(Results) == true)
                return false;
            return true;
        }

        public static bool DirectoryExists(string DirectoryPath)
        {
            return System.IO.Directory.Exists(DirectoryPath);
        }

        public static async  Task<string> GetDriveLetterAsync(string Label)
        {
            string ThisStr = default;
            await Task.Run(() =>
            {
                var CompleteDrives = System.IO.DriveInfo.GetDrives();
                //System.IO.DriveInfo Temps;
                foreach (var Temps in CompleteDrives)
                {
                    if (Temps.DriveType == System.IO.DriveType.Fixed)
                    {
                        if (Temps.IsReady == true)
                        {
                          if (Label.ToLower() == Temps.VolumeLabel.ToLower())
                               ThisStr =  Temps.Name.Substring(0, 1);
                           break;
                        } 
                    }
                }
            } 
            );

            if (ThisStr == default)
            {
                throw new BasicBlankException("No label for " + Label);
            }
            return ThisStr;
        }  

		public static async Task FilterDirectory(CustomBasicList<string> FirstList)
		{
			CustomBasicList<string> RemoveList = new CustomBasicList<string>();
			await Task.Run(() =>
			{
				FirstList.ForEach(Items =>
				{
					if (System.IO.Directory.EnumerateFiles(Items).Count() == 0)
						RemoveList.Add(Items);
				});
				
			});
			FirstList.RemoveGivenList(RemoveList, System.Collections.Specialized.NotifyCollectionChangedAction.Remove); //i think
		}

        public static async  Task<CustomBasicList<string>> GetDriveListAsync()
        {

            System.IO.DriveInfo[] CompleteDrives = null;
            CustomBasicList<string> NewList = new CustomBasicList<string>();
            await Task.Run(() =>
            {
                CompleteDrives = System.IO.DriveInfo.GetDrives();
                foreach (var Temps in CompleteDrives)
                {
                    if (Temps.DriveType == System.IO.DriveType.Fixed)
                    {
                        if (Temps.IsReady == true)
                            NewList.Add(Temps.RootDirectory.FullName);
                    }
                }
            }
            );
            
            return NewList;
        }

        private static DateTime GetCreatedDate(System.IO.FileInfo ThisFile)
        {
            if (ThisFile.CreationTime.IsDaylightSavingTime() == true)
                return ThisFile.CreationTime.AddHours(-1);
            return ThisFile.CreationTime;
        }

        private static DateTime GetModifiedDate(System.IO.FileInfo ThisFile)
        {
            if (ThisFile.LastWriteTime.IsDaylightSavingTime() == true)
                return ThisFile.LastWriteTime.AddHours(-1);
            return ThisFile.LastWriteTime;
        }

        public static string GetParentPath(string ThisPath)
        {
            var ThisDir = System.IO.Directory.GetParent(ThisPath);
            return ThisDir.FullName;
        }

        public static async Task <FileInfo> GetFileAsync(string FilePath)
        {
            System.IO.FileInfo ThisFile;
            FileInfo TempFile = null;
            await Task.Run(() =>
            {
               ThisFile = new System.IO.FileInfo(FilePath);

               TempFile = new FileInfo()
               {
                   FileSize = ThisFile.Length,
                   DateCreated = GetCreatedDate(ThisFile),
                   Directory = ThisFile.DirectoryName,
                   DateAccessed = ThisFile.LastAccessTime,
                   DateModified = GetModifiedDate(ThisFile),
                   FilePath = FilePath // this is needed so if i only have the fileinfo, i know the full path.
               };
               if (TempFile.DateModified > TempFile.DateAccessed)
                   TempFile.DateAccessed = TempFile.DateModified;// this means something is wrong.
            });           
            return TempFile;
        }

        //since i have more async support now, its best to use more awaits.


        public static string DirectoryName(string DirectoryPath)
        {
            var ThisItem = new System.IO.DirectoryInfo(DirectoryPath);
            return ThisItem.Name;
        }

        public static string FullFile(string FilePath)
        {
            var ThisItem = new System.IO.FileInfo(FilePath);
            return ThisItem.Name;
        }

        public static string FileName(string FilePath)
        {
            var ThisItem = new System.IO.FileInfo(FilePath);
            var ThisName = ThisItem.Name;
            return ThisName.Substring(0, ThisName.Count() - ThisItem.Extension.Count());
        }

        public static async Task<CustomBasicList<string>> DirectoryListAsync(string WhatPath, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var TDirectoryList = new CustomBasicList<string>();
            await Task.Run(() =>
            {
                TDirectoryList = System.IO.Directory.EnumerateDirectories(WhatPath, "*", searchOption).ToCustomBasicList();
            });
            return TDirectoryList;
        }

		public static async Task<CustomBasicList<string>> FileListAsync(string DirectoryPath, SearchOption ThisOption = SearchOption.TopDirectoryOnly)
        {
            CustomBasicList<string> tFileList = new CustomBasicList<string>();
            await Task.Run(() =>
            {
                tFileList = System.IO.Directory.EnumerateFiles(DirectoryPath, "*", ThisOption).ToCustomBasicList(); // hopefully will still work
            });
            return tFileList;
        }

		public static async Task<CustomBasicList<string>> FileListAsync(CustomBasicList<string> DirectoryList) //this can be just top because you already sent in a directory list
		{
			CustomBasicList<string> NewList = new CustomBasicList<string>();
			await DirectoryList.ForEachAsync(async x =>
			{
				var Temps = await FileListAsync(x);
				NewList.AddRange(Temps);
			});
			return NewList;
		}
        public static async Task<CustomBasicList<string>> GetSeveralSpecificFiles(string DirectoryPath, string ExtensionEndsAt, SearchOption SearchOption = SearchOption.TopDirectoryOnly) //to not break compatibility
        {
            var TempList = await FileListAsync(DirectoryPath, SearchOption);
            TempList.KeepConditionalItems(Items => Items.ToLower().EndsWith(ExtensionEndsAt.ToLower()));
            return TempList;
        }
        /// <summary>
        /// This is used in cases where you know what file names are okay and only those will show the complete paths.  i am guessing that if one from the good list is not found, then raise an exception
        /// </summary>
        /// <param name="DirectoryPath">Directory To Look For</param>
        /// <param name="ExtensionEndsAt">This is the extension of the file needed</param>
        /// <param name="GoodList">This is a list of all files you know you need.  has names without extensions.</param>
        /// <param name="SearchOption">default is all directories but can choose top if needed</param>
        /// <returns></returns>
        public static async Task<CustomBasicList<string>> GetSeveralSpecificFiles(string DirectoryPath, string ExtensionEndsAt, CustomBasicList<string> GoodList, SearchOption SearchOption = SearchOption.AllDirectories)
        {
            //step 1 is getting the specific files without the good list
            CustomBasicList<string> FirstList = await GetSeveralSpecificFiles(DirectoryPath, ExtensionEndsAt, SearchOption);
            FirstList.KeepConditionalItems(Items =>
            {
                string ThisName = FileName(Items).ToLower();
                if (GoodList.Exists(Temps => Temps.ToLower() == ThisName) == true)
                    return true;
                return false;
            });
            if (FirstList.Count != GoodList.Count)
                throw new BasicBlankException(@"Does not reconcile because there was an item on the good list that was not found or had duplicates");
            return FirstList;
        }

        //public static async Task<CustomBasicList<string>> GetSeveralSpecificFiles(string DirectoryPath, string ExtensionEndsAt)
        //{
        //    var TempList = await FileListAsync(DirectoryPath);
        //    TempList.KeepConditionalItems(Items => Items.ToLower().EndsWith(ExtensionEndsAt.ToLower()));
        //    return TempList;
        //}

        public static async Task<string> GetSpecificFile(string DirectoryPath, string ExtensionEndsAt)
        {
            var TempList = await FileListAsync(DirectoryPath);
            return TempList.FindOnlyOne(Items => Items.ToLower().EndsWith(ExtensionEndsAt.ToLower()));
        }
        /// <summary>
        /// This searches for files in not only top directory but sub directories as well
        /// </summary>
        /// <param name="DirectoryPath">Directory Where To Look</param>
        /// <param name="FileName">File Name Including Extension Needed</param>
        /// <returns></returns>
        public static async Task<string> SearchForFileName(string DirectoryPath, string FileName)
        {
            //this time i need first a list of all but even sub folders for it.
            CustomBasicList<string> tFileList = new CustomBasicList<string>();
            await Task.Run(() =>
            {
                tFileList = Directory.EnumerateFiles(DirectoryPath, FileName, SearchOption.AllDirectories).ToCustomBasicList();
            });
            if (tFileList.Count == 0)
                throw new BasicBlankException($"The File Name {FileName} Was Not Found At {DirectoryPath} Or Even Sub Directories");
            return tFileList.Single(); //if nothing is found, then will get runtime error
        }

        

        private static async  Task<string> PrivateAllTextAsync(string FilePath, Encoding encodes)
        {
            string ThisText;
            using (StreamReader s = new StreamReader(FilePath, encodes))
            {
                ThisText = await s.ReadToEndAsync();
                s.Close();
            }
            return ThisText;
        }

        private static async Task PrivateWriteAllTextAsync(string FilePath, string Text, bool Append, Encoding encoding, bool IsLine = false)
        {
            using (StreamWriter w = new StreamWriter(FilePath, Append, encoding))
            {
                if (IsLine == true)
                    await w.WriteLineAsync(Text);
                else
                    await w.WriteAsync(Text);
                await w.FlushAsync();
                w.Close();
            }
        }

        public static async Task<string> AllTextAsync(string FilePath)
        {
            return await PrivateAllTextAsync(FilePath, Encoding.Default); //i think
            
        }

        public static async Task<string> AllTextAsync(string FilePath, Encoding encoding)
        {
            return await PrivateAllTextAsync(FilePath, encoding);
        }


        public static async Task  WriteAllTextAsync(string FilePath, string WhatText)
        {
            await PrivateWriteAllTextAsync(FilePath, WhatText, false, Encoding.Default);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="WhatText"></param>
        /// <param name="encoding">Hint  Use UTF-8 in some cases.</param>
        /// <returns></returns>
        public static async Task WriteAllTextAsync(string FilePath, string WhatText, Encoding encoding)
        {
            await PrivateWriteAllTextAsync(FilePath, WhatText, false, encoding);
        }

        public static async Task WriteTextAsync(string FilePath, string WhatText, bool Append)
        {
            await PrivateWriteAllTextAsync(FilePath, WhatText, Append, Encoding.Default, true);
        }

        public static async Task FileCopyAsync(string OriginalFile, string NewFile)
        {
            await Task.Run(() => File.Copy(OriginalFile, NewFile, true));
        }

        public static async Task DeleteFolderAsync(string Path) //i like just delete folder instead
        {
            await Task.Run(() => Directory.Delete(Path));
        }

        public static async Task CreateFolderAsync(string Path)
        {
            await Task.Run(() => Directory.CreateDirectory(Path));
        }

        public static async Task DeleteSeveralFiles(string DirectoryPath, string StringEnd)
        {
            CustomBasicList<string> ThisList = await FileListAsync(DirectoryPath);
            ThisList.KeepConditionalItems(Items =>
            Items.ToLower().EndsWith(StringEnd.ToLower()));
            await ThisList.ForEachAsync(async Items => await DeleteFileAsync(Items));
        }

        public static async Task DeleteFileAsync(string Path)
        {
            await Task.Run(() => File.Delete(Path));
        }

        public static async Task RenameFileAsync(string OldFile, string NewName)
        {
            await Task.Run(() => File.Move(OldFile, NewName));
        }

        public static async Task RenameDirectoryAsync(string OldDirectory, string NewName)
        {
            await Task.Run(() => Directory.Move(OldDirectory, NewName));
        }

        public static async  Task<CustomBasicList<string>> TextFromFileListAsync(string FilePath)
        {
            CustomBasicList<string> ThisList = new CustomBasicList<string>();
            await Task.Run(() => ThisList = File.ReadAllLines(FilePath).ToCustomBasicList());
            return ThisList;
        }


        public static Stream GetStreamForReading(string FilePath) // some classes require the actual stream.  therefore will use this
        {
            return new FileStream(FilePath, FileMode.Open, FileAccess.Read);
        } // this

        public static async  Task<string> ResourcesAllTextFromFileAsync(object ThisObj, string FileName)
        {
            Assembly ThisA = Assembly.GetAssembly(ThisObj.GetType());
            return await ThisA.ResourcesAllTextFromFileAsync(FileName);
        }

        public static async Task<Stream> ResourcesGetStreamAsync(object ThisObj, string FileName)
        {
            Assembly ThisA = Assembly.GetAssembly(ThisObj.GetType());
            return await ThisA.ResourcesGetStreamAsync(FileName);
        }

        public static string GetMediaURIFromStream(object ThisObj, string FileName)
        {
            Assembly ThisA = Assembly.GetAssembly(ThisObj.GetType());
            return ThisA.GetMediaURIFromStream(FileName);
        }
    }
}
