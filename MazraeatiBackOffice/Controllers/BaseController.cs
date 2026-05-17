using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Controllers
{
    public class BaseController : Controller
    {
        public enum NotifyType
        {
            Success,
            Error
        }
        public List<int> ConvertStringListToIntList(string Ids = "")
        {
            try
            {
                if (string.IsNullOrEmpty(Ids))
                    return new List<int>();

                return Ids.Split(new[] { "," }, StringSplitOptions.None).Select(a => int.Parse(a)).ToList();

            }
            catch (Exception)
            {
                return new List<int>();
            }
        }
        public string HashPassword(string password)
        {
            return string.Join("", SHA1CryptoServiceProvider.Create().ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2"))).ToUpper();
        }
        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Error, message, persistForTheNextRequest);
        }
        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Success, message, persistForTheNextRequest);
        }
        protected virtual void AddNotification(NotifyType type, string message, bool persistForTheNextRequest)
        {
            string dataKey = string.Format("YN.notifications.{0}", type);
            if (persistForTheNextRequest)
            {
                if (TempData[dataKey] == null)
                    TempData[dataKey] = new List<string>();
                ((List<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                if (ViewData[dataKey] == null)
                    ViewData[dataKey] = new List<string>();
                ((List<string>)ViewData[dataKey]).Add(message);
            }
        }
    }
}
