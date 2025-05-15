using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace _2025._04._03._cukrászda
{
    public class ServerConnection
    {
        private HttpClient client = new HttpClient();
        private string baseURL = "";
        public ServerConnection(string url)
        {
            baseURL = url;
        }
        public async Task<List<Cake>> GetCakes()          // a Task paramétere egy lista, ami Cake típusú
        {
            List<Cake> all = new List<Cake>();
            string url = baseURL + "/cake";
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();     // ha van lekezeletlen hibánk, akkor futás leáll és kiírja, hogy mi a hiba, mert van a catch-be egy message kiírásunk.
                string responseInString = await response.Content.ReadAsStringAsync();   // a responseban van egy Contentünk és azt szeretnénk stringben eltárolni. Ezt csinálja a ReadAsStringAsync() függvény
                all = JsonConvert.DeserializeObject<List<Cake>>(responseInString);      // a DeserializeObject parancs karaktersorból csinál adatszerkezetet 
                                    // serializ-álás SerializeObject() -> karaktersort csinál a json adatszerkezetből
                                    // miért jó? -> mert csak ilyen formában tudom a servernek küldeni az adatot
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return all;
        }
        public async Task<bool> PostCake(Cake oneCake)
        {
            string url = baseURL + "/cake";

            try
            {
                var jsonData = new
                {
                    newCakeName = oneCake.name,
                    newPicture = oneCake.picture,
                    newDescription = oneCake.description,
                    newAllergenes = oneCake.allergenes,
                    newPrice = oneCake.price,
                    newStock = oneCake.stock
                };
                string jsonString = JsonConvert.SerializeObject(jsonData);
                StringContent sendThis = new StringContent(jsonString, Encoding.UTF8, "Application/JSON");  // soha nem a jsonStringet küldjük, mert még adok neki plusz két információt.
                HttpResponseMessage response = await client.PostAsync(url, sendThis);
                if(response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    Message errorCake = JsonConvert.DeserializeObject<Message>(errorMessage);
                    MessageBox.Show(errorCake.message, "Hiba");
                    return false;
                }
                response.EnsureSuccessStatusCode();     // ha van lekezeletlen hibánk, akkor futás leáll és kiírja, hogy mi a hiba, mert van a catch-be egy message kiírásunk.
                string responseString = await response.Content.ReadAsStringAsync();
                Message successCake = JsonConvert.DeserializeObject<Message>(responseString);
                MessageBox.Show(successCake.message, "Siker :)");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

            return true;
        }
        public async Task<bool> BuyCake(Cake oneCake)
        {
            string url = baseURL + "/buyCake";

            try
            {
                var jsonData = new
                {
                    id = oneCake.id,
                    cakeCount = oneCake.orderCount
                };
                string jsonString = JsonConvert.SerializeObject(jsonData);
                StringContent sendThis = new StringContent(jsonString, Encoding.UTF8, "Application/JSON");  // soha nem a jsonStringet küldjük, mert még adok neki plusz két információt.
                HttpResponseMessage response = await client.PostAsync(url, sendThis);
                response.EnsureSuccessStatusCode();     // ha van lekezeletlen hibánk, akkor futás leáll és kiírja, hogy mi a hiba, mert van a catch-be egy message kiírásunk.
                string responseString = await response.Content.ReadAsStringAsync();
                Message successCake = JsonConvert.DeserializeObject<Message>(responseString);
                MessageBox.Show(successCake.message, "Siker :)");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

            return true;
        }
        public async Task<bool> BuyCakes()
        {
            string url = baseURL + "/buyCakes";

            try
            {
                var jsonData = new
                {
                    cakes = Cart.cart.Select(cake => new {id = cake.id, cakeCount = cake.orderCount})
                };
                string jsonString = JsonConvert.SerializeObject(jsonData);
                StringContent sendThis = new StringContent(jsonString, Encoding.UTF8, "Application/JSON");  // soha nem a jsonStringet küldjük, mert még adok neki plusz két információt.
                HttpResponseMessage response = await client.PostAsync(url, sendThis);
                response.EnsureSuccessStatusCode();     // ha van lekezeletlen hibánk, akkor futás leáll és kiírja, hogy mi a hiba, mert van a catch-be egy message kiírásunk.
                string responseString = await response.Content.ReadAsStringAsync();
                Message successCake = JsonConvert.DeserializeObject<Message>(responseString);
                MessageBox.Show(successCake.message, "Siker :)");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

            return true;
        }
        public async Task<bool> RestockCake(Cake oneCake)
        {
            string url = baseURL + "/restockCake";

            try
            {
                var jsonData = new
                {
                    id = oneCake.id,
                    cakeCount = oneCake.orderCount
                };
                string jsonString = JsonConvert.SerializeObject(jsonData);
                StringContent sendThis = new StringContent(jsonString, Encoding.UTF8, "Application/JSON");  // soha nem a jsonStringet küldjük, mert még adok neki plusz két információt.
                HttpResponseMessage response = await client.PostAsync(url, sendThis);
                response.EnsureSuccessStatusCode();     // ha van lekezeletlen hibánk, akkor futás leáll és kiírja, hogy mi a hiba, mert van a catch-be egy message kiírásunk.
                string responseString = await response.Content.ReadAsStringAsync();
                Message successCake = JsonConvert.DeserializeObject<Message>(responseString);
                MessageBox.Show(successCake.message, "Siker :)");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

            return true;
        }
        public async Task<bool> RestockCakes()
        {
            string url = baseURL + "/restockCakes";

            try
            {
                var jsonData = new
                {
                    cakes = Cart.delivery.Select(cake => new { id = cake.id, cakeCount = cake.orderCount })
                };
                string jsonString = JsonConvert.SerializeObject(jsonData);
                StringContent sendThis = new StringContent(jsonString, Encoding.UTF8, "Application/JSON");  // soha nem a jsonStringet küldjük, mert még adok neki plusz két információt.
                HttpResponseMessage response = await client.PostAsync(url, sendThis);
                response.EnsureSuccessStatusCode();     // ha van lekezeletlen hibánk, akkor futás leáll és kiírja, hogy mi a hiba, mert van a catch-be egy message kiírásunk.
                string responseString = await response.Content.ReadAsStringAsync();
                Message successCake = JsonConvert.DeserializeObject<Message>(responseString);
                MessageBox.Show(successCake.message, "Siker :)");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

            return true;
        }
    }
}
