using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Interfaces
{
   public  interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file); //iformfile se refera la un file de tipul http request 

        Task<DeletionResult> DeletePhotoAsync(string publicId); //avem nevoie de id pentru a putea sterge poza
    }
}
