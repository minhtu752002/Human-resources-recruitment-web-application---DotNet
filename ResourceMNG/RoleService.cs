using ResourceEntity.Models;

namespace ResourceMNG
{
    public interface IRoleService
    {
        List<RoleDesc> GetRole();
    }
    public class RoleService : IRoleService
    {
        private readonly ResourceMngContext _dbContext;

        public RoleService(ResourceMngContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<RoleDesc> GetRole()
        {
            var httpContext = new HttpContextAccessor().HttpContext;
            var result = _dbContext.RoleDescs
                .Join(_dbContext.PhanQuyens,
                rd => rd.Id,
                pq => pq.RoleId,
                (rd, pq) => new { RoleDesc = rd, PhanQuyen = pq })
                .Where(joined => joined.PhanQuyen.UserId == httpContext.Session.GetInt32("IDUser"))
                .OrderBy(joined =>  joined.RoleDesc.Stt)
                .Select(joined => joined.RoleDesc)
                .ToList();
            return result;
        }


    }
}
