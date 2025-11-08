PRAGMA foreign_keys=OFF;
BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "__EFMigrationsLock" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK___EFMigrationsLock" PRIMARY KEY,
    "Timestamp" TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);
INSERT INTO __EFMigrationsHistory VALUES('20251031020533_Init','9.0.10');
INSERT INTO __EFMigrationsHistory VALUES('20251101003810_AddBrandCategoryTable','9.0.10');
INSERT INTO __EFMigrationsHistory VALUES('20251101004908_AddProductImagesTable','9.0.10');
INSERT INTO __EFMigrationsHistory VALUES('20251101005328_AddUniqueCodeToProducts','9.0.10');
INSERT INTO __EFMigrationsHistory VALUES('20251103075911_AddImageToCategory','9.0.10');
INSERT INTO __EFMigrationsHistory VALUES('20251103075948_AddImageToCategory2','9.0.10');
INSERT INTO __EFMigrationsHistory VALUES('20251103082347_AddIndexShowToCategory','9.0.10');
INSERT INTO __EFMigrationsHistory VALUES('20251104093621_AddIndexShowToBrand','9.0.10');
INSERT INTO __EFMigrationsHistory VALUES('20251106093219_AddDiscountPrecentToProduct','9.0.10');
INSERT INTO __EFMigrationsHistory VALUES('20251106093508_AddDiscountPrecentToProduct2','9.0.10');
INSERT INTO __EFMigrationsHistory VALUES('20251106093946_AddDiscountPrecentToProduct3','9.0.10');
CREATE TABLE IF NOT EXISTS "Brands" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Brands" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Slug" TEXT NOT NULL,
    "LogoUrl" TEXT NOT NULL,
    "Description" TEXT NULL,
    "Origin" TEXT NULL,
    "IsActive" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL DEFAULT (datetime('now'))
, "IndexShow" INTEGER NULL);
INSERT INTO Brands VALUES(1,'Samsung','samsung','',NULL,'Hàn Quốc',1,'2025-10-31 02:07:36',1);
INSERT INTO Brands VALUES(2,'Toshiba','toshiba','',NULL,'Nhật Bản',1,'2025-10-31 02:07:36',2);
INSERT INTO Brands VALUES(3,'LG','lg','',NULL,'Hàn Quốc',1,'2025-10-31 02:07:36',3);
INSERT INTO Brands VALUES(4,'Panasonic','panasonic','',NULL,'Nhật Bản',1,'2025-10-31 02:07:36',4);
INSERT INTO Brands VALUES(5,'Daikin','daikin','',NULL,'Nhật Bản',1,'2025-10-31 02:07:36',5);
INSERT INTO Brands VALUES(6,'Casper','casper','',NULL,'Thái Lan',1,'2025-10-31 02:07:36',11);
INSERT INTO Brands VALUES(7,'Funiki','funiki','',NULL,'Việt Nam',1,'2025-10-31 02:07:36',10);
INSERT INTO Brands VALUES(8,'Gree','gree','',NULL,'Trung Quốc',1,'2025-10-31 02:07:36',7);
INSERT INTO Brands VALUES(9,'Sony','sony','',NULL,'Nhật Bản',1,'2025-10-31 02:07:36',6);
INSERT INTO Brands VALUES(10,'Hitachi','hitachi','',NULL,'Nhật Bản',1,'2025-10-31 02:07:36',10);
INSERT INTO Brands VALUES(11,'Sharp','sharp','',NULL,'Nhật Bản',1,'2025-10-31 02:07:36',9);
INSERT INTO Brands VALUES(12,'Aqua','aqua','',NULL,'Nhật Bản',1,'2025-10-31 02:07:36',8);
INSERT INTO Brands VALUES(13,'Electrolux','electrolux','',NULL,'Thụy Điển',1,'2025-10-31 02:07:36',12);
INSERT INTO Brands VALUES(14,'Ariston','ariston','',NULL,'Ý (Italia)',1,'2025-10-31 02:07:36',14);
INSERT INTO Brands VALUES(15,'Kangaroo','kangaroo','',NULL,'Việt Nam',1,'2025-10-31 02:07:36',13);
INSERT INTO Brands VALUES(16,'Mutoshi','mutoshi','',NULL,'Việt Nam',1,'2025-10-31 02:07:36',20);
INSERT INTO Brands VALUES(17,'Karofi','karofi','',NULL,'Việt Nam',1,'2025-10-31 02:07:36',15);
INSERT INTO Brands VALUES(18,'Spelier','spelier','',NULL,'Đức',1,'2025-10-31 02:07:36',16);
INSERT INTO Brands VALUES(19,'Dusler','dusler','',NULL,'Đức',1,'2025-10-31 02:07:36',17);
INSERT INTO Brands VALUES(20,'Germatek','germatek','',NULL,'Đức',1,'2025-10-31 02:07:36',19);
INSERT INTO Brands VALUES(21,'Vanessa','vanessa','',NULL,'Việt Nam',1,'2025-10-31 02:07:36',18);
CREATE TABLE IF NOT EXISTS "Categories" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Categories" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Slug" TEXT NOT NULL,
    "ParentId" INTEGER NULL,
    "IsActive" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL DEFAULT (datetime('now'))
