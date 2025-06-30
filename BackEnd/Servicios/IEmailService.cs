namespace SistemaReservasApi.Servicios
{
    public interface IEmailService
    {
        Task SendReservationConfirmationEmailAsync(string userEmail, string userName, string canchaName, DateTime reservationDate);
       // Task SendReservationConfirmationEmailAsync(string emailCliente, string nombreCliente, string nombre, DateTime dateTime);
    }
}
