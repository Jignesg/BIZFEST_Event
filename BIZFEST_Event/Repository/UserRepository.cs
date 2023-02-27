using BIZFEST_Event.DataAccess;
using BIZFEST_Event.Interfaces;
using BIZFEST_Event.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using ZXing.QrCode.Internal;
using static QRCoder.PayloadGenerator;
using QRCode = QRCoder.QRCode;


namespace BIZFEST_Event.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _HostEnvironment;


        public UserRepository(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db=db;
            _HostEnvironment=hostEnvironment;
        }
        public IEnumerable<UsersRegistration> GetAllUser()
        {
            var response = _db.UserRegistration.ToList();
            return response;
        }
        public Task<int> CreateUser(UsersRegistration User)
        {

            #region QR Code
            string Path = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(User.ContactNo.ToString(), QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
                {
                    qrCodeImage.Save(ms, ImageFormat.Png);
                    //Path = "Image;base64" + Convert.ToBase64String(ms.ToArray());
                    Path = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray()));
                }
            }
            #endregion

            User.BrCodeURL = Path;
            _db.UserRegistration.Add(User);
            _db.SaveChanges();

            #region Send SMS
            string url = "http://sms.mobileadz.in/api/push.json?apikey=5bdc20250a594&sender=BNISRT&mobileno=8160719854&text=%3Ctext%3E";
            StreamWriter writer = null;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "POST";
            #endregion
            return Task.FromResult(0);
        }



        //public async Task<int> SoftDeleteUser(int Id)
        //{
        //    var objUser = new UsersRegistration();
        //    objUser = _db.UserRegistration.Where(x => x.Id == Id).First();

        //    _db.UserRegistration.Remove(objUser);
        //    _db.SaveChanges();
        //    return (0);
        //    //using (var connection = _DB.CreateConnection())
        //    //{
        //    //  var response=  await connection.ExecuteScalarAsync<int>(@"UPDATE UserEvent SET IsDelete =1 Where Id = @Id", new {Id = Id});
        //    //}
        //}
    }
}
