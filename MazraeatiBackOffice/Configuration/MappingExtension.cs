
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Office;
using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Core.LoyaltyPoints;
using MazraeatiBackOffice.Dto;
using MazraeatiBackOffice.Models;
using MazraeatiBackOffice.Models.LoyaltyPoints;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MazraeatiBackOffice.Extenstion
{
    public static class MappingExtension
    {

        #region Country
        public static CountryModel ToModel(this Country entity)
        {
            CountryModel model = new CountryModel();
            model.Id = entity.Id;
            model.Code = entity.Code;
            model.DescAr = entity.DescAr;
            model.DescEn = entity.DescEn;
            model.ImageUrl = entity.ImageUrl;
            model.Active = entity.Active;
            return model;
        }
        public static Country ToEntity(this CountryModel model)
        {
            Country entity = new Country();
            entity.Id = model.Id;
            entity.Code = model.Code;
            entity.DescAr = model.DescAr;
            entity.DescEn = model.DescEn;
            entity.ImageUrl = model.ImageUrl;
            entity.Active = model.Active;
            return entity;
        }
        #endregion

        #region City
        public static CityModel ToModel(this City entity, List<Country> countries = null)
        {
            CityModel model = new CityModel();
            model.Id = entity.Id;
            model.DescAr = entity.DescAr;
            model.DescEn = entity.DescEn;
            model.CountryId = entity.CountryId;
            model.CountryName = countries == null ? string.Empty : countries.Where(c => c.Id == entity.CountryId).FirstOrDefault().DescAr;
            model.Active = entity.Active;
            return model;
        }
        public static City ToEntity(this CityModel model)
        {
            City entity = new City();
            entity.Id = model.Id;
            entity.DescAr = model.DescAr;
            entity.DescEn = model.DescEn;
            entity.CountryId = model.CountryId;
            entity.Active = model.Active;
            return entity;
        }
        #endregion

        #region Farmer
        public static FarmerModel ToModel(this Farmer entity, List<Country> countries = null, List<City> cities = null
            , List<FarmerReservation> farmerReservations = null,
            List<FarmerFeedback> farmerFeedbacks = null, List<AppUser> users = null, List<Regions> regions = null)
        {
            FarmerModel model = new FarmerModel();
            model.Id = entity.Id;
            model.CountryId = entity.CountryId;
            model.CityId = entity.CityId;
            //entity.RegionId = model.RegionId;
            model.RegionId = entity.RegionId;
            model.UserId = entity.UserId;
            //model.CountryDesc = countries == null ? string.Empty : countries.Where(c => c.Id == entity.CountryId).FirstOrDefault().DescAr;
            //model.CityDesc = cities == null ? string.Empty : cities.Where(c => c.Id == entity.CityId).FirstOrDefault().DescAr;
            //model.UserDesc = users == null ? string.Empty : users.Where(c => c.Id == entity.UserId).FirstOrDefault().UserName;
            //model.RegionDesc = regions == null ? string.Empty : regions.Where(c => c.Id == entity.RegionId).FirstOrDefault().DescAr;
            //model.UserName = users == null ? string.Empty : users.Where(c => c.Id == entity.UserId).FirstOrDefault().UserName;
            model.CountryDesc = countries?
                .FirstOrDefault(c => c.Id == entity.CountryId)
                ?.DescAr ?? "";

            model.CityDesc = cities?
                .FirstOrDefault(c => c.Id == entity.CityId)
                ?.DescAr ?? "";

            model.UserDesc = users?
                .FirstOrDefault(c => c.Id == entity.UserId)
                ?.UserName ?? "";

            model.RegionDesc = regions?
                .FirstOrDefault(c => c.Id == entity.RegionId)
                ?.DescAr ?? "";

            model.UserName = users?
                .FirstOrDefault(c => c.Id == entity.UserId)
                ?.UserName ?? "";
            model.MobileNumber = entity.MobileNumber;
            //model.statusFarmAppUser = (FarmAppUserStatus)(entity.statusFarmAppUser ?? 0);
            model.statusFarmAppUser = (entity.statusFarmAppUser ?? 0);

            model.Number = entity.Number;
            model.Name = entity.Name;
            model.NameEn = entity.NameEn;

            model.Description = entity.Description;
            model.DescriptionEn = entity.DescriptionEn;

            model.Owner = entity.Owner;
            model.LocationDesc = entity.LocationDesc;
            model.LocationDescEn = entity.LocationDescEn;

            model.IssueDate = entity.IssueDate;
            model.ExpiryDate = entity.ExpiryDate;
            model.EStateArea = entity.EStateArea;
            model.Room = entity.Room;
            model.RoomDetails = entity.RoomDetails;
            model.RoomDetailsEn = entity.RoomDetailsEn;

            model.Bathroom = entity.Bathroom;
            model.BathroomDetails = entity.BathroomDetails;
            model.BathroomDetailsEn = entity.BathroomDetailsEn;

            model.LandArea = entity.LandArea;
            model.Floor = entity.Floor;
            model.InDoor = entity.InDoor;
            model.InDoorDescription = entity.InDoorDescription;
            model.InDoorDescriptionEn = entity.InDoorDescriptionEn;

            model.OutDoor = entity.OutDoor;
            model.OutDoorDescription = entity.OutDoorDescription;
            model.OutDoorDescriptionEn = entity.OutDoorDescriptionEn;

            model.kitchens = entity.kitchens;
            model.kitchensDescription = entity.kitchensDescription;
            model.kitchensDescriptionEn = entity.kitchensDescriptionEn;

            model.ExtraDetails = entity.ExtraDetails;
            model.ReservationDetails = entity.ReservationDetails;
            model.Family = entity.Family;
            model.Location = entity.Location;
            model.Longitude = entity.Longitude;
            model.Latitude = entity.Latitude;
            model.InsuranceAmt = entity.InsuranceAmt;
            model.DepositAmt = entity.DepositAmt;
            model.MaxPerson = entity.MaxPerson;
            model.ConfidentialMessageEn = entity.ConfidentialMessageEn;
            model.ConfidentialMessageAr = entity.ConfidentialMessageAr;
            model.Image3DLink = entity.Image3DLink;
            model.IsTrust = entity.IsTrust;
            model.IsVIP = entity.IsVIP;
            model.IsOffer = entity.IsOffer;
            model.IsApprove = entity.IsApprove;
            model.ReservationCount = farmerReservations != null ? farmerReservations.Count(f => f.FarmerId == model.Id && f.IsMahjouzReservation == false) : 0;
            model.FeedbackCount = farmerFeedbacks != null ? farmerFeedbacks.Count(f => f.FarmerId == model.Id) : 0;
            model.MobileOwnerAppUser = entity.MobileNumber;
            model.SerialFarmKey = entity.SerialFarmKey;
            return model;
        }
        public static Farmer ToEntity(this FarmerModel model)
        {
            Farmer entity = new Farmer();
            entity.Id = model.Id;
            entity.CountryId = model.CountryId;
            entity.CityId = model.CityId;
            entity.RegionId = model.RegionId;
            entity.UserId = model.UserId;
            entity.MobileNumber = model.MobileNumber;
            entity.Number = model.Number;
            entity.Name = model.Name;
            entity.NameEn = model.NameEn;
            //entity.statusFarmAppUser = (FarmAppUserStatus?)model.statusFarmAppUser;
            entity.statusFarmAppUser = model.statusFarmAppUser;

            entity.statusFarmAppUser = model.statusFarmAppUser;
            entity.Description = model.Description;
            entity.DescriptionEn = model.DescriptionEn;

            entity.Owner = model.Owner;
            entity.LocationDesc = model.LocationDesc;
            entity.LocationDescEn = model.LocationDescEn;

            entity.IssueDate = model.IssueDate;
            entity.ExpiryDate = model.ExpiryDate;
            entity.EStateArea = model.EStateArea;
            entity.Room = model.Room;
            entity.RoomDetails = model.RoomDetails;
            entity.RoomDetailsEn = model.RoomDetailsEn;

            entity.Bathroom = model.Bathroom;
            entity.BathroomDetails = model.BathroomDetails;
            entity.BathroomDetailsEn = model.BathroomDetailsEn;

            entity.LandArea = model.LandArea;
            entity.Floor = model.Floor;
            entity.InDoor = model.InDoor;
            entity.InDoorDescription = model.InDoorDescription;
            entity.InDoorDescriptionEn = model.InDoorDescriptionEn;

            entity.OutDoor = model.OutDoor;
            entity.OutDoorDescription = model.OutDoorDescription;
            entity.OutDoorDescriptionEn = model.OutDoorDescriptionEn;

            entity.kitchens = model.kitchens;
            entity.kitchensDescription = model.kitchensDescription;
            entity.kitchensDescriptionEn = model.kitchensDescriptionEn;

            entity.ExtraDetails = model.ExtraDetails;
            entity.ReservationDetails = model.ReservationDetails;
            entity.Family = model.Family;
            entity.Location = model.Location;
            entity.Longitude = model.Longitude;
            entity.Latitude = model.Latitude;
            entity.InsuranceAmt = model.InsuranceAmt;
            entity.DepositAmt = model.DepositAmt;
            entity.MaxPerson = model.MaxPerson;
            entity.ConfidentialMessageEn = model.ConfidentialMessageEn;
            entity.ConfidentialMessageAr = model.ConfidentialMessageAr;
            entity.Image3DLink = model.Image3DLink;
            entity.IsTrust = model.IsTrust;
            entity.IsVIP = model.IsVIP;
            entity.IsOffer = model.IsOffer;
            entity.IsApprove = model.IsApprove;
            entity.MobileOwnerAppUser = model.MobileNumber;
            entity.SerialFarmKey = model.SerialFarmKey;
            return entity;
        }
        #endregion

        #region Setting
        public static SettingModel ToModel(this Setting entity)
        {
            SettingModel model = new SettingModel();
            model.Id = entity.Id;
            model.Name = entity.Name;
            model.Value = entity.Value;
            model.IsEditior = entity.IsEditior;
            model.IsMedia = entity.IsMedia;
            model.DisplayName = entity.DisplayName;
            return model;
        }
        public static Setting ToEntity(this SettingModel model)
        {
            Setting entity = new Setting();
            entity.Id = model.Id;
            entity.Name = model.Name;
            entity.Value = model.Value;
            entity.IsEditior = model.IsEditior;
            entity.IsMedia = model.IsMedia;
            entity.DisplayName = model.DisplayName;
            return entity;
        }

        #endregion

        #region ImageSlider
        public static ImageSliderModel ToModel(this ImageSlider entity, List<Country> countries = null)
        {
            ImageSliderModel model = new ImageSliderModel();
            model.Id = entity.Id;
            model.CountryId = entity.CountryId;
            model.PageName = entity.PageName;
            model.CountryDesc = countries != null ? countries.FirstOrDefault(c => c.Id == entity.CountryId).DescAr : string.Empty;
            model.Image = entity.Image;
            model.ExtraText = entity.ExtraText;
            model.ExtraTextEn = entity.ExtraTextEn;

            model.RedirectLink = entity.RedirectLink;
            model.Target = entity.Target;
            model.Value = entity.Value;
            model.SortOrder = entity.SortOrder;
            model.Active = entity.Active;
            return model;
        }
        public static ImageSlider ToEntity(this ImageSliderModel model)
        {
            ImageSlider entity = new ImageSlider();
            entity.Id = model.Id;
            entity.CountryId = model.CountryId;
            entity.PageName = model.PageName;
            entity.Image = model.Image;
            entity.ExtraText = model.ExtraText;
            entity.ExtraTextEn = model.ExtraTextEn;

            entity.RedirectLink = model.RedirectLink;
            entity.Target = model.Target;
            entity.Value = model.Value;
            entity.SortOrder = model.SortOrder;
            entity.Active = model.Active;
            return entity;
        }

        #endregion

        #region Onboarding
        public static OnboardingModel ToModel(this Onboarding entity, List<Country> countries = null)
        {
            OnboardingModel model = new OnboardingModel();
            model.Id = entity.Id;
            model.CountryId = entity.CountryId;
            model.CountryDesc = countries != null ? countries.FirstOrDefault(c => c.Id == entity.CountryId).DescAr : string.Empty;
            model.Type = entity.Type;
            model.Url = entity.Url;
            model.ExtraText = entity.ExtraText;
            model.ExtraTextEn = entity.ExtraTextEn;

            model.ExpiryDate = entity.ExpiryDate;
            return model;
        }
        public static Onboarding ToEntity(this OnboardingModel model)
        {
            Onboarding entity = new Onboarding();
            entity.Id = model.Id;
            entity.CountryId = model.CountryId;
            entity.Type = model.Type;
            entity.Url = model.Url;
            entity.ExtraText = model.ExtraText;
            entity.ExtraTextEn = model.ExtraTextEn;

            entity.ExpiryDate = model.ExpiryDate;
            return entity;
        }

        #endregion

        #region Trip Section
        public static TripSectionModel ToModel(this TripSection entity, List<Country> countries = null)
        {
            TripSectionModel model = new TripSectionModel();
            model.Id = entity.Id;
            model.CountryId = entity.CountryId;
            model.CountryDesc = countries != null ? countries.FirstOrDefault(c => c.Id == entity.CountryId).DescAr : string.Empty;
            model.Title = entity.Title;
            model.MainImage = entity.MainImage;
            model.ExtraText = entity.ExtraText;
            model.ExtraTextEn = entity.ExtraTextEn;


            model.OrderId = entity.OrderId;
            model.Active = entity.Active;
            return model;
        }
        public static TripSection ToEntity(this TripSectionModel model)
        {
            TripSection entity = new TripSection();
            entity.Id = model.Id;
            entity.CountryId = model.CountryId;
            entity.Title = model.Title;
            entity.MainImage = model.MainImage;
            entity.ExtraText = model.ExtraText;
            entity.ExtraTextEn = model.ExtraTextEn;

            entity.OrderId = model.OrderId;
            entity.Active = model.Active;
            return entity;
        }

        #endregion

        #region :: Trip 

        public static TripModel ToModel(this Trip entity, List<City> cities = null, List<TripSection> tripSections = null)
        {
            TripModel model = new TripModel();
            model.Id = entity.Id;
            model.TripSectionId = entity.TripSectionId;
            model.TripSectionDesc = tripSections != null ? tripSections.FirstOrDefault(c => c.Id == entity.TripSectionId).Title : string.Empty;
            model.CityId = entity.CityId;
            model.CityDesc = cities != null ? cities.FirstOrDefault(c => c.Id == entity.CityId).DescAr : string.Empty;
            model.MobileNumber = entity.MobileNumber;
            model.Number = entity.Number;
            model.Name = entity.Name;
            model.Description = entity.Description;
            model.Owner = entity.Owner;
            model.LocationDesc = entity.LocationDesc;
            model.IssueDate = entity.IssueDate;
            model.ExpiryDate = entity.ExpiryDate;
            model.ExtraDetails = entity.ExtraDetails;
            model.ReservationDetails = entity.ReservationDetails;
            model.Location = entity.Location;
            model.InsuranceAmt = entity.InsuranceAmt;
            model.DepositAmt = entity.DepositAmt;
            model.MaxPerson = entity.MaxPerson;
            model.IsTrust = entity.IsTrust;
            model.IsVIP = entity.IsVIP;
            model.IsOffer = entity.IsOffer;
            model.IsApprove = entity.IsApprove;
            model.IsWinter = entity.IsWinter;
            return model;
        }
        public static Trip ToEntity(this TripModel model)
        {
            Trip entity = new Trip();
            entity.Id = model.Id;
            entity.Id = model.Id;
            entity.TripSectionId = model.TripSectionId;
            entity.CityId = model.CityId;
            entity.MobileNumber = model.MobileNumber;
            entity.Number = model.Number;
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Owner = model.Owner;
            entity.LocationDesc = model.LocationDesc;
            entity.IssueDate = model.IssueDate;
            entity.ExpiryDate = model.ExpiryDate;
            entity.ExtraDetails = model.ExtraDetails;
            entity.ReservationDetails = model.ReservationDetails;
            entity.Location = model.Location;
            entity.InsuranceAmt = model.InsuranceAmt;
            entity.DepositAmt = model.DepositAmt;
            entity.MaxPerson = model.MaxPerson;
            entity.IsTrust = model.IsTrust;
            entity.IsVIP = model.IsVIP;
            entity.IsOffer = model.IsOffer;
            entity.IsApprove = model.IsApprove;
            entity.IsWinter = model.IsWinter;
            return entity;
        }

        #endregion


        #region Farmer User
        public static FarmerUserModel ToModel(this FarmerUser entity)
        {
            FarmerUserModel model = new FarmerUserModel();
            model.Id = entity.Id;
            model.UserName = entity.UserName;
            model.Password = entity.Password;
            model.FarmerId = entity.FarmerId;
            model.CreatedDate = entity.CreatedDate;
            model.UpdateDate = entity.UpdateDate;
            return model;
        }
        public static FarmerUser ToEntity(this FarmerUserModel model)
        {
            FarmerUser entity = new FarmerUser();
            entity.Id = model.Id;
            entity.UserName = model.UserName;
            entity.Password = model.Password;
            entity.FarmerId = model.FarmerId;
            entity.CreatedDate = model.CreatedDate;
            entity.UpdateDate = model.UpdateDate;
            return entity;
        }
        #endregion

        #region Farmer Reservation
        public static FarmerReservationModel ToModel(this FarmerReservation entity, List<LookupValue> lookupValues)
        {
            FarmerReservationModel model = new FarmerReservationModel();
            model.Id = entity.Id;
            model.FarmerId = entity.FarmerId;
            model.CustomerId = entity.CustomerId;
            model.ReservationTypeId = entity.ReservationTypeId;
            //model.ReservationTypeDesc = lookupValues == null ? string.Empty : lookupValues.Where(c => c.Id == entity.ReservationTypeId).FirstOrDefault().ValueAr;
            model.ReservationTypeDesc = lookupValues?
                                .FirstOrDefault(c => c.Id == entity.ReservationTypeId)?
                                .ValueAr ?? string.Empty;
            model.ReservationDate = entity.ReservationDate;
            model.CustMobNum = entity.CustMobNum;
            model.CustomerName = entity.CustomerName;
            model.Reason = entity.Reason;
            model.NumberOfPerson = entity.NumberOfPerson;
            model.CostReservationAmtOnMahjouz = entity.CostReservationAmtOnMahjouz;
            model.ReservationAmt = entity.ReservationAmt;
            model.NetProfit = entity.NetProfit;
            model.ReservationDepositAmt = entity.ReservationDepositAmt;
            model.ReservationRemainAmt = entity.ReservationRemainAmt;
            model.Note = entity.Note;
            model.MobileOwnerAppUser = entity.MobileOwnerAppUser;
            model.IsMahjouzReservation = entity.IsMahjouzReservation;
            model.IsReciveCommission = entity.IsReciveCommission;
            model.AutomaticallyNote = entity.AutomaticallyNote;
            model.CreatedDate = entity.CreatedDate;


            //model.FarmerId = entity.FarmerId;
            //model.CustomerId = (int)entity.CustomerId;
            //model.ReservationTypeId = entity.ReservationTypeId;
            ////  التعامل مع NULL بأمان
            //model.ReservationTypeDesc = lookupValues?
            //    .FirstOrDefault(c => c.Id == entity.ReservationTypeId)?
            //    .ValueAr ?? string.Empty;
            ////  التأكد من وجود ReservationDate
            //model.ReservationDate = entity.ReservationDate;
            //model.CustMobNum = entity.CustMobNum ?? string.Empty;
            //model.CustomerName = entity.CustomerName ?? string.Empty;
            //model.NumberOfPerson = entity.NumberOfPerson;
            //model.CostReservationAmtOnMahjouz = entity.CostReservationAmtOnMahjouz;
            //model.ReservationAmt = entity.ReservationAmt;
            //model.NetProfit = entity.NetProfit;
            //model.ReservationDepositAmt = entity.ReservationDepositAmt;
            //model.ReservationRemainAmt = entity.ReservationRemainAmt;
            //model.Note = entity.Note ?? string.Empty;
            //model.MobileOwnerAppUser = entity.MobileOwnerAppUser ?? string.Empty;
            //model.IsMahjouzReservation = entity.IsMahjouzReservation;
            //model.IsReciveCommission = entity.IsReciveCommission;
            //model.AutomaticallyNote = entity.AutomaticallyNote ?? string.Empty;
            ////  التأكد من وجود CreatedDate
            //model.CreatedDate = entity.CreatedDate;
            //model.Reason = entity.Reason;
            //model.ReservStatus = entity.ReservStatus;
            return model;
        }
        public static FarmerReservation ToEntity(this FarmerReservationModel model)
        {
            FarmerReservation entity = new FarmerReservation();
            entity.Id = model.Id;
            entity.FarmerId = model.FarmerId;
            entity.CustomerId = model.CustomerId;
            entity.ReservationTypeId = model.ReservationTypeId;
            entity.ReservationDate = model.ReservationDate;
            entity.CustMobNum = model.CustMobNum;
            entity.Reason = model.Reason;
            entity.CustomerName = model.CustomerName;
            entity.NumberOfPerson = model.NumberOfPerson;
            entity.CostReservationAmtOnMahjouz = model.CostReservationAmtOnMahjouz;
            entity.ReservationAmt = model.ReservationAmt;
            entity.NetProfit = model.NetProfit;
            entity.ReservationDepositAmt = model.ReservationDepositAmt;
            entity.ReservationRemainAmt = model.ReservationRemainAmt;
            entity.Note = model.Note;
            entity.MobileOwnerAppUser = model.MobileOwnerAppUser;
            entity.IsMahjouzReservation = model.IsMahjouzReservation;
            entity.IsReciveCommission = model.IsReciveCommission;
            entity.AutomaticallyNote = model.AutomaticallyNote;
            entity.CreatedDate = model.CreatedDate;
            return entity;
        }
        #endregion

        #region Farmer Feedback
        public static FarmerFeedbackModel ToModel(this FarmerFeedback entity, List<LookupValue> lookupValues)
        {
            FarmerFeedbackModel model = new FarmerFeedbackModel();
            model.Id = entity.Id;
            model.FarmerId = entity.FarmerId;
            model.FeedbackId = entity.FeedbackId;
            model.FeedbackTypeDesc = lookupValues == null ? string.Empty : lookupValues.Where(c => c.Id == entity.FeedbackId).FirstOrDefault().ValueAr;
            model.Note = entity.Note;
            model.IsSolved = entity.IsSolved;
            model.CreatedDate = entity.CreatedDate;
            return model;
        }
        public static FarmerFeedback ToEntity(this FarmerFeedbackModel model)
        {
            FarmerFeedback entity = new FarmerFeedback();
            entity.Id = model.Id;
            entity.FarmerId = model.FarmerId;
            entity.FeedbackId = model.FeedbackId;
            entity.Note = model.Note;
            entity.IsSolved = model.IsSolved;
            entity.CreatedDate = model.CreatedDate;
            return entity;
        }
        #endregion

        #region Users
        public static CustomerModel ToModel(this Customer entity)
        {
            CustomerModel model = new CustomerModel();
            model.Id = entity.Id;
            model.FullName = entity.FullName;
            model.MobileNumber = entity.MobileNumber;
            return model;
        }
        public static Customer ToEntity(this CustomerModel model)
        {
            Customer entity = new Customer();
            entity.Id = model.Id;
            entity.FullName = model.FullName;
            entity.MobileNumber = model.MobileNumber;
            return entity;
        }
        #endregion

        #region Customer BlackList
        public static CustomerBlackListModel ToModel(this CustomerBlackList entity)
        {
            CustomerBlackListModel model = new CustomerBlackListModel();
            model.Id = entity.Id;
            model.CustName = entity.CustName;
            model.CustNameEn = entity.CustNameEn;

            model.CustMobileNum = entity.CustMobileNum;
            model.Reason = entity.Reason;
            model.ReasonEn = entity.ReasonEn;

            model.ImageUrl = entity.ImageUrl;
            model.IsApprove = entity.IsApprove;
            return model;
        }
        public static CustomerBlackList ToEntity(this CustomerBlackListModel model)
        {
            CustomerBlackList entity = new CustomerBlackList();
            entity.Id = model.Id;
            entity.CustName = model.CustName;
            entity.CustNameEn = model.CustNameEn;

            entity.CustMobileNum = model.CustMobileNum;
            entity.Reason = model.Reason;
            entity.ReasonEn = model.ReasonEn;

            entity.ImageUrl = model.ImageUrl;
            entity.IsApprove = model.IsApprove;
            return entity;
        }
        #endregion
        #region  
        public static FarmerExtraFeatureTypeDto ToModel(this FarmerExtraFeatureType entity,LookupValue lookup)
        {
            FarmerExtraFeatureTypeDto model = new FarmerExtraFeatureTypeDto();
            model.Id = entity.Id;
            model.FarmerId = entity.FarmerId;
            model.ExtraText = entity.ExtraText;
            model.TypeId = entity.TypeId;
            model.DescAr = entity.ExtraText;
            model.SwimmingPoolLength = entity.SwimmingPoolLength;
            model.SwimmingPoolWidth = entity.SwimmingPoolWidth;
            model.SwimmingPoolDepth = entity.SwimmingPoolDepth;
            //model.Code= lookup?.FirstOrDefault(c => c.Id == entity.TypeId)?.Code ?? "";

            return model;
        }
        public static FarmerExtraFeatureType ToEntity(this FarmerExtraFeatureTypeDto model)
        {
            FarmerExtraFeatureType entity = new FarmerExtraFeatureType();
            entity.Id = model.Id;
            entity.FarmerId = model.FarmerId;
            entity.ExtraText = model.ExtraText;
            entity.TypeId = model.TypeId;
            entity.SwimmingPoolLength = model.SwimmingPoolLength;
            entity.SwimmingPoolWidth = model.SwimmingPoolWidth;
            entity.SwimmingPoolDepth = model.SwimmingPoolDepth;

            return entity;
        }
        #endregion
        #region Farmer BlackList
        public static FarmerBlackListModel ToModel(this FarmerBlackList entity, List<Farmer> farms = null)
        {
            //For Retreive ....
            //, List<Farmer> farms = null
            FarmerBlackListModel model = new FarmerBlackListModel();
            model.Id = entity.Id;
            model.FarmerName = entity.FarmerName;
            model.FarmerNameEn = entity.FarmerNameEn;
            model.FarmerMobNum = entity.FarmerMobNum;
            model.Reason = entity.Reason;
            model.ReasonEn = entity.ReasonEn;
            model.ImageUrl = entity.ImageUrl;
            model.IsApprove = entity.IsApprove;
            model.FarmerId = entity.FarmerId;
            model.IsBlocked = entity.IsBlocked;
            //model.FarmNumber = entity.FarmNumber;
            //model.FarmNumber= farms?
            //    .FirstOrDefault(f => f.Id == entity.FarmerId)
            //    ?.Number ?? -1;
            if (entity.FarmerId.HasValue && farms != null)
            {
                var farm = farms.FirstOrDefault(f => f.Id == entity.FarmerId.Value);
                model.FarmNumber = farm != null ? farm.Number : -1;
            }
            else
            {
                model.FarmNumber = -1;
            }
            return model;
        }
        public static FarmerBlackList ToEntity(this FarmerBlackListModel model)
        {
            //For Add Or Update ....
            FarmerBlackList entity = new FarmerBlackList();
            entity.Id = model.Id;
            entity.FarmerName = model.FarmerName;
            entity.FarmerNameEn = model.FarmerNameEn;
            entity.FarmerMobNum = model.FarmerMobNum;
            entity.Reason = model.Reason;
            entity.ReasonEn = model.ReasonEn;
            entity.ImageUrl = model.ImageUrl;
            entity.IsApprove = model.IsApprove;
            entity.FarmerId = model.FarmerId;
            entity.IsBlocked = model.IsBlocked;
            //entity.FarmNumber = model.FarmNumber;

            return entity;
        }
        #endregion

        #region Lookup
        //public static LookupModel ToModel(this Lookup entity)
        //{
        //    LookupModel model = new LookupModel();
        //    model.Id = entity.Id;
        //    model.LookupCode = entity.LookupCode;
        //    model.LookupCodeDesc = (entity.LookupCode == "FarmerExtraFeatureType" ? "مزايا المزرعة" : "مزايا الاقسام الاخرى");
        //    return model;
        //}
        public static LookupModel ToModel(this Lookup entity)
        {
            LookupModel model = new LookupModel();
            model.Id = entity.Id;
            model.LookupCode = entity.LookupCode;

            // Apply the conditional logic based on entity.LookupCode
            switch (entity.Id)
            {
                case 2:
                    model.LookupCodeDesc = "مرفقات المزرعة";
                    break;
                case 3:
                    model.LookupCodeDesc = "نوع التسعير للمزرعة";
                    break;
                case 4:
                    model.LookupCodeDesc = "العملة";
                    break;
                case 5:
                    model.LookupCodeDesc = "مزايا الرحلات / الأقسام الأخرى";
                    break;
                case 6:
                    model.LookupCodeDesc = "نوع الحجز";
                    break;
                case 8:
                    model.LookupCodeDesc = "النص العلوى لتطبيق الموبايل User";
                    break;
                case 9:
                    model.LookupCodeDesc = "الصورة و النص الذى يتبعها لتظبيق ال User لرؤية كل المزارع";
                    break;
                default:
                    model.LookupCodeDesc = "مزايا الاقسام الاخرى";
                    break;
            }

            return model;
        }
        public static Lookup ToEntity(this LookupModel model)
        {
            Lookup entity = new Lookup();
            entity.Id = model.Id;
            entity.LookupCode = model.LookupCode;
            return entity;
        }
        #endregion

        #region Lookup Value
        //public static LookupValueModel ToModel(this LookupValue entity, string lookupCode)
        //{
        //    LookupValueModel model = new LookupValueModel();
        //    model.Id = entity.Id;
        //    model.LookupId = entity.LookupId;
        //    model.LookupDesc = (lookupCode == "FarmerExtraFeatureType" ? "مزايا المزرعة" : "مزايا الاقسام الاخرى"); ;
        //    model.Code = entity.Code;
        //    model.ValueAr = entity.ValueAr;
        //    model.ValueEn = entity.ValueEn;
        //    return model;
        //}
        public static LookupValueModel ToModel(this LookupValue entity, string lookupCode)
        {
            LookupValueModel model = new LookupValueModel();
            model.Id = entity.Id;
            model.LookupId = entity.LookupId;

            // Apply the conditional logic based on entity.LookupId
            switch (entity.LookupId)
            {
                case 2:
                    model.LookupDesc = "مرفقات المزرعة";
                    break;
                case 3:
                    model.LookupDesc = "نوع التسعير للمزرعة";
                    break;
                case 4:
                    model.LookupDesc = "العملة";
                    break;
                case 5:
                    model.LookupDesc = "مزايا الرحلات / الأقسام الأخرى";
                    break;
                case 6:
                    model.LookupDesc = "نوع الحجز";
                    break;
                case 8:
                    model.LookupDesc = "النص العلوى لتطبيق الموبايل User";
                    break;
                case 9:
                    model.LookupDesc = "الصورة و النص الذى يتبعها لتظبيق ال User لرؤية كل المزارع";
                    break;
                default:
                    // This will be the fallback if LookupId doesn't match 2, 3, or 4
                    model.LookupDesc = "مزايا الاقسام الاخرى";
                    break;
            }

            model.Code = entity.Code;
            model.ValueAr = entity.ValueAr;
            model.ValueEn = entity.ValueEn;
            model.Image = entity.Image;
            return model;
        }
        public static LookupValue ToEntity(this LookupValueModel model)
        {
            LookupValue entity = new LookupValue();
            entity.Id = model.Id;
            entity.LookupId = model.LookupId;
            entity.Code = model.Code;
            entity.ValueAr = model.ValueAr;
            entity.ValueEn = model.ValueEn;
            entity.Image = model.Image;

            return entity;
        }
        #endregion


        #region terms
        public static termsModel ToModel(this terms entity)
        {
            termsModel model = new termsModel();
            model.Id = entity.Id;
            model.DescAr = entity.DescAr;
            model.DescEn = entity.DescEn;
            return model;
        }
        public static terms ToEntity(this termsModel model)
        {
            terms entity = new terms();
            entity.Id = model.Id;
            entity.DescAr = model.DescAr;
            entity.DescEn = model.DescEn;
            return entity;
        }
        #endregion

        #region Region
        public static RegionModel ToModel(this Regions entity, List<City> cities = null)
        {
            RegionModel model = new RegionModel();
            model.Id = entity.Id;
            model.DescAr = entity.DescAr;
            model.DescEn = entity.DescEn;
            model.CityId = entity.CityId;
            //model.CityName = cities == null ? string.Empty : cities.Where(c => c.Id == entity.CityId).FirstOrDefault().DescAr;
            model.CityName = cities?
            .FirstOrDefault(c => c.Id == entity.CityId)
            ?.DescAr ?? string.Empty;
            return model;
        }
        public static Regions ToEntity(this RegionModel model)
        {
            Regions entity = new Regions();
            entity.Id = model.Id;
            entity.DescAr = model.DescAr;
            entity.DescEn = model.DescEn;
            entity.CityId = model.CityId;
            return entity;
        }
        #endregion

        #region Users
        public static UserModel ToModel(this AppUser entity)
        {
            UserModel model = new UserModel();
            model.Id = entity.Id;
            model.UserName = entity.UserName;
            model.MobilePhone = entity.MobilePhone;
            model.MobileNumber = entity.MobileNumber;
            model.PasswordHash = entity.PasswordHash;
            //model.UserType = entity.UserType;
            model.UserType = entity.UserType ?? "0";
            model.IsActive = entity.IsActive;
            model.IsDeleted = entity.IsDeleted;
            return model;
        }
        public static AppUser ToEntity(this UserModel model)
        {
            AppUser entity = new AppUser();
            entity.Id = model.Id;
            entity.UserName = model.UserName;
            entity.MobilePhone = model.MobilePhone;
            entity.MobileNumber = model.MobileNumber;
            entity.PasswordHash = model.PasswordHash;
            //entity.UserType = model.UserType;
            entity.UserType = model.UserType ?? "0";
            entity.IsActive = model.IsActive;
            entity.IsDeleted = model.IsDeleted;
            return entity;
        }
        #endregion

        #region AppUserBlackList
        public static AppUserBlackListModel ToModel(this AppUserBlackList entity)
        {
            AppUserBlackListModel model = new AppUserBlackListModel();
            model.Id = entity.Id;
            model.CustName = entity.CustName;
            model.CustNameEn = entity.CustNameEn;

            model.CustMobileNum = entity.CustMobileNum;
            model.Reason = entity.Reason;
            model.ReasonEn = entity.ReasonEn;

            model.ImageUrl = entity.ImageUrl;
            model.IsApprove = entity.IsApprove;
            model.UserId = entity.UserId;
            model.IsBlocked = entity.IsBlocked;
            return model;
        }
        public static AppUserBlackList ToEntity(this AppUserBlackListModel model)
        {
            AppUserBlackList entity = new AppUserBlackList();
            entity.Id = model.Id;
            entity.CustName = model.CustName;
            entity.CustNameEn = model.CustNameEn;

            entity.CustMobileNum = model.CustMobileNum;
            entity.Reason = model.Reason;
            entity.ReasonEn = model.ReasonEn;

            entity.ImageUrl = model.ImageUrl;
            entity.IsApprove = model.IsApprove;
            entity.UserId = model.UserId;
            entity.IsBlocked = model.IsBlocked;
            return entity;
        }
        #endregion



        #region CommonQuestions
        public static CommonQuestionsModel ToModel(this CommonQuestions entity)
        {
            CommonQuestionsModel model = new CommonQuestionsModel();
            model.Id = entity.Id;
            model.QuestAr = entity.QuestAr;
            model.AnswerAr = entity.AnswerAr;
            model.QuestEn = entity.QuestEn;
            model.AnswerEn = entity.AnswerEn;
            model.ImageUrl = entity.ImageUrl;
            return model;
        }
        public static CommonQuestions ToEntity(this CommonQuestionsModel model)
        {
            CommonQuestions entity = new CommonQuestions();
            entity.Id = model.Id;
            entity.QuestAr = model.QuestAr;
            entity.AnswerAr = model.AnswerAr;
            entity.QuestEn = model.QuestEn;
            entity.AnswerEn = model.AnswerEn;
            entity.ImageUrl = model.ImageUrl;
            return entity;
        }
        #endregion





        #region CommonQuestionsVisitors
        public static CommonQuestionsVisitorsModel ToModel(this CommonQuestionsVisitors entity)
        {
            CommonQuestionsVisitorsModel model = new CommonQuestionsVisitorsModel();
            model.Id = entity.Id;
            model.QuestAr = entity.QuestAr;
            model.AnswerAr = entity.AnswerAr;
            model.QuestEn = entity.QuestEn;
            model.AnswerEn = entity.AnswerEn;
            model.ImageUrl = entity.ImageUrl;
            return model;
        }
        public static CommonQuestionsVisitors ToEntity(this CommonQuestionsVisitorsModel model)
        {
            CommonQuestionsVisitors entity = new CommonQuestionsVisitors();
            entity.Id = model.Id;
            entity.QuestAr = model.QuestAr;
            entity.AnswerAr = model.AnswerAr;
            entity.QuestEn = model.QuestEn;
            entity.AnswerEn = model.AnswerEn;
            entity.ImageUrl = model.ImageUrl;
            return entity;
        }
        #endregion



        #region Sport ToModel / ToEntity

        /// <summary>
        /// تحويل من Entity إلى Model (مثل Farmer بالضبط)
        /// </summary>
        public static SportModel ToModel(this Sport entity,
            List<Country> countries = null,
            List<City> cities = null,
            List<Regions> regions = null,
            List<AppUser> users = null,
            List<SportType> sportTypes = null,
            List<SportImage> sportImages = null,
            List<SportVideo> sportVideos = null,
            List<SportPriceList> priceList = null)
        {
            SportModel model = new SportModel();

            // معلومات أساسية
            model.Id = entity.Id;
            model.CountryId = entity.CountryId;
            model.CityId = entity.CityId;
            model.RegionId = entity.RegionId ?? 0;
            model.UserId = entity.UserId ?? 0;
            model.SportTypeId = entity.SportTypeId;
            // الأسماء والوصف (مثل Farmer)
            model.NameAr = entity.NameAr;
            model.NameEn = entity.NameEn;
            model.DescriptionAr = entity.DescriptionAr;
            model.DescriptionEn = entity.DescriptionEn;
            model.Owner = entity.Owner;
            model.LocationDesc = entity.LocationDescAr;
            model.LocationDescEn = entity.LocationDescEn;
            model.MobileNumber = entity.MobileNumber;

            // الأرقام والتواريخ
            model.Number = entity.Number;
            model.SerialSportKey = entity.SerialSportKey;
            model.IssueDate = entity.IssueDate;
            model.ExpiryDate = entity.ExpiryDate;
            model.CreatedDate = entity.CreatedDate;
            model.ModifiedDate = entity.ModifiedDate;

            // الخصائص العامة (مثل Farmer)
            model.IsTrust = entity.IsTrust;
            model.IsVIP = entity.IsVIP;
            model.IsOffer = entity.IsOffer;
            model.IsWinter = entity.IsWinter;
            model.IsApprove = entity.IsApprove;
            model.IsActive = entity.IsActive;
            model.IsBlocked = entity.IsBlocked;
            model.statusSportAppUser = entity.statusSportAppUser;

            // تفاصيل الحجز (مثل Farmer)
            model.GeographicLocation = entity.GeographicLocation;
            model.MaxPerson = entity.MaxPerson;
            model.ConfidentialMessageAr = entity.ConfidentialMessageAr;
            model.ConfidentialMessageEn = entity.ConfidentialMessageEn;
            model.Image3DLink = entity.Image3DLink;
            model.ReservationDetails = entity.ReservationDetails;
            model.ExtraDetails = entity.ExtraDetails;
            // ===== تفاصيل العقار (حسب نوع الرياضة) =====
            // كرة القدم
            //model.FootballFloorType = entity.FootballFloorType;
            //model.FootballCourtDimensions = entity.FootballCourtDimensions;
            //model.FootballPlayerCount = entity.FootballPlayerCount;
            //model.FootballPitchType = entity.FootballPitchType;
            //model.FootballIsIndoor = entity.FootballIsIndoor;
            //model.FootballIsOutdoor = entity.FootballIsOutdoor;
            //model.FootballLightingSystem = entity.FootballLightingSystem;
            //model.FootballSuitableForOfficial = entity.FootballSuitableForOfficial;
            //model.FootballSuitableForTraining = entity.FootballSuitableForTraining;

            //// البادل
            //model.PadelPitchType = entity.PadelPitchType;
            //model.PadelNumberOfCourts = entity.PadelNumberOfCourts;
            //model.PadelHasCeiling = entity.PadelHasCeiling;
            //model.PadelNightLighting = entity.PadelNightLighting;
            //model.PadelGlassType = entity.PadelGlassType;
            //model.PadelCourtLevel = entity.PadelCourtLevel;

            //// التنس
            //model.TennisPitchType = entity.TennisPitchType;
            //model.TennisNumberOfCourts = entity.TennisNumberOfCourts;
            //model.TennisNightLighting = entity.TennisNightLighting;
            //model.TennisIsSingles = entity.TennisIsSingles;
            //model.TennisIsDoubles = entity.TennisIsDoubles;
            //model.TennisSuitableForTournaments = entity.TennisSuitableForTournaments;

            //// كرة السلة
            //model.BasketBallNumberOfBaskets = entity.BasketBallNumberOfBaskets;
            //model.BasketBallCourtSize = entity.BasketBallCourtSize;

            //// كرة الطائرة
            //model.VollyBallNetHeightAdjustable = entity.VollyBallNetHeightAdjustable;
            //model.VollyBallIsIndoor = entity.VollyBallIsIndoor;
            //model.VollyBallIsOutdoor = entity.VollyBallIsOutdoor;
            //model.VollyBallNightLighting = entity.VollyBallNightLighting;
            //model.VollyBallPitchType = entity.VollyBallPitchType;
            //model.VollyBallNumberOfCourts = entity.VollyBallNumberOfCourts;

            //// المسابح
            //model.SwimmingPoolType = entity.SwimmingPoolType;
            //model.SwimmingPoolLength = entity.SwimmingPoolLength;
            //model.SwimmingPoolWidth = entity.SwimmingPoolWidth;
            //model.SwimmingPoolDepth = entity.SwimmingPoolDepth;
            //model.SwimmingPoolNumberOfPools = entity.SwimmingPoolNumberOfPools;
            //model.SwimmingPoolHasChildrenPool = entity.SwimmingPoolHasChildrenPool;
            //model.SwimmingPoolHasAdultsPool = entity.SwimmingPoolHasAdultsPool;
            //model.SwimmingPoolWaterTemperature = entity.SwimmingPoolWaterTemperature;
            //model.SwimmingPoolSterilizationSystem = entity.SwimmingPoolSterilizationSystem;
            //model.SwimmingPoolHasWaterSlides = entity.SwimmingPoolHasWaterSlides;
            //model.SwimmingPoolHasJacuzzi = entity.SwimmingPoolHasJacuzzi;
            //model.SwimmingPoolSuitableForTraining = entity.SwimmingPoolSuitableForTraining;
            //model.SwimmingPoolSuitableForEvents = entity.SwimmingPoolSuitableForEvents;

            //// الفروسية
            //model.EquestrianismActivityType = entity.EquestrianismActivityType;
            //model.EquestrianismNumberOfHorses = entity.EquestrianismNumberOfHorses;
            //model.EquestrianismTrainingLevel = entity.EquestrianismTrainingLevel;
            //model.EquestrianismTourDuration = entity.EquestrianismTourDuration;
            //model.EquestrianismAllowedAge = entity.EquestrianismAllowedAge;
            //model.EquestrianismAllowedWeight = entity.EquestrianismAllowedWeight;
            //model.EquestrianismHasAccompanyingTrainer = entity.EquestrianismHasAccompanyingTrainer;
            //model.EquestrianismTrackIndoor = entity.EquestrianismTrackIndoor;
            //model.EquestrianismTrackOutdoor = entity.EquestrianismTrackOutdoor;

            //// الرماية
            //model.ShootingIndoor = entity.ShootingIndoor;
            //model.ShootingOutdoor = entity.ShootingOutdoor;
            //model.ShootingAirShooting = entity.ShootingAirShooting;
            //model.ShootingFireShooting = entity.ShootingFireShooting;
            //model.ShootingBowAndArrow = entity.ShootingBowAndArrow;
            //model.ShootingShootingTrainer = entity.ShootingShootingTrainer;
            //model.ShootingEquipmentAvailable = entity.ShootingEquipmentAvailable;
            //model.ShootingEquipmentRental = entity.ShootingEquipmentRental;

            //// Pickleball
            //model.PickleballIndoor = entity.PickleballIndoor;
            //model.PickleballOutdoor = entity.PickleballOutdoor;
            //model.PickleballPaddles = entity.PickleballPaddles;
            //model.PickleballBalls = entity.PickleballBalls;
            //model.PickleballLighting = entity.PickleballLighting;

            //// تنس الطاولة
            //model.TableTennisProfessional = entity.TableTennisProfessional;
            //model.TableTennisPaddles = entity.TableTennisPaddles;
            //model.TableTennisBalls = entity.TableTennisBalls;
            //model.TableTennisTraining = entity.TableTennisTraining;

            //// الاسكواش
            //model.SquashGlassCourt = entity.SquashGlassCourt;
            //model.SquashNormalCourt = entity.SquashNormalCourt;
            //model.SquashPaddles = entity.SquashPaddles;
            //model.SquashBalls = entity.SquashBalls;
            //model.SquashTraining = entity.SquashTraining;

            //// الريشة الطائرة
            //model.BadmintonIndoor = entity.BadmintonIndoor;
            //model.BadmintonProfessionalFloor = entity.BadmintonProfessionalFloor;
            //model.BadmintonPaddles = entity.BadmintonPaddles;
            //model.BadmintonShuttlecocks = entity.BadmintonShuttlecocks;
            //model.BadmintonTraining = entity.BadmintonTraining;

            //// الكرة الطائرة (شاطئية)
            //model.BeachVolleyballIndoor = entity.BeachVolleyballIndoor;
            //model.BeachVolleyballOutdoor = entity.BeachVolleyballOutdoor;
            //model.BeachVolleyballSandFloor = entity.BeachVolleyballSandFloor;
            //model.BeachVolleyballIndoorFloor = entity.BeachVolleyballIndoorFloor;
            //model.BeachVolleyballLighting = entity.BeachVolleyballLighting;
            //model.BeachVolleyballProfessionalNet = entity.BeachVolleyballProfessionalNet;
            //model.BeachVolleyballBalls = entity.BeachVolleyballBalls;

            //// كرة السلة (إضافات)
            //model.BasketballIndoor = entity.BasketballIndoor;
            //model.BasketballOutdoor = entity.BasketballOutdoor;
            //model.BasketballWoodFloor = entity.BasketballWoodFloor;
            //model.BasketballRubberFloor = entity.BasketballRubberFloor;
            //model.BasketballLighting = entity.BasketballLighting;
            //model.BasketballStands = entity.BasketballStands;
            //model.BasketballScoreboard = entity.BasketballScoreboard;
            //model.BasketballBalls = entity.BasketballBalls;
            //model.BasketballTraining = entity.BasketballTraining;

            //// التنس (إضافات)
            //model.TennisIndoor = entity.TennisIndoor;
            //model.TennisOutdoor = entity.TennisOutdoor;
            //model.TennisAcrylicFloor = entity.TennisAcrylicFloor;
            //model.TennisClayFloor = entity.TennisClayFloor;
            //model.TennisGrassFloor = entity.TennisGrassFloor;
            //model.TennisLighting = entity.TennisLighting;
            //model.TennisPaddles = entity.TennisPaddles;
            //model.TennisBalls = entity.TennisBalls;
            //model.TennisTrainer = entity.TennisTrainer;
            //model.TennisAcademy = entity.TennisAcademy;
            //model.TennisTournaments = entity.TennisTournaments;

            //// البادل (إضافات)
            //model.PadelIndoor = entity.PadelIndoor;
            //model.PadelOutdoor = entity.PadelOutdoor;
            //model.PadelPanoramic = entity.PadelPanoramic;
            //model.PadelNormal = entity.PadelNormal;
            //model.PadelLighting = entity.PadelLighting;
            //model.PadelPaddlesRental = entity.PadelPaddlesRental;
            //model.PadelBallsAvailable = entity.PadelBallsAvailable;
            //model.PadelTrainer = entity.PadelTrainer;
            //model.PadelAcademy = entity.PadelAcademy;
            //model.PadelTournaments = entity.PadelTournaments;
            //model.PadelHourlyBooking = entity.PadelHourlyBooking;

            // ===== الأسماء من الـ Lists (مثل Farmer) =====
            model.CountryDesc = countries?
                .FirstOrDefault(c => c.Id == entity.CountryId)
                ?.DescAr ?? "";

            model.CityDesc = cities?
                .FirstOrDefault(c => c.Id == entity.CityId)
                ?.DescAr ?? "";

            model.RegionDesc = regions?
                .FirstOrDefault(r => r.Id == entity.RegionId)
                ?.DescAr ?? "";

            model.UserDesc = users?
                .FirstOrDefault(u => u.Id == entity.UserId)
                ?.UserName ?? "";

            model.UserName = users?
                .FirstOrDefault(u => u.Id == entity.UserId)
                ?.UserName ?? "";

            model.SportTypeDesc = sportTypes?
                .FirstOrDefault(s => s.Id == entity.SportTypeId)
                ?.NameAr ?? "";

            // الصور والفيديوهات والأسعار
            model.SportImages = sportImages ?? new List<SportImage>();
            model.SportVideos = sportVideos ?? new List<SportVideo>();
            model.PriceList = priceList ?? new List<SportPriceList>();

            return model;
        }

        /// <summary>
        /// تحويل من Model إلى Entity (مثل Farmer بالضبط)
        /// </summary>
        public static Sport ToEntity(this SportModel model)
        {
            Sport entity = new Sport();

            // معلومات أساسية
            entity.Id = model.Id;
            entity.CountryId = model.CountryId;
            entity.CityId = model.CityId;
            entity.RegionId = model.RegionId;
            entity.UserId = model.UserId;
            entity.SportTypeId = model.SportTypeId;

            // الأسماء والوصف
            entity.NameAr = model.NameAr;
            entity.NameEn = model.NameEn;
            entity.DescriptionAr = model.DescriptionAr;
            entity.DescriptionEn = model.DescriptionEn;
            entity.Owner = model.Owner;
            entity.LocationDescAr = model.LocationDesc;
            entity.LocationDescEn = model.LocationDescEn;
            entity.MobileNumber = model.MobileNumber;

            // الأرقام والتواريخ
            entity.Number = model.Number;
            entity.SerialSportKey = model.SerialSportKey;
            entity.IssueDate = model.IssueDate;
            entity.ExpiryDate = model.ExpiryDate;
            //model.IssueDate = entity.IssueDate ?? DateTime.Now;
            //model.ExpiryDate = entity.ExpiryDate ?? DateTime.Now.AddMonths(3);
            entity.CreatedDate = model.CreatedDate;
            entity.ModifiedDate = model.ModifiedDate;

            // الخصائص العامة
            entity.IsTrust = model.IsTrust;
            entity.IsVIP = model.IsVIP;
            entity.IsOffer = model.IsOffer;
            entity.IsWinter = model.IsWinter;
            entity.IsApprove = model.IsApprove;
            entity.IsActive = model.IsActive;
            entity.IsBlocked = model.IsBlocked;
            entity.statusSportAppUser = model.statusSportAppUser;

            // تفاصيل الحجز
            entity.GeographicLocation = model.GeographicLocation;
            entity.MaxPerson = model.MaxPerson;
            entity.ConfidentialMessageAr = model.ConfidentialMessageAr;
            entity.ConfidentialMessageEn = model.ConfidentialMessageEn;
            entity.Image3DLink = model.Image3DLink;
            entity.ReservationDetails = model.ReservationDetails;
            entity.ExtraDetails = model.ExtraDetails;

            // ===== تفاصيل العقار =====
            // كرة القدم
            //entity.FootballFloorType = model.FootballFloorType;
            //entity.FootballCourtDimensions = model.FootballCourtDimensions;
            //entity.FootballPlayerCount = model.FootballPlayerCount;
            //entity.FootballPitchType = model.FootballPitchType;
            //entity.FootballIsIndoor = model.FootballIsIndoor;
            //entity.FootballIsOutdoor = model.FootballIsOutdoor;
            //entity.FootballLightingSystem = model.FootballLightingSystem;
            //entity.FootballSuitableForOfficial = model.FootballSuitableForOfficial;
            //entity.FootballSuitableForTraining = model.FootballSuitableForTraining;

            //// البادل
            //entity.PadelPitchType = model.PadelPitchType;
            //entity.PadelNumberOfCourts = model.PadelNumberOfCourts;
            //entity.PadelHasCeiling = model.PadelHasCeiling;
            //entity.PadelNightLighting = model.PadelNightLighting;
            //entity.PadelGlassType = model.PadelGlassType;
            //entity.PadelCourtLevel = model.PadelCourtLevel;

            //// التنس
            //entity.TennisPitchType = model.TennisPitchType;
            //entity.TennisNumberOfCourts = model.TennisNumberOfCourts;
            //entity.TennisNightLighting = model.TennisNightLighting;
            //entity.TennisIsSingles = model.TennisIsSingles;
            //entity.TennisIsDoubles = model.TennisIsDoubles;
            //entity.TennisSuitableForTournaments = model.TennisSuitableForTournaments;

            //// كرة السلة
            //entity.BasketBallNumberOfBaskets = model.BasketBallNumberOfBaskets;
            //entity.BasketBallCourtSize = model.BasketBallCourtSize;

            //// كرة الطائرة
            //entity.VollyBallNetHeightAdjustable = model.VollyBallNetHeightAdjustable;
            //entity.VollyBallIsIndoor = model.VollyBallIsIndoor;
            //entity.VollyBallIsOutdoor = model.VollyBallIsOutdoor;
            //entity.VollyBallNightLighting = model.VollyBallNightLighting;
            //entity.VollyBallPitchType = model.VollyBallPitchType;
            //entity.VollyBallNumberOfCourts = model.VollyBallNumberOfCourts;

            //// المسابح
            //entity.SwimmingPoolType = model.SwimmingPoolType;
            //entity.SwimmingPoolLength = model.SwimmingPoolLength;
            //entity.SwimmingPoolWidth = model.SwimmingPoolWidth;
            //entity.SwimmingPoolDepth = model.SwimmingPoolDepth;
            //entity.SwimmingPoolNumberOfPools = model.SwimmingPoolNumberOfPools;
            //entity.SwimmingPoolHasChildrenPool = model.SwimmingPoolHasChildrenPool;
            //entity.SwimmingPoolHasAdultsPool = model.SwimmingPoolHasAdultsPool;
            //entity.SwimmingPoolWaterTemperature = model.SwimmingPoolWaterTemperature;
            //entity.SwimmingPoolSterilizationSystem = model.SwimmingPoolSterilizationSystem;
            //entity.SwimmingPoolHasWaterSlides = model.SwimmingPoolHasWaterSlides;
            //entity.SwimmingPoolHasJacuzzi = model.SwimmingPoolHasJacuzzi;
            //entity.SwimmingPoolSuitableForTraining = model.SwimmingPoolSuitableForTraining;
            //entity.SwimmingPoolSuitableForEvents = model.SwimmingPoolSuitableForEvents;

            //// الفروسية
            //entity.EquestrianismActivityType = model.EquestrianismActivityType;
            //entity.EquestrianismNumberOfHorses = model.EquestrianismNumberOfHorses;
            //entity.EquestrianismTrainingLevel = model.EquestrianismTrainingLevel;
            //entity.EquestrianismTourDuration = model.EquestrianismTourDuration;
            //entity.EquestrianismAllowedAge = model.EquestrianismAllowedAge;
            //entity.EquestrianismAllowedWeight = model.EquestrianismAllowedWeight;
            //entity.EquestrianismHasAccompanyingTrainer = model.EquestrianismHasAccompanyingTrainer;
            //entity.EquestrianismTrackIndoor = model.EquestrianismTrackIndoor;
            //entity.EquestrianismTrackOutdoor = model.EquestrianismTrackOutdoor;

            //// الرماية
            //entity.ShootingIndoor = model.ShootingIndoor;
            //entity.ShootingOutdoor = model.ShootingOutdoor;
            //entity.ShootingAirShooting = model.ShootingAirShooting;
            //entity.ShootingFireShooting = model.ShootingFireShooting;
            //entity.ShootingBowAndArrow = model.ShootingBowAndArrow;
            //entity.ShootingShootingTrainer = model.ShootingShootingTrainer;
            //entity.ShootingEquipmentAvailable = model.ShootingEquipmentAvailable;
            //entity.ShootingEquipmentRental = model.ShootingEquipmentRental;

            //// Pickleball
            //entity.PickleballIndoor = model.PickleballIndoor;
            //entity.PickleballOutdoor = model.PickleballOutdoor;
            //entity.PickleballPaddles = model.PickleballPaddles;
            //entity.PickleballBalls = model.PickleballBalls;
            //entity.PickleballLighting = model.PickleballLighting;

            //// تنس الطاولة
            //entity.TableTennisProfessional = model.TableTennisProfessional;
            //entity.TableTennisPaddles = model.TableTennisPaddles;
            //entity.TableTennisBalls = model.TableTennisBalls;
            //entity.TableTennisTraining = model.TableTennisTraining;

            //// الاسكواش
            //entity.SquashGlassCourt = model.SquashGlassCourt;
            //entity.SquashNormalCourt = model.SquashNormalCourt;
            //entity.SquashPaddles = model.SquashPaddles;
            //entity.SquashBalls = model.SquashBalls;
            //entity.SquashTraining = model.SquashTraining;

            //// الريشة الطائرة
            //entity.BadmintonIndoor = model.BadmintonIndoor;
            //entity.BadmintonProfessionalFloor = model.BadmintonProfessionalFloor;
            //entity.BadmintonPaddles = model.BadmintonPaddles;
            //entity.BadmintonShuttlecocks = model.BadmintonShuttlecocks;
            //entity.BadmintonTraining = model.BadmintonTraining;

            //// الكرة الطائرة (شاطئية)
            //entity.BeachVolleyballIndoor = model.BeachVolleyballIndoor;
            //entity.BeachVolleyballOutdoor = model.BeachVolleyballOutdoor;
            //entity.BeachVolleyballSandFloor = model.BeachVolleyballSandFloor;
            //entity.BeachVolleyballIndoorFloor = model.BeachVolleyballIndoorFloor;
            //entity.BeachVolleyballLighting = model.BeachVolleyballLighting;
            //entity.BeachVolleyballProfessionalNet = model.BeachVolleyballProfessionalNet;
            //entity.BeachVolleyballBalls = model.BeachVolleyballBalls;

            //// كرة السلة (إضافات)
            //entity.BasketballIndoor = model.BasketballIndoor;
            //entity.BasketballOutdoor = model.BasketballOutdoor;
            //entity.BasketballWoodFloor = model.BasketballWoodFloor;
            //entity.BasketballRubberFloor = model.BasketballRubberFloor;
            //entity.BasketballLighting = model.BasketballLighting;
            //entity.BasketballStands = model.BasketballStands;
            //entity.BasketballScoreboard = model.BasketballScoreboard;
            //entity.BasketballBalls = model.BasketballBalls;
            //entity.BasketballTraining = model.BasketballTraining;

            //// التنس (إضافات)
            //entity.TennisIndoor = model.TennisIndoor;
            //entity.TennisOutdoor = model.TennisOutdoor;
            //entity.TennisAcrylicFloor = model.TennisAcrylicFloor;
            //entity.TennisClayFloor = model.TennisClayFloor;
            //entity.TennisGrassFloor = model.TennisGrassFloor;
            //entity.TennisLighting = model.TennisLighting;
            //entity.TennisPaddles = model.TennisPaddles;
            //entity.TennisBalls = model.TennisBalls;
            //entity.TennisTrainer = model.TennisTrainer;
            //entity.TennisAcademy = model.TennisAcademy;
            //entity.TennisTournaments = model.TennisTournaments;

            //// البادل (إضافات)
            //entity.PadelIndoor = model.PadelIndoor;
            //entity.PadelOutdoor = model.PadelOutdoor;
            //entity.PadelPanoramic = model.PadelPanoramic;
            //entity.PadelNormal = model.PadelNormal;
            //entity.PadelLighting = model.PadelLighting;
            //entity.PadelPaddlesRental = model.PadelPaddlesRental;
            //entity.PadelBallsAvailable = model.PadelBallsAvailable;
            //entity.PadelTrainer = model.PadelTrainer;
            //entity.PadelAcademy = model.PadelAcademy;
            //entity.PadelTournaments = model.PadelTournaments;
            //entity.PadelHourlyBooking = model.PadelHourlyBooking;

            return entity;
        }

        #endregion

        #region SportFeature ToModel / ToEntity

        //public static SportFeatureDto ToModel(this SportFeature entity)
        //{
        //    return new SportFeatureDto
        //    {
        //        Id = entity.Id,
        //        TypeId = entity.SportTypeId,
        //        FeatureText = entity.FeatureTextAr,
        //        FeatureTextEn = entity.FeatureTextEn,
        //        IsCheck = false,
        //        DescriptionAr = "",
        //        DescriptionEn = ""
        //    };
        //}

        //public static SportFeature ToEntity(this SportFeatureDto model)
        //{
        //    return new SportFeature
        //    {
        //        Id = model.Id,
        //        SportTypeId = model.TypeId,
        //        FeatureTextAr = model.FeatureText,
        //        FeatureTextEn = model.FeatureTextEn,
        //        IsActive = true
        //    };
        //}

        #endregion

        #region SportSportFeature ToModel / ToEntity

        //public static SportFeatureDto ToModel(this SportSportFeature entity, SportFeature feature = null)
        //{
        //    return new SportFeatureDto
        //    {
        //        Id = entity.Id,
        //        SportId = entity.SportId,
        //        TypeId = entity.SportFeatureId,
        //        FeatureText = feature?.FeatureTextAr ?? "",
        //        FeatureTextEn = feature?.FeatureTextEn ?? "",
        //        IsCheck = entity.IsChecked,
        //        DescriptionAr = entity.DescriptionAr,
        //        DescriptionEn = entity.DescriptionEn
        //    };
        //}


        //public static SportSportFeature ToEntity(this SportSportFeatureDto model)
        //{
        //    return new SportSportFeature
        //    {
        //        Id = model.Id,
        //        SportId = model.SportId,
        //        SportFeatureId = model.SportFeatureId,
        //        IsChecked = model.IsChecked,
        //        DescriptionAr = model.DescriptionAr,
        //        DescriptionEn = model.DescriptionEn
        //    };
        //}

        #endregion

        #region GeneralFacility ToModel / ToEntity

        //public static GeneralFacilityDto ToModel(this GeneralFacility entity)
        //{
        //    return new GeneralFacilityDto
        //    {
        //        Id = entity.Id,
        //        FacilityId = entity.Id,
        //        FacilityText = entity.FacilityTextAr,
        //        FacilityTextEn = entity.FacilityTextEn,
        //        IsCheck = false
        //    };
        //}

        public static GeneralFacility ToEntity(this GeneralFacilityDto model)
        {
            return new GeneralFacility
            {
                Id = model.Id,
                FacilityTextAr = model.FacilityText,
                FacilityTextEn = model.FacilityTextEn,
                IsActive = true
            };
        }

        #endregion

        #region SportGeneralFacility ToModel / ToEntity

        public static GeneralFacilityDto ToModel(this SportGeneralFacility entity, GeneralFacility facility = null)
        {
            return new GeneralFacilityDto
            {
                Id = entity.Id,
                SportId = entity.SportId,
                FacilityId = entity.GeneralFacilityId,
                FacilityText = facility?.FacilityTextAr ?? "",
                FacilityTextEn = facility?.FacilityTextEn ?? "",
                IsCheck = entity.IsActive
            };
        }

        public static SportGeneralFacility ToEntity(this SportGeneralFacilityDto model)
        {
            return new SportGeneralFacility
            {
                Id = model.Id,
                SportId = model.SportId,
                GeneralFacilityId = model.GeneralFacilityId,
                IsActive = model.IsActive
            };
        }

        #endregion

        #region AdditionalService ToModel / ToEntity

        //public static AdditionalServiceDto ToModel(this AdditionalService entity)
        //{
        //    return new AdditionalServiceDto
        //    {
        //        Id = entity.Id,
        //        ServiceId = entity.Id,
        //        ServiceText = entity.ServiceTextAr,
        //        ServiceTextEn = entity.ServiceTextEn,
        //        IsCheck = false
        //    };
        //}

        public static AdditionalService ToEntity(this AdditionalServiceDto model)
        {
            return new AdditionalService
            {
                Id = model.Id,
                ServiceTextAr = model.ServiceText,
                ServiceTextEn = model.ServiceTextEn,
                IsActive = true
            };
        }

        #endregion

        #region SportAdditionalService ToModel / ToEntity

        public static AdditionalServiceDto ToModel(this SportAdditionalService entity, AdditionalService service = null)
        {
            return new AdditionalServiceDto
            {
                Id = entity.Id,
                SportId = entity.SportId,
                ServiceId = entity.AdditionalServiceId,
                ServiceText = service?.ServiceTextAr ?? "",
                ServiceTextEn = service?.ServiceTextEn ?? "",
                IsCheck = entity.IsActive
            };
        }

        public static SportAdditionalService ToEntity(this SportAdditionalServiceDto model)
        {
            return new SportAdditionalService
            {
                Id = model.Id,
                SportId = model.SportId,
                AdditionalServiceId = model.AdditionalServiceId,
                IsActive = model.IsActive
            };
        }

        #endregion
        #region SafetyFeature ToModel / ToEntity

        //public static SafetyFeatureDto ToModel(this SafetyFeature entity)
        //{
        //    return new SafetyFeatureDto
        //    {
        //        Id = entity.Id,
        //        TypeId = entity.Id,
        //        FeatureText = entity.FeatureTextAr,
        //        FeatureTextEn = entity.FeatureTextEn,
        //        IsCheck = false,
        //        DescriptionAr = "",
        //        DescriptionEn = ""
        //    };
        //}

        public static SafetyFeature ToEntity(this SafetyFeatureDto model)
        {
            return new SafetyFeature
            {
                Id = model.Id,
                FeatureTextAr = model.FeatureText,
                FeatureTextEn = model.FeatureTextEn,
                IsActive = true
            };
        }

        #endregion

        #region SportSafetyFeature ToModel / ToEntity

        public static SafetyFeatureDto ToModel(this SportSafetyFeature entity, SafetyFeature feature = null)
        {
            return new SafetyFeatureDto
            {
                Id = entity.Id,
                SportId = entity.SportId,
                TypeId = entity.SafetyFeatureId,
                FeatureText = feature?.FeatureTextAr ?? "",
                FeatureTextEn = feature?.FeatureTextEn ?? "",
                IsCheck = entity.IsChecked,
                DescriptionAr = entity.DescriptionAr,
                DescriptionEn = entity.DescriptionEn
            };
        }

        public static SportSafetyFeature ToEntity(this SportSafetyFeatureDto model)
        {
            return new SportSafetyFeature
            {
                Id = model.Id,
                SportId = model.SportId,
                SafetyFeatureId = model.SafetyFeatureId,
                IsChecked = model.IsChecked,
                DescriptionAr = model.DescriptionAr,
                DescriptionEn = model.DescriptionEn
            };
        }

        #endregion

        #region GeneralFacility ToModel / ToEntity

        public static GeneralFacilityModel ToModel(this GeneralFacility entity)
        {
            return new GeneralFacilityModel
            {
                Id = entity.Id,
                FacilityTextAr = entity.FacilityTextAr,
                FacilityTextEn = entity.FacilityTextEn,
                IconClass = entity.IconClass,
                IsActive = entity.IsActive
            };
        }

        public static GeneralFacility ToEntity(this GeneralFacilityModel model)
        {
            return new GeneralFacility
            {
                Id = model.Id,
                FacilityTextAr = model.FacilityTextAr,
                FacilityTextEn = model.FacilityTextEn,
                IconClass = model.IconClass,
                IsActive = model.IsActive
            };
        }

        #endregion
        #region AdditionalService ToModel / ToEntity

        public static AdditionalServiceModel ToModel(this AdditionalService entity)
        {
            return new AdditionalServiceModel
            {
                Id = entity.Id,
                ServiceTextAr = entity.ServiceTextAr,
                ServiceTextEn = entity.ServiceTextEn,
                IconClass = entity.IconClass,
                IsActive = entity.IsActive
            };
        }

        public static AdditionalService ToEntity(this AdditionalServiceModel model)
        {
            return new AdditionalService
            {
                Id = model.Id,
                ServiceTextAr = model.ServiceTextAr,
                ServiceTextEn = model.ServiceTextEn,
                IconClass = model.IconClass,
                IsActive = model.IsActive
            };
        }

        #endregion

        #region SafetyFeature ToModel / ToEntity

        public static SafetyFeatureModel ToModel(this SafetyFeature entity)
        {
            return new SafetyFeatureModel
            {
                Id = entity.Id,
                FeatureTextAr = entity.FeatureTextAr,
                FeatureTextEn = entity.FeatureTextEn,
                IconClass = entity.IconClass,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public static SafetyFeature ToEntity(this SafetyFeatureModel model)
        {
            return new SafetyFeature
            {
                Id = model.Id,
                FeatureTextAr = model.FeatureTextAr,
                FeatureTextEn = model.FeatureTextEn,
                IconClass = model.IconClass,
                IsActive = model.IsActive,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate
            };
        }

        #endregion

        #region SportType ToModel / ToEntity

        public static SportTypeModel ToModel(this SportType entity)
        {
            return new SportTypeModel
            {
                Id = entity.Id,
                NameAr = entity.NameAr,
                NameEn = entity.NameEn,
                IconClass = entity.IconClass,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public static SportType ToEntity(this SportTypeModel model)
        {
            return new SportType
            {
                Id = model.Id,
                NameAr = model.NameAr,
                NameEn = model.NameEn,
                IconClass = model.IconClass,
                IsActive = model.IsActive,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate
            };
        }

        #endregion

        #region SportFeature ToModel / ToEntity

        //public static SportFeatureModel ToModel(this SportFeature entity)
        //{
        //    return new SportFeatureModel
        //    {
        //        Id = entity.Id,
        //        SportTypeId = entity.SportTypeId,
        //        FeatureTextAr = entity.FeatureTextAr,
        //        FeatureTextEn = entity.FeatureTextEn,
        //        IconClass = entity.IconClass,
        //        IsActive = entity.IsActive
        //    };
        //}

        //public static SportFeature ToEntity(this SportFeatureModel model)
        //{
        //    return new SportFeature
        //    {
        //        Id = model.Id,
        //        SportTypeId = model.SportTypeId,
        //        FeatureTextAr = model.FeatureTextAr,
        //        FeatureTextEn = model.FeatureTextEn,
        //        IconClass = model.IconClass,
        //        IsActive = model.IsActive
        //    };
        //}

        #endregion


        #region SportFeature ToModel / ToEntity

        public static SportFeatureModel ToModel(this SportFeature entity)
        {
            return new SportFeatureModel
            {
                Id = entity.Id,
                SportTypeId = entity.SportTypeId,
                FeatureTextAr = entity.FeatureTextAr,
                FeatureTextEn = entity.FeatureTextEn,
                IconClass = entity.IconClass,
                IsActive = entity.IsActive
            };
        }

        public static SportFeature ToEntity(this SportFeatureModel model)
        {
            return new SportFeature
            {
                Id = model.Id,
                SportTypeId = model.SportTypeId,
                FeatureTextAr = model.FeatureTextAr,
                FeatureTextEn = model.FeatureTextEn,
                IconClass = model.IconClass,
                IsActive = model.IsActive
            };
        }

        #endregion

        #region SportSportFeature ToModel / ToEntity

        public static SportFeatureDto ToModel(this SportSportFeature entity, SportFeature feature = null)
        {
            return new SportFeatureDto
            {
                Id = entity.Id,
                SportId = entity.SportId,
                TypeId = entity.SportFeatureId,
                FeatureText = feature?.FeatureTextAr ?? "",
                FeatureTextEn = feature?.FeatureTextEn ?? "",
                IsCheck = entity.IsChecked,
                DescriptionAr = entity.DescriptionAr,
                DescriptionEn = entity.DescriptionEn
            };
        }

        public static SportSportFeature ToEntity(this SportFeatureDto model)
        {
            return new SportSportFeature
            {
                Id = model.Id,
                SportId = model.SportId,
                SportFeatureId = model.TypeId,
                IsChecked = model.IsCheck,
                DescriptionAr = model.DescriptionAr,
                DescriptionEn = model.DescriptionEn
            };
        }

        #endregion


        //#region SportPrice ToModel / ToEntity

        //public static SportPriceModel ToModel(this Sport entity, List<SportPriceList> prices = null)
        //{
        //    var model = new SportPriceModel
        //    {
        //        SportId = entity.Id,
        //        SportName = entity.NameAr,
        //        MinBookingHours = prices?.FirstOrDefault()?.MinBookingHours ?? 1,
        //        Person = prices?.FirstOrDefault()?.Person ?? 1
        //    };

        //    // إنشاء قائمة الأسعار للأيام السبعة
        //    for (int day = 1; day <= 7; day++)
        //    {
        //        var existing = prices?.FirstOrDefault(p => p.Day == day);
        //        model.PriceList.Add(new SportPriceDayDto
        //        {
        //            Id = existing?.Id ?? 0,
        //            SportId = entity.Id,
        //            Day = day,
        //            HourlyPrice = existing?.HourlyPrice ?? 0,
        //            PeakHourlyPrice = existing?.PeakHourlyPrice,
        //            PeakStartTime = existing?.PeakStartTime,
        //            PeakEndTime = existing?.PeakEndTime,
        //            OfferHourlyPrice = existing?.OfferHourlyPrice
        //        });
        //    }

        //    return model;
        //}

        //public static SportPriceList ToEntity(this SportPriceDayDto model, int person, int minBookingHours)
        //{
        //    return new SportPriceList
        //    {
        //        Id = model.Id,
        //        SportId = model.SportId,
        //        Day = model.Day,
        //        Person = person,
        //        HourlyPrice = model.HourlyPrice,
        //        PeakHourlyPrice = model.PeakHourlyPrice,
        //        PeakStartTime = model.PeakStartTime,
        //        PeakEndTime = model.PeakEndTime,
        //        OfferHourlyPrice = model.OfferHourlyPrice,
        //        MinBookingHours = minBookingHours
        //    };
        //}

        //#endregion



        #region SportReservation ToModel / ToEntity

        public static SportReservationModel ToModel(this SportReservation entity)
        {
            return new SportReservationModel
            {
                Id = entity.Id,
                SportId = (int)entity.SportId,
                //SportName = entity.Sport?.NameAr,
                SportTypeId = (int)entity.SportTypeId,
                CustomerId = (int)entity.CustomerId,
                //SportTypeName = entity.SportType?.NameAr,
                CustomerName = entity.CustomerName,
                CustMobNum = entity.CustMobNum,
                MobileOwnerAppUser = entity.MobileOwnerAppUser,
                ReservationDate = entity.ReservationDate,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                TotalHours = entity.TotalHours,
                PersonCount = entity.PersonCount,
                CostReservationAmtOnMahjouz = entity.CostReservationAmtOnMahjouz,
                ReservationAmt = entity.ReservationAmt,
                ReservationDepositAmt = entity.ReservationDepositAmt,
                NetProfit = entity.NetProfit,
                ReservationRemainAmt = entity.ReservationRemainAmt,
                ReservStatus = entity.ReservStatus,
                Reason = entity.Reason,
                Note = entity.Note,
                IsMahjouzReservation = entity.IsMahjouzReservation,
                IsReceiveCommession = entity.IsReceiveCommession,
                DeviceId = entity.DeviceId,
                TokenCustomer = entity.TokenCustomer,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public static SportReservation ToEntity(this SportReservationModel model)
        {
            return new SportReservation
            {
                Id = model.Id,
                SportId = model.SportId,
                SportTypeId = model.SportTypeId,
                CustomerId = model.CustomerId,
                CustomerName = model.CustomerName,
                CustMobNum = model.CustMobNum,
                MobileOwnerAppUser = model.MobileOwnerAppUser,
                ReservationDate = model.ReservationDate,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                TotalHours = model.TotalHours,
                PersonCount = model.PersonCount,
                CostReservationAmtOnMahjouz = model.CostReservationAmtOnMahjouz,
                ReservationAmt = model.ReservationAmt,
                ReservationDepositAmt = model.ReservationDepositAmt,
                NetProfit = model.NetProfit,
                ReservationRemainAmt = model.ReservationRemainAmt,
                ReservStatus = model.ReservStatus,
                Reason = model.Reason,
                Note = model.Note,
                IsMahjouzReservation = model.IsMahjouzReservation,
                IsReceiveCommession = model.IsReceiveCommession,
                DeviceId = model.DeviceId,
                TokenCustomer = model.TokenCustomer,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate
            };
        }

        #endregion








        #region SportPropertyTemplate ToModel / ToEntity

        public static SportPropertyTemplateDto ToModel(this SportPropertyTemplate entity)
        {
            return new SportPropertyTemplateDto
            {
                Id = entity.Id,
                SportTypeId = entity.SportTypeId,
                PropertyKey = entity.PropertyKey,
                PropertyLabelAr = entity.PropertyLabelAr,
                PropertyLabelEn = entity.PropertyLabelEn,
                PropertyType = (int)entity.PropertyType,
                IsRequired = entity.IsRequired,
                SortOrder = entity.SortOrder,
                IsActive = entity.IsActive
            };
        }

        public static SportPropertyTemplate ToEntity(this SportPropertyTemplateDto model)
        {
            return new SportPropertyTemplate
            {
                Id = model.Id,
                SportTypeId = model.SportTypeId,
                PropertyKey = model.PropertyKey,
                PropertyLabelAr = model.PropertyLabelAr,
                PropertyLabelEn = model.PropertyLabelEn,
                PropertyType = (PropertyTypeEnum)model.PropertyType,
                IsRequired = model.IsRequired,
                SortOrder = model.SortOrder,
                IsActive = model.IsActive
            };
        }

        #endregion

        #region SportPropertyOption ToModel / ToEntity

        public static SportPropertyOptionDto ToModel(this SportPropertyOption entity)
        {
            return new SportPropertyOptionDto
            {
                Id = entity.Id,
                PropertyTemplateId = entity.PropertyTemplateId,
                OptionValue = entity.OptionValue,
                OptionTextAr = entity.OptionTextAr,
                OptionTextEn = entity.OptionTextEn,
                SortOrder = entity.SortOrder,
                IsActive = entity.IsActive
            };
        }

        public static SportPropertyOption ToEntity(this SportPropertyOptionDto model)
        {
            return new SportPropertyOption
            {
                Id = model.Id,
                PropertyTemplateId = model.PropertyTemplateId,
                OptionValue = model.OptionValue,
                OptionTextAr = model.OptionTextAr,
                OptionTextEn = model.OptionTextEn,
                SortOrder = model.SortOrder,
                IsActive = model.IsActive
            };
        }

        #endregion

        #region SportPropertyValue ToModel / ToEntity

        public static SportPropertyValueDto ToModel(this SportPropertyValue entity)
        {
            return new SportPropertyValueDto
            {
                Id = entity.Id,
                SportId = entity.SportId,
                PropertyTemplateId = entity.PropertyTemplateId,
                ValueText = entity.ValueText,
                ValueBool = entity.ValueBool,
                ValueOptionId = entity.ValueOptionId
            };
        }

        public static SportPropertyValue ToEntity(this SportPropertyValueDto model)
        {
            return new SportPropertyValue
            {
                Id = model.Id,
                SportId = model.SportId,
                PropertyTemplateId = model.PropertyTemplateId,
                ValueText = model.ValueText,
                ValueBool = model.ValueBool,
                ValueOptionId = model.ValueOptionId
            };
        }

        #endregion


        #region Loyalty Points

        #region LoyaltyActivityType

        public static LoyaltyActivityTypeModel ToModel(this LoyaltyActivityType entity)
        {
            return new LoyaltyActivityTypeModel
            {
                Id = entity.Id,
                NameAr = entity.NameAr,
                NameEn = entity.NameEn,
                Code = entity.Code,
                SportTypeId = entity.SportTypeId,
                IconClass = entity.IconClass,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public static LoyaltyActivityType ToEntity(this LoyaltyActivityTypeModel model)
        {
            return new LoyaltyActivityType
            {
                Id = model.Id,
                NameAr = model.NameAr,
                NameEn = model.NameEn,
                Code = model.Code,
                SportTypeId = model.SportTypeId,
                IconClass = model.IconClass,
                IsActive = model.IsActive,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate
            };
        }

        #endregion

        #region LoyaltyPointRule

        public static LoyaltyPointRuleModel ToModel(this LoyaltyPointRule entity)
        {
            return new LoyaltyPointRuleModel
            {
                Id = entity.Id,
                ActivityTypeId = entity.ActivityTypeId,
                ReferenceType = entity.ReferenceType,
                ReferenceId = entity.ReferenceId,
                Code = entity.Code,
                Points = entity.Points,
                IsActive = entity.IsActive,
                IsDefault = entity.IsDefault,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public static LoyaltyPointRule ToEntity(this LoyaltyPointRuleModel model)
        {
            return new LoyaltyPointRule
            {
                Id = model.Id,
                ActivityTypeId = model.ActivityTypeId,
                ReferenceType = model.ReferenceType,
                ReferenceId = model.ReferenceId,
                Code = model.Code,
                Points = model.Points,
                IsActive = model.IsActive,
                IsDefault = model.IsDefault,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate
            };
        }

        #endregion

        #region LoyaltyTier

        public static LoyaltyTierModel ToModel(this LoyaltyTier entity)
        {
            return new LoyaltyTierModel
            {
                Id = entity.Id,
                NameAr = entity.NameAr,
                NameEn = entity.NameEn,
                IconClass = entity.IconClass,
                MinPoints = entity.MinPoints,
                MaxPoints = entity.MaxPoints,
                DiscountPercent = entity.DiscountPercent,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate
            };
        }

        public static LoyaltyTier ToEntity(this LoyaltyTierModel model)
        {
            return new LoyaltyTier
            {
                Id = model.Id,
                NameAr = model.NameAr,
                NameEn = model.NameEn,
                IconClass = model.IconClass,
                MinPoints = model.MinPoints,
                MaxPoints = model.MaxPoints,
                DiscountPercent = model.DiscountPercent,
                IsActive = model.IsActive,
                CreatedDate = model.CreatedDate
            };
        }

        #endregion

        #region CustomerLoyaltyAccount

        public static CustomerLoyaltyAccountModel ToModel(this CustomerLoyaltyAccount entity)
        {
            return new CustomerLoyaltyAccountModel
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                TotalPoints = entity.TotalPoints,
                AvailablePoints = entity.AvailablePoints,
                RedeemedPoints = entity.RedeemedPoints,
                ExpiredPoints = entity.ExpiredPoints,
                CurrentTierId = entity.CurrentTierId,
                ExpireDate = entity.ExpireDate,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate
            };
        }

        public static CustomerLoyaltyAccount ToEntity(this CustomerLoyaltyAccountModel model)
        {
            return new CustomerLoyaltyAccount
            {
                Id = model.Id,
                CustomerId = model.CustomerId,
                TotalPoints = model.TotalPoints,
                AvailablePoints = model.AvailablePoints,
                RedeemedPoints = model.RedeemedPoints,
                ExpiredPoints = model.ExpiredPoints,
                CurrentTierId = model.CurrentTierId,
                ExpireDate = model.ExpireDate,
                CreatedDate = model.CreatedDate,
                UpdatedDate = model.UpdatedDate
            };
        }

        #endregion

        #region LoyaltyTransaction

        public static LoyaltyTransactionModel ToModel(this LoyaltyTransaction entity)
        {
            return new LoyaltyTransactionModel
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                TransactionType = entity.TransactionType,
                Points = entity.Points,
                ReferenceId = entity.ReferenceId,
                ReferenceType = entity.ReferenceType,
                Description = entity.Description,
                TransactionDate = entity.TransactionDate,
                ExpireDate = entity.ExpireDate,
                CreatedBy = entity.CreatedBy
            };
        }

        public static LoyaltyTransaction ToEntity(this LoyaltyTransactionModel model)
        {
            return new LoyaltyTransaction
            {
                Id = (int)model.Id,
                CustomerId = model.CustomerId,
                TransactionType = model.TransactionType,
                Points = model.Points,
                ReferenceId = model.ReferenceId,
                ReferenceType = model.ReferenceType,
                Description = model.Description,
                TransactionDate = model.TransactionDate,
                ExpireDate = model.ExpireDate,
                CreatedBy = model.CreatedBy
            };
        }

        #endregion

        #region LoyaltyRedeemRule

        public static LoyaltyRedeemRuleModel ToModel(this LoyaltyRedeemRule entity)
        {
            return new LoyaltyRedeemRuleModel
            {
                Id = entity.Id,
                Points = entity.Points,
                DiscountAmount = entity.DiscountAmount,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate
            };
        }

        public static LoyaltyRedeemRule ToEntity(this LoyaltyRedeemRuleModel model)
        {
            return new LoyaltyRedeemRule
            {
                Id = model.Id,
                Points = model.Points,
                DiscountAmount = model.DiscountAmount,
                IsActive = model.IsActive,
                CreatedDate = model.CreatedDate
            };
        }

        #endregion

        #region LoyaltyBookingActivity

        public static LoyaltyBookingActivityModel ToModel(this LoyaltyBookingActivity entity)
        {
            return new LoyaltyBookingActivityModel
            {
                Id = entity.Id,
                BookingType = entity.BookingType,
                ActivityTypeId = entity.ActivityTypeId,
                IsActive = entity.IsActive
            };
        }

        public static LoyaltyBookingActivity ToEntity(this LoyaltyBookingActivityModel model)
        {
            return new LoyaltyBookingActivity
            {
                Id = model.Id,
                BookingType = model.BookingType,
                ActivityTypeId = model.ActivityTypeId,
                IsActive = model.IsActive
            };
        }

        #endregion

        #region ReservationLoyaltyDiscount

        public static ReservationLoyaltyDiscountModel ToModel(this ReservationLoyaltyDiscount entity)
        {
            return new ReservationLoyaltyDiscountModel
            {
                Id = entity.Id,
                ReservationId = entity.ReservationId,
                ReservationType = entity.ReservationType,
                CustomerId = entity.CustomerId,
                PointsUsed = entity.PointsUsed,
                DiscountAmount = entity.DiscountAmount,
                CreatedDate = entity.CreatedDate
            };
        }

        public static ReservationLoyaltyDiscount ToEntity(this ReservationLoyaltyDiscountModel model)
        {
            return new ReservationLoyaltyDiscount
            {
                Id = model.Id,
                ReservationId = model.ReservationId,
                ReservationType = model.ReservationType,
                CustomerId = model.CustomerId,
                PointsUsed = model.PointsUsed,
                DiscountAmount = model.DiscountAmount,
                CreatedDate = model.CreatedDate
            };
        }

        #endregion

        #endregion




        //#region LoyaltyActivityType

        //public static LoyaltyActivityTypeModel ToModel(this LoyaltyActivityType entity)
        //{
        //    return new LoyaltyActivityTypeModel
        //    {
        //        Id = entity.Id,
        //        NameAr = entity.NameAr,
        //        NameEn = entity.NameEn,
        //        Code = entity.Code,
        //        SportTypeId = entity.SportTypeId,
        //        IconClass = entity.IconClass,
        //        IsActive = entity.IsActive,
        //        CreatedDate = entity.CreatedDate,
        //        ModifiedDate = entity.ModifiedDate
        //    };
        //}

        //public static LoyaltyActivityType ToEntity(this LoyaltyActivityTypeModel model)
        //{
        //    return new LoyaltyActivityType
        //    {
        //        Id = model.Id,
        //        NameAr = model.NameAr,
        //        NameEn = model.NameEn,
        //        Code = model.Code,
        //        SportTypeId = model.SportTypeId,
        //        IconClass = model.IconClass,
        //        IsActive = model.IsActive,
        //        CreatedDate = model.CreatedDate,
        //        ModifiedDate = model.ModifiedDate
        //    };
        //}

        //#endregion

        //#region LoyaltyPointRuleSport

        //public static LoyaltyPointRuleSportModel ToModel(this LoyaltyPointRuleSport entity)
        //{
        //    return new LoyaltyPointRuleSportModel
        //    {
        //        Id = entity.Id,
        //        ActivityTypeId = entity.ActivityTypeId,
        //        SportId = entity.SportId,
        //        Points = entity.Points,
        //        IsActive = entity.IsActive,
        //        IsDefault = entity.IsDefault,
        //        CreatedDate = entity.CreatedDate,
        //        ModifiedDate = entity.ModifiedDate
        //    };
        //}

        //public static LoyaltyPointRuleSport ToEntity(this LoyaltyPointRuleSportModel model)
        //{
        //    return new LoyaltyPointRuleSport
        //    {
        //        Id = model.Id,
        //        ActivityTypeId = model.ActivityTypeId,
        //        SportId = model.SportId,
        //        Points = model.Points,
        //        IsActive = model.IsActive,
        //        IsDefault = model.IsDefault,
        //        CreatedDate = model.CreatedDate,
        //        ModifiedDate = model.ModifiedDate
        //    };
        //}

        //#endregion

        //#region LoyaltyPointRuleFarm

        //public static LoyaltyPointRuleFarmModel ToModel(this LoyaltyPointRuleFarm entity)
        //{
        //    return new LoyaltyPointRuleFarmModel
        //    {
        //        Id = entity.Id,
        //        ActivityTypeId = entity.ActivityTypeId,
        //        FarmId = entity.FarmId,
        //        Points = entity.Points,
        //        IsActive = entity.IsActive,
        //        IsDefault = entity.IsDefault,
        //        CreatedDate = entity.CreatedDate,
        //        ModifiedDate = entity.ModifiedDate
        //    };
        //}

        //public static LoyaltyPointRuleFarm ToEntity(this LoyaltyPointRuleFarmModel model)
        //{
        //    return new LoyaltyPointRuleFarm
        //    {
        //        Id = model.Id,
        //        ActivityTypeId = model.ActivityTypeId,
        //        FarmId = model.FarmId,
        //        Points = model.Points,
        //        IsActive = model.IsActive,
        //        IsDefault = model.IsDefault,
        //        CreatedDate = model.CreatedDate,
        //        ModifiedDate = model.ModifiedDate
        //    };
        //}

        //#endregion

        //#region LoyaltyPointRuleGeneral

        //public static LoyaltyPointRuleGeneralModel ToModel(this LoyaltyPointRuleGeneral entity)
        //{
        //    return new LoyaltyPointRuleGeneralModel
        //    {
        //        Id = entity.Id,
        //        ActivityTypeId = entity.ActivityTypeId,
        //        Points = entity.Points,
        //        IsActive = entity.IsActive,
        //        IsDefault = entity.IsDefault,
        //        CreatedDate = entity.CreatedDate,
        //        ModifiedDate = entity.ModifiedDate
        //    };
        //}

        //public static LoyaltyPointRuleGeneral ToEntity(this LoyaltyPointRuleGeneralModel model)
        //{
        //    return new LoyaltyPointRuleGeneral
        //    {
        //        Id = model.Id,
        //        ActivityTypeId = model.ActivityTypeId,
        //        Points = model.Points,
        //        IsActive = model.IsActive,
        //        IsDefault = model.IsDefault,
        //        CreatedDate = model.CreatedDate,
        //        ModifiedDate = model.ModifiedDate
        //    };
        //}

        //#endregion

        //#region LoyaltyTransaction

        //public static LoyaltyTransactionModel ToModel(this LoyaltyTransaction entity)
        //{
        //    return new LoyaltyTransactionModel
        //    {
        //        Id = entity.Id,
        //        CustomerId = entity.CustomerId,
        //        TransactionType = entity.TransactionType,
        //        Points = entity.Points,
        //        ReferenceId = entity.ReferenceId,
        //        ReferenceType = entity.ReferenceType,
        //        Description = entity.Description,
        //        TransactionDate = entity.TransactionDate,
        //        ExpireDate = entity.ExpireDate,
        //        CreatedBy = entity.CreatedBy
        //    };
        //}

        //public static LoyaltyTransaction ToEntity(this LoyaltyTransactionModel model)
        //{
        //    return new LoyaltyTransaction
        //    {
        //        Id = (int)model.Id,
        //        CustomerId = model.CustomerId,
        //        TransactionType = model.TransactionType,
        //        Points = model.Points,
        //        ReferenceId = model.ReferenceId,
        //        ReferenceType = model.ReferenceType,
        //        Description = model.Description,
        //        TransactionDate = model.TransactionDate,
        //        ExpireDate = model.ExpireDate,
        //        CreatedBy = model.CreatedBy
        //    };
        //}

        //#endregion

        //#region ReservationLoyaltyDiscount

        //public static ReservationLoyaltyDiscountModel ToModel(this ReservationLoyaltyDiscount entity)
        //{
        //    return new ReservationLoyaltyDiscountModel
        //    {
        //        Id = entity.Id,
        //        ReservationId = entity.ReservationId,
        //        ReservationType = entity.ReservationType,
        //        CustomerId = entity.CustomerId,
        //        PointsUsed = entity.PointsUsed,
        //        DiscountAmount = entity.DiscountAmount,
        //        CreatedDate = entity.CreatedDate
        //    };
        //}

        //public static ReservationLoyaltyDiscount ToEntity(this ReservationLoyaltyDiscountModel model)
        //{
        //    return new ReservationLoyaltyDiscount
        //    {
        //        Id = model.Id,
        //        ReservationId = model.ReservationId,
        //        ReservationType = model.ReservationType,
        //        CustomerId = model.CustomerId,
        //        PointsUsed = model.PointsUsed,
        //        DiscountAmount = model.DiscountAmount,
        //        CreatedDate = model.CreatedDate
        //    };
        //}

        //#endregion

        //#region LoyaltyTier

        //public static LoyaltyTierModel ToModel(this LoyaltyTier entity)
        //{
        //    return new LoyaltyTierModel
        //    {
        //        Id = entity.Id,
        //        NameAr = entity.NameAr,
        //        NameEn = entity.NameEn,
        //        IconClass = entity.IconClass,
        //        MinPoints = entity.MinPoints,
        //        MaxPoints = entity.MaxPoints,
        //        DiscountPercent = entity.DiscountPercent,
        //        IsActive = entity.IsActive,
        //        CreatedDate = entity.CreatedDate
        //    };
        //}

        //public static LoyaltyTier ToEntity(this LoyaltyTierModel model)
        //{
        //    return new LoyaltyTier
        //    {
        //        Id = model.Id,
        //        NameAr = model.NameAr,
        //        NameEn = model.NameEn,
        //        IconClass = model.IconClass,
        //        MinPoints = model.MinPoints,
        //        MaxPoints = model.MaxPoints,
        //        DiscountPercent = model.DiscountPercent,
        //        IsActive = model.IsActive,
        //        CreatedDate = model.CreatedDate
        //    };
        //}

        //#endregion

        //#region CustomerLoyaltyAccount

        //public static CustomerLoyaltyAccountModel ToModel(this CustomerLoyaltyAccount entity)
        //{
        //    return new CustomerLoyaltyAccountModel
        //    {
        //        Id = entity.Id,
        //        CustomerId = entity.CustomerId,
        //        TotalPoints = entity.TotalPoints,
        //        AvailablePoints = entity.AvailablePoints,
        //        RedeemedPoints = entity.RedeemedPoints,
        //        ExpiredPoints = entity.ExpiredPoints,
        //        CurrentTierId = entity.CurrentTierId,
        //        ExpireDate = entity.ExpireDate,
        //        CreatedDate = entity.CreatedDate,
        //        UpdatedDate = entity.UpdatedDate
        //    };
        //}

        //public static CustomerLoyaltyAccount ToEntity(this CustomerLoyaltyAccountModel model)
        //{
        //    return new CustomerLoyaltyAccount
        //    {
        //        Id = model.Id,
        //        CustomerId = model.CustomerId,
        //        TotalPoints = model.TotalPoints,
        //        AvailablePoints = model.AvailablePoints,
        //        RedeemedPoints = model.RedeemedPoints,
        //        ExpiredPoints = model.ExpiredPoints,
        //        CurrentTierId = model.CurrentTierId,
        //        ExpireDate = model.ExpireDate,
        //        CreatedDate = model.CreatedDate,
        //        UpdatedDate = model.UpdatedDate
        //    };
        //}

        //#endregion

        //#region LoyaltyRedeemRule

        //public static LoyaltyRedeemRuleModel ToModel(this LoyaltyRedeemRule entity)
        //{
        //    return new LoyaltyRedeemRuleModel
        //    {
        //        Id = entity.Id,
        //        Points = entity.Points,
        //        DiscountAmount = entity.DiscountAmount,
        //        IsActive = entity.IsActive,
        //        CreatedDate = entity.CreatedDate
        //    };
        //}

        //public static LoyaltyRedeemRule ToEntity(this LoyaltyRedeemRuleModel model)
        //{
        //    return new LoyaltyRedeemRule
        //    {
        //        Id = model.Id,
        //        Points = model.Points,
        //        DiscountAmount = model.DiscountAmount,
        //        IsActive = model.IsActive,
        //        CreatedDate = model.CreatedDate
        //    };
        //}

        //#endregion












    }

}