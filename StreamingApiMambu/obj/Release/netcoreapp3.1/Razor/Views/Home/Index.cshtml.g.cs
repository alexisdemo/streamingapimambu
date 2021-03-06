#pragma checksum "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/Home/Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "59f67f6f0571375c82a1fef3f772e6693424959d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/_ViewImports.cshtml"
using StreamingApiMambu;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/_ViewImports.cshtml"
using StreamingApiMambu.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"59f67f6f0571375c82a1fef3f772e6693424959d", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4ce6c64c7ce32e0cf6110588e98a4aa937a837aa", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<StreamingApiMambu.Models.RequestStreamingApi>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/Home/Index.cshtml"
  
    ViewData["Title"] = "Home Page";

#line default
#line hidden
#nullable disable
            WriteLiteral("\n<div class=\"text-center\">\n    <h1 class=\"display-4\">Mambu App - Streaming API</h1>\n</div>\n\n\n");
#nullable restore
#line 11 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/Home/Index.cshtml"
 if (ViewBag.SystemWarningMessage != null)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div>\n        <div class=\"alert alert-danger\" role=\"alert\">\n\n");
#nullable restore
#line 16 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/Home/Index.cshtml"
             foreach (var item in ViewBag.SystemWarningMessage)
            {
                

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/Home/Index.cshtml"
           Write(item.ErrorMessage);

#line default
#line hidden
#nullable disable
            WriteLiteral(" <br>\n");
#nullable restore
#line 19 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/Home/Index.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("\n        </div>\n    </div>\n");
#nullable restore
#line 23 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/Home/Index.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\n");
#nullable restore
#line 25 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/Home/Index.cshtml"
 using (Html.BeginForm("Index", "Home", FormMethod.Post, new { id = "empform" }))
{

#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"form-row\">\n\n    <div class=\"col-md-12\">\n        ");
#nullable restore
#line 30 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/Home/Index.cshtml"
   Write(Html.LabelFor(Model => Model.MambuServer, "StreamingApiMambu Server"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n        ");
#nullable restore
#line 31 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/Home/Index.cshtml"
   Write(Html.TextBoxFor(Model => Model.MambuServer, new { @class = "form-control" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n    </div>\n\n\n    <div class=\"col\">\n        ");
#nullable restore
#line 36 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/Home/Index.cshtml"
   Write(Html.LabelFor(Model => Model.ApiKey, "Api Key"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n        ");
#nullable restore
#line 37 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/Home/Index.cshtml"
   Write(Html.TextBoxFor(Model => Model.ApiKey, new { @class = "form-control" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n    </div>\n    <div class=\"col\">\n        ");
#nullable restore
#line 40 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/Home/Index.cshtml"
   Write(Html.LabelFor(Model => Model.TimeExecution, "Time Execution"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n        ");
#nullable restore
#line 41 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/Home/Index.cshtml"
   Write(Html.TextBoxFor(Model => Model.TimeExecution, new { @class = "form-control" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n    </div>\n\n    <div class=\"col-12\">\n        ");
#nullable restore
#line 45 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/Home/Index.cshtml"
   Write(Html.LabelFor(Model => Model.Topic, "Topic"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n        ");
#nullable restore
#line 46 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/Home/Index.cshtml"
   Write(Html.TextBoxFor(Model => Model.Topic, new { @class = "form-control" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n    </div>\n\n");
            WriteLiteral("\n");
#nullable restore
#line 57 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/Home/Index.cshtml"
     if (TempData["EnableExecute"] == null || TempData["EnableExecute"].ToString() == "true")
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"col-12\">\n            <button type=\"submit\" class=\"btn btn-primary\">Ejecutar Lectura</button>\n        </div>\n");
#nullable restore
#line 62 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/Home/Index.cshtml"
    }
    else
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"col-12\">\n            <button type=\"submit\" class=\"btn btn-primary disabled\" disabled>Ejecutar Lectura</button>\n        </div>\n");
#nullable restore
#line 68 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/Home/Index.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\n\n</div>\n");
#nullable restore
#line 72 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/Home/Index.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\n<br />\n");
#nullable restore
#line 75 "/Users/salinas/Projects/TestForm/StreamingApiMambu/Views/Home/Index.cshtml"
Write(Html.Partial("_GridMambu"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n\n\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<StreamingApiMambu.Models.RequestStreamingApi> Html { get; private set; }
    }
}
#pragma warning restore 1591
