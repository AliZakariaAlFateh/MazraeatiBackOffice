using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MazraeatiBackOffice.Controllers;
using MazraeatiBackOffice;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Models;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Configuration;
using ClosedXML.Excel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;

namespace MazraeatiBackOffice.Controllers
{
    public class FarmersController : BaseController
    {
        private readonly IRepository<Farmer> _FarmerRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<LookupValue> _LookupValueRepository;
        private readonly IRepository<FarmerExtraFeatureType> _FarmerExtraFeatureType;
        private readonly IRepository<FarmerPriceList> _FarmerPriceList;
        private readonly IRepository<FarmerUser> _FarmerUserRepository;
        private readonly IRepository<FarmerImage> _FarmerImage;
        private readonly IRepository<FarmerVideo> _FarmerVideo;
        private readonly IRepository<FarmerReservation> _FarmerReservation;
        private readonly IRepository<FarmerFeedback> _FarmerFeedback;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;

        private readonly DataContext _context;
        public FarmersController(IRepository<City> cityRepository, IRepository<Country> countryRepository, IRepository<Farmer> FarmerRepository,
            IRepository<LookupValue> LookupValueRepository, IRepository<FarmerExtraFeatureType> FarmerExtraFeatureType,
            IRepository<FarmerPriceList> FarmerPriceList, IRepository<FarmerImage> FarmerImage, IRepository<FarmerVideo> FarmerVideo,
            IRepository<FarmerUser> FarmerUserRepository, IRepository<FarmerReservation> FarmerReservation, IRepository<FarmerFeedback> FarmerFeedback,
            IUnitOfWork UnitOfWork, IWebHostEnvironment hostEnvironment, IConfiguration configuration, DataContext context)


        {
            _FarmerRepository = FarmerRepository;
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
            _LookupValueRepository = LookupValueRepository;
            _FarmerExtraFeatureType = FarmerExtraFeatureType;
            _FarmerPriceList = FarmerPriceList;
            _FarmerImage = FarmerImage;
            _FarmerVideo = FarmerVideo;
            _FarmerUserRepository = FarmerUserRepository;
            _FarmerReservation = FarmerReservation;
            _FarmerFeedback = FarmerFeedback;
            _UnitOfWork = UnitOfWork;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
            _context = context;
        }


