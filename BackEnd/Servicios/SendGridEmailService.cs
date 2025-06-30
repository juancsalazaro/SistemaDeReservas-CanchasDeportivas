// Asegúrate de que el namespace refleje la nueva ubicación
namespace SistemaReservasApi.Servicios
{
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using Microsoft.Extensions.Configuration;
    using System.Threading.Tasks;
    using System;

    public class SendGridEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public SendGridEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendReservationConfirmationEmailAsync(string userEmail, string userName, string canchaName, DateTime reservationDate)
        {
            var apiKey = _configuration["SendGridApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("La API Key de SendGrid no está configurada.");
            }

            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("ecohotelmadeguadua@gmail.com", "Canchas 5 Stars");
            var to = new EmailAddress(userEmail, userName);

            var subject = $"Confirmación de tu reserva: {canchaName}";

            var plainTextContent = $"Hola {userName}, tu reserva para la cancha {canchaName} el día {reservationDate.ToShortDateString()} a las {reservationDate.ToShortTimeString()} ha sido confirmada.";

            var htmlContent = $@"
                <html>
                <body>
                    <h2>¡Reserva Confirmada!</h2>
                    <p>Hola <strong>{userName}</strong>,</p>
                    <p>Tu reserva para la cancha <strong>{canchaName}</strong> ha sido confirmada con éxito.</p>
                    <ul>
                        <li><strong>Fecha:</strong> {reservationDate.ToLongDateString()}</li>
                        <li><strong>Hora:</strong> {reservationDate.ToShortTimeString()}</li>
                    </ul>
                    <p>¡Gracias por usar nuestro servicio!</p>
                </body>
                </html>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);
        }
    }
}
