using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Helper
{
    public class PagedList<T>:List<T>
    {
        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count/(double)pageSize); //pt a calcula nr de pagini
            PageSize = pageSize;
            TotalCount = count;
            AddRange(items);//pentru a avea acces la itemele din page list.
        }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source,int pageNumber,int pageSize)//aceasta metoda creeaza o instanta a clasei si returneaza o instanta noua cu noile informatii
        {
            var count = await source.CountAsync();

            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();//nu dam skip la niciun record si luam 5.
                                                                                                    //de exemplu daca suntem pe pagina 2  si luam 5 records,o sa dam skip la primele 5 si trecem pe pag 2

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

    }
}
