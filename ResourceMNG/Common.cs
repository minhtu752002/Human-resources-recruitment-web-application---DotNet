using ResourceEntity.Models;

namespace ResourceMNG
{
    public class Common
    {
        public static string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
        public static string DateToString (DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }
		public static string DateToYear(DateTime date)
		{
			return date.ToString("yyyy");
		}
		public static string DateToHour (DateTime date)
        {
            return date.ToString("t");
        }
        public static class BackGroundCoLor
        {
            public const string NavColor = "#414141";
            public const string HeaderColor = "#fff";
            public const string Color = "rgb(3, 169, 244)";

        }
        //public static bool CheckQuyen(int id)
        //{
        //    var context = new ResourceMngContext();
        //    var httpContext = new HttpContextAccessor().HttpContext;
        //    var count = context.PhanQuyens.Count(m => m.UserId == httpContext.Session.GetInt32("IDUser") & m.RoleId == id);
        //    if (count == 0)
        //    { 
        //        return false;
        //    }
        //    return true;
        //}
    }
}
