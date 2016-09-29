create procedure uspCheckProductName
(	
	@Name nvarchar(50)
)
as
begin 
	select ID from Products where ProductName=@Name
end
go
alter procedure uspGetAllProducts
as
begin 
	select ID,ProductName,Price,PriceOfPRoduct,CountOfProduct,Tarix from Products 
end
go
alter procedure uspGetAllProducts
as
begin 
	select ID as Nömrə,ProductName as Ad,Price as [Satış Qiyməti],PriceOfPRoduct as [Alış Qiyməti],CountOfProduct as [Depodakı Say],Tarix from Products 
end