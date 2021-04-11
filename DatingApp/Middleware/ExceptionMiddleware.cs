using DatingApp.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace DatingApp.Middleware
{
    public class ExceptionMiddleware //middleware ul se ocupa de exceptii, cand adaugam un middleware intr-un api,ne trb un constructor in care bagam

    {
        private readonly RequestDelegate _next;  //un request delegate e ceea ce urmeaza in middleware pipeline.
        private readonly ILogger<ExceptionMiddleware> _logger; //Illogger pentru a ne loga exceptia in terminal 
        private readonly IHostEnvironment _env; //de asemenea vrem sa verificam in ce enviroment suntem,production development etc

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }


        //ii dam middleware-ului metoda specifica, 
        //cand bagam un middleware avem acces la requestul care vine
        // se intampla in contextul unui httprequest
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);  //primul lucru pe care il facem este sa luam contextul si  sa il dam mai departe la un middleware
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);//pentru a vedea eroarea in terminal
                context.Response.ContentType = "application/json"; //scriem exceptia in raspuns;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; //practic 500

                var response = _env.IsDevelopment() //verificam in ce enviroment suntem // operatorul se intreaba ce facem daca asta este development mode
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) //in caz ca e null folosim "?" pt a nu cauza o execptie .
                    : new ApiException(context.Response.StatusCode, "Internal Server Eror"); // nu le dam detalii
               // In simple terms, a stacktrace is a list of the method calls that the application was in the middle of when an Exception was thrown.
                
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }; //trebuie ca raspunsul in json sa fie CamelCase
                var json = JsonSerializer.Serialize(response, options); //ii aplicam optiunile cand il serializam
                await context.Response.WriteAsync(json);
            }
        }
    }
}

