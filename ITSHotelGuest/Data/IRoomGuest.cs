using System.Threading.Tasks;

namespace ITSHotelGuest.Models
{
    public interface IRoomGuest
    {
        Task<bool> CheckRoomGuest(RoomGuestModel roomGuestModel);
    }
}