        public FarmerModel NewFillModel(FarmerModel model)
        {
            model.Owner = "من المالك";
            model.Countries = _countryRepository.Table.Where(a => a.Id == 2 && a.Active == true).ToList();
            model.Cities = _cityRepository.Table.Where(a => a.Active == true && a.CountryId == 2 && a.Id != 20).ToList();
            List<LookupValue> lookupValue = _LookupValueRepository.Table.Where(l => l.LookupId == 2).ToList();
            foreach (LookupValue Value in lookupValue)
            {
                model.ExtraFeature.Add(new FarmerExtraFeatureTypeDto()
                {
                    FarmerId = 0,
                    TypeId = Value.Id,
                    ExtraText = Value.ValueAr,
                    ExtraTextDescriptionAr = "",
                    IsCheck = false

                });
            }
            model.PriceList = new List<FarmerPriceList>()
            {
                new FarmerPriceList(){ Id = 0 , FarmerId = 0 , Day = 1 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = 0 , Day = 2 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = 0 , Day = 3 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = 0 , Day = 4 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = 0 , Day = 5 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = 0 , Day = 6 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = 0 , Day = 7 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0, MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" }
            };
            return model;
        }

        public FarmerModel EditFillModel(FarmerModel model)
        {
            model.Countries = _countryRepository.Table.Where(a => a.Id == 2 && a.Active == true).ToList();
            model.Cities = _cityRepository.Table.Where(a => a.Active == true && a.CountryId == 2 && a.Id != 20).ToList();
            //model.Images=_FarmerImage.Table.where(x=>x.)
            model.FarmerImages = _FarmerImage.Table.Where(i => i.FarmerId == model.Id && i.Active == true).OrderBy(i => i.Sort).ToList();
            model.FarmerVideos = _FarmerVideo.Table.Where(v => v.FarmerId == model.Id && v.Active == true).OrderBy(v => v.Sort).ToList();
            List<FarmerExtraFeatureType> farmerExtraFeatureTypes = _FarmerExtraFeatureType.Table.Where(x => x.FarmerId == model.Id).ToList();
            List<LookupValue> lookupValue = _LookupValueRepository.Table.Where(l => l.LookupId == 2).ToList();
            foreach (LookupValue Value in lookupValue)
            {
                if (farmerExtraFeatureTypes.Count(f => f.TypeId == Value.Id) > 0)
                {
                    model.ExtraFeature.Add(new FarmerExtraFeatureTypeDto()
                    {
                        Id = farmerExtraFeatureTypes.FirstOrDefault(f => f.TypeId == Value.Id).Id,
                        FarmerId = model.Id,
                        TypeId = Value.Id,
                        ExtraText = Value.ValueAr,
                        ExtraTextDescriptionAr = farmerExtraFeatureTypes.Where(f => f.TypeId == Value.Id).FirstOrDefault().ExtraTextDescriptionAr,
                        ExtraTextDescriptionEn = farmerExtraFeatureTypes.Where(f => f.TypeId == Value.Id).FirstOrDefault().ExtraTextDescriptionEn,
                        IsCheck = farmerExtraFeatureTypes.Count(f => f.TypeId == Value.Id) > 0 ? true : false
                    });
                }
                else
                {
                    model.ExtraFeature.Add(new FarmerExtraFeatureTypeDto()
                    {
                        FarmerId = model.Id,
                        TypeId = Value.Id,
                        ExtraText = Value.ValueAr,//lable
                        ExtraTextDescriptionAr = "",
                        IsCheck = farmerExtraFeatureTypes.Count(f => f.TypeId == Value.Id) > 0 ? true : false

                    });
                }

            }
            model.PriceList = _FarmerPriceList.Table.Where(f => f.FarmerId == model.Id).OrderBy(f => f.Person).ThenBy(f => f.Day).ToList();
            return model;
        }

        public FarmerUserModel NewFillFarmerUserModel(FarmerUserModel model)
        {
            model.Id = 0;
            return model;
        }

        public IActionResult Index()
        {
            var Countries = _countryRepository.Table.Where(f => f.Id == 2).ToList();
            var Cities = _cityRepository.Table.Where(f => f.CountryId == 2).ToList();
            var Reservation = _FarmerReservation.Table.ToList();
            var FarmerFeedback = _FarmerFeedback.Table.ToList();
            var farmerBlackListIds = _UnitOfWork.FarmerBlackListRepository.Table.Where(a => a.FarmerId != null && a.IsBlocked == true)
                                     .Select(a => a.FarmerId).ToList();
            var model = _FarmerRepository.Table.Where(f => f.CountryId == 2 && !farmerBlackListIds.Contains(f.Id)).OrderByDescending(a => a.Id)
                .Select(c => c.ToModel(Countries, Cities, Reservation, FarmerFeedback));
            var farmer_counry_city = (from fr in
                                     _context.Farmers
                                     join cu in _context.Country on 
                                     fr.CountryId equals cu.Id
                                     join Ci in _context.City on 
                                     cu.Id equals Ci.CountryId
                                     where fr.CountryId == 6
                                     orderby fr.Name ascending
                                      select new { farmerName=fr.Name,countryName=cu.DescAr,cityName=Ci.DescAr }).ToList();

            //here we are select the country and the farms inside it ...
            var farms_inside_Country =
            (from cu in _context.Country.AsEnumerable()
             join fr in _context.Farmers.AsEnumerable()
            on cu.Id equals fr.CountryId
            into farmersgroup
             select new { countryName = cu.DescAr, farms = farmersgroup
                         .ToList() })
                         .ToList();

            var farms_inside_Country1 = _context.Country.
                Select(C => new { countryname=C.DescAr,
                farms= _context.Farmers
                .Where(F=>F.CountryId==C.Id)
                .ToList()
                })
                .AsEnumerable()
                .ToList();
            ViewBag.activePage = "المزارع";
            ViewBag.cities = Cities.Where(c => c.CountryId == 2);
            ViewBag.DefaultDate = DateTime.Now;
            return View(model);
        }



        //public IActionResult ReservationFarmers()
        //{
        //    var Reservation = _FarmerReservation.Table.ToList();
        //    return View(Reservation);
        //}
        //public IActionResult ReservationFarmers(int id)
        //{
        //    var Farmers = _FarmerRepository.Table.ToList();
        //    List<LookupValue> lookupValue = _LookupValueRepository.Table.Where(l => l.LookupId == 2).ToList();
        //    List<LookupValue> lookupValueReservationTypeId = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
        //    var currentMonthesReservation = _FarmerReservation.Table.Where(f => f.ReservationDate.Month == DateTime.Now.Month)
        //    .Select(x => new FarmerReservationModel
        //    {
        //        Id = x.Id,
        //        FarmerId = x.FarmerId,
        //        ReservationTypeId = x.ReservationTypeId,
        //        ReservationDate = x.ReservationDate,
        //        CustMobNum = x.CustMobNum,
        //        CustomerName = x.CustomerName,
        //        Note = x.Note,
        //        IsReciveCommission = x.IsReciveCommission,
        //        AutomaticallyNote = x.AutomaticallyNote,
        //        CreatedDate = x.CreatedDate,
        //        LookupValues = null
        //    })
        //    .ToList();
        //    var previousMonthesReservation = _FarmerReservation.Table.Where(f => f.ReservationDate.Month == (DateTime.Now.Month - 1))
        //    .Select(x => new FarmerReservationModel
        //    {
        //        Id = x.Id,
        //        FarmerId = x.FarmerId,
        //        ReservationTypeId = x.ReservationTypeId,
        //        ReservationDate = x.ReservationDate,
        //        CustMobNum = x.CustMobNum,
        //        CustomerName = x.CustomerName,
        //        Note = x.Note,
        //        IsReciveCommission = x.IsReciveCommission,
        //        AutomaticallyNote = x.AutomaticallyNote,
        //        CreatedDate = x.CreatedDate,
        //        LookupValues = null
        //    })
        //    .ToList();
        //    //var now = DateTime.Today;
        //    //var prev = now.AddMonths(-1);

        //    //var currentMonthReservations = _FarmerReservation.Table
        //    //    .Where(f => f.ReservationDate.Month == now.Month && f.ReservationDate.Year == now.Year)
        //    //    .ToList();

        //    //var previousMonthReservations = _FarmerReservation.Table
        //    //    .Where(f => f.ReservationDate.Month == prev.Month && f.ReservationDate.Year == prev.Year)
        //    //    .ToList();
        //    var reservations = _FarmerReservation.Table
        //        //.AsNoTracking() // uncomment if EF Core and read-only
        //        .Select(x => new FarmerReservationModel
        //        {
        //            Id = x.Id,
        //            FarmerId = x.FarmerId,
        //            ReservationTypeId = x.ReservationTypeId,
        //            ReservationDate = x.ReservationDate,
        //            CustMobNum = x.CustMobNum,
        //            CustomerName = x.CustomerName,
        //            Note = x.Note,
        //            IsReciveCommission = x.IsReciveCommission,
        //            AutomaticallyNote = x.AutomaticallyNote,
        //            CreatedDate = x.CreatedDate,
        //            LookupValues = null // map if you actually need it
        //        })
        //        .ToList();


        //    if (id == 1)
        //        return View(currentMonthesReservation);
        //    else if (id == -1)
        //        return View(previousMonthesReservation);
        //    else
        //        return View(reservations);

        //    // IMPORTANT: now you pass List<FarmerReservationModel> to the view
        //    //return View(reservations);
        //}


        public IActionResult ReservationFarmers(int id, int? year = null, int? month = null)
        {
            // 1) Dictionary للأنواع (in-memory)
            var typeMap = _LookupValueRepository.Table
                .Where(l => l.LookupId == 6)
                .Select(l => new { l.Id, l.ValueAr })
                .ToList()
                .ToDictionary(x => x.Id, x => x.ValueAr);

            // 2) حدّد مرجع الشهر (anchor)
            DateTime anchor;
            if (year.HasValue && month.HasValue)
            {
                anchor = new DateTime(year.Value, month.Value, 1);
            }
            else
            {
                // لو المستخدم ما حدّدش، نبدأ من اليوم
                //anchor = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                anchor= new DateTime(
                                    DateTime.Today.Year,
                                    DateTime.Today.Month,
                                    DateTime.Today.Day
                                );
            }

            // 3) حدود الشهور
            DateTime firstThis = anchor;
            DateTime firstNext = firstThis.AddMonths(1);
            DateTime firstPrev = firstThis.AddMonths(-1);

            // 4) اختَر الفلترة حسب id (1 = الشهر الحالي، -1 = السابق، غير كده = الكل)
            var baseQuery = _FarmerReservation.Table;

            IQueryable<MazraeatiBackOffice.Core.FarmerReservation> filteredQuery = id switch
            {
                1 => baseQuery.Where(r => r.ReservationDate >= firstThis && r.ReservationDate < firstNext),
                -1 => baseQuery.Where(r => r.ReservationDate >= firstPrev && r.ReservationDate < firstThis),
                _ => baseQuery
            };

            // 5) هات البيانات بشكل قابل للترجمة لـ SQL
            var raw = filteredQuery
                .Select(x => new
                {
                    x.Id,
                    x.FarmerId,
                    x.ReservationTypeId,
                    x.ReservationDate,
                    x.CustMobNum,
                    x.CustomerName,
                    x.Note,
                    x.IsReciveCommission,
                    x.AutomaticallyNote,
                    x.CreatedDate
                })
                .ToList();

            // 6) في حالة “الشهر السابق” أو “الحالي” ومع ذلك فاضيين،
            //    ولو المستخدم ما حدّدش year/month، نِفِل باك لأحدث شهر موجود في البيانات.
            if ((id == 1 || id == -1) && !year.HasValue && !month.HasValue && raw.Count == 0)
            {
                var maxDate = _FarmerReservation.Table
                    .Select(r => r.ReservationDate)
                    .DefaultIfEmpty()
                    .Max();

                if (maxDate != default)
                {
                    if (id == 1)
                    {
                        anchor = new DateTime(
                                            DateTime.Today.Year,
                                            DateTime.Today.Month,
                                            DateTime.Today.Day );
                    }
                    else
                    {
                        anchor = new DateTime(maxDate.Year, maxDate.Month, 1);
                    }
                    
                    firstThis = anchor;
                    firstNext = firstThis.AddMonths(1);
                    firstPrev = firstThis.AddMonths(-1);

                    var retryQuery = id == 1
                        ? baseQuery.Where(r => r.ReservationDate >= firstThis && r.ReservationDate < firstNext)
                        : baseQuery.Where(r => r.ReservationDate >= firstPrev && r.ReservationDate < firstThis);

                    raw = retryQuery
                        .Select(x => new
                        {
                            x.Id,
                            x.FarmerId,
                            x.ReservationTypeId,
                            x.ReservationDate,
                            x.CustMobNum,
                            x.CustomerName,
                            x.Note,
                            x.IsReciveCommission,
                            x.AutomaticallyNote,
                            x.CreatedDate
                        })
                        .ToList();
                }
            }

            // 7) ماب نهائي للـ ViewModel + اسم النوع بالعربي من الـ dictionary
            var models = raw.Select(x =>
            {
                typeMap.TryGetValue(x.ReservationTypeId, out var typeNameAr);
                return new FarmerReservationModel
                {
                    Id = x.Id,
                    FarmerId = x.FarmerId,
                    ReservationTypeId = x.ReservationTypeId,
                    ReservationTypeDesc = typeNameAr,
                    ReservationDate = x.ReservationDate,
                    CustMobNum = x.CustMobNum,
                    CustomerName = x.CustomerName,
                    Note = x.Note,
                    IsReciveCommission = x.IsReciveCommission,
                    AutomaticallyNote = x.AutomaticallyNote,
                    CreatedDate = x.CreatedDate,
                    LookupValues = null
                };
            }).ToList();

            return View(models);
        }



