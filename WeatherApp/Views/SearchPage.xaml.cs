using Mapsui;
using Mapsui.Projections;
using Mapsui.Tiling;
using WeatherApp.Models;
using WeatherApp.ViewModels;

namespace WeatherApp.Views;

public partial class SearchPage : ContentPage
{
    public SearchPage(SearchViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        MapControl.Map = new Mapsui.Map
        {
            Layers =
            {
                OpenStreetMap.CreateTileLayer()
            }
        };
    }

    private void MapControl_Loaded(object sender, EventArgs e)  
    {
        MapControl.Map.Widgets.Clear();

        var lisbon = SphericalMercator.FromLonLat(-9.1393, 38.7223);

        MapControl.Map.Navigator.CenterOn(lisbon.x, lisbon.y);
        MapControl.Map.Navigator.ZoomTo(5);

        MapControl.Map.Navigator.OverrideZoomBounds = new MMinMax(5, 10);
        MapControl.Map.Navigator.RotationLock = true;

    }

    private void MapControl_MapTapped(object sender, MapEventArgs e)
    {
        if (MapControl.Map is null) return;

        var (lon, lat) = SphericalMercator.ToLonLat(
        e.WorldPosition.X,
        e.WorldPosition.Y);

        if (BindingContext is SearchViewModel vm)
        {
            vm.SelectedLocation = new CityLocation()
            {
                Latitude = lat,
                Longitude = lon
            };
        }
    }
}
