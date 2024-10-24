using System.Collections.Generic;
using System.Runtime.InteropServices;
using Extism;
using Newtonsoft.Json;

namespace Plugin;
public class Program
{
    [UnmanagedCallersOnly]
    public static int SupergraphRequest()
    {
        List<string> validClientNames = ["client-header-1", "client-header-2", "client-header-3"];
        bool hasValidHeader = false;
        SupergraphRequestPayload payload = Pdk.GetInputJson(SourceGenerationContext.Default.SupergraphRequestPayload);

        if (payload.headers.ContainsKey("apollographql-client-name"))
        {
            string clientName = payload.headers["apollographql-client-name"][0];
            
            foreach (string validClientName in validClientNames)
            {
                if(validClientName == clientName)
                {
                    hasValidHeader = true;
                    break;
                }
            }
        } 
        if(!hasValidHeader)
        {
            Control control = new Control(401);
            payload.control = control;

            Extensions extensions = new Extensions("CLIENT_NOT_FOUND");
            Error error = new Error("This client is not recognized", extensions);
            
            // Add the error to the body extensions
            payload.AddError(error);
        }

        var serializedPayload = JsonConvert.SerializeObject(payload, Formatting.None);
        Pdk.SetOutput(serializedPayload);
        return 0;
    }

    // Note: a `Main` method is required for the app to compile
    public static void Main() {}
}