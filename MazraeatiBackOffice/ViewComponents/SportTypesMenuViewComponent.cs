using MazraeatiBackOffice.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MazraeatiBackOffice.ViewComponents
{
    public class SportTypesMenuViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public SportTypesMenuViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IViewComponentResult Invoke()
        {
            // All Sport Types ....
            var sportTypes = _unitOfWork.SportTypeRepository
                .Table
                .Where(s => s.IsActive == true)
                .OrderBy(s => s.NameAr)
                .ToList();

            return View(sportTypes);
        }
    }
}
