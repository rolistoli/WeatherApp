using Mapsui;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling;
using WeatherApp.Models;
using WeatherApp.ViewModels;

namespace WeatherApp.Views;

public partial class SearchPage : ContentPage
{
    private LocationPopup? popup;

    public SearchPage(SearchViewModel viewModel, LocationPopupViewModel popupViewModel)
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

        MapControl.Map.Widgets.Clear();

        // create popup using the dedicated popup view model and add it to the visual tree
        popup = new LocationPopup(popupViewModel) { IsVisible = false };

        if (Content is Layout layout)
        {
            layout.Children.Add(popup);
        }
    }

    private void MapControl_Loaded(object sender, EventArgs e)  
    {
        var lisbon = SphericalMercator.FromLonLat(-9.1393, 38.7223);

        MapControl.Map.Navigator.CenterOn(lisbon.x, lisbon.y);
        MapControl.Map.Navigator.ZoomTo(30);

        MapControl.Map.Navigator.OverrideZoomBounds = new MMinMax(50,300);
        MapControl.Map.Navigator.RotationLock = true;

    }

    private async void MapControl_MapTapped(object sender, MapEventArgs e)
    {
        if (MapControl.Map is null) return;

        var (lon, lat) = SphericalMercator.ToLonLat(
        e.WorldPosition.X,
        e.WorldPosition.Y);

        if (BindingContext is SearchViewModel vm)
        {
            //vm.SelectedLocation = new CityLocation()
            //{
            //    Latitude = lat,
            //    Longitude = lon
            //};

            // show small popup at bottom with options
            var loc = new CityLocation { Latitude = lat, Longitude = lon };
            if (popup is not null)
            {
                await popup.BindLocationAsync(loc);
                popup.IsVisible = true;
            }
            AddPinAtLocation(lat, lon);
        }
    }

    private void AddPinAtLocation(double latitude, double longitude)
    {
        try
        {
            var map = MapControl.Map;
            if (map is null) return;

            var (lon,lat ) = SphericalMercator.FromLonLat(longitude, latitude);

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
