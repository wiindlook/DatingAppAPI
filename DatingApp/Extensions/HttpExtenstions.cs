using DatingApp.Helper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DatingApp.Extensions
{
    public static class HttpExtenstions
    {
        public static void AddPaginationHeader(this HttpResponse response,int currentPage,int itemsPerPage,int totalitems,int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalitems, totalPages);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            response.Headers.Add("Pagination",JsonSerializer.Serialize(paginationHeader,options));//custom header
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination"); //pentru a face headerul avalabile trb sa adaugam un clause header

           


        }
    }
}
