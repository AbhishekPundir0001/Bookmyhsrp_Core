using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;

namespace BookMyHsrp.Libraries.HsrpWithColorSticker.Services
{
    public interface IHsrpWithColorStickerService
    {
        Task<dynamic> VahanInformation(VahanDetailsDto requestDto);
        Task<dynamic> SessionBookingDetails(dynamic requestDto);

    }
}
