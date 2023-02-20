using WorkshopApi.Database;
using WorkshopConfTool.Shared.Models;

namespace WorkshopApi.Extensions
{
    public static class ConferenceExtensions
    {
        public static ConferenceOverview ToOverview(this Conference entity)
            => new ConferenceOverview() { ID = entity.ID, Title = entity.Title, };

        public static ConferenceDetails ToDetail(this Conference entity)
            => new ConferenceDetails()
            {
                ID = entity.ID,
                Title = entity.Title,
                DateFrom = entity.DateFrom,
                DateTo = entity.DateTo,
                Country = entity.Country,
                City = entity.City,
                Url = entity.Url,
            };

        public static Conference ToEntity(this ConferenceDetails model)
            => new Conference()
            {
                ID = model.ID,
                Title = model.Title,
                DateFrom = model.DateFrom,
                DateTo = model.DateTo,
                Country = model.Country,
                City = model.City,
                Url = model.Url,
            };

    }
}
