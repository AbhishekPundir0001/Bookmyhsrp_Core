using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.HsrpWithColorSticker.Services
{
    public interface IHsrpWithColorStickerService
    {
        Task<dynamic> VahanInformation(dynamic requestDto);
        Task<dynamic> SessionBookingDetails(dynamic requestDto);

    }
}
