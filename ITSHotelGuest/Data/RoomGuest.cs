using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace ITSHotelGuest.Models
{
    public class RoomGuest : IRoomGuest
    {
        private readonly string _connectionString;

        public RoomGuest(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> CheckRoomGuest(RoomGuestModel roomGuestModel)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result = await connection
                    .QueryFirstOrDefaultAsync<int>("SELECT COUNT(*) " +
                                                    " FROM tbRoomGuest " +
                                                    " WHERE Token = @Token " +
                                                    "   AND RoomNumber = @RoomNumber "
                                                    , roomGuestModel);
                return result >= 1 ? true : false;
            }
            return false;
        }
    }
}
