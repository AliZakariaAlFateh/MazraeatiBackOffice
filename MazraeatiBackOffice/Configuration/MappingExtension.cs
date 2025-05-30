
using System.Linq;
using System.Collections.Generic;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Models;

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
        public static FarmerModel ToModel(this Farmer entity, List<Country> countries = null, List<City> cities = null, List<FarmerReservation> farmerReservations = null,List<FarmerFeedback> farmerFeedbacks = null)
        {
            FarmerModel model = new FarmerModel();
            model.Id = entity.Id;
            model.CountryId = entity.CountryId;
            model.CityId = entity.CityId;
            model.CountryDesc = countries == null ? string.Empty : countries.Where(c => c.Id == entity.CountryId).FirstOrDefault().DescAr;
            model.CityDesc = cities == null ? string.Empty : cities.Where(c => c.Id == entity.CityId).FirstOrDefault().DescAr;
            model.MobileNumber = entity.MobileNumber;
            model.Number = entity.Number;
            model.Name = entity.Name;
            model.Description = entity.Description;
            model.Owner = entity.Owner;
            model.LocationDesc = entity.LocationDesc;
            model.IssueDate = entity.IssueDate;
            model.ExpiryDate = entity.ExpiryDate;
            model.EStateArea = entity.EStateArea;
            model.Room = entity.Room;
            model.RoomDetails = entity.RoomDetails;
            model.Bathroom = entity.Bathroom;
            model.BathroomDetails = entity.BathroomDetails;
            model.LandArea = entity.LandArea;
            model.Floor = entity.Floor;
            model.InDoor = entity.InDoor;
            model.InDoorDescription = entity.InDoorDescription;
            model.OutDoor = entity.OutDoor;
            model.OutDoorDescription = entity.OutDoorDescription;
            model.kitchens = entity.kitchens;
            model.kitchensDescription = entity.kitchensDescription;
            model.ExtraDetails = entity.ExtraDetails;
            model.ReservationDetails = entity.ReservationDetails;
            model.Family = entity.Family;
            model.Location = entity.Location;
            model.InsuranceAmt = entity.InsuranceAmt;
            model.DepositAmt = entity.DepositAmt;
            model.MaxPerson = entity.MaxPerson;
            model.IsTrust = entity.IsTrust;
            model.IsVIP = entity.IsVIP;
            model.IsOffer = entity.IsOffer;
            model.IsApprove = entity.IsApprove;
            model.ReservationCount = farmerReservations != null ? farmerReservations.Count(f => f.FarmerId == model.Id) : 0;
            model.FeedbackCount = farmerFeedbacks != null ? farmerFeedbacks.Count(f => f.FarmerId == model.Id) : 0;
            return model;
        }
        public static Farmer ToEntity(this FarmerModel model)
        {
            Farmer entity = new Farmer();
            entity.Id = model.Id;
            entity.CountryId = model.CountryId;
            entity.CityId = model.CityId;
            entity.MobileNumber = model.MobileNumber;
            entity.Number = model.Number;
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Owner = model.Owner;
            entity.LocationDesc = model.LocationDesc;
            entity.IssueDate = model.IssueDate;
            entity.ExpiryDate = model.ExpiryDate;
            entity.EStateArea = model.EStateArea;
            entity.Room = model.Room;
            entity.RoomDetails = model.RoomDetails;
            entity.Bathroom = model.Bathroom;
            entity.BathroomDetails = model.BathroomDetails;
            entity.LandArea = model.LandArea;
            entity.Floor = model.Floor;
            entity.InDoor = model.InDoor;
            entity.InDoorDescription = model.InDoorDescription;
            entity.OutDoor = model.OutDoor;
            entity.OutDoorDescription = model.OutDoorDescription;
            entity.kitchens = model.kitchens;
            entity.kitchensDescription = model.kitchensDescription;
            entity.ExtraDetails = model.ExtraDetails;
            entity.ReservationDetails = model.ReservationDetails;
            entity.Family = model.Family;
            entity.Location = model.Location;
            entity.InsuranceAmt = model.InsuranceAmt;
            entity.DepositAmt = model.DepositAmt;
            entity.MaxPerson = model.MaxPerson;
            entity.IsTrust = model.IsTrust;
            entity.IsVIP = model.IsVIP;
            entity.IsOffer = model.IsOffer;
            entity.IsApprove = model.IsApprove;
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
            model.ReservationTypeId = entity.ReservationTypeId;
            model.ReservationTypeDesc = lookupValues == null ? string.Empty : lookupValues.Where(c => c.Id == entity.ReservationTypeId).FirstOrDefault().ValueAr;
            model.ReservationDate = entity.ReservationDate;
            model.CustMobNum = entity.CustMobNum;
            model.CustomerName = entity.CustomerName;
            model.Note = entity.Note;
            model.IsReciveCommission = entity.IsReciveCommission;
            model.AutomaticallyNote = entity.AutomaticallyNote;
            model.CreatedDate = entity.CreatedDate;
            return model;
        }
        public static FarmerReservation ToEntity(this FarmerReservationModel model)
        {
            FarmerReservation entity = new FarmerReservation();
            entity.Id = model.Id;
            entity.FarmerId = model.FarmerId;
            entity.ReservationTypeId = model.ReservationTypeId;
            entity.ReservationDate = model.ReservationDate;
            entity.CustMobNum = model.CustMobNum;
            entity.CustomerName = model.CustomerName;
            entity.Note = model.Note;
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

        #region Customer BlackList
        public static CustomerBlackListModel ToModel(this CustomerBlackList entity)
        {
            CustomerBlackListModel model = new CustomerBlackListModel();
            model.Id = entity.Id;
            model.CustName = entity.CustName;
            model.CustMobileNum = entity.CustMobileNum;
            model.Reason = entity.Reason;
            model.ImageUrl = entity.ImageUrl;
            model.IsApprove = entity.IsApprove;
            return model;
        }
        public static CustomerBlackList ToEntity(this CustomerBlackListModel model)
        {
            CustomerBlackList entity = new CustomerBlackList();
            entity.Id = model.Id;
            entity.CustName = model.CustName;
            entity.CustMobileNum = model.CustMobileNum;
            entity.Reason = model.Reason;
            entity.ImageUrl = model.ImageUrl;
            entity.IsApprove = model.IsApprove;
            return entity;
        }
        #endregion

        #region Farmer BlackList
        public static FarmerBlackListModel ToModel(this FarmerBlackList entity)
        {
            FarmerBlackListModel model = new FarmerBlackListModel();
            model.Id = entity.Id;
            model.FarmerName = entity.FarmerName;
            model.FarmerMobNum = entity.FarmerMobNum;
            model.Reason = entity.Reason;
            model.ImageUrl = entity.ImageUrl;
            model.IsApprove = entity.IsApprove;
            return model;
        }
        public static FarmerBlackList ToEntity(this FarmerBlackListModel model)
        {
            FarmerBlackList entity = new FarmerBlackList();
            entity.Id = model.Id;
            entity.FarmerName = model.FarmerName;
            entity.FarmerMobNum = model.FarmerMobNum;
            entity.Reason = model.Reason;
            entity.ImageUrl = model.ImageUrl;
            entity.IsApprove = model.IsApprove;
            return entity;
        }
        #endregion

        #region Lookup
        public static LookupModel ToModel(this Lookup entity)
        {
            LookupModel model = new LookupModel();
            model.Id = entity.Id;
            model.LookupCode = entity.LookupCode;
            model.LookupCodeDesc = (entity.LookupCode == "FarmerExtraFeatureType" ? "مزايا المزرعة" : "مزايا الاقسام الاخرى");
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
        public static LookupValueModel ToModel(this LookupValue entity,string lookupCode)
        {
            LookupValueModel model = new LookupValueModel();
            model.Id = entity.Id;
            model.LookupId = entity.LookupId;
            model.LookupDesc = (lookupCode == "FarmerExtraFeatureType" ? "مزايا المزرعة" : "مزايا الاقسام الاخرى"); ;
            model.Code = entity.Code;
            model.ValueAr = entity.ValueAr;
            model.ValueEn = entity.ValueEn;
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
            return entity;
        }
        #endregion

    }

}