, "Image" TEXT NOT NULL DEFAULT '', "IndexShow" INTEGER NULL);
INSERT INTO Categories VALUES(1,'Tivi','tivi',NULL,1,'2025-10-31 07:23:27','https://cdnv2.tgdd.vn/mwg-static/common/Common/64/d1/64d11a09c75ea322dbc547739886e1d5.png',2);
INSERT INTO Categories VALUES(2,'Tủ Lạnh','tu-lanh',NULL,1,'2025-10-31 07:23:27','https://cdnv2.tgdd.vn/mwg-static/common/Common/64/85/6485154d19085e781f44d057f1c63c71.png',3);
INSERT INTO Categories VALUES(3,'Máy giặt','may-giat',NULL,1,'2025-10-31 07:23:27','https://cdnv2.tgdd.vn/mwg-static/common/Common/0a/b9/0ab938f5b5b2993d568351bceb721407.png',1);
INSERT INTO Categories VALUES(4,'Điều hòa','dieu-hoa',NULL,1,'2025-10-31 07:23:27','https://cdnv2.tgdd.vn/mwg-static/common/Common/48/a6/48a6bd2b6d7ad2712eb93772b3578deb.png',4);
INSERT INTO Categories VALUES(5,'Bình nóng lạnh','binh-nong-lanh',NULL,1,'2025-10-31 07:23:27','https://cdnv2.tgdd.vn/mwg-static/dmx/Common/e5/94/e594135d5eed6cc128fe2a9c62154ad9.png',8);
INSERT INTO Categories VALUES(6,'Bếp từ đôi','bep-tu-doi',NULL,1,'2025-10-31 07:23:27','https://cdnv2.tgdd.vn/mwg-static/dmx/Common/44/0a/440ad5a5abd2b19e2d7ec9c3cf5cb707.png',9);
INSERT INTO Categories VALUES(7,'Quạt','quat',NULL,1,'2025-10-31 07:23:27','https://cdnv2.tgdd.vn/mwg-static/dmx/Common/8c/1b/8c1b71a6a8fc062456825e6483b26e6b.png',7);
INSERT INTO Categories VALUES(8,'Máy lọc nước','may-loc-nuoc',NULL,1,'2025-10-31 07:23:27','https://cdnv2.tgdd.vn/mwg-static/common/Common/ff/40/ff40b05375a001ea1f246cfd81fcbd12.png',6);
INSERT INTO Categories VALUES(10,'Tủ đông','tu-dong','',1,'2025-10-31 07:23:27','https://cdnv2.tgdd.vn/mwg-static/dmx/Common/5f/fc/5ffc124606fecac8c77bceb28b9c5c05.png',5);
INSERT INTO Categories VALUES(11,'Máy lọc không khí','may-loc-khong-khi','',1,'2025-10-31 07:23:27','https://cdnv2.tgdd.vn/mwg-static/dmx/Common/04/2e/042e6d1427540a418b516a9576e79b20.png',10);
CREATE TABLE IF NOT EXISTS "News" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_News" PRIMARY KEY AUTOINCREMENT,
    "Title" TEXT NOT NULL,
    "Slug" TEXT NOT NULL,
    "Content" TEXT NOT NULL,
    "Image" TEXT NOT NULL,
    "IsActive" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL DEFAULT (datetime('now'))
);
CREATE TABLE IF NOT EXISTS "ProductSpecs" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_ProductSpecs" PRIMARY KEY AUTOINCREMENT,
    "ProductId" INTEGER NOT NULL,
    "SpecName" TEXT NOT NULL,
    "SpecValue" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL DEFAULT (datetime('now')),
    CONSTRAINT "FK_ProductSpecs_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "BrandCategories" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_BrandCategories" PRIMARY KEY AUTOINCREMENT,
    "BrandId" INTEGER NOT NULL,
    "CategoryId" INTEGER NOT NULL,
    CONSTRAINT "FK_BrandCategories_Brands_BrandId" FOREIGN KEY ("BrandId") REFERENCES "Brands" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_BrandCategories_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE CASCADE
);
INSERT INTO BrandCategories VALUES(1,1,3);
INSERT INTO BrandCategories VALUES(2,3,3);
INSERT INTO BrandCategories VALUES(3,2,3);
INSERT INTO BrandCategories VALUES(4,4,3);
INSERT INTO BrandCategories VALUES(5,13,3);
INSERT INTO BrandCategories VALUES(6,12,3);
INSERT INTO BrandCategories VALUES(7,11,3);
INSERT INTO BrandCategories VALUES(8,1,1);
INSERT INTO BrandCategories VALUES(9,3,1);
INSERT INTO BrandCategories VALUES(10,2,1);
INSERT INTO BrandCategories VALUES(11,9,1);
INSERT INTO BrandCategories VALUES(12,1,2);
INSERT INTO BrandCategories VALUES(13,3,2);
INSERT INTO BrandCategories VALUES(14,2,2);
INSERT INTO BrandCategories VALUES(15,4,2);
INSERT INTO BrandCategories VALUES(16,10,2);
INSERT INTO BrandCategories VALUES(17,8,4);
INSERT INTO BrandCategories VALUES(18,7,4);
INSERT INTO BrandCategories VALUES(19,6,4);
INSERT INTO BrandCategories VALUES(20,4,4);
INSERT INTO BrandCategories VALUES(21,3,4);
INSERT INTO BrandCategories VALUES(22,5,4);
INSERT INTO BrandCategories VALUES(23,17,8);
INSERT INTO BrandCategories VALUES(24,15,8);
INSERT INTO BrandCategories VALUES(25,16,8);
INSERT INTO BrandCategories VALUES(26,12,8);
INSERT INTO BrandCategories VALUES(27,15,5);
INSERT INTO BrandCategories VALUES(28,14,5);
INSERT INTO BrandCategories VALUES(29,21,6);
INSERT INTO BrandCategories VALUES(30,20,6);
INSERT INTO BrandCategories VALUES(31,18,6);
INSERT INTO BrandCategories VALUES(32,19,6);
INSERT INTO BrandCategories VALUES(33,16,6);
CREATE TABLE IF NOT EXISTS "ProductImages" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_ProductImages" PRIMARY KEY AUTOINCREMENT,
    "ImageUrl" TEXT NOT NULL,
    "AltText" TEXT NULL,
    "SortOrder" INTEGER NOT NULL,
    "ProductId" INTEGER NOT NULL,
    CONSTRAINT "FK_ProductImages_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "Products" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Products" PRIMARY KEY AUTOINCREMENT,
    "BrandId" INTEGER NULL,
    "CategoryId" INTEGER NULL,
    "Code" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL DEFAULT (datetime('now')),
    "Description" TEXT NULL,
    "Detail" TEXT NOT NULL,
    "DiscountPercent" INTEGER NULL,
    "DiscountPrice" REAL NULL,
    "Gallery" TEXT NOT NULL,
    "Image" TEXT NOT NULL,
    "IsActive" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    "Price" TEXT NOT NULL,
    "Slug" TEXT NOT NULL,
    CONSTRAINT "FK_Products_Brands_BrandId" FOREIGN KEY ("BrandId") REFERENCES "Brands" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Products_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE RESTRICT
);
INSERT INTO Products VALUES(35,10,2,'FW650PGV8(GBK)','2025-11-03 03:02:10','','',12,16400000.0,'','https://cdn.mediamart.vn/images/product/t-lnh-hitachi-inverter-509l-fw650pgv8gbk_5c22e092.jpg',1,'Tủ lạnh Hitachi Inverter 509L FW650PGV8(GBK)','18650000.0','tủ-lạnh-hitachi-inverter-509l-fw650pgv8gbk');
INSERT INTO Products VALUES(36,10,2,'R-FW690PGV7X GBK','2025-11-03 03:02:10','','',14,19200000.0,'','https://cdn.tgdd.vn/Products/Images/1943/289457/tu-lanh-hitachi-540-lit-r-fw690pgv7x-gbk2-1-700x467.jpg',1,'Tủ lạnh Hitachi Inverter 540 lít Multi Door R-FW690PGV7X GBK','22400000.0','tủ-lạnh-hitachi-inverter-540-lít-multi-door-r-fw690pgv7x-gbk');
INSERT INTO Products VALUES(37,10,2,'R-WB640PGV1 GCK','2025-11-03 06:56:37','','',11,23750000.0,'','https://cdn.tgdd.vn/Products/Images/1943/287757/tu-lanh-hitachi-569-lit-r-wb640pgv1-gck2-1-700x467.jpg',1,'Tủ lạnh Hitachi Inverter 569 lít Multi Door R-WB640PGV1 GCK','26910000.0','tủ-lạnh-hitachi-inverter-569-lít-multi-door-r-wb640pgv1-gck');
INSERT INTO Products VALUES(38,10,2,'R-WB640VGV0 GBK','2025-11-03 06:56:37','','',5,29800000.0,'','https://cdn.tgdd.vn/Products/Images/1943/273295/hitachi-inverter-569-lit-r-wb640vgv0-3-700x467.jpg',1,'Tủ lạnh Hitachi Inverter 569 lít Multi Door R-WB640VGV0 GBK','31410000.0','tủ-lạnh-hitachi-inverter-569-lít-multi-door-r-wb640vgv0-gbk');
INSERT INTO Products VALUES(39,1,2,'RT20HAR8DBU','2025-11-03 06:56:37','','',24,5120000.0,'','https://cdn.tgdd.vn/Products/Images/1943/220320/samsung-rt20har8dbu-sv-1-700x467.jpg',1,'Tủ lạnh Samsung Inverter 208 lít RT20HAR8DBU/SV','6750000.0','tủ-lạnh-samsung-inverter-208-lít-rt20har8dbu-sv');
INSERT INTO Products VALUES(40,1,2,'RT22M4032BU','2025-11-03 06:56:37','','',33,5750000.0,'','https://cdn.tgdd.vn/Products/Images/1943/220323/samsung-rt22m4032bu-sv-1-1-700x467.jpg',1,'Tủ lạnh Samsung Inverter 236 lít RT22M4032BU/SV','8700000.0','tủ-lạnh-samsung-inverter-236-lít-rt22m4032bu-sv');
INSERT INTO Products VALUES(41,1,2,'RT25M4032BU/SV','2025-11-03 06:56:37','','',27,6100000.0,'','https://cdn.tgdd.vn/Products/Images/1943/220326/samsung-rt25m4032bu-sv-1-700x467.jpg',1,'Tủ lạnh Samsung Inverter 256 lít RT25M4032BU/SV','8410000.0','tủ-lạnh-samsung-inverter-256-lít-rt25m4032bu-sv');
INSERT INTO Products VALUES(42,1,2,'RT35CG5544B1SV','2025-11-03 06:56:37','','',21,8350000.0,'','https://cdn.tgdd.vn/Products/Images/1943/308108/samsung-inverter-345l-rt35cg5544b1sv-2-700x467.jpg',1,'Tủ lạnh Samsung Inverter 345 lít RT35CG5544B1SV','10610000.0','tủ-lạnh-samsung-inverter-345-lít-rt35cg5544b1sv');
INSERT INTO Products VALUES(43,1,2,'RT38CG6584B1SV','2025-11-03 06:56:37','','',17,9370000.0,'','https://cdn.tgdd.vn/Products/Images/1943/306554/samsung-inverter-382-lit-rt38cg6584b1sv-1-700x467.jpg',1,'Tủ lạnh Samsung Inverter 382 lít RT38CG6584B1SV','11330000.0','tủ-lạnh-samsung-inverter-382-lít-rt38cg6584b1sv');
INSERT INTO Products VALUES(44,1,2,'RT42CG6584B1SV','2025-11-03 06:56:37','','',19,9770000.0,'','https://cdn.tgdd.vn/Products/Images/1943/306553/samsung-inverter-406-lit-rt42cg6584b1sv1-700x467.jpg',1,'Tủ lạnh Samsung Inverter 406 lít RT42CG6584B1SV','12170000.0','tủ-lạnh-samsung-inverter-406-lít-rt42cg6584b1sv');
INSERT INTO Products VALUES(45,1,2,'RS57DG400EM9SV','2025-11-03 06:56:37','','',11,11230000.0,'','https://cdn.tgdd.vn/Products/Images/1943/328616/tu-lanh-samsung-inverter-583-lit-side-by-side-rs57dg400em9sv-1-700x467.jpg',1,'Tủ lạnh Samsung Inverter 583 lít Side By Side RS57DG400EM9SV','12760000.0','tủ-lạnh-samsung-inverter-583-lít-side-by-side-rs57dg400em9sv');
INSERT INTO Products VALUES(46,1,2,'RF48A4010B4/SV','2025-11-03 06:56:37','','',9,16000000.0,'','https://cdn.tgdd.vn/Products/Images/1943/240213/samsung-inverter-488-lit-rf48a4010b4-sv-2-1-700x467.jpg',1,'Tủ lạnh Samsung Inverter 488 lít Multi Door RF48A4010B4/SV','17670000.0','tủ-lạnh-samsung-inverter-488-lít-multi-door-rf48a4010b4-sv');
INSERT INTO Products VALUES(47,2,2,'RT234WE','2025-11-04 08:13:41','','',11,4920000.0,'','https://cdn.tgdd.vn/Products/Images/1943/310650/tu-lanh-toshiba-inverter-180-lit-gr-rt234we-pmv-52-1-2-700x467.jpg',1,'Tủ lạnh Toshiba Inverter 180 lít GR-RT234WE-PMV(52)','5530000.0','tủ-lạnh-toshiba-inverter-180-lít-gr-rt234we-pmv52');
INSERT INTO Products VALUES(48,2,2,'RT252WE','2025-11-04 08:13:41','','',9,5130000.0,'','https://cdn.tgdd.vn/Products/Images/1943/310649/tu-lanh-toshiba-inverter-194-lit-gr-rt252we-pmv-52-1-1-700x467.jpg',1,'Tủ lạnh Toshiba Inverter 194 lít GR-RT252WE-PMV(52)','5670000.0','tủ-lạnh-toshiba-inverter-194-lít-gr-rt252we-pmv52');
INSERT INTO Products VALUES(49,2,2,'RT329WE','2025-11-04 08:13:41','','',8,5810000.0,'','https://cdn.tgdd.vn/Products/Images/1943/310653/toshiba-inverter-253-lit-gr-rt329we-pmv-52-1-2-700x467.jpg',1,'Tủ lạnh Toshiba Inverter 253 lít GR-RT329WE-PMV(52)','6370000.0','tủ-lạnh-toshiba-inverter-253-lít-gr-rt329we-pmv52');
INSERT INTO Products VALUES(50,2,2,'RT303WE','2025-11-04 08:13:41','','',14,5500000.0,'','https://cdn.tgdd.vn/Products/Images/1943/310651/tu-lanh-toshiba-inverter-233-lit-gr-rt303we-pmv-52-1-2-700x467.jpg',1,'Tủ lạnh Toshiba Inverter 233 lít GR-RT303WE-PMV(52)','6460000.0','tủ-lạnh-toshiba-inverter-233-lít-gr-rt303we-pmv52');
INSERT INTO Products VALUES(51,2,2,'RT468WE','2025-11-04 08:13:41','','',13,7340000.0,'','https://web-res.midea.com/content/dam/toshiba-aem/vn/tu-lanh/tu-lanh-ngan-da-tren/rt416-rt468-rt559/1.jpg',1,'Tủ lạnh Toshiba Inverter 338 lít GR-RT468WE-PMV(58)-MM','8440000.0','tủ-lạnh-toshiba-inverter-338-lít-gr-rt468we-pmv58-mm');
INSERT INTO Products VALUES(52,2,2,'RT416WE','2025-11-04 08:13:41','','',19,7100000.0,'','https://cdn.tgdd.vn/Products/Images/1943/319610/tu-lanh-toshiba-inverter-312-lit-gr-rt416we-pmv-58-mm-1-2-700x467.jpg',1,'Tủ lạnh Toshiba Inverter 312 lít GR-RT416WE-PMV(58)-MM','8810000.0','tủ-lạnh-toshiba-inverter-312-lít-gr-rt416we-pmv58-mm');
INSERT INTO Products VALUES(53,2,2,'RS755WIA XK','2025-11-04 08:13:41','','',18,18350000.0,'','https://cdn.tgdd.vn/Products/Images/1943/319609/tu-lanh-toshiba-inverter-568-lit-side-by-side-gr-rs755wi-pgv-22-xk-1-1-700x467.jpg',1,'Tủ lạnh Toshiba Inverter 568 lít Side By Side GR-RS755WIA-PGV(22)-XK','22390000.0','tủ-lạnh-toshiba-inverter-568-lít-side-by-side-gr-rs755wia-pgv22-xk');
INSERT INTO Products VALUES(54,2,2,'RS780WI  XK','2025-11-04 08:13:41','','',16,15500000.0,'','https://cdn.tgdd.vn/Products/Images/1943/276571/toshiba-inverter-596-lit-gr-rs780wi-pgv-22-xk-4-1-700x467.jpg',1,'Tủ lạnh Toshiba Inverter 596 lít Side By Side GR-RS780WI-PGV(22)-XK','18650000.0','tủ-lạnh-toshiba-inverter-596-lít-side-by-side-gr-rs780wi-pgv22-xk');
INSERT INTO Products VALUES(55,2,2,'RF606WI AG','2025-11-04 08:13:41','','',18,14200000.0,'','https://cdnv2.tgdd.vn/mwg-static/dmx/Products/Images/1943/342628/tu-lanh-toshiba-inverter-471-lit-multi-door-gr-rf606wi-pmv-60-ag-1-638925805355623460-700x467.jpg',1,'Tủ lạnh Toshiba Inverter 471 lít Multi Door GR-RF606WI-PMV(60)-AG','17490000.0','tủ-lạnh-toshiba-inverter-471-lít-multi-door-gr-rf606wi-pmv60-ag');
INSERT INTO Products VALUES(56,2,2,'RF677WI PGV(22) XK','2025-11-04 08:13:41','','',17,14570000.0,'','https://cdn.tgdd.vn/Products/Images/1943/321758/tu-lanh-toshiba-inverter-515-lit-gr-rf677wi-pgv-22-xk-2-1-700x467.jpg',1,'Tủ lạnh Toshiba Inverter 515 lít Multi Door GR-RF677WI-PGV(22)-XK','17630000.0','tủ-lạnh-toshiba-inverter-515-lít-multi-door-gr-rf677wi-pgv22-xk');
INSERT INTO Products VALUES(57,1,1,'50du7700','2025-11-04 09:01:26','','',15,9010000.0,'','https://images.samsung.com/is/image/samsung/p6pim/vn/ua50du7700kxxv/gallery/vn-crystal-uhd-du7000-496200-ua50du7700kxxv-540272474?$Q90_1164_776_PNG$',1,'Smart Tivi Samsung 4K 50 inch 50DU7700 Crystal UHD','10610000.0','smart-tivi-samsung-4k-50-inch-50du7700-crystal-uhd');
INSERT INTO Products VALUES(58,1,1,'65du8000','2025-11-04 09:01:26','','',22,12000000.0,'','https://images.samsung.com/is/image/samsung/p6pim/vn/ua65du8000kxxv/gallery/vn-uhd-4k-tv-ua65du8000kxxv-m-t-tr--c-542460681?$Q90_1164_776_PNG$',1,'Smart Tivi Crystal UHD Samsung 4K 65 inch UA65DU8000','15580000.0','smart-tivi-crystal-uhd-samsung-4k-65-inch-ua65du8000');
INSERT INTO Products VALUES(59,1,1,'43du8500','2025-11-04 09:01:26','','',22,7350000.0,'','https://cdn2.cellphones.com.vn/insecure/rs:fill:0:358/q:90/plain/https://cellphones.com.vn/media/catalog/product/1/1/11_60_3.png',1,'Smart Tivi Samsung UHD 4K 43 INCH 2024 (43DU8500)','9538000.0','smart-tivi-samsung-uhd-4k-43-inch-2024-43du8500');
INSERT INTO Products VALUES(60,1,1,'55du8500','2025-11-04 09:01:26','','',23,10350000.0,'','https://cdn.tgdd.vn/Products/Images/1942/322686/Slider/1-1020x570.jpg',1,'Smart Tivi Crystal UHD Samsung 4K 55 inch UA55DU8000','13450000.0','smart-tivi-crystal-uhd-samsung-4k-55-inch-ua55du8000');
INSERT INTO Products VALUES(61,1,1,'65du8500','2025-11-04 09:01:26','','',19,12320000.0,'','https://images.samsung.com/is/image/samsung/p6pim/vn/ua65du8500kxxv/gallery/vn-crystal-uhd-du8500-ua65du8500kxxv-540269702?$Q90_1164_776_PNG$',1,'Smart Tivi Samsung UHD 4K 65 INCH 2024','15360000.0','smart-tivi-samsung-uhd-4k-65-inch-2024');
INSERT INTO Products VALUES(62,1,1,'75du8500','2025-11-04 09:01:26','','',14,17400000.0,'','https://cdn2.cellphones.com.vn/insecure/rs:fill:0:358/q:90/plain/https://cellphones.com.vn/media/catalog/product/1/1/11_60_7.png',1,'Smart Tivi Samsung UHD 4K 75 INCH 2024 (75DU8500)','20290000.0','smart-tivi-samsung-uhd-4k-75-inch-2024-75du8500');
INSERT INTO Products VALUES(63,1,1,'55q60D','2025-11-04 09:01:26','','',22,10920000.0,'','https://cdn.mediamart.vn/images/product/qled-tivi-4k-samsung-55q60d-55-inch-smart-tv_febf4457.webp',1,'QLED Tivi 4K Samsung 55Q60D 55 inch Smart TV','14100000.0','qled-tivi-4k-samsung-55q60d-55-inch-smart-tv');
INSERT INTO Products VALUES(64,1,1,'65q60da','2025-11-04 09:01:26','','',19,12870000.0,'','https://cdnv2.tgdd.vn/mwg-static/dmx/Products/Images/1942/322673/tivi-qled-samsung-4k-65-inch-qa65q65d-1-638687436771499863-700x467.jpg',1,'Smart Tivi QLED Samsung 4K 65 inch QA65Q65D','16060000.0','smart-tivi-qled-samsung-4k-65-inch-qa65q65d');
INSERT INTO Products VALUES(65,1,1,'75qn90ba','2025-11-04 09:01:26','','',22,29800000.0,'','https://cdn.tgdd.vn/Products/Images/1942/273396/smart-tivi-neo-qled-4k-75-inch-samsung-qa75qn90b-1-700x467.jpg',1,'Smart Tivi Neo QLED 4K 75 inch Samsung QA75QN90B','38310000.0','smart-tivi-neo-qled-4k-75-inch-samsung-qa75qn90b');
INSERT INTO Products VALUES(66,1,1,'50U8000F','2025-11-04 09:01:26','','',15,8420000.0,'','https://images.samsung.com/is/image/samsung/p6pim/vn/ua50u8000fkxxv/gallery/vn-uhd-4k-tv-ua50u8000fkxxv-m-t-tr--c-m-u--en-546021446?$Q90_1164_776_PNG$',1,'Smart Tivi Samsung UHD 4K 50 inch 2025 (50U8000F)','10020000.0','smart-tivi-samsung-uhd-4k-50-inch-2025-50u8000f');
INSERT INTO Products VALUES(67,1,1,'55U8000F','2025-11-04 09:01:26','','',16,9430000.0,'','https://images.samsung.com/is/image/samsung/p6pim/vn/ua55u8000fkxxv/gallery/vn-uhd-4k-tv-ua55u8000fkxxv-m-t-tr--c-m-u--en-546021480?$Q90_1164_776_PNG$',1,'Smart Tivi Samsung UHD 4K 55 inch 2025 (55U8000F)','11310000.0','smart-tivi-samsung-uhd-4k-55-inch-2025-55u8000f');
INSERT INTO Products VALUES(68,1,1,'65U8000F','2025-11-04 09:01:26','','',13,11620000.0,'','https://images.samsung.com/is/image/samsung/p6pim/vn/ua55u8000fkxxv/gallery/vn-uhd-4k-tv-ua55u8000fkxxv-m-t-tr--c-m-u--en-546021480?$Q90_1164_776_PNG$',1,'Smart Tivi Samsung UHD 4K 65 inch 2025 (65U8000F)','13450000.0','smart-tivi-samsung-uhd-4k-65-inch-2025-65u8000f');
INSERT INTO Products VALUES(69,1,1,'65U8500F','2025-11-04 09:01:26','','',17,13100000.0,'','https://images.samsung.com/is/image/samsung/p6pim/vn/ua65u8500fkxxv/gallery/vn-uhd-4k-tv-ua65u8500fkxxv-m-t-tr--c-m-u--en-547462087?$Q90_1164_776_PNG$',1,'Smart Tivi Crystal UHD Samsung 4K 65 inch UA65U8500F','15790000.0','smart-tivi-crystal-uhd-samsung-4k-65-inch-ua65u8500f');
INSERT INTO Products VALUES(70,1,1,'65S90F','2025-11-04 09:01:26','','',17,37200000.0,'','https://cdn.tgdd.vn/Products/Images/1942/337211/Slider/smart-tivi-oled-samsung-ai-4k-65-inch-qa65s90f638938965316813437.jpg',1,'Smart Tivi OLED Samsung AI 4K 65 inch QA65S90F','45180000.0','smart-tivi-oled-samsung-ai-4k-65-inch-qa65s90f');
INSERT INTO Products VALUES(71,1,1,'65S95F','2025-11-04 09:01:26','','',13,44790000.0,'','https://cdnv2.tgdd.vn/mwg-static/dmx/Products/Images/1942/337213/smart-tivi-oled-samsung-ai-4k-65-inch-qa65s95f-1-638811121208707451-700x467.jpg',1,'Smart Tivi OLED Samsung AI 4K 65 inch QA65S95F','51650000.0','smart-tivi-oled-samsung-ai-4k-65-inch-qa65s95f');
INSERT INTO Products VALUES(72,1,1,'100QN80F','2025-11-04 09:01:26','','',18,99000000.0,'','https://cdn.tgdd.vn/Products/Images/1942/341321/Slider/smart-tivi-neo-qled-samsung-ai-4k-100-inch-qa100qn80f638955149959948183.jpg',1,'Smart Tivi Neo QLED Samsung AI 4K 100 inch QA100QN80F','121300000.0','smart-tivi-neo-qled-samsung-ai-4k-100-inch-qa100qn80f');
INSERT INTO Products VALUES(73,9,1,'50x81dk','2025-11-05 00:28:50','','',22,11300000.0,'','https://cdn.tgdd.vn/Products/Images/1942/277426/sony-4k-50inch-kd-50x81dk-1-700x467.jpg',1,'Google Tivi Sony 4K 50 inch KD-50X81DK','14600000.0','google-tivi-sony-4k-50-inch-kd-50x81dk');
INSERT INTO Products VALUES(74,9,1,'43s25','2025-11-05 00:28:50','','',15,10800000.0,'','https://cdnv2.tgdd.vn/mwg-static/dmx/Products/Images/1942/339083/google-tivi-sony-4k-43-inch-k-43s25vm2-1-638844796993399464-700x467.jpg',1,'Google Tivi Sony 4K 43 inch K-43S25VM2','12810000.0','google-tivi-sony-4k-43-inch-k-43s25vm2');
INSERT INTO Products VALUES(75,9,1,'55s25','2025-11-05 00:28:50','','',16,13650000.0,'','https://cdnv2.tgdd.vn/mwg-static/dmx/Products/Images/1942/339081/google-tivi-sony-4k-55-inch-k-55s25vm2-1-638844792894979207-700x467.jpg',1,'Google Tivi Sony 4K 55 inch K-55S25VM2','16350000.0','google-tivi-sony-4k-55-inch-k-55s25vm2');
INSERT INTO Products VALUES(76,9,1,'65s25','2025-11-05 00:28:50','','',22,16370000.0,'','https://cdnv2.tgdd.vn/mwg-static/dmx/Products/Images/1942/339080/google-tivi-sony-4k-65-inch-k-65s25vm2-2-638844790396324054-700x467.jpg',1,'Google Tivi Sony 4K 65 inch K-65S25VM2','21070000.0','google-tivi-sony-4k-65-inch-k-65s25vm2');
INSERT INTO Products VALUES(77,9,1,'55s30','2025-11-05 00:28:50','','',19,14450000.0,'','https://cdnv2.tgdd.vn/mwg-static/dmx/Products/Images/1942/325313/google-tivi-sony-4k-55-inch-k-55s30-1-638703403753106159-700x467.jpg',1,'Smart Google Tivi Sony 4K 55 inch K-55S30Tivi Samsung UHD 4K 65 INCH 2024','17870000.0','smart-google-tivi-sony-4k-55-inch-k-55s30tivi-samsung-uhd-4k-65-inch-2024');
INSERT INTO Products VALUES(78,9,1,'65s30','2025-11-05 00:28:50','','',20,17540000.0,'','https://cdnv2.tgdd.vn/mwg-static/dmx/Products/Images/1942/325310/google-tivi-sony-4k-65-inch-k-65s30-2-638689098064562149-700x467.jpg',1,'Google Tivi Sony 4K 65 inch K-65S30','21930000.0','google-tivi-sony-4k-65-inch-k-65s30');
INSERT INTO Products VALUES(79,9,1,'75s30','2025-11-05 00:28:50','','',15,26200000.0,'','https://cdnv2.tgdd.vn/mwg-static/dmx/Products/Images/1942/325314/google-tivi-sony-4k-75-inch-k-75s30-1-638705978127459913-700x467.jpg',1,'Google Tivi Sony 4K 75 inch K-75S30','31050000.0','google-tivi-sony-4k-75-inch-k-75s30');
INSERT INTO Products VALUES(80,9,1,'75s20','2025-11-05 00:28:50','','',20,22890000.0,'','https://dienmayanphu.vn/media/product/2812_tivi_sony_4k_75_inch_k_75s20_m2_11_org.jpg',1,'Google Tivi Sony 4K 75 inch K-75S20M2 Mới 2025','28800000.0','google-tivi-sony-4k-75-inch-k-75s20m2-mới-2025');
INSERT INTO Products VALUES(81,9,1,'65s20','2025-11-05 00:28:50','','',19,15830000.0,'','https://bizweb.dktcdn.net/thumb/grande/100/475/305/products/2025-05-27-143842-87d29e97-5171-4eff-8e34-af6d214c5ab4-3d0b4d13-ade6-4696-a480-1e221b1ea544.png?v=1748335594800',1,'Smart TiVi Sony BRAVIA 2 II 65 inch 4K HDR K-65S20','19580000.0','smart-tivi-sony-bravia-2-ii-65-inch-4k-hdr-k-65s20');
INSERT INTO Products VALUES(82,9,1,'55s20','2025-11-05 00:28:50','','',14,12680000.0,'','https://bizweb.dktcdn.net/thumb/grande/100/475/305/products/640621363e6fa2ce137b95e136553b07-ca03ae96-b10a-4198-a732-34eeef2fe76f-b9e6cf42-b97e-47bd-88d2-8deee0f3c535-d5d646bf-2efa-44b4-b81a-5cf846df39da-193f0ae5-c910-494f-8fa5-982e1e7b5c53.png?v=1748335080687',1,'Smart TiVi Sony BRAVIA 2 II 55 inch 4K HDR K-55S20 mẫu 2025','14830000.0','smart-tivi-sony-bravia-2-ii-55-inch-4k-hdr-k-55s20-mẫu-2025');
INSERT INTO Products VALUES(83,9,1,'55x90k','2025-11-05 00:28:50','','',18,14870000.0,'','https://cdn.tgdd.vn/Products/Images/1942/277436/google-sony-4k-55-inch-xr-55x90k-02-700x467.jpg',1,'Google Tivi Sony 4K 55 inch XR-55X90K','18320000.0','google-tivi-sony-4k-55-inch-xr-55x90k');
INSERT INTO Products VALUES(84,1,1,'75x77L','2025-11-05 00:28:50','','',14,18690000.0,'','https://cdnv2.tgdd.vn/mwg-static/dmx/Products/Images/1942/308366/google-tivi-sony-4k-75-inch-kd-75x77l-1-638711534900366456-700x467.jpg',1,'Google Tivi Sony 4K 75 inch KD-75X77L','21840000.0','google-tivi-sony-4k-75-inch-kd-75x77l');
INSERT INTO Products VALUES(85,3,1,'43lm5750','2025-11-05 00:40:46','','',16,6350000.0,'','https://cdn.tgdd.vn/Products/Images/1942/203143/lg-43lm5700ptc-1-2-700x467.jpg',1,'Smart Tivi LG 43 inch 43LM5700PTC','7613000.0','smart-tivi-lg-43-inch-43lm5700ptc');
INSERT INTO Products VALUES(86,3,1,'43au7350','2025-11-05 00:40:46','','',17,7230000.0,'','https://cdn2.cellphones.com.vn/insecure/rs:fill:0:358/q:90/plain/https://cellphones.com.vn/media/catalog/product/s/m/smart-tivi-lg-uhd-43ua7350-4k-43-inch-2025_1_.png',1,'Smart Tivi LG UHD 4K 43 inch 2025 (43UA7350)','8776000.0','smart-tivi-lg-uhd-4k-43-inch-2025-43ua7350');
INSERT INTO Products VALUES(87,3,1,'43uq7050','2025-11-05 00:40:46','','',16,7130000.0,'','https://www.lg.com/content/dam/channel/wcms/vn/images/tivi/43uq7050psa_atvq_eavh_vn_c/gallery/large01.jpg?w=800',1,'Tivi LG UHD UQ7050 43 inch 4K Smart TV','8589000.0','tivi-lg-uhd-uq7050-43-inch-4k-smart-tv');
INSERT INTO Products VALUES(88,3,1,'50ua7350','2025-11-05 00:40:46','','',19,8540000.0,'','https://www.lg.com/content/dam/channel/wcms/vn/2025_ms_lg-com/tv/uhd/ua73/gp1/gallery/50-ua73-a/gallery/uhd-ua73-2025-50-gallery-01.jpg/jcr:content/renditions/thum-1600x1062.jpeg?w=800',1,'TV LG AI 2025 UHD AI UA73 50 inch','10650000.0','tv-lg-ai-2025-uhd-ai-ua73-50-inch');
INSERT INTO Products VALUES(89,3,1,'55ua7350','2025-11-05 00:40:46','','',19,9260000.0,'','https://www.lg.com/content/dam/channel/wcms/vn/2025_ms_lg-com/tv/uhd/ua73/gp1/gallery/55-ua73-a/gallery/uhd-ua73-2025-55-gallery-01.jpg/jcr:content/renditions/thum-1600x1062.jpeg?w=800',1,'TV LG AI 2025 UHD AI UA73 55 inch','11470000.0','tv-lg-ai-2025-uhd-ai-ua73-55-inch');
INSERT INTO Products VALUES(90,3,1,'65ut7350','2025-11-05 00:40:46','','',17,10960000.0,'','https://www.lg.com/content/dam/channel/wcms/vn/images/tivi/65ut7350psb_atvq_eavh_vn_c/gallery/uhd-ut73-65-55-50-a-gallery-01-v2.jpg/jcr:content/renditions/thum-1600x1062.jpeg?w=800',1,'65 Inch LG UHD AI UT73 4K Smart TV 2024','13270000.0','65-inch-lg-uhd-ai-ut73-4k-smart-tv-2024');
INSERT INTO Products VALUES(91,3,1,'55ut8050','2025-11-05 00:40:46','','',22,9850000.0,'','https://www.lg.com/content/dam/channel/wcms/vn/images/tivi/55ut8050psb_atv_eavh_vn_c/gallery/uhd-ut80-65-55-50-b-gallery-01-v3.jpg/jcr:content/renditions/thum-1600x1062.jpeg?w=800',1,'TV LG UHD AI 55 Inch 55UT8050PSB','12790000.0','tv-lg-uhd-ai-55-inch-55ut8050psb');
INSERT INTO Products VALUES(92,2,1,'43E31RP','2025-11-05 00:45:58','','',14,5420000.0,'','https://cdn11.dienmaycholon.vn/filewebdmclnew/DMCL21/Picture//Apro/Apro_product_36831/smart-tivi-tosh_main_709_1020.png.webp',1,'Smart Tivi Toshiba Full HD 43 Inch 43E31RP','6366000.0','smart-tivi-toshiba-full-hd-43-inch-43e31rp');
INSERT INTO Products VALUES(93,2,1,'55E330NP','2025-11-05 00:45:58','','',19,8020000.0,'','https://cdn11.dienmaycholon.vn/filewebdmclnew/DMCL21/Picture//Apro/Apro_product_35082/smart-tivi-toshiba-4k-55-inch-55e330np-main--340.png',1,'Smart Tivi Toshiba 4K 55 Inch 55E330NP','10010000.0','smart-tivi-toshiba-4k-55-inch-55e330np');
INSERT INTO Products VALUES(94,2,1,'65E330NP','2025-11-05 00:45:58','','',20,10160000.0,'','https://cdn11.dienmaycholon.vn/filewebdmclnew/DMCL21/Picture//Apro/Apro_product_35083/smart-tivi-tosh_main_656_1020.png.webp',1,'Smart Tivi Toshiba 4K 65 Inch 65E330NP','12770000.0','smart-tivi-toshiba-4k-65-inch-65e330np');
INSERT INTO Products VALUES(95,2,1,'75C350NP','2025-11-05 00:45:58','','',18,14230000.0,'','https://cdn11.dienmaycholon.vn/filewebdmclnew/DMCL21/Picture//Apro/Apro_product_35084/smart-tivi-toshiba-4k-75-inch-75c350np-main--761.png',1,'Smart Tivi Toshiba 4K 75 Inch 75C350NP','17360000.0','smart-tivi-toshiba-4k-75-inch-75c350np');
INSERT INTO Products VALUES(96,3,2,'LTB21BLMD','2025-11-05 01:38:15','','',16,5420000.0,'','https://www.lg.com/content/dam/channel/wcms/vn/rf/ltb21blmd/gallery/DZ_1.jpg/jcr:content/renditions/thum-1600x1062.jpeg?w=800',1,'Tủ lạnh ngăn đá trên Essence 217L đen mờ LTB21BLMD','6512000.0','tủ-lạnh-ngăn-đá-trên-essence-217l-đen-mờ-ltb21blmd');
INSERT INTO Products VALUES(97,3,2,'LTB31BLM','2025-11-05 01:38:15','','',18,7170000.0,'','https://www.lg.com/content/dam/channel/wcms/vn/tu-lanh/tu-lanh-ngan-da-tren/ltb31blm_aeppevn/gallery1/DZ-1.jpg/jcr:content/renditions/thum-1600x1062.jpeg?w=800',1,'Tủ lạnh LG ngăn đá trên Smart Inverter™ 315L màu đen LTB31BLM','8831000.0','tủ-lạnh-lg-ngăn-đá-trên-smart-inverter™-315l-màu-đen-ltb31blm');
INSERT INTO Products VALUES(98,3,2,'LTB31BLMA','2025-11-05 01:38:15','','',22,7820000.0,'','https://www.lg.com/content/dam/channel/wcms/vn/rf/ltb31blma/2010-1.jpg/jcr:content/renditions/thum-1600x1062.jpeg?w=800',1,'Tủ lạnh LG ngăn đá trên 315L','10100000.0','tủ-lạnh-lg-ngăn-đá-trên-315l');
INSERT INTO Products VALUES(99,3,2,'LTD33BLM','2025-11-05 01:38:15','','',14,8940000.0,'','https://www.lg.com/content/dam/channel/wcms/vn/tu-lanh/tu-lanh-ngan-da-tren/ltd33blm_aeppevn/gallery1/DZ-1.jpg/jcr:content/renditions/thum-1600x1062.jpeg?w=800',1,'Tủ lạnh LG ngăn đá trên Smart Inverter™ 332L màu đen LTD33BLM','10400000.0','tủ-lạnh-lg-ngăn-đá-trên-smart-inverter™-332l-màu-đen-ltd33blm');
INSERT INTO Products VALUES(100,3,2,'LTD37BLM','2025-11-05 01:38:15','','',15,8460000.0,'','https://www.lg.com/content/dam/channel/wcms/vn/images/tu-lanh/ltd37blm-aeppevn-eavh-vn-c/gallery/DZ-1.jpg/jcr:content/renditions/thum-1600x1062.jpeg?w=800',1,'Tủ lạnh LG ngăn đá trên Smart Inverter™ với công nghệ DoorCooling+™ 374L màu đen LTD37BLM','9975000.0','tủ-lạnh-lg-ngăn-đá-trên-smart-inverter™-với-công-nghệ-doorcooling+™-374l-màu-đen-ltd37blm');
INSERT INTO Products VALUES(101,3,2,'LFB47SVM','2025-11-05 01:38:15','','',20,12880000.0,'','https://cdnv2.tgdd.vn/mwg-static/dmx/Products/Images/1943/337189/tu-lanh-lg-inverter-474-lit-multi-door-lfb47svm-1-638809191721030420-700x467.jpg',1,'Tủ lạnh LG Inverter 474 lít Multi Door LFB47SVM','16130000.0','tủ-lạnh-lg-inverter-474-lít-multi-door-lfb47svm');
INSERT INTO Products VALUES(102,3,2,'LFB47BLG','2025-11-05 01:38:15','','',15,14240000.0,'','https://www.lg.com/content/dam/channel/wcms/vn/tu-lanh/tu-lanh-french-door/-lfb47blg-amapevn/gallery/2010-10.jpg/jcr:content/renditions/thum-1600x1062.jpeg?w=800',1,'Tủ lạnh LG French door mặt gương màu đen 474L LFB47BLG','16790000.0','tủ-lạnh-lg-french-door-mặt-gương-màu-đen-474l-lfb47blg');
INSERT INTO Products VALUES(103,3,2,'G257BL','2025-11-05 01:38:15','','',22,24870000.0,'','https://www.lg.com/content/dam/channel/wcms/vn/images/tu-lanh/gr-g257bl_aeppevn_eavh_vn_c/gallery/2010-new-01.jpg/jcr:content/renditions/thum-1600x1062.jpeg?w=800',1,'Tủ lạnh LG Instaview lấy nước ngoài UVnano 635L màu đen GR-G257BL','32280000.0','tủ-lạnh-lg-instaview-lấy-nước-ngoài-uvnano-635l-màu-đen-gr-g257bl');
INSERT INTO Products VALUES(104,3,2,'LSI63BLMA','2025-11-05 01:38:15','','',19,20300000.0,'','https://www.lg.com/content/dam/channel/wcms/vn/rf/lsi63blma/gallery/2010-1.jpg/jcr:content/renditions/thum-1600x1062.jpeg?w=800',1,'Tủ lạnh LG Side by Side lấy nước ngoài 641L màu đen LSI63BLMA','25260000.0','tủ-lạnh-lg-side-by-side-lấy-nước-ngoài-641l-màu-đen-lsi63blma');
INSERT INTO Products VALUES(105,3,2,'LFB58BLMA','2025-11-05 01:38:15','','',23,20400000.0,'','https://www.lg.com/content/dam/channel/wcms/vn/rf/lfb58blma/gallery/multi-door-f-vn7-2025-gv-b23ffqpb-gallery-gallery-2010-01.jpg/jcr:content/renditions/thum-1600x1062.jpeg?w=800',1,'Tủ lạnh LG French Door màu đen lì 574L LFB58BLMA','26510000.0','tủ-lạnh-lg-french-door-màu-đen-lì-574l-lfb58blma');
INSERT INTO Products VALUES(106,3,2,'B256BL','2025-11-05 01:38:15','','',19,11530000.0,'','https://www.lg.com/content/dam/channel/wcms/vn/images/tu-lanh/gr-b256bl_ahbpevn_eavh_vn_c/gallery/DZ-01.jpg?w=800',1,'Tủ lạnh LG Side by side 519L màu đen GR-B256BL','14320000.0','tủ-lạnh-lg-side-by-side-519l-màu-đen-gr-b256bl');
INSERT INTO Products VALUES(107,3,2,'X257BG','2025-11-05 01:38:15','','',19,32300000.0,'','https://www.lg.com/content/dam/channel/wcms/vn/images/tu-lanh/gr-x257bg_aeepevn_eavh_vn_c/gallery/DZ-13.jpg?w=800',1,'Tủ lạnh LG Instaview Door-in-door và ngăn lấy nước ngoài UV nano 635L màu be GR-X257BG','40320000.0','tủ-lạnh-lg-instaview-door-in-door-và-ngăn-lấy-nước-ngoài-uv-nano-635l-màu-be-gr-x257bg');
INSERT INTO Products VALUES(108,4,2,'NR-TV261APSV','2025-11-05 03:32:03','','',19,7460000.0,'','https://cdn.tgdd.vn/Products/Images/1943/237337/panasonic-nr-tv261apsv-3-1-700x467.jpg',1,'Tủ lạnh Panasonic Inverter 234 lít NR-TV261APSV','9227000.0','tủ-lạnh-panasonic-inverter-234-lít-nr-tv261apsv');
INSERT INTO Products VALUES(109,4,2,'NR-SP275CPAV','2025-11-05 03:32:03','','',22,7940000.0,'','https://cdnv2.tgdd.vn/mwg-static/dmx/Products/Images/1943/339481/tu-lanh-panasonic-inverter-251-lit-nr-sp275cpav-1-638965762329009789-700x467.jpg',1,'Tủ lạnh Panasonic Inverter 251 lít NR-SP275CPAV','10300000.0','tủ-lạnh-panasonic-inverter-251-lít-nr-sp275cpav');
INSERT INTO Products VALUES(110,4,2,'NR-BV331CPKV','2025-11-05 03:32:03','','',21,13070000.0,'','https://www.panasonic.com/content/dam/pim/vn/vi/NR/NR-BV3/NR-BV331CPKV/ast-1790884.png.pub.thumb.644.644.png',1,'Tủ lạnh 2 cửa NR-BV331CPKV','16650000.0','tủ-lạnh-2-cửa-nr-bv331cpkv');
INSERT INTO Products VALUES(111,4,2,'NR-BX421GPKV','2025-11-05 03:32:03','','',21,13780000.0,'','https://cdn.tgdd.vn/Products/Images/1943/235433/panasonic-nr-bx421gpkv-2-700x467.jpg',1,'Tủ lạnh Panasonic Inverter 377 lít NR-BX421GPKV','17530000.0','tủ-lạnh-panasonic-inverter-377-lít-nr-bx421gpkv');
INSERT INTO Products VALUES(112,4,2,'NR-BX421XGKV','2025-11-05 03:32:03','','',18,14880000.0,'','https://cdn11.dienmaycholon.vn/filewebdmclnew/DMCL21/Picture//Apro/Apro_product_33567/tu-lanh-panasonic-inverter-368-lit-nrbx421xgkv-main-33567.png',1,'Tủ Lạnh Panasonic Inverter 380 Lít NR-BX421XGKV','18220000.0','tủ-lạnh-panasonic-inverter-380-lít-nr-bx421xgkv');
INSERT INTO Products VALUES(113,4,2,'NR-BX421WGKV','2025-11-05 03:32:03','','',18,14080000.0,'','https://cdn.tgdd.vn/Products/Images/1943/235433/panasonic-nr-bx421gpkv-2-700x467.jpg',1,'Tủ lạnh Panasonic Inverter 377 lít NR-BX421GPKV','17350000.0','tủ-lạnh-panasonic-inverter-377-lít-nr-bx421gpkv');
INSERT INTO Products VALUES(114,4,2,'NR-BX471GPKV','2025-11-05 03:32:03','','',22,14470000.0,'','https://cdn.nguyenkimmall.com/images/thumbnails/600/336/detailed/1143/T%E1%BB%A7_l%E1%BA%A1nh_Panasonic_Inverter_417_l%C3%ADt_NR-BX471GPKV1.jpg',1,'Tủ lạnh Panasonic Inverter 417 lít NR-BX471GPKV','18650000.0','tủ-lạnh-panasonic-inverter-417-lít-nr-bx471gpkv');
INSERT INTO Products VALUES(115,4,2,'NR-BX471XGKV','2025-11-05 03:32:03','','',19,15980000.0,'','https://cdn11.dienmaycholon.vn/filewebdmclnew/DMCL21/Picture//Apro/Apro_product_33568/tu-lanh-panasonic-inverter-410-lit-nrbx471xgkv-main-33568.png',1,'Tủ Lạnh Panasonic Inverter 420 Lít NR-BX471XGKV','19840000.0','tủ-lạnh-panasonic-inverter-420-lít-nr-bx471xgkv');
INSERT INTO Products VALUES(116,4,2,'NR-BX471WGKV','2025-11-05 03:32:03','','',15,15280000.0,'','https://cdn11.dienmaycholon.vn/filewebdmclnew/DMCL21/Picture//Apro/Apro_product_24358/tu-lanh-panason_main_18_1020.png.webp',1,'Tủ Lạnh Panasonic Inverter 420 Lít NR-BX471WGKV','18180000.0','tủ-lạnh-panasonic-inverter-420-lít-nr-bx471wgkv');
INSERT INTO Products VALUES(117,4,2,'NR-BV361GPKV','2025-11-05 03:32:03','','',17,14160000.0,'','https://www.panasonic.com/content/dam/pim/vn/vi/NR/NR-BV3/NR-BV361GPKV/ast-1790937.png.pub.thumb.644.644.png',1,'Tủ lạnh Panasonic Inverter 325 lít NR-BV361GPKV','17080000.0','tủ-lạnh-panasonic-inverter-325-lít-nr-bv361gpkv');
INSERT INTO Products VALUES(118,4,2,'NR-BC361VGMV','2025-11-05 03:32:03','','',16,12930000.0,'','https://cdn.tgdd.vn/Products/Images/1943/304095/tu-lanh-panasonic-325-lit-nr-bc361vgmv-3-700x467.jpg',1,'Tủ lạnh Panasonic Inverter 325 lít NR-BC361VGMV','15540000.0','tủ-lạnh-panasonic-inverter-325-lít-nr-bc361vgmv');
INSERT INTO Products VALUES(119,4,2,'NR-BV361WGKV','2025-11-05 03:32:03','','',16,14480000.0,'','https://cdn.tgdd.vn/Products/Images/1943/296255/tu-lanh-panasonic-325-lit-nr-bv361wgkv2-1-700x467.jpg',1,'Tủ lạnh Panasonic Inverter 325 lít NR-BV361WGKV','17290000.0','tủ-lạnh-panasonic-inverter-325-lít-nr-bv361wgkv');
INSERT INTO Products VALUES(120,4,2,'NR-CW530XMMV','2025-11-05 03:32:03','','',19,27150000.0,'','https://cdn.tgdd.vn/Products/Images/1943/272101/panasonic-inverter-495-lit-nr-cw530xmmv-3-1-700x467.jpg',1,'Tủ lạnh Panasonic Inverter 495 lít Multi Door PRIME+ Edition NR-CW530XMMV','33630000.0','tủ-lạnh-panasonic-inverter-495-lít-multi-door-prime+-edition-nr-cw530xmmv');
INSERT INTO Products VALUES(121,4,2,'NR-X561BK-VN','2025-11-05 03:32:03','','',21,20250000.0,'','https://cdnv2.tgdd.vn/mwg-static/dmx/Products/Images/1943/332039/tu-lanh-panasonic-inverter-510-lit-multi-door-nr-x561bk-vn-1-638675431613999829-700x467.jpg',1,'Tủ lạnh Panasonic Inverter 510 lít Multi Door NR-X561BK-VN​','25930000.0','tủ-lạnh-panasonic-inverter-510-lít-multi-door-nr-x561bk-vn​');
INSERT INTO Products VALUES(122,4,2,'NR-X561GB-VN','2025-11-05 03:32:03','','',15,24560000.0,'','https://cdnv2.tgdd.vn/mwg-static/dmx/Products/Images/1943/332040/tu-lanh-panasonic-inverter-510-lit-multi-door-nr-x561gb-vn-1-638675440272032797-700x467.jpg',1,'Tủ lạnh Panasonic Inverter 510 lít Multi Door NR-X561GB-VN','29070000.0','tủ-lạnh-panasonic-inverter-510-lít-multi-door-nr-x561gb-vn');
INSERT INTO Products VALUES(123,4,2,'NR-DZ601Y GKV','2025-11-05 03:32:03','','',17,24250000.0,'','https://cdn.tgdd.vn/Products/Images/1943/265476/panasonic-550-lit-nr-dz601ygkv-3-700x467.jpg',1,'Tủ lạnh Panasonic Inverter 550 lít Multi Door NR-DZ601YGKV','29260000.0','tủ-lạnh-panasonic-inverter-550-lít-multi-door-nr-dz601ygkv');
INSERT INTO Products VALUES(124,4,2,'NR-DZ601VGKV','2025-11-05 03:32:03','','',18,20220000.0,'','https://cdn.tgdd.vn/Products/Images/1943/240703/panasonic-inverter-550-lit-nr-dz601vgkv-2-700x467.jpg',1,'Tủ lạnh Panasonic Inverter 550 lít Multi Door NR-DZ601VGKV','24800000.0','tủ-lạnh-panasonic-inverter-550-lít-multi-door-nr-dz601vgkv');
INSERT INTO sqlite_sequence VALUES('Brands',21);
INSERT INTO sqlite_sequence VALUES('Categories',11);
INSERT INTO sqlite_sequence VALUES('BrandCategories',33);
INSERT INTO sqlite_sequence VALUES('Products',124);
CREATE TRIGGER update_discount_percent
AFTER INSERT ON Products
FOR EACH ROW
WHEN NEW.Price > 0
BEGIN
  UPDATE Products
  SET DiscountPercent = CAST(((NEW.Price - COALESCE(NEW.DiscountPrice, NEW.Price)) * 100.0 / NEW.Price) AS INTEGER)
  WHERE id = NEW.id;
