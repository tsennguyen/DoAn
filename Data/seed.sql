-- Enable IDENTITY_INSERT for Categories
SET IDENTITY_INSERT Categories ON;

-- Insert Categories
INSERT INTO Categories (Id, Name, Description, Slug, Status)
VALUES 
(1, N'Bong da', N'Dung cu va trang phuc bong da', 'bong-da', 1),
(2, N'Bong ro', N'Dung cu va trang phuc bong ro', 'bong-ro', 1),
(3, N'Cau long', N'Dung cu va trang phuc cau long', 'cau-long', 1),
(4, N'Giay bong da', N'Giay thi dau cho bong da', 'giay-bong-da', 1),
(5, N'Giay bong ro', N'Giay thi dau cho bong ro', 'giay-bong-ro', 1),
(6, N'Giay cau long', N'Giay thi dau cho cau long', 'giay-cau-long', 1),
(7, N'Bong thi dau', N'Cac loai bong thi dau', 'bong-thi-dau', 1),
(8, N'Trang phuc bong da', N'Quan ao the thao bong da', 'quan-ao-bong-da', 1),
(9, N'Trang phuc bong ro', N'Quan ao the thao bong ro', 'quan-ao-bong-ro', 1),
(10, N'Trang phuc cau long', N'Quan ao the thao cau long', 'quan-ao-cau-long', 1),
(11, N'Phu kien bong da', N'Phu kien nhu gang tay, tat bong da', 'phu-kien-bong-da', 1),
(12, N'Phu kien bong ro', N'Phu kien nhu bang co tay, bang tran bong ro', 'phu-kien-bong-ro', 1),
(13, N'Phu kien cau long', N'Phu kien nhu quan can vot, tui dung vot', 'phu-kien-cau-long', 1),
(14, N'Dung cu luyen tap', N'Dung cu tap luyen the thao', 'dung-cu-tap-luyen', 1),
(15, N'Combo the thao', N'Combo khuyen mai dung cu the thao', 'combo-the-thao', 1);

SET IDENTITY_INSERT Categories OFF;

-- Enable IDENTITY_INSERT for Brands
SET IDENTITY_INSERT Brands ON;

-- Insert Brands
INSERT INTO Brands (Id, Name, Description, Slug, Status, Logo)
VALUES
(1, N'Nike', N'Thuong hieu the thao noi tieng', 'nike', 1, 'nike.png'),
(2, N'Adidas', N'Trang phuc va giay the thao', 'adidas', 1, 'adidas.png'),
(3, N'Puma', N'Thoi trang the thao cao cap', 'puma', 1, 'puma.png'),
(4, N'Yonex', N'Thuong hieu cau long hang dau', 'yonex', 1, 'yonex.png'),
(5, N'Li-Ning', N'Thuong hieu the thao Trung Quoc', 'li-ning', 1, 'lining.png'),
(6, N'Molten', N'Bong thi dau chat luong cao', 'molten', 1, 'molten.png'),
(7, N'Spalding', N'Bong ro tieu chuan', 'spalding', 1, 'spalding.png'),
(8, N'Mitre', N'Thuong hieu bong da lau doi', 'mitre', 1, 'mitre.png'),
(9, N'Kawasaki', N'Phu kien va vot cau long', 'kawasaki', 1, 'kawasaki.png'),
(10, N'Under Armour', N'Thoi trang the thao My', 'under-armour', 1, 'ua.png'),
(11, N'Reebok', N'Trang phuc va giay the thao', 'reebok', 1, 'reebok.png'),
(12, N'Asics', N'Giay chay va the thao', 'asics', 1, 'asics.png'),
(13, N'New Balance', N'Thoi trang va giay the thao', 'new-balance', 1, 'nb.png'),
(14, N'Decathlon', N'He thong the thao tong hop', 'decathlon', 1, 'decathlon.png'),
(15, N'Mizuno', N'Dung cu the thao cao cap', 'mizuno', 1, 'mizuno.png');

SET IDENTITY_INSERT Brands OFF;

-- Enable IDENTITY_INSERT for Products
SET IDENTITY_INSERT Products ON;

