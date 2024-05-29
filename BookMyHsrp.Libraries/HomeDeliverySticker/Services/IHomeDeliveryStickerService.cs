using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookMyHsrp.Libraries.Sticker.Models.StickerModel;

namespace BookMyHsrp.Libraries.HomeDeliverySticker.Services
{
    public interface IHomeDeliveryStickerService 
    {
        Task<dynamic> IsHomeDeliveryAllowed(int stateId, string CheckFor);
        Task<dynamic> CheckPinCode(dynamic sessionValue, string pincode);
        Task<ResponseDto> UpdateAvailibility(dynamic updateAvalibility, dynamic session, dynamic userdetails);
    }
}
