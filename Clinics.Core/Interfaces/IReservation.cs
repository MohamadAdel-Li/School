using Clinics.Core.DTOs;
using Clinics.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.Interfaces
{
    public interface IReservation :IGenericRepository<Reservation>
    {
        Task<ReservationDTO> GetReservation(int id);
        Task<IEnumerable<ReservationDTO>> GetReservations();
        Task<PostReservationDTO> AddReservation(PostReservationDTO postReservationDTO);
    }
}
