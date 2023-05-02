using Microsoft.AspNetCore.Mvc;
using NetCoreActionResults.Models;

namespace NetCoreActionResults.Views.Shared.Components.Test
{
    //Kural 1: View componentler viewcomponent'den kalıtım alır.
    //Kural 2: View component içerisinde sadece 1 method bulunur ve adı InvokeAsync olarak tanımlanır.
    //Kural 3: View componentler performans amaçlı async task formatında tanımlanır. Asenkrondur.
    public class TestViewComponent : ViewComponent
    {
        //Title değeri göndererek ya da title parametresi boş geçilerek çağırabiliriz.
        public async Task<IViewComponentResult> InvokeAsync(string? title)
        {
            //İlgili db işlemleri vs yapılıp veri controller yerine bu methos içerisinde çekilerek view'e model olarak gönderilir.
            //Yani view'in ihtiyaç duyduğu model bu class olarak controller actiondan izole bir şekilde tanımlanır.
            //Çift soru işareti kendinden önce gelen değerin null olup olmamasını kontrol eder. Null ise ViewComponent string(model.Title'ın içindeki default tanımlı değer) döner null değilse title parametresinin değeri döner.
            var model = new ViewComponentViewModel();
            model.Title = title ?? model.Title;

            return View(model);
        }
    }
}
