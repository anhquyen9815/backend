-- Trigger tự tính % giảm giá khi insert
CREATE TRIGGER IF NOT EXISTS update_discount_percent_insert
AFTER INSERT ON Products
FOR EACH ROW
WHEN NEW.Price > 0
BEGIN
    UPDATE Products
    SET DiscountPercent = CAST(((NEW.Price - COALESCE(NEW.DiscountPrice, NEW.Price)) * 100.0 / NEW.Price) AS INTEGER)
    WHERE Id = NEW.Id;
END;

-- Trigger tự tính % giảm giá khi update
CREATE TRIGGER IF NOT EXISTS update_discount_percent_update
AFTER UPDATE OF Price, DiscountPrice ON Products
FOR EACH ROW
WHEN NEW.Price > 0
BEGIN
    UPDATE Products
    SET DiscountPercent = CAST(((NEW.Price - COALESCE(NEW.DiscountPrice, NEW.Price)) * 100.0 / NEW.Price) AS INTEGER)
    WHERE Id = NEW.Id;
END;
