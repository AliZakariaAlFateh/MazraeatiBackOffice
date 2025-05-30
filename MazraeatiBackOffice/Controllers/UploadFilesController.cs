using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models;

namespace MazraeatiBackOffice.Controllers
{
    [Route("api/Upload")]
    [ApiController]
    public class UploadFilesController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<FarmerImage> _FarmerImageRepository;
        private readonly IRepository<FarmerVideo> _FarmerVideoRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        public UploadFilesController(IRepository<FarmerImage> FarmerImageRepository, IRepository<FarmerVideo> FarmerVideoRepository, IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _FarmerImageRepository = FarmerImageRepository;
            _FarmerVideoRepository = FarmerVideoRepository;
            _UnitOfWork = unitOfWork;
            webHostEnvironment = hostEnvironment;
        }

        [HttpPost("image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UploadNewFile([FromForm] string farmerId, List<IFormFile> formFile) 
        {
            try
            {
                if (formFile.Count == 0)
                    return BadRequest(new { message = "No File Exists" });


                for (int index = 0; index < formFile.Count; index++)
                {
                    FarmerImage farmerImage = new FarmerImage();
                    farmerImage.FarmerId = int.Parse(farmerId);
                    farmerImage.Url = "farmer/" + GenericFunction.UploadedFile(formFile[index], webHostEnvironment, "farmer");
                    farmerImage.Active = false;
                    farmerImage.Sort = 1;
                    farmerImage.Vip = false;

                    try
                    {
                        _UnitOfWork.FarmerImageRepository.Insert(farmerImage);
                        _UnitOfWork.Save();
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
              

                try
                {
                    return Ok(new { isSuccess = true, Message = "", data = "" });
                }
                catch(Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("video")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UploadNewVideoFile([FromForm] string farmerId, IFormFile formFile)
        {
            try
            {
                if (formFile.Length == 0)
                    return BadRequest(new { message = "No File Exists" });


                FarmerVideo farmerVideo = new FarmerVideo();
                farmerVideo.FarmerId = int.Parse(farmerId);
                farmerVideo.Url = "Videos/" + GenericFunction.UploadedVideo(formFile, webHostEnvironment);
                farmerVideo.Active = false;
                farmerVideo.Sort = 1;

                try
                {
                    _UnitOfWork.FarmerVideoRepository.Insert(farmerVideo);
                    _UnitOfWork.Save();
                    return Ok(new { isSuccess = true, Message = "", data = "" });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }


        [HttpPost("farmerBlackList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UploadFarmerBlackList([FromForm] string farmerMobNum, string farmerName, string reason, IFormFile formFile)
        {
            try
            {
                FarmerBlackList farmerBlackList = new FarmerBlackList();
                farmerBlackList.FarmerMobNum = farmerMobNum;
                farmerBlackList.FarmerName = farmerName;
                farmerBlackList.Reason = reason;

                if (formFile.Length > 0)
                    farmerBlackList.ImageUrl = "blacklist/" + GenericFunction.UploadedFile(formFile, webHostEnvironment, "blacklist");

                farmerBlackList.IsApprove = false;

                try
                {
                    _UnitOfWork.FarmerBlackListRepository.Insert(farmerBlackList);
                    _UnitOfWork.Save();
                    return Ok(new { isSuccess = true, Message = "", data = "" });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("customerBlackList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UploadCustomerBlackList([FromForm] string custMobileNum, [FromForm]  string custName, [FromForm]  string reason, IFormFile formFile)
        {
            try
            {
                CustomerBlackList customerBlackList = new CustomerBlackList();
                customerBlackList.CustMobileNum = custMobileNum;
                customerBlackList.CustName = custName;
                customerBlackList.Reason = reason;

                if (formFile != null)
                    customerBlackList.ImageUrl = "blacklist/" + GenericFunction.UploadedFile(formFile, webHostEnvironment, "blacklist");

                customerBlackList.IsApprove = false;

                try
                {
                    _UnitOfWork.CustomerBlackListRepository.Insert(customerBlackList);
                    _UnitOfWork.Save();
                    return Ok(new { isSuccess = true, Message = "", data = "" });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