    [HttpGet]
    public IActionResult ReservationFarmersPage(
    int id,
    int? year,
    int? month,
    int page = 1,
    int pageSize = 10
    )
        {
            var typeMap = _LookupValueRepository.Table
                .Where(l => l.LookupId == 6)
                .Select(l => new { l.Id, l.ValueAr })
                .ToDictionary(x => x.Id, x => x.ValueAr);

            DateTime anchor = (year.HasValue && month.HasValue)
                ? new DateTime(year.Value, month.Value, 1)
                : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            DateTime firstThis = anchor;
            DateTime firstNext = firstThis.AddMonths(1);
            DateTime firstPrev = firstThis.AddMonths(-1);

            var baseQuery = _FarmerReservation.Table.AsQueryable();

            IQueryable<FarmerReservation> filteredQuery = id switch
            {
                1 => baseQuery.Where(r => r.ReservationDate >= firstThis && r.ReservationDate < firstNext),
                -1 => baseQuery.Where(r => r.ReservationDate >= firstPrev && r.ReservationDate < firstThis),
                _ => baseQuery
            };

            int totalCount = filteredQuery.Count();

            var items = filteredQuery
                .OrderByDescending(r => r.ReservationDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new
                {
                    x.Id,
                    x.FarmerId,
                    Type = typeMap.ContainsKey(x.ReservationTypeId)
                        ? typeMap[x.ReservationTypeId]
                        : "",
                    ReservationDate = x.ReservationDate.ToString("yyyy-MM-dd"),
                    x.CustomerName,
                    x.CustMobNum,
                    x.IsReciveCommission,
                    x.Note,
                    CreatedDate = x.CreatedDate.ToString("yyyy-MM-dd HH:mm")
                })
                .ToList();

            return Json(new
            {
                items,
                totalCount,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                currentPage = page
            });
        }


        [HttpGet]
        public IActionResult ReservationFarmersAjax(
            int id,
            int? year,
            int? month,
            int page = 1,
            int pageSize = 10,
            string search = "",
            string type = "",
            DateTime? fromDate = null,
            DateTime? toDate = null
        )
        {
            var typeMap = _LookupValueRepository.Table
                .Where(l => l.LookupId == 6)
                .ToDictionary(l => l.Id, l => l.ValueAr);

            DateTime anchor = (year.HasValue && month.HasValue)
                ? new DateTime(year.Value, month.Value, 1)
                : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            DateTime firstThis = anchor;
            DateTime firstNext = firstThis.AddMonths(1);
            DateTime firstPrev = firstThis.AddMonths(-1);

            IQueryable<FarmerReservation> q = id switch
            {
                1 => _FarmerReservation.Table.Where(r => r.ReservationDate >= firstThis && r.ReservationDate < firstNext),
                -1 => _FarmerReservation.Table.Where(r => r.ReservationDate >= firstPrev && r.ReservationDate < firstThis),
                _ => _FarmerReservation.Table
            };

            // 🔍 SEARCH
            if (!string.IsNullOrWhiteSpace(search))
            {
                q = q.Where(r =>
                    r.CustomerName.Contains(search) ||
                    r.CustMobNum.Contains(search) ||
                    r.Note.Contains(search) ||
                    r.AutomaticallyNote.Contains(search)
                );
            }

            // 📅 DATE FILTER
            if (fromDate.HasValue)
                q = q.Where(r => r.ReservationDate >= fromDate.Value);

            if (toDate.HasValue)
                q = q.Where(r => r.ReservationDate <= toDate.Value);

            // 🏷 TYPE FILTER
            if (!string.IsNullOrWhiteSpace(type))
            {
                q = q.Where(r => typeMap[r.ReservationTypeId] == type);
            }

            int totalCount = q.Count();

            var items = q
                .OrderByDescending(r => r.ReservationDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new
                {
                    r.FarmerId,
                    Type = typeMap[r.ReservationTypeId],
                    ReservationDate = r.ReservationDate.ToString("yyyy-MM-dd"),
                    r.CustomerName,
                    r.CustMobNum,
                    r.IsReciveCommission,
                    r.Note,
                    CreatedDate = r.CreatedDate.ToString("yyyy-MM-dd HH:mm")
                })
                .ToList();

            return Json(new
            {
                items,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                currentPage = page
            });
        }



        [HttpPost]
        public IActionResult Index(string search, int CityBy, DateTime ReservationDate)
        {
            if (string.IsNullOrEmpty(search) && CityBy == 0 && ReservationDate == DateTime.MinValue)
                return RedirectToAction("Index");

            search = string.IsNullOrEmpty(search) ? "" : search;
            //DateTime? _reservationDate = ReservationDate != null ? null : DateTime.Parse(ReservationDate);

            var farmerBlackListIds = _UnitOfWork.FarmerBlackListRepository.Table.Where(a => a.FarmerId != null && a.IsBlocked == true)
                                     .Select(a => a.FarmerId).ToList();
            var Countries = _countryRepository.Table.Where(f => f.Id == 2).ToList();
            var Cities = _cityRepository.Table.Where(f => f.CountryId == 2).ToList();
            var FarmerFeedback = _FarmerFeedback.Table.ToList();

            IQueryable<FarmerModel> model;
            var Reservation = _FarmerReservation.Table.ToList();
            if (ReservationDate != DateTime.MinValue)
            {
                Reservation = Reservation.Where(r => r.ReservationDate.Date == ReservationDate.Date).ToList();
                model = _FarmerRepository.Table.Where( f => f.CountryId == 2 && !farmerBlackListIds.Contains(f.Id)).OrderByDescending(a => a.Id).Where(a => (a.Name.Contains(search) ||
                                                                                        a.MobileNumber.Contains(search) ||
                                                                                        a.Number.ToString().Contains(search)) && (CityBy == 0 || (CityBy > 0 && a.CityId == CityBy)) &&
                                                                                        (!Reservation.Select(r => r.FarmerId).Contains(a.Id)) && a.CountryId == 2
                                                                                   ).Select(c => c.ToModel(Countries, Cities, Reservation, FarmerFeedback));
            }
            else
            {
                model = _FarmerRepository.Table.Where(f => f.CountryId == 2 && !farmerBlackListIds.Contains(f.Id)).OrderByDescending(a => a.Id).Where(a => (a.Name.Contains(search) ||
                                                                                                       a.MobileNumber.Contains(search) ||
                                                                                                       a.Number.ToString().Contains(search)) && (CityBy == 0 || (CityBy > 0 && a.CityId == CityBy)) &&
                                                                                                       a.CountryId == 2
                                                                                   ).Select(c => c.ToModel(Countries, Cities, Reservation, FarmerFeedback));
            }

            ViewBag.activePage = "المزارع";
            ViewBag.search = search;
            ViewBag.cities = Cities.Where(c => c.CountryId == 2);
            //AZ
            ViewBag.CityBy = CityBy;
            
            ViewBag.DefaultDate = DateTime.Now;
            return View(model);
        }

        #region :: Farmer Pending Approve

        public IActionResult FarmerPending()
        {
            var Countries = _countryRepository.Table.Where(f => f.Id == 2).ToList();
            var Cities = _cityRepository.Table.Where(f => f.CountryId == 2).ToList();

            var Reservation = _FarmerReservation.Table.ToList();
            var FarmerFeedback = _FarmerFeedback.Table.ToList();
            //var model = _FarmerRepository.Table.Where(f => f.CountryId == 6 && f.IsApprove == false).OrderByDescending(a => a.Id).Select(c => c.ToModel(Countries, Cities, Reservation, FarmerFeedback));
            var model = _FarmerRepository.Table.Where(f => f.CountryId == 2 && f.IsApprove == false).OrderByDescending(a => a.Id).Select(c => c.ToModel(Countries, Cities, Reservation, FarmerFeedback));

            //var model = _FarmerRepository.Table.Where(f => f.CountryId == 6 && f.IsApprove == false).OrderByDescending(a => a.Id).Select(c => c.ToModel(Countries, Cities, Reservation, FarmerFeedback));
            ViewBag.activePage = "المزارع الغير موافق عليهم";
            ViewBag.cities = Cities.Where(c => c.CountryId == 2);
            ViewBag.DefaultDate = DateTime.Now;
            return View(model);
        }

