using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransportWebApi.Data;
using System.Diagnostics;
using TransportWebApi.Model;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;

namespace TransportWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationManage : ControllerBase
    {
        private readonly TransPortDbContext _dbContext;
        public LocationManage(TransPortDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        [Route("GetLocation")]
        public async Task<dynamic> GetLocation(int locationId,int demandAmount)
        {
            if (locationId > 0)
            {
                Dictionary<int,int> distance = new Dictionary<int,int>();
                string summary = "";
                string traveldist = "";
                var orderlocation = _dbContext.Location.Where(x => x.locationid == locationId).ToList();
                var locationIdes=_dbContext.Location.Where(x=>(x.locationtype==1 || x.locationtype==3) && (x.availamount>=demandAmount) && (x.locationid!=locationId)).ToList();
                if (locationIdes.Count>0)
                {
                    var temp=100000000;
                    int id = 0;
                    List<dynamic> steps = new List<dynamic>();
                    JObject tempdata = new JObject();
                    foreach(var item in locationIdes)
                    {
                        var client = new HttpClient();
                        var request = new HttpRequestMessage(HttpMethod.Get, "https://maps.googleapis.com/maps/api/directions/json?origin=" + item.coordinates + "&destination=" + orderlocation[0].coordinates +"&key=AIzaSyD7KtQoq29-5TqELLdPBSQoqCD376-qGjA");
                        request.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiUGF3YW4gS3VtYXIiLCJQYXNzd29yZCI6IjEyMzQ1IiwiZXhwIjoxNzAyNjI3NjAwLCJpc3MiOiJQYXdhbiBLdW1hciIsImF1ZCI6IlJlYWRlciJ9.Wp4nvRUJexx_0k8_ZT-rzfTJByxD5xjH2XRG4lh9Ekw");
                        var response = await client.SendAsync(request);
                        response.EnsureSuccessStatusCode();
                        JObject data= JObject.Parse(await response.Content.ReadAsStringAsync());
                        traveldist = Convert.ToString(data["routes"][0]["legs"][0]["distance"]["text"]);
                        int value = Convert.ToInt32(data["routes"][0]["legs"][0]["distance"]["value"]);
                        if (temp> value)
                        {
                            temp = value;
                            id = item.locationid;
                            tempdata = data;
                            
                            //var obj = new { route= Convert.ToString(data["routes"][0]["summary"]), };
                        }
                    }
                    summary = Convert.ToString(tempdata["routes"][0]["summary"]);
                    int length = Convert.ToInt32(tempdata["routes"][0]["legs"][0]["steps"].Count());                    
                    for (int i = (int)length / 3; i < length; i = i + (int)length / 3)
                    {
                        steps.Add(new { cordinate = tempdata["routes"][0]["legs"][0]["steps"][i]["end_location"]["lat"].ToString()+","+tempdata["routes"][0]["legs"][0]["steps"][i]["end_location"]["lng"].ToString() });
                        _dbContext.CheckPoints.Add(new CheckPoints() { checkpointcoordinates= tempdata["routes"][0]["legs"][0]["steps"][i]["end_location"]["lat"].ToString() + "," + tempdata["routes"][0]["legs"][0]["steps"][i]["end_location"]["lng"].ToString() });
                        _dbContext.SaveChanges();
                    }
                    distance.Add(id,temp);
                    var lastdata = new {route=summary, steps=steps, TravelDistance=traveldist};
                    
                    
                    return lastdata;
                }
            }
            return "please enter location id";

        }
    }
}
