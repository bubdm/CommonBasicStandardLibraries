using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class ObjectExtensions
    {
        public static T AutoMap<T>(this object payLoad) //this does the most simple option.  if i need something more complex, i will have more details to figure out what to do.
        {
            //for now, i think this is fine.  i may have to redo some things.  however, i'm not sure what the best way to handle different scenarios currently.
            string results = JsonConvert.SerializeObject(payLoad);
            return JsonConvert.DeserializeObject<T>(results);
        } //saves the extra lines of code.  i like it even better as extensions.  if you map a list to a single item, will error out.  that is okay.

        

    }
}
