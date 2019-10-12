using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolutionRenamer.Blazor
{
    public class GetRequest
    {
        public string URL { get; set; } = "https://github.com/thewebchameleon/TemplateV2.Razor/archive/master.zip";

        public string Keyword { get; set; } = "TemplateV2";

        public string ProjectName { get; set; }


    }
}
