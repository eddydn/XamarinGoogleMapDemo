using Android.App;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using System;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.Runtime;
using Android.Content;
using Android.Graphics;

namespace XamarinGoogleMapDemo
{
    [Activity(Label = "XamarinGoogleMapDemo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity,IOnMapReadyCallback,ILocationListener, GoogleMap.IOnInfoWindowClickListener
    {
        GoogleMap map;
        Spinner spinner;
        LocationManager locationManager;
        String provider;
        
         public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;

            //Optional
            googleMap.UiSettings.ZoomControlsEnabled = true;
            googleMap.UiSettings.CompassEnabled = true;
            googleMap.MoveCamera(CameraUpdateFactory.ZoomIn());
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
             SetContentView (Resource.Layout.Main);
            spinner = FindViewById<Spinner>(Resource.Id.spinner);
            MapFragment mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);
          

            spinner.ItemSelected += Spinner_ItemSelected;


            locationManager = (LocationManager)GetSystemService(Context.LocationService);
            provider = locationManager.GetBestProvider(new Criteria(), false);

            Location location = locationManager.GetLastKnownLocation(provider);
            if (location == null)
                System.Diagnostics.Debug.WriteLine("No Location");
           
           
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            switch (e.Position)
            {
                case 0: //Hybird
                    map.MapType = GoogleMap.MapTypeHybrid;
                    break;
                case 1: //None
                    map.MapType = GoogleMap.MapTypeNone;
                    break;
                case 2: //Normal
                    map.MapType = GoogleMap.MapTypeNormal;
                    break;
                case 3: //Statellite
                    map.MapType = GoogleMap.MapTypeSatellite;
                    break;
                case 4: //Terrain
                    map.MapType = GoogleMap.MapTypeTerrain;
                    break;
                default:
                    map.MapType = GoogleMap.MapTypeNone;
                    break;
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            locationManager.RequestLocationUpdates(provider, 400, 1, this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            locationManager.RemoveUpdates(this);
        }

        public void OnLocationChanged(Location location)
        {
            Double lat, lng;
            lat = location.Latitude;
            lng = location.Longitude;

            MarkerOptions makerOptions = new MarkerOptions();
            makerOptions.SetPosition(new LatLng(lat, lng));
            makerOptions.SetTitle("My Position");
            makerOptions.SetSnippet("This is my device's position");
            map.AddMarker(makerOptions);
            map.SetOnInfoWindowClickListener(this); // Add event click on marker icon

            //Draw a circle on Maps
            //CircleOptions circleOptions = new CircleOptions();
            //circleOptions.InvokeCenter(new LatLng(lat, lng));
            //circleOptions.InvokeRadius(100000); // metters
            //circleOptions.InvokeStrokeColor(Color.Blue);
            //circleOptions.InvokeStrokeWidth(10);
            //circleOptions.InvokeFillColor(Color.Argb(80, 0, 0, 255));
            //map.AddCircle(circleOptions);


            //Draw a polygon on Map
            //PolygonOptions polygonOptions = new PolygonOptions();
            //polygonOptions.Add(new LatLng(lat, lng));
            //polygonOptions.Add(new LatLng(lat+0.1, lng));
            //polygonOptions.Add(new LatLng(lat + 0.1, lng + 0.2));
            //polygonOptions.Add(new LatLng(lat, lng+0.2));

            //polygonOptions.InvokeFillColor(Color.Argb(80, 0, 0, 255));
            //polygonOptions.InvokeStrokeColor(Color.Blue);
            //polygonOptions.InvokeStrokeWidth(10);
            //map.AddPolygon(polygonOptions);

            //Draw a polylines on Map
            //PolylineOptions polylineOptions = new PolylineOptions();
            //polylineOptions.Add(new LatLng(lat, lng));
            //polylineOptions.Add(new LatLng(lat, lng + 1));
            //map.AddPolyline(polylineOptions);



            //Move Camera
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(new LatLng(lat, lng));
            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            map.MoveCamera(cameraUpdate);

        }

        public void OnProviderDisabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            //throw new NotImplementedException();
        }

        public void OnInfoWindowClick(Marker marker)
        {
            Toast.MakeText(this, $"Icon {marker.Title} is clicked", ToastLength.Short).Show();
        }
    }
}

