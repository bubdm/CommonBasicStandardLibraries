using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
using System.IO;
using System.IO.Compression;
using System.Linq;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.FileFunctions.FileFunctions;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.ZipClasses
{
    public class CustomZipClass
    {

        private readonly CustomBasicList<PrivateZipInfo> _zipList = new CustomBasicList<PrivateZipInfo>();

        public void Clear()
        {
            _zipList.Clear();
        }

        public void AddFileToZip(string path)
        {
            _zipList.Add(new PrivateZipInfo()
            {
                Path = path
            });
        }
        public void AddFileToZip(string path, string folder)
        {
            _zipList.Add(new PrivateZipInfo()
            {
                Path = path,
                Folder = folder
            });
        }

        public void SaveZipFile(string zipPath, bool clearContents = true)
        {
            if (_zipList.Count == 0)
            {
                throw new BasicBlankException("There are no files for this zip file");
            }
            if (File.Exists(zipPath))
            {
                //i think it will replace existing one no matter what here.
                File.Delete(zipPath);
            }
            using FileStream zipToOpen = new FileStream(@"c:\TempFiles\E.zip", FileMode.Create);
            using ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create);
            //ZipArchiveEntry rr = archive.CreateEntryFromFile(@"C:\TempFiles\problem.png", @"Folder\Good.png");


            _zipList.ForEach(x =>
            {
                string relative;
                string name = FullFile(x.Path);
                if (x.Folder == "")
                {
                    relative = name;
                    //ZipArchiveEntry rr = archive.CreateEntryFromFile(x.Path, ""); //iffy.
                }
                else
                {
                    relative = Path.Combine(x.Folder, name);
                    //ZipArchiveEntry rr = archive.CreateEntryFromFile(x.Path, x.Folder);
                }
                ZipArchiveEntry rr = archive.CreateEntryFromFile(x.Path, relative);
            });
            if (clearContents)
            {
                _zipList.Clear();
            }
        }

        //good news is i can now open zip file to get what is needed.
        public CustomBasicList<OpenedZipFile> OpenZipFile(string path)
        {
            using ZipArchive archive = ZipFile.OpenRead(path);



            CustomBasicList<OpenedZipFile> output = new CustomBasicList<OpenedZipFile>();
            foreach (ZipArchiveEntry entry in archive.Entries)
            {


                OpenedZipFile zip = new OpenedZipFile();
                //putting the entry means nothing because it gets disposed so too late.
                //zip.Entry = entry;
                zip.ModifyDate = entry.LastWriteTime.DateTime; //try this way.
                zip.FileName = entry.Name;
                if (entry.FullName == entry.Name)
                {
                    zip.FolderName = "";
                }
                else
                {
                    zip.FolderName = entry.FullName.Replace(entry.Name, "");
                }
                output.Add(zip);
            }

            if (output.Count == 0)
            {
                throw new BasicBlankException("There are no files found in the zip file");
            }
            return output;

        }

        public void UnzipAll(string zipFile, string extractPath) //nice to have a choice.
        {
            ZipFile.ExtractToDirectory(zipFile, extractPath);
        }
        public void UnzipFile(string zipFile, string extractPath, CustomBasicList<OpenedZipFile> files)
        {
            using ZipArchive archive = ZipFile.OpenRead(zipFile);
            CustomBasicList<ZipArchiveEntry> entries = new CustomBasicList<ZipArchiveEntry>();
            files.ForEach(file =>
            {
                ZipArchiveEntry zip = archive.Entries.Where(x => x.Name == file.FileName).Single();
                entries.Add(zip);
            });

            if (Directory.Exists(extractPath) == false)
            {
                throw new BasicBlankException("Destination does not exist");
            }
            entries.ForEach(x =>
            {
                CustomBasicList<string> folders = x.FullName.Split(@"\").ToCustomBasicList();
                folders.RemoveLastItem();
                string temps = extractPath;
                folders.ForEach(folder =>
                {
                    temps = Path.Combine(temps, folder);
                    if (Directory.Exists(temps) == false)
                    {
                        Directory.CreateDirectory(temps);
                    }
                });


                x.ExtractToFile(@$"C:\TempFiles\ResultZip\{x.FullName}", true);
            });

        }

    }
}
