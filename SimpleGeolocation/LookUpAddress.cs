using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;

namespace SimpleGeolocation
{
    public class LookUpAddress
    {
        public GeolocationAccessStatus? accessStatus { get; private set; } = null;

        private Geoposition position;

        public async Task<string> GetCurrentLocationOneShot()
        {
            if (!accessStatus.HasValue)
            {
                accessStatus = await Geolocator.RequestAccessAsync();
            }
            string result = "";
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    // Create Geolocator and define perodic-based tracking (2 second interval).
                    var _geolocator = new Geolocator { ReportInterval = 2000 };
                    _geolocator.DesiredAccuracy = PositionAccuracy.High;
                    position = await _geolocator.GetGeopositionAsync();
                    result = $"{position.Coordinate.Latitude},{position.Coordinate.Longitude}";
                    break;
            }

            return result;
        }

        public async Task<string> ReverseGeocodePosition()
        {
            //MapService.ServiceToken = " "; 
            // The location to reverse geocode.
            BasicGeoposition location = new BasicGeoposition();
            location.Latitude =  position.Coordinate.Latitude;
            location.Longitude =  position.Coordinate.Longitude;
            Geopoint pointToReverseGeocode = new Geopoint(location);
            
            // Reverse geocode the specified geographic location.
            MapLocationFinderResult result =
                  await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeocode);
            string strResult = "Warning: MapServiceToken not specified. Get one at https://www.bingmapsportal.com/";
            // If the query returns results, display the name of the town
            // contained in the address of the first result.
            if (result.Status == MapLocationFinderStatus.Success)
            {
                strResult = "You are in the town of " +
                      result.Locations[0].Address.Town + " " + result.Locations[0].Address.Region;
            }

            return strResult;
            
        }
    }
    
}