END;
CREATE TRIGGER update_discount_percent_on_update
AFTER UPDATE OF Price, DiscountPrice ON Products
FOR EACH ROW
WHEN NEW.Price > 0
BEGIN
  UPDATE Products
  SET DiscountPercent = CAST(((NEW.Price - COALESCE(NEW.DiscountPrice, NEW.Price)) * 100.0 / NEW.Price) AS INTEGER)
  WHERE id = NEW.id;
END;
CREATE TRIGGER update_discount_percent_insert
AFTER INSERT ON Products
FOR EACH ROW
WHEN NEW.Price > 0
BEGIN
    UPDATE Products
    SET DiscountPercent = CAST(((NEW.Price - COALESCE(NEW.DiscountPrice, NEW.Price)) * 100.0 / NEW.Price) AS INTEGER)
    WHERE Id = NEW.Id;
END;
CREATE TRIGGER update_discount_percent_update
AFTER UPDATE OF Price, DiscountPrice ON Products
FOR EACH ROW
WHEN NEW.Price > 0
BEGIN
    UPDATE Products
    SET DiscountPercent = CAST(((NEW.Price - COALESCE(NEW.DiscountPrice, NEW.Price)) * 100.0 / NEW.Price) AS INTEGER)
    WHERE Id = NEW.Id;
END;
CREATE INDEX "IX_ProductSpecs_ProductId" ON "ProductSpecs" ("ProductId");
CREATE UNIQUE INDEX "IX_BrandCategories_BrandId_CategoryId" ON "BrandCategories" ("BrandId", "CategoryId");
CREATE INDEX "IX_BrandCategories_CategoryId" ON "BrandCategories" ("CategoryId");
CREATE INDEX "IX_ProductImages_ProductId" ON "ProductImages" ("ProductId");
CREATE INDEX "IX_Products_BrandId" ON "Products" ("BrandId");
CREATE INDEX "IX_Products_CategoryId" ON "Products" ("CategoryId");
CREATE UNIQUE INDEX "IX_Products_Code" ON "Products" ("Code");
COMMIT;
