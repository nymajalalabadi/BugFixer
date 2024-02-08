using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.domain.ViewModels.Common
{
    public class Paging<T>
    {
        public Paging()
        {
            CurrentPage = 1;
            HowManyShowBeforeAfter = 3;
            TakeEntity = 10;
            Entities = new List<T>();
        }

        public int CurrentPage { get; set; }

        public int StartPage { get; set; }

        public int EndPage { get; set; }

        public int TotalPage { get; set; }

        public int HowManyShowBeforeAfter { get; set; }

        public int TakeEntity { get; set; }

        public int SkipEntity { get; set; }

        public int AllEntityCount { get; set; }

        public List<T> Entities { get; set; }

        public PagingViewModel GetPaging()
        {
            var result = new PagingViewModel
            {
                CurrentPage = this.CurrentPage,
                EndPage = this.EndPage,
                StartPage = this.StartPage
            };

            return result;
        }

        public async Task SetPaging(IQueryable<T> query)
        {
            AllEntityCount = query.Count();

            TotalPage = (int)Math.Ceiling(AllEntityCount / (double)TakeEntity);

            CurrentPage = CurrentPage < 1 ? 1 : CurrentPage;

            CurrentPage = CurrentPage > TotalPage ? TotalPage : CurrentPage;

            SkipEntity = (CurrentPage - 1) * TakeEntity;

            StartPage = CurrentPage - HowManyShowBeforeAfter > 0 ? CurrentPage - HowManyShowBeforeAfter : 1;

            EndPage = CurrentPage + HowManyShowBeforeAfter > TotalPage
                ? TotalPage
                : CurrentPage + HowManyShowBeforeAfter;

            if (query.Any())
            {
                Entities = await query.Skip(SkipEntity).Take(TakeEntity).ToListAsync();
            }
        }
    }

    public class PagingViewModel
    {
        public int CurrentPage { get; set; }

        public int StartPage { get; set; }

        public int EndPage { get; set; }
    }
}
