using Mapsui;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling;
using WeatherApp.Models;
using WeatherApp.Services.Interfaces;
using WeatherApp.ViewModels;

namespace WeatherApp.Views;

public partial class SearchPage : ContentPage
{
    private readonly INavigationService navigationService;

    public SearchPage(SearchViewModel viewModel, INavigationService navigationService)
    {
        InitializeComponent();
        BindingContext = viewModel;
        this.navigationService = navigationService;

        MapControl.Map = new Mapsui.Map
        {
            Layers =
            {
                OpenStreetMap.CreateTileLayer()
            }
        };

        MapControl.Map.Widgets.Clear();
    }

    private void MapControl_Loaded(object sender, EventArgs e)
    {
        var lisbon = SphericalMercator.FromLonLat(-9.1393, 38.7223);

        MapControl.Map.Navigator.CenterOn(lisbon.x, lisbon.y);
        MapControl.Map.Navigator.ZoomTo(50);

        MapControl.Map.Navigator.OverrideZoomBounds = new MMinMax(50, 2000);
        MapControl.Map.Navigator.RotationLock = true;
    }

    private void CityEntry_Focused(object sender, FocusEventArgs e)
    {
        // clear any existing pins when the search entry gains focus
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
        catch
        {
            // ignore
        }
    }

    private async void MapControl_MapTapped(object sender, MapEventArgs e)
    {
        if (MapControl.Map is null)
            return;

        var (lon, lat) = SphericalMercator.ToLonLat(
            e.WorldPosition.X,
            e.WorldPosition.Y);

        if (BindingContext is SearchViewModel vm)
        {
            var loc = new CityLocation { Latitude = lat, Longitude = lon };

            // clear search entry and results
            vm.CityName = string.Empty;
            vm.HasResults = false;
            vm.Results.Clear();

            // mark navigating state so UI shows activity indicator
            vm.IsNavigating = true;

            // draw pin first
            AddPinAtLocation(lat, lon);

            // allow UI to render the pin before heavy work
            await Task.Yield();

            // show popup via navigation service (modal) using DI
            await navigationService.ShowLocationPopupAsync(loc);

            vm.IsNavigating = false;
        }
    }

    private void AddPinAtLocation(double latitude, double longitude)
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
        catch
        {
            // ignore mapping errors
        }
    }
}