        [HttpPost]
        public IActionResult FarmerPending(string search, int CityBy, DateTime ReservationDate)
        {
            if (string.IsNullOrEmpty(search) && CityBy == 0 && ReservationDate == DateTime.MinValue)
                return RedirectToAction("FarmerPending");

            search = string.IsNullOrEmpty(search) ? "" : search;
            //DateTime _reservationDate = string.IsNullOrEmpty(ReservationDate) ? null : DateTime.Parse(ReservationDate);

            var Countries = _countryRepository.Table.Where(f => f.Id == 2).ToList();
            var Cities = _cityRepository.Table.Where(f => f.CountryId == 2).ToList();
            var FarmerFeedback = _FarmerFeedback.Table.ToList();

            IQueryable<FarmerModel> model;
            var Reservation = _FarmerReservation.Table.ToList();
            if (ReservationDate != DateTime.MinValue)
            {
                Reservation = Reservation.Where(r => r.ReservationDate.Date == ReservationDate.Date).ToList();
                model = _FarmerRepository.Table.OrderByDescending(a => a.Id).Where(a => (a.Name.Contains(search) ||
                                                                                        a.MobileNumber.Contains(search) ||
                                                                                        a.Number.ToString().Contains(search)) && (CityBy == 0 || (CityBy > 0 && a.CityId == CityBy)) &&
                                                                                        (Reservation.Select(r => r.FarmerId).Contains(a.Id)) && a.CountryId == 2
                                                                                   ).Select(c => c.ToModel(Countries, Cities, Reservation, FarmerFeedback));
            }
            else
            {
                model = _FarmerRepository.Table.OrderByDescending(a => a.Id).Where(a => (a.Name.Contains(search) ||
                                                                                                       a.MobileNumber.Contains(search) ||
                                                                                                       a.Number.ToString().Contains(search)) && (CityBy == 0 || (CityBy > 0 && a.CityId == CityBy))
                                                                                                       && a.CountryId == 2
                                                                                   ).Select(c => c.ToModel(Countries, Cities, Reservation, FarmerFeedback));
            }
            ViewBag.activePage = "المزارع الغير موافق عليهم";
            ViewBag.search = search;
            ViewBag.cities = Cities.Where(c => c.CountryId == 2);
            ViewBag.DefaultDate = DateTime.Now;
            return View(model);
        }

        #endregion

        #region :: Farmer Not Completed Information

        public IActionResult FarmerNotCompletedInfo()
        {
            var Countries = _countryRepository.Table.Where(f => f.Id == 2).ToList();
            var Cities = _cityRepository.Table.Where(f => f.CountryId == 2).ToList();
            var Reservation = _FarmerReservation.Table.ToList();
            var FarmerFeedback = _FarmerFeedback.Table.ToList();
            var FarmerImage = _FarmerImage.Table.ToList().Select(x => x.FarmerId).ToArray();
            var FarmerPriceList = _FarmerPriceList.Table.ToList().Select(x => x.FarmerId).ToArray();

            var model = _FarmerRepository.Table.Where(f => f.CountryId == 2 && !FarmerImage.Contains(f.Id) || !FarmerPriceList.Contains(f.Id))
                                    .OrderByDescending(a => a.Id).Select(c => c.ToModel(Countries, Cities, Reservation, FarmerFeedback));

            ViewBag.activePage = "المزارع غير مكتملة المعلومات";
            ViewBag.cities = Cities.Where(c => c.CountryId == 2);
            ViewBag.DefaultDate = DateTime.Now;
            return View(model);
        }

        [HttpPost]
        public IActionResult FarmerNotCompletedInfo(string search, int CityBy, DateTime ReservationDate)
        {
            if (string.IsNullOrEmpty(search) && CityBy == 0 && ReservationDate == DateTime.MinValue)
                return RedirectToAction("FarmerNotCompletedInfo");

            search = string.IsNullOrEmpty(search) ? "" : search;
            //DateTime _reservationDate = string.IsNullOrEmpty(ReservationDate) ? null : DateTime.Parse(ReservationDate);

            var Countries = _countryRepository.Table.Where(f => f.Id == 2).ToList();
            var Cities = _cityRepository.Table.Where(f => f.CountryId == 2).ToList();
            var FarmerFeedback = _FarmerFeedback.Table.ToList();
            var FarmerImage = _FarmerImage.Table.ToList().Select(x => x.FarmerId).ToArray();
            var FarmerPriceList = _FarmerPriceList.Table.ToList().Select(x => x.FarmerId).ToArray();

            IQueryable<FarmerModel> model;
            var Reservation = _FarmerReservation.Table.ToList();
            model = _FarmerRepository.Table.Where(f => !FarmerImage.Contains(f.Id) || !FarmerPriceList.Contains(f.Id))
                                                                        .OrderByDescending(a => a.Id).Where(a => (a.Name.Contains(search) ||
                                                                                                   a.MobileNumber.Contains(search) ||
                                                                                                   a.Number.ToString().Contains(search)) && (CityBy == 0 || (CityBy > 0 && a.CityId == CityBy))
                                                                                                   && a.CountryId == 2
                                                                               ).Select(c => c.ToModel(Countries, Cities, Reservation, FarmerFeedback));
            ViewBag.activePage = "المزارع غير مكتملة المعلومات";
            ViewBag.search = search;
            ViewBag.cities = Cities.Where(c => c.CountryId == 2);
            ViewBag.DefaultDate = DateTime.Now;
            return View(model);
        }

        #endregion

        public IActionResult Create()
        {
            ViewBag.activePage = "المزارع";
            return View(NewFillModel(new FarmerModel()));
        }

