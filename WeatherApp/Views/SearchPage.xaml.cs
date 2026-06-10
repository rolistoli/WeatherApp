using Mapsui;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling;
using WeatherApp.Models;
using WeatherApp.Services;
using WeatherApp.ViewModels;

namespace WeatherApp.Views;

public partial class SearchPage : ContentPage
{
    public SearchPage(SearchViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        MapControl.Map = new Mapsui.Map();
        MapControl.Map.Widgets.Clear();
    }

    private async void MapControl_Loaded(object sender, EventArgs e)
    {
        try
        {
            // lisbon coordinates
            var (lon, lat) = SphericalMercator.FromLonLat(-9.1393, 38.7223);

            MapControl.Map.Navigator.CenterOn(lon, lat);
            MapControl.Map.Navigator.ZoomTo(50);

            MapControl.Map.Navigator.OverrideZoomBounds = new MMinMax(50, 2000);
            MapControl.Map.Navigator.RotationLock = true;

            MapControl.Map.Layers.Add(OpenStreetMap.CreateTileLayer());
        }
        catch (Exception ex)
        {
            await ErrorPopupService.ShowErrorAsync(ex.Message);
        }
    }

    private async void CityEntry_Focused(object sender, FocusEventArgs e)
    {
        try
        {
            var map = MapControl.Map;
            if (map is null)
                return;

            var existing = map.Layers.FirstOrDefault(l => l.Name == "Pins");
            if (existing is not null)
            {
                map.Layers.Remove(existing);
                MapControl.Refresh();
            }
        }
        catch (Exception ex)
        {
            await ErrorPopupService.ShowErrorAsync(ex.Message);
        }
    }

    private async void MapControl_MapTapped(object sender, MapEventArgs e)
    {
        try
        {
            if (BindingContext is SearchViewModel vm)
            {
                var (lon, lat) = SphericalMercator.ToLonLat(e.WorldPosition.X, e.WorldPosition.Y);

                var loc = new CityLocation { Latitude = lat, Longitude = lon };

                var hasLocation = await vm.HandleMapTapAsync(loc);
                if (hasLocation)
                {
                    AddPinAtLocation(lat, lon);
                }
            }
        }
        catch (Exception ex)
        {
            await ErrorPopupService.ShowErrorAsync(ex.Message);
        }
    }

    private async void AddPinAtLocation(double latitude, double longitude)
    {
        try
        {

            var map = MapControl.Map;
            if (map is null)
                return;

            var (lon, lat) = SphericalMercator.FromLonLat(longitude, latitude);

            // remove previous pin layer if exists
            var existing = map.Layers.FirstOrDefault(l => l.Name == "Pins");
            if (existing is not null)
            {
                map.Layers.Remove(existing);
            }

            var pinLayer = new MemoryLayer
            {
                Name = "Pins",
                Features = new[]
                {
                    new PointFeature(lon, lat)
                    {
                        Styles =
                        {
                            new SymbolStyle
                            {
                                SymbolScale = 0.2,
                                Fill = new Mapsui.Styles.Brush { Color = Mapsui.Styles.Color.Red }
                            }
                        }
                    }
                }
            };

            map.Layers.Add(pinLayer);
            MapControl.Refresh();
        }
        catch (Exception ex)
        {
            await ErrorPopupService.ShowErrorAsync(ex.Message);
        }
    }
}
