using BIZFEST_Event.DataAccess;
using BIZFEST_Event.Interfaces;
using BIZFEST_Event.Models;
using Microsoft.Extensions.Logging;
using QRCoder;
using System.Drawing.Imaging;
using System.Drawing;

namespace BIZFEST_Event.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _db;
        // private readonly IDbRepository _DB;

        public EventRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task<int> CreateEvent(UserEvent Event)
        {
            #region QR Code
            string Path = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(Event.EventName.ToString(), QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
                {
                    qrCodeImage.Save(ms, ImageFormat.Png);
                    //Path = "Image;base64" + Convert.ToBase64String(ms.ToArray());
                     Path = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray()));
                }
            }
            #endregion
            Event.BrCodeURL = Path;

            _db.UserEvent.Add(Event);
            _db.SaveChanges();
            return Task.FromResult(0);
        }

        public IEnumerable<UserEvent> GetAllEvents()
        {
            List<UserEvent> response = _db.UserEvent.Where(x => x.IsDeleted== false).ToList();
            return response;
        }

        public async Task<int> SoftDeleteEvent(int Id)
        {
            var objEvent = new UserEvent();
            objEvent = _db.UserEvent.Where(x => x.Id == Id).First();
            objEvent.IsDeleted = true;
            _db.UserEvent.Update(objEvent);
            _db.SaveChanges();
            return (0);
            //using (var connection = _DB.CreateConnection())
            //{
            //  var response=  await connection.ExecuteScalarAsync<int>(@"UPDATE UserEvent SET IsDelete =1 Where Id = @Id", new {Id = Id});
            //}
        }
    }


}