        [HttpPost]
        public IActionResult Create(FarmerModel model, IFormFile formFile)
        {
            LogFile logFile = new LogFile();

            try
            {
                if (ModelState.IsValid)
                {
                    // get max number of farms
                    long nMaxNumber = _FarmerRepository.Table.ToList().Select(x => x.Number).DefaultIfEmpty(0).Max();
                    int nId = _FarmerRepository.Table.ToList().Select(x => x.Id).DefaultIfEmpty(0).Max();

                    // create new farms
                    model.Number = nMaxNumber + 1;
                    model.Description = "";
                    //model.Owner = "من المالك";
                    model.IssueDate = DateTime.Now;
                    model.ExpiryDate = DateTime.Now.AddMonths(3);

                    _UnitOfWork.FarmerRepository.InsertEntity(model.ToEntity());
                    _UnitOfWork.Save();


                    int nFarmsId = _FarmerRepository.Table.FirstOrDefault(f => f.Number == (nMaxNumber + 1)).Id;

                    // add extra feature
                    foreach (FarmerExtraFeatureTypeDto extra in model.ExtraFeature.Where(e => e.IsCheck == true))
                    {
                        FarmerExtraFeatureType farmerExtraFeatureType = new FarmerExtraFeatureType();
                        farmerExtraFeatureType.FarmerId = nFarmsId;
                        farmerExtraFeatureType.TypeId = extra.TypeId;
                        farmerExtraFeatureType.ExtraText = extra.ExtraText;
                        farmerExtraFeatureType.ExtraTextDescriptionAr = extra.ExtraTextDescriptionAr;
                        farmerExtraFeatureType.ExtraTextDescriptionEn = extra.ExtraTextDescriptionEn;


                        _UnitOfWork.FarmerExtraFeatureTypeRepository.Insert(farmerExtraFeatureType);

                    }

                    // add price list
                    foreach (FarmerPriceList priceList in model.PriceList)
                    {
                        FarmerPriceList farmerPriceList = new FarmerPriceList();
                        farmerPriceList.FarmerId = nFarmsId;
                        farmerPriceList.Day = priceList.Day;
                        farmerPriceList.MorningPrice = priceList.MorningPrice;
                        farmerPriceList.EveningPrice = priceList.EveningPrice;
                        farmerPriceList.FullDayPrice = priceList.FullDayPrice;
                        farmerPriceList.OfferPrice = priceList.OfferPrice;
                        farmerPriceList.OfferEveningPrice = priceList.OfferEveningPrice;
                        farmerPriceList.OfferFullDayPrice = priceList.OfferFullDayPrice;
                        farmerPriceList.MorningPeriodText = priceList.MorningPeriodText;
                        farmerPriceList.EveningPeriodText = priceList.EveningPeriodText;
                        farmerPriceList.FullDayPeriodText = string.IsNullOrEmpty(priceList.FullDayPeriodText) ? "" : priceList.FullDayPeriodText;

                        _UnitOfWork.FarmerPriceListRepository.Insert(farmerPriceList);

                    }


                    // add image
                    int nSortImage = 1;
                    if (model.Images != null)
                    {
                        foreach (IFormFile file in model.Images)
                        {
                            FarmerImage farmerImage = new FarmerImage();

                            if (file != null)
                                farmerImage.Url = "farmer/" + GenericFunction.UploadedFile(file, webHostEnvironment, "farmer");

                            farmerImage.FarmerId = nFarmsId;
                            farmerImage.Sort = nSortImage;
                            farmerImage.Vip = true;
                            farmerImage.Active = true;

                            _UnitOfWork.FarmerImageRepository.Insert(farmerImage);
                            nSortImage++;
                        }
                    }


                    // add videos
                    int nSortVideo = 1;
                    if (model.Videos != null)
                    {
                        foreach (IFormFile file in model.Videos)
                        {
                            FarmerVideo farmerVideo = new FarmerVideo();

                            if (file != null)
                                farmerVideo.Url = GenericFunction.UploadedVideo(file, webHostEnvironment);

                            farmerVideo.FarmerId = nFarmsId;
                            farmerVideo.Sort = nSortVideo;
                            farmerVideo.Active = true;

                            _UnitOfWork.FarmerVideoRepository.Insert(farmerVideo);
                            nSortVideo++;
                        }
                    }


                    _UnitOfWork.Save();
                    SuccessNotification("تم اضافة السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification("error while saving farms , please contact to administrator");
                logFile.LogCustomInfo("Create Farmer - Exception Message ", e.Message);
                logFile.LogCustomInfo("Create Farmer - Stack Trace Message ", e.StackTrace);
                logFile.LogCustomInfo("Create Farmer - Inner Exception Message ", e.InnerException.ToString());
            }
            //ViewBag.CountryId = model.CountryId;
            //ViewBag.CityId = model.CityId;
            //AZ
            model.Countries = _countryRepository.Table.Where(a => a.Id == 2 && a.Active == true).ToList();
            model.Cities = _cityRepository.Table.Where(a => a.Active == true && a.CountryId == 2 && a.Id != 20).ToList();
            //return View(NewFillModel(new FarmerModel()));"
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            Farmer farmer = _UnitOfWork.FarmerRepository.GetById(id);
            var farmerExtraFeature = _UnitOfWork.FarmerExtraFeatureTypeRepository.Table.Where(a => a.FarmerId == farmer.Id).ToList();
            if (farmer == null)
                return RedirectToAction("Index");


            ViewBag.activePage = "المزارع";
            return View(EditFillModel(farmer.ToModel()));
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        //public IActionResult Edit(FarmerModel model, IFormFile formFile)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            int maxOrderIdImage = 0;
        //            int maxOrderIdVideo = 0;

        //            if (_UnitOfWork.FarmerImageRepository.Table.ToList().Count(f => f.FarmerId == model.Id) > 0)
        //                maxOrderIdImage = _UnitOfWork.FarmerImageRepository.Table.ToList().Where(f => f.FarmerId == model.Id).Select(x => x.Sort).DefaultIfEmpty(0).Max();

        //            if (_UnitOfWork.FarmerVideoRepository.Table.ToList().Count(f => f.FarmerId == model.Id) > 0)
        //                maxOrderIdVideo = _UnitOfWork.FarmerVideoRepository.Table.ToList().Where(f => f.FarmerId == model.Id).Select(x => x.Sort).DefaultIfEmpty(0).Max();

        //            if (model.PriceList != null)
        //            {
        //                // edit price list
        //                foreach (FarmerPriceList priceList in model.PriceList)
        //                {
        //                    _UnitOfWork.FarmerPriceListRepository.Update(priceList);
        //                }
        //            }

        //            if (model.ExtraFeature != null)
        //            {
        //                // edit extra feature
        //                foreach (FarmerExtraFeatureTypeDto extra in model.ExtraFeature)
        //                {
        //                    FarmerExtraFeatureType farmerExtraFeatureType = new FarmerExtraFeatureType();
        //                    farmerExtraFeatureType.Id = extra.Id;
        //                    farmerExtraFeatureType.FarmerId = model.Id;
        //                    farmerExtraFeatureType.ExtraText = extra.ExtraText;
        //                    farmerExtraFeatureType.ExtraTextDescriptionAr = extra.ExtraTextDescriptionAr;
        //                    farmerExtraFeatureType.ExtraTextDescriptionEn = extra.ExtraTextDescriptionEn;
        //                    farmerExtraFeatureType.TypeId = extra.TypeId;

        //                    if (extra.Id > 0)
        //                    {
        //                        if (!extra.IsCheck)
        //                        {
        //                            _UnitOfWork.FarmerExtraFeatureTypeRepository.Delete(farmerExtraFeatureType);
        //                        }
        //                        else
        //                        {
        //                            if (_FarmerExtraFeatureType.Table.Count(f => f.FarmerId == model.Id && f.TypeId == extra.TypeId) == 0)
        //                                _UnitOfWork.FarmerExtraFeatureTypeRepository.Insert(farmerExtraFeatureType);
        //                            else if (_FarmerExtraFeatureType.Table.Count(f => f.FarmerId == model.Id && f.TypeId == extra.TypeId) == 1)
        //                            {
        //                                _UnitOfWork.FarmerExtraFeatureTypeRepository.Update(farmerExtraFeatureType);
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (extra.IsCheck)
        //                            _UnitOfWork.FarmerExtraFeatureTypeRepository.Insert(farmerExtraFeatureType);
        //                    }
        //                }
        //            }


        //            // add new image
        //            int nSortImage = maxOrderIdImage + 1;
        //            if (model.Images != null)
        //            {
        //                foreach (IFormFile file in model.Images)
        //                {
        //                    FarmerImage farmerImage = new FarmerImage();

        //                    if (file != null)
        //                        farmerImage.Url = "farmer/" + GenericFunction.UploadedFile(file, webHostEnvironment, "farmer");

        //                    farmerImage.FarmerId = model.Id;
        //                    farmerImage.Sort = nSortImage;
        //                    farmerImage.Vip = true;
        //                    farmerImage.Active = true;

        //                    _UnitOfWork.FarmerImageRepository.Insert(farmerImage);
        //                    nSortImage++;
        //                }
        //            }


        //            // add new video
        //            int nSortVideo = maxOrderIdVideo + 1;
        //            if (model.Videos != null)
        //            {
        //                foreach (IFormFile file in model.Videos)
        //                {
        //                    FarmerVideo farmerVideo = new FarmerVideo();

        //                    if (file != null)
        //                        farmerVideo.Url = GenericFunction.UploadedVideo(file, webHostEnvironment);

        //                    farmerVideo.FarmerId = model.Id;
        //                    farmerVideo.Sort = nSortVideo;
        //                    farmerVideo.Active = true;

        //                    _UnitOfWork.FarmerVideoRepository.Insert(farmerVideo);
        //                    nSortVideo++;
        //                }
        //            }

        //            _UnitOfWork.FarmerRepository.Update(model.ToEntity());
        //            _UnitOfWork.Save();
        //            SuccessNotification("تم تحديث السجل بنجاح");
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorNotification("error while saving farms , please contact to administrator");
        //    }
        //    //AZ
        //    model.Countries = _countryRepository.Table.Where(a => a.Id == 2 && a.Active == true).ToList();
        //    model.Cities = _cityRepository.Table.Where(a => a.Active == true && a.CountryId == 2 && a.Id != 20).ToList();
        //    return View(model);
        //}









        public IActionResult Edit(FarmerModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int maxOrderIdImage = 0;
                    int maxOrderIdVideo = 0;

                    if (_UnitOfWork.FarmerImageRepository.Table.ToList().Count(f => f.FarmerId == model.Id) > 0)
                        maxOrderIdImage = _UnitOfWork.FarmerImageRepository.Table.ToList().Where(f => f.FarmerId == model.Id).Select(x => x.Sort).DefaultIfEmpty(0).Max();

                    if (_UnitOfWork.FarmerVideoRepository.Table.ToList().Count(f => f.FarmerId == model.Id) > 0)
                        maxOrderIdVideo = _UnitOfWork.FarmerVideoRepository.Table.ToList().Where(f => f.FarmerId == model.Id).Select(x => x.Sort).DefaultIfEmpty(0).Max();

                    if (model.PriceList != null)
                    {
                        // edit price list
                        foreach (FarmerPriceList priceList in model.PriceList)
                        {
                            _UnitOfWork.FarmerPriceListRepository.Update(priceList);
                        }
                    }

                    if (model.ExtraFeature != null)
                    {
                        // edit extra feature
                        foreach (FarmerExtraFeatureTypeDto extra in model.ExtraFeature)
                        {
                            FarmerExtraFeatureType farmerExtraFeatureType = new FarmerExtraFeatureType();
                            farmerExtraFeatureType.Id = extra.Id;
                            farmerExtraFeatureType.FarmerId = model.Id;
                            farmerExtraFeatureType.ExtraText = extra.ExtraText;
                            farmerExtraFeatureType.ExtraTextDescriptionAr = extra.ExtraTextDescriptionAr;
                            farmerExtraFeatureType.ExtraTextDescriptionEn = extra.ExtraTextDescriptionEn;
                            farmerExtraFeatureType.TypeId = extra.TypeId;

                            if (extra.Id > 0)
                            {
                                if (!extra.IsCheck)
                                {
                                    _UnitOfWork.FarmerExtraFeatureTypeRepository.Delete(farmerExtraFeatureType);
                                }
                                else
                                {
                                    if (_FarmerExtraFeatureType.Table.Count(f => f.FarmerId == model.Id && f.TypeId == extra.TypeId) == 0)
                                        _UnitOfWork.FarmerExtraFeatureTypeRepository.Insert(farmerExtraFeatureType);
                                    else if (_FarmerExtraFeatureType.Table.Count(f => f.FarmerId == model.Id && f.TypeId == extra.TypeId) == 1)
                                    {
                                        _UnitOfWork.FarmerExtraFeatureTypeRepository.Update(farmerExtraFeatureType);
                                    }
                                }
                            }
                            else
                            {
                                if (extra.IsCheck)
                                    _UnitOfWork.FarmerExtraFeatureTypeRepository.Insert(farmerExtraFeatureType);
                            }
                        }
                    }


                    // add new image
                    int nSortImage = maxOrderIdImage + 1;
                    if (model.Images != null)
                    {
                        foreach (IFormFile file in model.Images)
                        {
                            FarmerImage farmerImage = new FarmerImage();

                            if (file != null)
                                farmerImage.Url = "farmer/" + GenericFunction.UploadedFile(file, webHostEnvironment, "farmer");

                            farmerImage.FarmerId = model.Id;
                            farmerImage.Sort = nSortImage;
                            farmerImage.Vip = true;
                            farmerImage.Active = true;

                            _UnitOfWork.FarmerImageRepository.Insert(farmerImage);
                            nSortImage++;
                        }
                    }


                    // add new video
                    int nSortVideo = maxOrderIdVideo + 1;
                    if (model.Videos != null)
                    {
                        foreach (IFormFile file in model.Videos)
                        {
                            FarmerVideo farmerVideo = new FarmerVideo();

                            if (file != null)
                                farmerVideo.Url = GenericFunction.UploadedVideo(file, webHostEnvironment);

                            farmerVideo.FarmerId = model.Id;
                            farmerVideo.Sort = nSortVideo;
                            farmerVideo.Active = true;

                            _UnitOfWork.FarmerVideoRepository.Insert(farmerVideo);
                            nSortVideo++;
                        }
                    }

                    _UnitOfWork.FarmerRepository.Update(model.ToEntity());
                    _UnitOfWork.Save();
                    SuccessNotification("تم تحديث السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                // Return the exception message to the user
                ErrorNotification($"Error while saving farms: {e.Message}. Please contact the administrator.");
            }
            //AZ
            model.Countries = _countryRepository.Table.Where(a => a.Id == 2 && a.Active == true).ToList();
            model.Cities = _cityRepository.Table.Where(a => a.Active == true && a.CountryId == 2 && a.Id != 20).ToList();
            return View(model);
        }







        public IActionResult Delete(int id)
        {
            string result = "1";
            Farmer farmer = _FarmerRepository.GetById(id);
            if (farmer == null)
                return Json("Record Not Exists");

            try
            {
                _UnitOfWork.FarmerRepository.Delete(farmer);
                _UnitOfWork.Save();
                SuccessNotification("Delete Succesfuly");
            }
            catch (Exception ex)
            {
                result = "There is data associated with this record";
            }

            return Json(result);
        }



        #region :: Edit Image 

        public IActionResult FarmerImages(int id)
        {
            List<FarmerImage> farmerImages = _UnitOfWork.FarmerImageRepository.Table.Where(x => x.FarmerId == id).ToList();
            FarmerImagesModel farmerImagesModel = new FarmerImagesModel();
            farmerImagesModel.Images = farmerImages;
            farmerImagesModel.FarmerName = _UnitOfWork.FarmerRepository.GetById(id).Name;
            farmerImagesModel.FarmerId = id;
            ViewBag.activePage = "صور المزارع";
            return View(farmerImagesModel);
        }


        [HttpPost]
        public IActionResult EditFarmsVIPImage(string Ids)
        {
            try
            {
                if (string.IsNullOrEmpty(Ids))
                    return Json("الرجاء اختيار الصور");

                List<int> vs = ConvertStringListToIntList(Ids);

                foreach (var item in vs)
                {
                    FarmerImage farmsImage = _FarmerImage.Table.FirstOrDefault(i => i.Id == item);
                    farmsImage.Vip = !farmsImage.Vip;
                    _UnitOfWork.FarmerImageRepository.Update(farmsImage);
                }

                _UnitOfWork.Save();
                return Json(1);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        [HttpPost]
        public IActionResult EditActiveOrNotImage(string Ids)
        {
            try
            {
                if (string.IsNullOrEmpty(Ids))
                    return Json("الرجاء اختيار الصور");

                List<int> vs = ConvertStringListToIntList(Ids);

                foreach (var item in vs)
                {
                    FarmerImage farmsImage = _FarmerImage.Table.FirstOrDefault(i => i.Id == item);
                    farmsImage.Active = !farmsImage.Active;
                    _UnitOfWork.FarmerImageRepository.Update(farmsImage);
                }

                _UnitOfWork.Save();
                return Json(1);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        [HttpPost]
        public IActionResult DeleteFarmsImage(string Ids)
        {
            try
            {
                if (string.IsNullOrEmpty(Ids))
                    return Json("الرجاء اختيار الصور");

                List<int> vs = ConvertStringListToIntList(Ids);

                foreach (var item in vs)
                {
                    FarmerImage farmsImage = _FarmerImage.Table.FirstOrDefault(i => i.Id == item);
                    _UnitOfWork.FarmerImageRepository.Delete(farmsImage);
                }

                _UnitOfWork.Save();
                return Json(1);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }


        #region :: Download Multiple Files

        [HttpPost]
        public IActionResult SaveFarmerImage(int farmerId)
        {
            List<FarmerImage> _images = _FarmerImage.Table.Where(f => f.FarmerId == farmerId).ToList();
            foreach (FarmerImage img in _images)
            {
                string url = $"{Request.Scheme}://{Request.Host}/Images/{img.Url}";
                WebClient client = new WebClient();
                Stream stream = client.OpenRead(url);
                Bitmap bitmap = new Bitmap(stream);

                if (bitmap != null)
                {
                    bitmap.Save("Image1.png", ImageFormat.Png);
                }

                stream.Flush();
                stream.Close();
                client.Dispose();
            }
            return Json(1);
        }

        [HttpPost]
        public IActionResult DeleteAllImg(int farmerId)
        {
            try
            {
                List<FarmerImage> farmerImges = _FarmerImage.Table.Where(f => f.FarmerId == farmerId).ToList();
                foreach (FarmerImage farmsImage in farmerImges)
                {
                    _UnitOfWork.FarmerImageRepository.Delete(farmsImage);
                }

                _UnitOfWork.Save();
                return Json(1);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        #endregion


        #endregion


        #region :: Edit Videos

        public IActionResult FarmerVideos(int id)
        {
            List<FarmerVideo> farmerVideos = _UnitOfWork.FarmerVideoRepository.Table.Where(x => x.FarmerId == id).ToList();
            FarmerVideosModel farmerVideosModel = new FarmerVideosModel();
            farmerVideosModel.Videos = farmerVideos;
            farmerVideosModel.FarmerName = _UnitOfWork.FarmerRepository.GetById(id).Name;
            farmerVideosModel.FarmerId = id;
            ViewBag.activePage = "فيديوهات المزارع";
            return View(farmerVideosModel);
        }


        [HttpPost]
        public IActionResult EditActiveOrNotVideo(string Ids)
        {
            try
            {
                if (string.IsNullOrEmpty(Ids))
                    return Json("الرجاء اختيار الفيدوهات");

                List<int> vs = ConvertStringListToIntList(Ids);

                foreach (var item in vs)
                {
                    FarmerVideo farmsVideos = _FarmerVideo.Table.FirstOrDefault(i => i.Id == item);
                    farmsVideos.Active = !farmsVideos.Active;
                    _UnitOfWork.FarmerVideoRepository.Update(farmsVideos);
                }

                _UnitOfWork.Save();
                return Json(1);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        [HttpPost]
        public IActionResult DeleteFarmsVideo(string Ids)
        {
            try
            {
                if (string.IsNullOrEmpty(Ids))
                    return Json("الرجاء اختيار الفيدوهات");

                List<int> vs = ConvertStringListToIntList(Ids);

                foreach (var item in vs)
                {
                    FarmerVideo farmsVideos = _FarmerVideo.Table.FirstOrDefault(i => i.Id == item);
                    _UnitOfWork.FarmerVideoRepository.Delete(farmsVideos);
                }

                _UnitOfWork.Save();
                return Json(1);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        #endregion

        #region :: Farmer User

        public IActionResult FarmerUser(int farmerId)
        {
            Farmer farmer = _UnitOfWork.FarmerRepository.Table.FirstOrDefault(f => f.Id == farmerId);

            #region :: Check If Have Any User For Farmer 

            FarmerUserModel farmerUserModel = new FarmerUserModel();
            int _count = _UnitOfWork.FarmerUserRepository.Table.Count(f => f.FarmerId == farmerId);
            if (_count > 0)
            {
                farmerUserModel = _FarmerUserRepository.Table.Where(f => f.FarmerId == farmerId).Select(c => c.ToModel()).FirstOrDefault();
            }
            else
            {
                farmerUserModel.Id = 0;
                farmerUserModel.FarmerId = farmerId;
            }


            #endregion


            ViewBag.activePage = "معلومات الدخول للتطبيق الادمن";
            ViewBag.farmerName = farmer.Name;

            return View(farmerUserModel);
        }

        [HttpPost]
        public IActionResult FarmerUser(FarmerUserModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    #region :: Validation 


                    model.UserName = model.UserName.Replace(" ", string.Empty);
                    model.Password = model.Password.Replace(" ", string.Empty);

                    int userNameExists = _FarmerUserRepository.Table.Count(f => f.UserName == model.UserName && f.Id != model.Id);
                    if (userNameExists > 0)
                    {
                        ErrorNotification("اسم المستخدم موجود مسبقا يجب عدم تكراره");
                        return View(model);
                    }

                    #endregion

                    if (model.Id == 0)
                    {
                        model.Id = 0;
                        model.CreatedDate = DateTime.Now;
                        model.UpdateDate = DateTime.Now;
                        _UnitOfWork.FarmerUserRepository.Insert(model.ToEntity());
                    }
                    else
                    {
                        model.UpdateDate = DateTime.Now;
                        _UnitOfWork.FarmerUserRepository.Update(model.ToEntity());
                    }


                    _UnitOfWork.Save();
                    SuccessNotification("تم اضافة / تعديل  السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {

            }
            return View(NewFillFarmerUserModel(new FarmerUserModel()));
        }

        #endregion

        #region :: Farmer Price List

        public IActionResult FarmerPriceList(int farmerId)
        {
            if (farmerId == 0)
            {
                ViewBag.activePage = "المزارع";
                TempData["ErrorMessage"] = "الرجاء حفظ المزرعة أولا";
                //return View("Create", NewFillModel(new FarmerModel()));
                return RedirectToAction("Create", "Farmers");
            }
            Farmer farmer = _UnitOfWork.FarmerRepository.Table.FirstOrDefault(f => f.Id == farmerId);

            FarmerPriceModel farmerPriceModel = new FarmerPriceModel();

            farmerPriceModel.FarmerId = farmerId;

            farmerPriceModel.PriceList = new List<FarmerPriceList>()
            {
                new FarmerPriceList(){ Id = 0 , FarmerId = farmerId , Day = 1 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = farmerId , Day = 2 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = farmerId , Day = 3 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = farmerId , Day = 4 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = farmerId , Day = 5 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = farmerId , Day = 6 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = farmerId , Day = 7 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0, MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" }
            };

            ViewBag.activePage = "اسعار المزرعة";
            ViewBag.farmerName = farmer.Name;
            return View(farmerPriceModel);
        }

        [HttpPost]
        public IActionResult FarmerPriceList(FarmerPriceModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    #region :: Validation 

                    if (model.Person <= 0)
                    {
                        ErrorNotification("يجب ادخال عدد الاشخاص");
                        return View(model);
                    }

                    int personExists = _FarmerPriceList.Table.Count(f => f.Person == model.Person && f.FarmerId == model.FarmerId);
                    if (personExists > 0)
                    {
                        ErrorNotification("عدد الاشخاص موجود مسبقا");
                        return View(model);
                    }

                    #endregion

                    foreach (var item in model.PriceList)
                    {
                        item.FarmerId = model.FarmerId;
                        item.Person = model.Person;
                        _UnitOfWork.FarmerPriceListRepository.Insert(item);
                    }

                    _UnitOfWork.Save();
                    SuccessNotification("تم اضافة / تعديل  السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {

            }
            return View(NewFillFarmerUserModel(new FarmerUserModel()));
        }

        [HttpPost]
        public IActionResult DeletePriceList(DeleteFarmerPriceModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    #region :: Validation 

                    if (model.FarmerId <= 0)
                    {
                        ErrorNotification("يجب ادخال رقم المزرعة");
                        return View(model);
                    }

                    #endregion


                    List<FarmerPriceList> farmerPriceList = _FarmerPriceList.Table.Where(f => f.FarmerId == model.FarmerId & f.Person == model.Person).Distinct().ToList();

                    foreach (FarmerPriceList item in farmerPriceList)
                    {
                        _UnitOfWork.FarmerPriceListRepository.Delete(item);
                    }

                    _UnitOfWork.Save();
                    SuccessNotification("تم الحذف بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {

            }
            return View(NewFillFarmerUserModel(new FarmerUserModel()));
        }

        #endregion

        #region Export To Excel Sheet Newer

        //public IActionResult FarmerExcel(string search, int CityBy, DateTime? ReservationDate) // Added parameters
        //{
        //    using (var workbook = new XLWorkbook())
        //    {
        //        var worksheet = workbook.Worksheets.Add("مزارع");
        //        var currentRow = 1;

        //        List<City> cities = _cityRepository.Table.ToList();
        //        List<FarmerReservation> farmerReservations = _FarmerReservation.Table.ToList();

        //        // Start with a base query for farmers
        //        IQueryable<Farmer> farmsToExport = _FarmerRepository.Table.AsQueryable();

        //        // Apply filters based on received parameters
        //        if (!string.IsNullOrEmpty(search))
        //        {
        //            farmsToExport = farmsToExport.Where(f => f.Name.Contains(search) ||
        //                                                    f.MobileNumber.Contains(search) ||
        //                                                    f.Number.ToString().Contains(search));
        //        }

        //        if (CityBy > 0) // Only filter if a specific city is selected (not "0" which means "all cities")
        //        {
        //            farmsToExport = farmsToExport.Where(f => f.CityId == CityBy);
        //        }

        //        if (ReservationDate.HasValue && ReservationDate.Value != DateTime.MinValue)
        //        {
        //            // Get FarmerIds that have reservations on the specified date
        //            var farmersWithReservationsOnDate = farmerReservations
        //                .Where(r => r.ReservationDate.Date == ReservationDate.Value.Date)
        //                .Select(r => r.FarmerId)
        //                .Distinct()
        //                .ToList();

        //            // Filter the farms to only include those with reservations on that date
        //            farmsToExport = farmsToExport.Where(f => farmersWithReservationsOnDate.Contains(f.Id));
        //        }

        //        // Order the results (optional, but good for consistency)
        //        farmsToExport = farmsToExport.OrderByDescending(f => f.Id);

        //        // Execute the query and get the filtered list
        //        var filteredFarms = farmsToExport.ToList();


        //        // --- Excel Header ---
        //        worksheet.Cell(currentRow, 1).Value = "#";
        //        worksheet.Cell(currentRow, 2).Value = "اسم المزرعة";
        //        worksheet.Cell(currentRow, 3).Value = "رقم المزرعة";
        //        worksheet.Cell(currentRow, 4).Value = "المدينة";
        //        worksheet.Cell(currentRow, 5).Value = "عدد الحجوزات خلال الشهر"; // Note: This still calculates total reservations, not just for the filtered month
        //        worksheet.Cell(currentRow, 6).Value = "رقم التلفون";

        //        // --- Populate Data ---
        //        foreach (var model in filteredFarms)
        //        {
        //            currentRow++;
        //            worksheet.Cell(currentRow, 1).Value = model.Id.ToString();
        //            worksheet.Cell(currentRow, 2).Value = model.Name;
        //            worksheet.Cell(currentRow, 3).Value = model.Number;
        //            // Use null-conditional operator to avoid NullReferenceException if city not found
        //            worksheet.Cell(currentRow, 4).Value = cities.FirstOrDefault(c => c.Id == model.CityId)?.DescAr;
        //            // This still counts all reservations for the farmer, not just those matching ReservationDate filter
        //            worksheet.Cell(currentRow, 5).Value = farmerReservations.Count(f => f.FarmerId == model.Id);
        //            worksheet.Cell(currentRow, 6).Value = model.MobileNumber;
        //        }

        //        using (var stream = new MemoryStream())
        //        {
        //            workbook.SaveAs(stream);
        //            var content = stream.ToArray();

        //            return File(
        //                content,
        //                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //                "Farmers_Syria.xlsx");
        //        }
        //    }
        //}

        [HttpGet]
        public IActionResult FarmerExcel(string search, int CityBy, DateTime? ReservationDate,string? filename)
        {

            if (filename == null)
            {
                filename = "جميع مزارع الأردن";
            }
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("مزارع");
                var currentRow = 1;

                List<City> cities = _cityRepository.Table.ToList();
                List<FarmerReservation> farmerReservations = _FarmerReservation.Table.ToList();

                IQueryable<Farmer> farmsToExport = _FarmerRepository.Table.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    farmsToExport = farmsToExport.Where(f => f.Name.Contains(search) ||
                                                            f.MobileNumber.Contains(search) ||
                                                            f.Number.ToString().Contains(search));
                }

                if (CityBy > 0)
                {
                    farmsToExport = farmsToExport.Where(f => f.CityId == CityBy);
                }

                if (ReservationDate.HasValue && ReservationDate.Value != DateTime.MinValue)
                {
                    var farmersWithReservationsOnDate = farmerReservations
                        .Where(r => r.ReservationDate.Date == ReservationDate.Value.Date)
                        .Select(r => r.FarmerId)
                        .Distinct()
                        .ToList();

                    farmsToExport = farmsToExport.Where(f => farmersWithReservationsOnDate.Contains(f.Id));
                }

                farmsToExport = farmsToExport.OrderByDescending(f => f.Id);
                var filteredFarms = farmsToExport.ToList();

                worksheet.Cell(currentRow, 1).Value = "#";
                worksheet.Cell(currentRow, 2).Value = "اسم المزرعة";
                worksheet.Cell(currentRow, 3).Value = "رقم المزرعة";
                worksheet.Cell(currentRow, 4).Value = "المدينة";
                worksheet.Cell(currentRow, 5).Value = "عدد الحجوزات خلال الشهر";
                worksheet.Cell(currentRow, 6).Value = "رقم التلفون";

                foreach (var model in filteredFarms)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = model.Id.ToString();
                    worksheet.Cell(currentRow, 2).Value = model.Name;
                    worksheet.Cell(currentRow, 3).Value = model.Number;
                    worksheet.Cell(currentRow, 4).Value = cities.FirstOrDefault(c => c.Id == model.CityId)?.DescAr;
                    worksheet.Cell(currentRow, 5).Value = farmerReservations.Count(f => f.FarmerId == model.Id);
                    worksheet.Cell(currentRow, 6).Value = model.MobileNumber;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    // Set the Content-Disposition header for filename suggestion

                    Response.Headers.Add("Content-Disposition", $"attachment; filename={filename}.xlsx");

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    );
                }
            }
        }
        #endregion



        #region

        //public IActionResult FarmerExcel()
        //{
        //    using (var workbook = new XLWorkbook())
        //    {
        //        var worksheet = workbook.Worksheets.Add("مزارع");
        //        var currentRow = 1;

        //        List<City> cities = _cityRepository.Table.ToList();
        //        List<FarmerReservation> farmerReservations = _FarmerReservation.Table.ToList();
        //        List<Farmer> farms = _FarmerRepository.Table.ToList();

        //        worksheet.Cell(currentRow, 1).Value = "#";
        //        worksheet.Cell(currentRow, 2).Value = "اسم المزرعة";
        //        worksheet.Cell(currentRow, 3).Value = "رقم المزرعة";
        //        worksheet.Cell(currentRow, 4).Value = "المدينة";
        //        worksheet.Cell(currentRow, 5).Value = "عدد الحجوزات خلال الشهر";
        //        worksheet.Cell(currentRow, 6).Value = "رقم التلفون";


        //        foreach (var model in farms)
        //        {
        //            currentRow++;
        //            worksheet.Cell(currentRow, 1).Value = model.Id.ToString();
        //            worksheet.Cell(currentRow, 2).Value = model.Name;
        //            worksheet.Cell(currentRow, 3).Value = model.Number;
        //            worksheet.Cell(currentRow, 4).Value = cities.FirstOrDefault(c => c.Id == model.CityId).DescAr;
        //            worksheet.Cell(currentRow, 5).Value = farmerReservations.Count(f => f.FarmerId == model.Id);
        //            worksheet.Cell(currentRow, 6).Value = model.MobileNumber;
        //        }

        //        using (var stream = new MemoryStream())
        //        {
        //            workbook.SaveAs(stream);
        //            var content = stream.ToArray();

        //            return File(
        //                content,
        //                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //                "Farmers.xlsx");
        //        }
        //    }
        //}

        #endregion




        [HttpPost]
        public IActionResult DeleteFarmerImage(int imageId)
        {
            try
            {
                var imageToDelete = _FarmerImage.Table.FirstOrDefault(i => i.Id == imageId);

                if (imageToDelete == null)
                {
                    Console.WriteLine($"Image with ID {imageId} not found for deletion.");
                    return Json(new { success = false, message = "الصورة غير موجودة في قاعدة البيانات." }); // More specific message
                }

                string imagePath = Path.Combine(webHostEnvironment.WebRootPath, imageToDelete.Url.TrimStart('/'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                    Console.WriteLine($"Physical image file deleted: {imagePath}");
                }
                else
                {
                    Console.WriteLine($"Physical image file not found at: {imagePath}. Deleting database record only.");
                }

                _FarmerImage.Delete(imageToDelete);
                // _unitOfWork.Commit(); // Uncomment if you're using a Unit of Work pattern and configured it

                // *** THIS IS THE CRUCIAL CHANGE FOR YOUR FRONTEND MESSAGE ***
                return Json(new { success = true, message = "تم حذف الصورة بنجاح." });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting image (ID: {imageId}): {ex.Message}");
                return Json(new { success = false, message = $"حدث خطأ أثناء حذف الصورة: {ex.Message}" });
            }
        }

        [HttpPost]
        public IActionResult DeleteFarmerVideo(int videoId)
        {
            try
            {
                var videoToDelete = _FarmerVideo.Table.FirstOrDefault(v => v.Id == videoId);

                if (videoToDelete == null)
                {
                    Console.WriteLine($"Video with ID {videoId} not found for deletion.");
                    return Json(new { success = false, message = "الفيديو غير موجود في قاعدة البيانات." }); // More specific message
                }

                string videoPath = Path.Combine(webHostEnvironment.WebRootPath, videoToDelete.Url.TrimStart('/'));

                if (System.IO.File.Exists(videoPath))
                {
                    System.IO.File.Delete(videoPath);
                    Console.WriteLine($"Physical video file deleted: {videoPath}");
                }
                else
                {
                    Console.WriteLine($"Physical video file not found at: {videoPath}. Deleting database record only.");
                }

                _FarmerVideo.Delete(videoToDelete);
                // _unitOfWork.Commit(); // Uncomment if you're using a Unit of Work pattern and configured it

                // *** THIS IS THE CRUCIAL CHANGE FOR YOUR FRONTEND MESSAGE ***
                return Json(new { success = true, message = "تم حذف الفيديو بنجاح." });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting video (ID: {videoId}): {ex.Message}");
                return Json(new { success = false, message = $"حدث خطأ أثناء حذف الفيديو: {ex.Message}" });
            }
        }

    }
}
