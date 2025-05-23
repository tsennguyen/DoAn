using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shopping_Laptop.Models;
using Shopping_Laptop.Models.ViewModels;
using Shopping_Laptop.Repository;
using System.Transactions;

namespace Shopping_Laptop.Controllers
{
    public class CartController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<CartController> _logger;

        public CartController(DataContext context, ILogger<CartController> logger)
        {
            _dataContext = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            try
            {
                List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
                var shippingPriceCookie = Request.Cookies["ShippingPrice"];
                decimal shippingPrice = 0;
                var coupon_code = Request.Cookies["CouponTitle"];

                if (!string.IsNullOrEmpty(shippingPriceCookie))
                {
                    shippingPrice = JsonConvert.DeserializeObject<decimal>(shippingPriceCookie ?? "0");
                }

                CartItemViewModel cartVM = new()
                {
                    CartItems = cartItems,
                    GrandTotal = cartItems.Sum(x => x.Price * x.Quantity),
                    ShippingCost = shippingPrice,
                    CouponCode = coupon_code ?? string.Empty
                };

                return View(cartVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading cart");
                TempData["Error"] = "Có lỗi xảy ra khi tải giỏ hàng. Vui lòng thử lại sau.";
                return View(new CartItemViewModel());
            }
        }

        public IActionResult Checkout()
        {
            return View("~/Views/Checkout/Index.cshtml");
        }

        public async Task<IActionResult> Add(int id, int quantity = 1)
        {
            try
            {
                var product = await _dataContext.Products
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    _logger.LogWarning("Attempted to add non-existent product ID {ProductId} to cart", id);
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm." });
                }

                quantity = Math.Max(1, quantity);

                if (product.Quantity < quantity)
                {
                    _logger.LogInformation("Insufficient stock for product {ProductId}. Requested: {Quantity}, Available: {Available}", 
                        id, quantity, product.Quantity);
                    return Json(new { success = false, message = $"Không đủ số lượng tồn kho cho sản phẩm {product.Name}. Chỉ còn {product.Quantity} sản phẩm." });
                }

                var cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
                var cartItem = cart.FirstOrDefault(c => c.ProductId == id);

                if (cartItem == null)
                {
                    cart.Add(new CartItemModel(product) { Quantity = quantity });
                }
                else
                {
                    if (product.Quantity < cartItem.Quantity + quantity)
                    {
                        return Json(new { success = false, message = $"Không đủ số lượng tồn kho cho sản phẩm {product.Name}. Trong giỏ đã có {cartItem.Quantity}, chỉ còn {product.Quantity} tồn kho." });
                    }
                    cartItem.Quantity += quantity;
                }

                HttpContext.Session.SetJson("Cart", cart);
                _logger.LogInformation("Added product {ProductId} to cart. Quantity: {Quantity}", id, quantity);
                return Json(new { success = true, message = "Sản phẩm đã được thêm vào giỏ hàng." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding product {ProductId} to cart", id);
                return Json(new { success = false, message = "Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng. Vui lòng thử lại sau." });
            }
        }

        public IActionResult Decrease(int id)
        {
            try
            {
                var cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
                if (cart == null) return RedirectToAction("Index");

                var cartItem = cart.FirstOrDefault(c => c.ProductId == id);
                if (cartItem == null) return RedirectToAction("Index");

                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    _logger.LogInformation("Decreased quantity for product {ProductId} in cart", id);
                }
                else
                {
                    cart.RemoveAll(p => p.ProductId == id);
                    _logger.LogInformation("Removed product {ProductId} from cart due to quantity decrease to 0", id);
                }

                if (cart.Count == 0)
                {
                    HttpContext.Session.Remove("Cart");
                }
                else
                {
                    HttpContext.Session.SetJson("Cart", cart);
                }

                TempData["Success"] = "Đã giảm số lượng sản phẩm trong giỏ hàng.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while decreasing quantity for product {ProductId}", id);
                TempData["Error"] = "Có lỗi xảy ra khi cập nhật giỏ hàng. Vui lòng thử lại sau.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Increase(int id)
        {
            try
            {
                var product = await _dataContext.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product == null)
                {
                    TempData["Error"] = "Không tìm thấy sản phẩm.";
                    return RedirectToAction("Index");
                }

                var cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
                if (cart == null) return RedirectToAction("Index");

                var cartItem = cart.FirstOrDefault(c => c.ProductId == id);
                if (cartItem == null) return RedirectToAction("Index");

                if (cartItem.Quantity >= 1 && product.Quantity > cartItem.Quantity)
                {
                    cartItem.Quantity++;
                    _logger.LogInformation("Increased quantity for product {ProductId} in cart", id);
                    TempData["Success"] = "Đã tăng số lượng sản phẩm thành công.";
                }
                else
                {
                    cartItem.Quantity = product.Quantity;
                    _logger.LogWarning("Attempted to increase quantity beyond stock for product {ProductId}", id);
                    TempData["Success"] = "Đã đạt số lượng tối đa có thể thêm vào giỏ hàng.";
                }

                HttpContext.Session.SetJson("Cart", cart);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while increasing quantity for product {ProductId}", id);
                TempData["Error"] = "Có lỗi xảy ra khi cập nhật giỏ hàng. Vui lòng thử lại sau.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Remove(int id)
        {
            try
            {
                var cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
                if (cart == null) return RedirectToAction("Index");

                cart.RemoveAll(p => p.ProductId == id);
                _logger.LogInformation("Removed product {ProductId} from cart", id);

                if (cart.Count == 0)
                {
                    HttpContext.Session.Remove("Cart");
                }
                else
                {
                    HttpContext.Session.SetJson("Cart", cart);
                }

                TempData["Success"] = "Đã xóa sản phẩm khỏi giỏ hàng.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while removing product {ProductId} from cart", id);
                TempData["Error"] = "Có lỗi xảy ra khi xóa sản phẩm khỏi giỏ hàng. Vui lòng thử lại sau.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Clear()
        {
            try
            {
                HttpContext.Session.Remove("Cart");
                _logger.LogInformation("Cart cleared");
                TempData["Success"] = "Đã xóa toàn bộ sản phẩm trong giỏ hàng.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while clearing cart");
                TempData["Error"] = "Có lỗi xảy ra khi xóa giỏ hàng. Vui lòng thử lại sau.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Route("Cart/GetShipping")]
        public async Task<IActionResult> GetShipping(ShippingModel shippingModel, string quan, string tinh, string phuong)
        {
            try
            {
                if (string.IsNullOrEmpty(tinh) || string.IsNullOrEmpty(quan) || string.IsNullOrEmpty(phuong))
                {
                    return Json(new { error = "Vui lòng chọn đầy đủ thông tin địa chỉ." });
                }

                var existingShipping = await _dataContext.Shippings
                    .FirstOrDefaultAsync(x => x.City == tinh && x.District == quan && x.Ward == phuong);

                decimal shippingPrice = existingShipping?.Price ?? 50000;
                var shippingPriceJson = JsonConvert.SerializeObject(shippingPrice);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                };

                Response.Cookies.Append("ShippingPrice", shippingPriceJson, cookieOptions);
                _logger.LogInformation("Shipping price set for {City}, {District}, {Ward}: {Price}", tinh, quan, phuong, shippingPrice);

                return Json(new { shippingPrice });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting shipping price for {City}, {District}, {Ward}", tinh, quan, phuong);
                return Json(new { error = "Có lỗi xảy ra khi tính phí vận chuyển. Vui lòng thử lại sau." });
            }
        }

        [HttpGet]
        [Route("Cart/RemoveShippingCookie")]
        public IActionResult DeleteShipping()
        {
            try
            {
                Response.Cookies.Delete("ShippingPrice");
                _logger.LogInformation("Shipping price cookie removed");
                return RedirectToAction("Index", "Cart");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while removing shipping price cookie");
                TempData["Error"] = "Có lỗi xảy ra khi xóa thông tin vận chuyển. Vui lòng thử lại sau.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Route("Cart/GetCoupon")]
        public async Task<IActionResult> GetCoupon(CouponModel couponModel, string coupon_value)
        {
            try
            {
                if (string.IsNullOrEmpty(coupon_value))
                {
                    return Json(new { error = "Vui lòng nhập mã giảm giá." });
                }

                var validCoupon = await _dataContext.Coupons
                    .FirstOrDefaultAsync(x => x.Name == coupon_value && x.Quantity >= 1);

                if (validCoupon == null)
                {
                    _logger.LogInformation("Invalid or depleted coupon attempted: {CouponCode}", coupon_value);
                    return Json(new { error = "Mã giảm giá không hợp lệ hoặc đã hết lượt sử dụng." });
                }

                string couponTitle = $"{validCoupon.Name} - {validCoupon.Description}";
                if (validCoupon.EndDate < DateTime.Now)
                {
                    _logger.LogInformation("Expired coupon attempted: {CouponCode}", coupon_value);
                    return Json(new { error = "Mã giảm giá đã hết hạn." });
                }

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                };

                Response.Cookies.Append("CouponTitle", couponTitle, cookieOptions);
                _logger.LogInformation("Coupon applied successfully: {CouponCode}", coupon_value);
                return Json(new { success = true, message = "Áp dụng mã giảm giá thành công." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while applying coupon {CouponCode}", coupon_value);
                return Json(new { error = "Có lỗi xảy ra khi áp dụng mã giảm giá. Vui lòng thử lại sau." });
            }
        }

        public async Task<IActionResult> UpdateCart(int? productId)
        {
            if (productId == null)
            {
                _logger.LogWarning("Attempted to update cart with null productId");
                return NotFound();
            }

            try
            {
                var product = await _dataContext.Products.FindAsync(productId);
                if (product == null)
                {
                    _logger.LogWarning("Attempted to update cart with non-existent product ID {ProductId}", productId);
                    return NotFound();
                }

                var cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
                var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);

                if (cartItem != null)
                {
                    if (cartItem.Quantity > product.Quantity)
                    {
                        cartItem.Quantity = product.Quantity;
                        _logger.LogInformation("Adjusted cart quantity to match available stock for product {ProductId}", productId);
                        TempData["Warning"] = "Số lượng sản phẩm đã được điều chỉnh theo số lượng tồn kho.";
                    }

                    HttpContext.Session.SetJson("Cart", cart);
                    _logger.LogInformation("Cart updated successfully for product {ProductId}", productId);
                    TempData["Success"] = "Giỏ hàng đã được cập nhật.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating cart for product {ProductId}", productId);
                TempData["Error"] = "Có lỗi xảy ra khi cập nhật giỏ hàng. Vui lòng thử lại sau.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}