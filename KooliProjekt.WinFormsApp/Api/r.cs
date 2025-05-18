using System;

namespace KooliProjekt.WinFormsApp.Api
{
    public class r : Result
    {
        public r(bool success, string error) : base(success, error) { }

        public static r Ok() => new r(true, null);
        public static r Fail(string error) => new r(false, error);
        public static r Fail(Exception ex) => new r(false, ex.Message);
    }
}