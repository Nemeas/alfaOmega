using System;

namespace AlfaOmega.helpers
{
    class Result
    {
        public Result(string res, string attachement)
        {
            Res = res;
            Attachement = attachement;
        }

        public string Res { get; set; }
        public string Attachement { get; set; }
    }
}