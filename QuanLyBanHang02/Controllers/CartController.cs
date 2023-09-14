using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using QLNHoa.Models;
using QuanLyBanHang02.Models;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuanLyBanHang02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        QLNHoaContext da = new QLNHoaContext();



        // GET: api/<CartController>
        [HttpGet("Get list carts")]
        public Response GetListCart()
        {
            List<CartModel> carts = new List<CartModel>();
            SqlConnection connection = new SqlConnection();
            SqlDataAdapter da = new SqlDataAdapter("Select * From Orders;", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CartModel perfumes = new CartModel();
                    perfumes.MaNh = Convert.ToInt32(dt.Rows[i]["MaNH"]);
                    perfumes.TenNh = Convert.ToString(dt.Rows[i]["TenNH"]);
                    perfumes.DonGia = Convert.ToInt32(dt.Rows[i]["DonGia"]);
                    carts.Add(perfumes);
                }
                if (carts.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Data found";
                    response.ListProducts = carts;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No data found";
                    response.ListProducts = null;
                }
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.ListProducts = null;
            }

            return response;
        }


        [HttpPost("Add cart")]
        public Response AddCart(CartModel cartModel)
        {
            Response response = new Response();
            if (cartModel.MaNh > 0)
            {
                SqlCommand cmd = new SqlCommand("Insert into Cart(MaNh) values('" + cartModel.MaNh + "')");
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Item added";
                }
                else
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Item added";
                }
            }
            else
            {
                response.StatusCode = 200;
                response.StatusMessage = "No item found";
            }
            return response;
        }



        //// PUT api/<CartController>/5
        //[HttpPut("Edit a Cart")]
        //public void EditPerfume([FromBody] NuocHoa nuochoa)
        //{
        //    using (var tran = da.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            Perfume p = da.Perfumes.FirstOrDefault(s => s.MaNh == nuochoa.MaNh);
        //            p.MaNh = nuochoa.MaNh;
        //            p.TenNh = nuochoa.TenNh;
        //            p.TheTich = nuochoa.TheTich;
        //            p.Huong = nuochoa.Huong;
        //            p.DonGia = nuochoa.DonGia;
        //            da.Perfumes.Update(p);
        //            da.SaveChanges();
        //            tran.Commit();
        //        }
        //        catch (Exception)
        //        {
        //            tran.Rollback();

        //            throw;
        //        }
        //    }
        //}

        //// DELETE api/<CartController>/5
        //[HttpDelete("Delete a Perfume")]
        //public void DeletePerfume(int id)
        //{
        //    try
        //    {
        //        Perfume p = da.Perfumes.FirstOrDefault(s => s.MaNh == id);
        //        da.Perfumes.Remove(p);
        //        da.SaveChanges();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}


        [HttpGet("{Momo Payment}")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{Momo Payment}")]
        private ActionResult View()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{Payment}")]
        public IActionResult Payment()
        {
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = "test";
            string returnUrl = "https://localhost:44394/Home/ConfirmPaymentClient";
            string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = "1000";
            string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };
            string responseFromMoMo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
            JObject jmessage = JObject.Parse(responseFromMoMo);
            return Redirect(jmessage.GetValue("payUrl").ToString());
        }
        [HttpGet("{Result}")]
        public ActionResult ConfirmPaymentClient(Result result)
        {
            //lấy kết quả Momo trả về và hiển thị thông báo cho người dùng (có thể lấy dữ liệu ở đây cập nhật xuống db)
            string rMessage = result.Message;
            string rOrderId = result.OrderId;
            string rErrorCode = result.ErrorCode; // = 0: thanh toán thành công
            return View();
        }

        [HttpPost]
        public void SavePayment()
        {
            //cập nhật dữ liệu vào db
            String a = "";
        }
    }
}
