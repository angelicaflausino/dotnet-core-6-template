using Company.Default.Domain.Options;

namespace Company.Default.Domain.Contracts.Base
{
    public abstract class FilterParameterBase
    {
        private int _page = ApiOptions.DefaultPage;
        private int _size = ApiOptions.DefaultPageSize;
        private string _sortBy = ApiOptions.DefaultSortBy;

        public virtual int Page { get => _page; set => _page = value; }
        public virtual int Size { get => _size; set => _size = value; }
        public virtual string SortBy { get => _sortBy; set => _sortBy = value ?? ApiOptions.DefaultSortBy; }
    }
}
