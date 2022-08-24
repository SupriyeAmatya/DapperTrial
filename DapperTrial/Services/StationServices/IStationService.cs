using DapperTrial.Models.DataModel;

namespace DapperTrial.Services.StationServices
{
    public interface IStationService
    {

        public void AddStation(Station station);
        IEnumerable<Station> GetStations();

        Station DetailsStation(int Id);
        Station GetStationById(int Id);
        void UpdateStation(Station station);
        void DeleteStation(int Id);
    }
}