-- Insert Products
INSERT INTO Products
(Id, Name, Slug, Description, Price, BrandId, CategoryId, Image, Quantity, Sold, OldPrice, CreatedDate, Discount, Rating, UpdatedDate)
VALUES
(1, N'Giay bong da Nike Mercurial', 'giay-bong-da-nike-mercurial', N'Giay thi dau chuyen nghiep cho cau thu bong da', 1800000, 1, 4, 'https://product.hstatic.net/1000061481/product/anh_sp_add_web_3-02-02-01-01-01-01-01-01-02_dd84f18cee484cb8b225246468f962e2_1024x1024.jpg', 100, 20, 2000000, GETDATE(), 10, 4.8, GETDATE()),
(2, N'Quan ao bong da Adidas', 'quan-ao-bong-da-adidas', N'Bo do thi dau thoang mat', 650000, 2, 8, 'https://www.sport9.vn/images/thumbs/002/0021475_bo-quan-ao-bong-da-doi-tuyen-quoc-gia-duc-mau-den-do.jpeg?preset=small', 200, 50, 750000, GETDATE(), 13, 4.5, GETDATE()),
(3, N'Bong da Molten F5V5000', 'bong-da-molten-f5v5000', N'Bong da dat tieu chuan thi dau', 450000, 6, 7, 'https://product.hstatic.net/200000278317/product/bong-da-molten-f5v5000-so-5-_-trang-xanh-den-tdzoj_1bd496c76d384b00bcd92d29ce5740b7_grande.jpg', 50, 10, 500000, GETDATE(), 10, 4.7, GETDATE()),
(4, N'Vot cau long Yonex Astrox 88D', 'vot-cau-long-yonex-astrox-88d', N'Vot danh cho nguoi choi nang cao', 3200000, 4, 3, 'https://shopvnb.com//uploads/san_pham/vot-cau-long-yonex-astrox-88d-pro-chinh-hang-1.webp', 30, 5, 3500000, GETDATE(), 8, 4.9, GETDATE()),
(5, N'Giay cau long Li-Ning Ranger', 'giay-cau-long-li-ning-ranger', N'Giay bam san tot cho thi dau cau long', 1250000, 5, 6, 'https://sneakerdaily.vn/wp-content/uploads/2023/10/Giay-Lining-cau-long-Ranger-6-AYAS014-2.jpg', 70, 15, 1400000, GETDATE(), 11, 4.6, GETDATE()),
(6, N'Ao bong ro Spalding Pro', 'ao-bong-ro-spalding', N'Trang phuc thi dau chuyen nghiep bong ro', 580000, 7, 9, 'https://down-vn.img.susercontent.com/file/vn-11134207-7r98o-lqwwzmh4938959', 120, 25, 600000, GETDATE(), 3, 4.4, GETDATE()),
(7, N'Giay bong ro Puma Clyde', 'giay-bong-ro-puma-clyde', N'Phong cach va hieu suat cao', 2100000, 3, 5, 'https://www.jordan1.vn/wp-content/uploads/2023/09/194454_01.png_bd8a6545b34c4aa59549ec7ffa36d888.png', 60, 10, 2300000, GETDATE(), 9, 4.3, GETDATE()),
(8, N'Bong ro Spalding NBA', 'bong-ro-spalding-nba', N'Bong thi dau chuan NBA', 750000, 7, 7, 'https://supersports.com.vn/cdn/shop/products/76-015z-2.jpg?v=1646907065&width=2000', 40, 12, 800000, GETDATE(), 6, 4.7, GETDATE()),
(9, N'Combo cau long Yonex', 'combo-cau-long-yonex', N'Bao gom vot, tui, quan can va bong', 3900000, 4, 15, 'https://shopvnb.com//uploads/san_pham/set-vot-cau-long-yonex-astrox-sv-noi-dia-2.webp', 20, 8, 4200000, GETDATE(), 7, 4.8, GETDATE()),
(10, N'Bong cau long Kawasaki Feather', 'bong-cau-long-kawasaki', N'Bong long vu thi dau chuyen nghiep', 280000, 9, 3, 'https://down-vn.img.susercontent.com/file/cn-11134207-7r98o-ltyxr70vbomr7d', 100, 30, 300000, GETDATE(), 7, 4.5, GETDATE()),
(11, N'Giay cau long Yonex SHB', 'giay-cau-long-yonex-shb', N'Giay cau long nhe va em', 1350000, 4, 6, 'https://shopvnb.com//uploads/gallery/giay-cau-long-yonex-shb-39ex-wt-gd-chinh-hang%20copy.jpg', 90, 22, 1500000, GETDATE(), 10, 4.6, GETDATE()),
(12, N'Bo tap luyen bong da Nike', 'bo-tap-luyen-nike', N'Dung cu tap luyen ky thuat bong da', 800000, 1, 14, 'https://vinasport.com.vn/wp-content/uploads/2020/03/Danh-s%C3%A1ch-c%C3%A1ch-ph%E1%BB%A5-ki%E1%BB%87n-c%E1%BA%A7n-chu%E1%BA%A9n-b%E1%BB%8B-cho-b%C3%A9-khi-ch%C6%A1i-b%C3%B3ng-%C4%91%C3%A1.png', 30, 5, 900000, GETDATE(), 11, 4.3, GETDATE()),
(13, N'Phu kien bong ro Under Armour', 'phu-kien-bong-ro-ua', N'Bang tran va co tay cao cap', 350000, 10, 12, 'https://giaybongro.vn/upload/images/1008781200/83/4434_1704193921.jpg', 70, 18, 390000, GETDATE(), 10, 4.2, GETDATE()),
(14, N'Tui dung vot Li-Ning', 'tui-vot-li-ning', N'Tui the thao da nang cho vot cau long', 420000, 5, 13, 'https://product.hstatic.net/200000099191/product/9c38aaacd240359a50f38888f6651039_81dadf79bfa04f889bc4bf998bcb0f63.jpg', 60, 14, 450000, GETDATE(), 7, 4.4, GETDATE()),
(15, N'Quan ao cau long Yonex', 'ao-cau-long-yonex', N'Bo do the thao chinh hang', 580000, 4, 10, 'https://shopvnb.com//uploads/san_pham/ao-cau-long-yonex-a218-nam-trang-xanh-1.webp', 100, 20, 620000, GETDATE(), 6, 4.7, GETDATE()),
(16, N'Giay bong ro Adidas Harden', 'giay-bong-ro-adidas-harden', N'Giay hieu suat cao cho bong ro', 2300000, 2, 5, 'https://bizweb.dktcdn.net/100/413/756/products/image-1674527390606.png?v=1675313676230', 55, 13, 2500000, GETDATE(), 8, 4.8, GETDATE()),
(17, N'Bong da Mitre Delta', 'bong-da-mitre-delta', N'Bong thi dau giai Anh', 490000, 8, 7, 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTsOATJrl19CbGWyoNnUUkJDF0xnk10uzmVlQ&s', 70, 20, 550000, GETDATE(), 11, 4.6, GETDATE()),
(18, N'Ao bong da Puma Evo', 'ao-bong-da-puma-evo', N'Bo do the thao cho bong da chuyen nghiep', 670000, 3, 8, 'https://product.hstatic.net/1000385420/product/puma_it_evo_training_tee_hong_8b68cf0f06364cd0aa5dce2b2c35daca.jpg', 85, 19, 700000, GETDATE(), 4, 4.5, GETDATE()),
(19, N'Combo bong da Adidas', 'combo-bong-da-adidas', N'Bao gom giay, bong va phu kien', 3200000, 2, 15, 'https://product.hstatic.net/1000341630/product/9_1be34d9ed68a482497d7a2e5876eb09a_master.jpg', 25, 7, 3500000, GETDATE(), 9, 4.7, GETDATE()),
(20, N'Quan ao bong ro New Balance', 'quan-ao-bong-ro-nb', N'Trang phuc thi dau bong ro chat luong', 600000, 13, 9, 'https://file.hstatic.net/200000495177/file/review_giay_bong_ro_new_balance_12_9276cbfdae4f4beabc6d69737bd45170_grande.jpg', 95, 21, 650000, GETDATE(), 8, 4.3, GETDATE()),
(21, N'Giay cau long Mizuno Wave', 'giay-cau-long-mizuno-wave', N'Giay nhe va on dinh cho thi dau', 1400000, 15, 6, 'https://product.hstatic.net/200000852613/product/wave_claw_2_trang_xanh_hong_vang_ac2cc212a21b453bb031cd6861e962c3_aae93a98ac8a45c18ed03127de3409a1_master.png', 65, 10, 1600000, GETDATE(), 12, 4.5, GETDATE()),
(22, N'Ao cau long Li-Ning Pro', 'ao-cau-long-li-ning-pro', N'Chat lieu mat va tham hut tot', 590000, 5, 10, 'https://product.hstatic.net/200000099191/product/z4260604917303_0cb32ed7eee8ec83e200d5796c1ca3ca_cd85d7dbb61248beb87f4ab58af410a1.jpg', 110, 26, 630000, GETDATE(), 7, 4.6, GETDATE()),
(23, N'Phu kien cau long Decathlon', 'phu-kien-cau-long-decathlon', N'Combo luoi + can vot + tui', 310000, 14, 13, 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSEAvjdUsfgk9ur9-I2iBdQ5sf95TdUcGTpuw&s', 80, 15, 350000, GETDATE(), 11, 4.4, GETDATE()),
(24, N'Ao bong da New Balance Elite', 'ao-bong-da-nb-elite', N'Ao thoang mat cao cap', 690000, 13, 8, 'https://product.hstatic.net/1000385420/product/nb_best_tech_training_jersey_vang_chuoi_1f353c07b90b464cb49d8899a0ed4a0d_master.jpg', 90, 22, 750000, GETDATE(), 8, 4.5, GETDATE()),
(25, N'Bong ro Decathlon Training', 'bong-ro-decathlon-training', N'Phu hop luyen tap trong nha', 420000, 14, 7, 'https://contents.mediadecathlon.com/p2154429/k$2fc1fe4d01a5e4dbaea0aa9ece21ad8d/qu%E1%BA%A3-b%C3%B3ng-r%E1%BB%95-bt100-c%E1%BB%A1-7-cho-nam-t%E1%BB%AB-13-tu%E1%BB%95i-tr%E1%BB%9F-l%C3%AAn-cam-tarmak-8495726.jpg', 45, 8, 450000, GETDATE(), 6, 4.2, GETDATE()),
(26, N'Giay bong da Asics DS Light', 'giay-bong-da-asics-ds', N'Giay da bong san co tu nhien', 1600000, 12, 4, 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTKfe-7xTkMW599c6eBD12bIXgXJJ4nIzdmbQ&s', 70, 18, 1800000, GETDATE(), 11, 4.5, GETDATE()),
(27, N'Phu kien bong da Nike Shin Guard', 'phu-kien-bong-da-nike-shin', N'Bao ve ong chan thi dau', 300000, 1, 11, 'https://product.hstatic.net/1000061481/product/nms00177_943d58875c8b4df0baa0c33d79b70a37_1024x1024.jpg', 120, 25, 350000, GETDATE(), 14, 4.6, GETDATE()),
(28, N'Bong cau long Yonex Mavis 350', 'bong-cau-long-yonex-mavis', N'Bong cau bang nhua chat luong', 250000, 4, 3, 'https://shopvnb.com//uploads/san_pham/ong-cau-long-nhua-yonex-mav-350-6-in-1-vang-1.webp', 150, 35, 270000, GETDATE(), 7, 4.7, GETDATE()),
(29, N'Tui the thao Adidas', 'tui-the-thao-adidas', N'Tui dung do da nang', 520000, 2, 13, 'https://bizweb.dktcdn.net/thumb/grande/100/340/361/products/essentialslinearduffelbagmediu-553af3a6-8322-4241-895e-70872ea0e18c.jpg?v=1742786238300', 65, 10, 580000, GETDATE(), 10, 4.3, GETDATE()),
(30, N'Bo combo luyen tap bong ro', 'combo-tap-luyen-bong-ro', N'Bong + coc tap + luoi', 980000, 14, 14, 'https://down-vn.img.susercontent.com/file/vn-11134207-7qukw-lk7tvyi9xmo2fd', 30, 6, 1050000, GETDATE(), 7, 4.4, GETDATE());

SET IDENTITY_INSERT Products OFF; 