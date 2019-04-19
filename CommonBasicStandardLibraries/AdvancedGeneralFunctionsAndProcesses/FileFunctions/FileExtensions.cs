using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Text;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.FileFunctions
{
    public static class FileExtensions
    {

        //will try to make them awaitable.

        public static string ResourceLocation = "Resources"; //c# requires the actual folder where you place the resource to read the embedded resource.

        public static async  Task<string> FixBase64ForFileDataAsync(this string str_Image)
        {
            // *** Need to clean up the text in case it got corrupted travelling in an XML file
            // i think its best to have as public.  because its possible its only corrupted because of this.
            // has had the experience before with smart phones.
            // however; with mango and windows phones 7; I can use a compact edition database (which would be very helpful).
            // if doing this; then what would have to happen is I would have to have a method to check back in the music information.
            // maybe needs to be xml afterall (don't know though).  otherwise; may have to do serializing/deserializing.
            // some stuff is iffy at this point.
            // at this point, its proven to work.  so i will keep.  this will be good even for iphones/androids, other operating systems even

            StringBuilder sbText;
            string ThisStr=default; //i like settng to default.
            await Task.Run(() =>
            {
                sbText = new System.Text.StringBuilder(str_Image, str_Image.Length);
                sbText.Replace(@"\r\n", string.Empty);
                sbText.Replace(" ", string.Empty);
                ThisStr = sbText.ToString();
            });
            return ThisStr;
        }

        private static async Task<Stream> GetStreamAsync(Assembly ThisAssembly, string FileName)
        {
            Stream ThisStream=default;

            await Task.Run(() =>
            {
                //var ThisAssembly = Assembly.GetAssembly(ThisObj.GetType());
                
                //var ThisAssembly = Assembly.(""); //try this one.    
                if (FileName.Contains("/") == true || FileName.Contains(@"\") == true)
                    throw new Exception(@"Cannot contain the / or \ in the file name.   Its already smart enough to figure out even if put in folders", null);
                var ThisList = ThisAssembly.GetManifestResourceNames();
                var FirstName = ThisAssembly.GetName().Name; // needs 2 things affterall.  looks like simplier in .net standard 2.0
                FirstName = FirstName.Replace(" ", "_");
                // if there are other things that needs replacing, should do here
                // i think this is best for maximum shortcuts
                string InternalPath;
                if (ResourceLocation == "")
                    InternalPath = FirstName + "." + FileName;
                else
                    InternalPath = FirstName + "." + ResourceLocation + "." + FileName;
                // InternalPath = InternalPath.Replace("/", ".")
                ThisStream = ThisAssembly.GetManifestResourceStream(InternalPath);
                if (ThisStream ==null)
                    throw new FileNotFoundException(FileName + " does not exist");
            });
            

           
            return ThisStream;
        }

        public static string GetMediaURIFromStream(this Assembly ThisAssembly, string FileName) //for the other, it has to create assembly.  however, no extensions.
        {
            //var ThisAssembly = Assembly.GetAssembly(ThisObj.GetType());
            if (FileName.Contains("/") == true || FileName.Contains(@"\") == true)
                throw new Exception(@"Cannot contain the / or \ in the file name.   Its already smart enough to figure out even if put in folders", null);
            var FirstName = ThisAssembly.GetName().Name; // needs 2 things affterall.  looks like simplier in .net standard 2.0
            FirstName = FirstName.Replace(" ", "_");
            // if there are other things that needs replacing, should do here
            // i think this is best for maximum shortcuts
            string InternalPath;
            InternalPath = FirstName + "." + FileName;
            return InternalPath;
        }

        public static async Task<string> ResourcesAllTextFromFileAsync(this Assembly ThisAssembly, string FileName)
        {
            var ThisStream = await GetStreamAsync(ThisAssembly, FileName);
            using (var ThisRead = new StreamReader(ThisStream))
            {
                return await ThisRead.ReadToEndAsync();
            }
        }

        //public async static Task<string> ResourcesAllTextFromFileAsync(this object ThisObj, string FileName)
        //{
        //    var ThisStream = GetStream(ThisObj, FileName);
        //    using (var ThisRead = new StreamReader(ThisStream))
        //    {
        //        return await ThisRead.ReadToEndAsync();
        //    }
        //}

        //will attempt to async only.  hopefully i don't regret this.
        //on the other hand the main on console now support await

        public static async Task<Stream> ResourcesGetStreamAsync(this Assembly ThisAssembly, string FileName)
        {
            return await GetStreamAsync(ThisAssembly, FileName);
        }


        public static Stream ResourcesGetStream(this Assembly ThisAssembly, string FileName)
        {
            //sometimes, it can't be async.
            if (FileName.Contains("/") == true || FileName.Contains(@"\") == true)
                throw new Exception(@"Cannot contain the / or \ in the file name.   Its already smart enough to figure out even if put in folders", null);
            var ThisList = ThisAssembly.GetManifestResourceNames();
            var FirstName = ThisAssembly.GetName().Name; // needs 2 things affterall.  looks like simplier in .net standard 2.0
            FirstName = FirstName.Replace(" ", "_");
            // if there are other things that needs replacing, should do here
            // i think this is best for maximum shortcuts
            string InternalPath;
            if (ResourceLocation == "")
                InternalPath = FirstName + "." + FileName;
            else
                InternalPath = FirstName + "." + ResourceLocation + "." + FileName;
            // InternalPath = InternalPath.Replace("/", ".")
            Stream ThisStream = ThisAssembly.GetManifestResourceStream(InternalPath);
            if (ThisStream == null)
                throw new FileNotFoundException(FileName + " does not exist");
            return ThisStream;
        }

        // no need for extensions because i already have the extension library.  i will just add 2 more to it for the awaits


        //public static void SaveBinaryData(this string Data, string Path)
        //{
        //    // looks like i cannot copy/paste the code because its too different
        //    // Dim Fins = New System.IO.FileStream(Path, FileMode.Create)
        //    using (var Fins = new System.IO.FileStream(Path, FileMode.Create))
        //    { 
        //        // Dim bufferSize As Integer = 4096
        //        System.Byte[] Bytes = System.Convert.FromBase64String(Data);
        //        Fins.Write(Bytes, 0, Bytes.Length);
        //        Fins.Flush();
        //        Fins.Dispose(); // maybe disposing of the fins may help.
        //    }
        //}

        public async static Task SaveBinaryDataAsync(this string Data, string Path)
        {
            using (var Fins = new System.IO.FileStream(Path, FileMode.Create))
            {
                // Dim bufferSize As Integer = 4096
                System.Byte[] Bytes = System.Convert.FromBase64String(Data);
                await Fins.WriteAsync(Bytes, 0, Bytes.Length);
                await Fins.FlushAsync();
                //Fins.Dispose(); // trying to dispose of fins as well.  maybe that will help
                //since we have using, hopefully does not need dispose now.
            }
        }


    }
}
