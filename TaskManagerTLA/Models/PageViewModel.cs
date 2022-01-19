using System;

namespace TaskManagerTLA.Models
{
    public class PageViewModel
    {
        public int PageNumber { get; private set; }
        public int TotalPage { get; private set; }

        public PageViewModel(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPage = (int)Math.Ceiling(count / (double)pageSize);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageNumber > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageNumber < TotalPage);
            }
        }
    }
}
