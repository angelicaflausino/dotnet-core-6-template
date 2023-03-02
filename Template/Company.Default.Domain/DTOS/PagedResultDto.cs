namespace $safeprojectname$.Dtos
{
    public class PagedResultDto<T> where T : class
    {
        public PagedResultDto()
        {
            this.Result = new List<T>();
        }

        public int Total { get; set; }
        public IList<T> Result { get; set; }
    }
}
