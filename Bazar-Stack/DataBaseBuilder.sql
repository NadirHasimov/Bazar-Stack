use Master
go
drop database BazarStack
go
create database BazarStack
use 
BazarStack
go
CREATE TABLE [dbo].[DateOfSaledProducts] (
    [ID]                 INT             IDENTITY (1, 1) NOT NULL,
    [ProductID]          INT             NOT NULL,
    [Pirce]              DECIMAL (18, 2) NOT NULL,
    [PriceOfProduct]     DECIMAL (18, 2) NOT NULL,
    [CountOfSoldProduct] INT             NOT NULL,
    [D_A_T_E]            DATE            NOT NULL,
    [DateTimeWithHours]  NVARCHAR (100)  NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

go

CREATE TABLE [dbo].[Products] (
    [ID]                 INT             IDENTITY (1, 1) NOT NULL,
    [ProductName]        NVARCHAR (50)   NOT NULL,
    [Price]              DECIMAL (18, 2) NOT NULL,
    [CountOfProduct]     INT             NOT NULL,
    [Condition]          BIT             DEFAULT ((1)) NOT NULL,
    [PriceOfPRoduct]     DECIMAL (18, 2) NULL,
    [CountOfSoldProduct] INT             DEFAULT ((0)) NULL,
    [Tarix]              DATE            NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [Check_Count] CHECK ([CountOfProduct]>(0))
);


GO
 create trigger Delete_From_Database_Check_ID on Products for Delete
 as
 begin
 declare @maks int
	select @maks=max(Products.ID) from Products
	if(@maks is null)  dbcc checkIdent('Products',reseed,0)
	else dbcc checkIdent('Products',reseed,@maks)
 end
GO
 create trigger Insert_From_Database_Check_ID on Products for Insert
 as
 begin
 declare @maks int
	select @maks=max(Products.ID) from Products
	if(@maks is null)  dbcc checkIdent('Products',reseed,0)
	else dbcc checkIdent('Products',reseed,@maks)
 end
go
create procedure uspBenefitsDueToDay
(
	@date1 date,@date2 date
)
as
begin	
	select DateOfSaledProducts.Pirce,DateOfSaledProducts.PriceOfProduct,
	DateOfSaledProducts.CountOfSoldProduct from DateOfSaledProducts where DateOfSaledProducts.D_A_T_E between @date1 and @date2
end
go
create procedure uspCheckProductName
(	
	@Name nvarchar(50)
)
as
begin 
	select ID from Products where ProductName=@Name
end
go
create procedure uspCountOfProduct
as
begin
	select  CountOfProduct from Products
end
go
CREATE procedure uspDeleteDateOfSaledProducts
(
	@date1 date,@date2 date
)
 as
 begin
	delete DateOfSaledProducts where D_a_t_e between @date1 and @date2
 end
go
CREATE procedure uspDeleteProducts
(
	@ID int 
)
as
begin 
	delete Products where ID=@ID
end
go
CREATE procedure uspFindProduct
(
	@name nvarchar(100)
)
as
begin
	select ID as Nömrə,ProductName as Ad,Price as [Alis Qiyməti],
	PriceOfProduct as [Satis Qiyməti] ,
	CountOfProduct as [Depodaki Say],
	Tarix as [əlavə olunma tarixi]
	from Products where ProductName like '%'+@name+'%'
end
go
CREATE procedure uspGetAllProducts
as
begin 
	select ID as Nömrə,ProductName as [Məhsulun Adi],Price as [Alis Qiyməti],PriceOfPRoduct as [Satis Qiyməti],
	CountOfProduct as [Depodaki Say],Tarix as [əlavə olunma tarixi] 
	from Products 
end
go
CREATE procedure uspGetBenefit
as
begin
	Select Products.Price,Products.PriceOfPRoduct,Products.CountOfProduct from Products
end
go
CREATE procedure uspGetBenefitInDate
(
	@date1 date , @date2 date
)
as
begin
	select Products.PriceOfPRoduct,Products.Price,Products.CountOfSoldProduct from Products where Tarix between @date1 and @date2
end
go
create procedure uspGetCountOfSoldProducts
as 
begin 
	select count(*) from Products where Condition=0
end
go
create procedure uspGetCountOfUnSoldProducts
as 
begin
	 select count(*) from Products where Condition=1
end
go
 CREATE procedure uspGetListOfProductsInDate
 as
 begin 
	select Products.ProductName as Ad,DateOfSaledProducts.Pirce as [Alis Qiyməti],
	DateOfSaledProducts.PriceOfProduct as [Satis Qiyməti]
	,DateOfSaledProducts.DateTimeWithHours as [Satilma Tarixi],DateOfSaledProducts.CountOfSoldProduct as [Satilan məhsulun sayi]
	from Products inner join DateOfSaledProducts on Products.ID=DateOfSaledProducts.ProductID
 end
go
CREATE procedure uspGetTopSelledProducts
as
begin
	select Products.ID as Nömrə,Products.ProductName as Ad,Products.Price as [Alis Qiyməti],
	Products.PriceOfPRoduct as [Satis Qiyməti],
	Products.CountOfSoldProduct as Satilmis_Məhsulun_Sayi,
	Products.Tarix as [əlavə olunma tarixi]
	from Products order by -CountOfSoldProduct
end
go
CREATE procedure uspInsertProduct
(
	@ProductName nvarchar(50), @Price decimal(18,2),@PriceOfProduct decimal(18,2),@Count int, @Date date
)
as
begin
	declare @maks int
	select @maks=max(Products.ID) from Products
	if(@maks is null) dbcc checkident('products',reseed,0)
	else dbcc checkident('Products',reseed,@maks)
	insert into Products (ProductName,Price,PriceOfPRoduct,CountOfProduct,Tarix) values (@ProductName,@Price,@PriceOfProduct ,@Count,@Date)
end
go
CREATE procedure uspSaleProduct
(
@ID int,@Count int,@date dateTIME,@Price decimal(18,2),@PriceOfProduct decimal(18,2),@date2 nvarchar(100)
)
as
begin
	update Products set CountOfProduct=CountOfProduct-@Count,CountOfSoldProduct=CountOfSoldProduct+@Count where ID=@Id ; 
	insert into DateOfSaledProducts (ProductID,Pirce,PriceOfProduct,CountOfSoldProduct,D_A_T_E,DateTimeWithHours) values  (@ID,@Price,@PriceOfProduct,@Count,@date,@date2)
end
go
CREATE procedure [uspUpdateProducTable]
(
	@ID int,@Count int,@Price decimal(18,2),@Name nvarchar(50),@PriceOfProduct decimal(18,2)
)
as
begin
	update Products set CountOfProduct=@Count,Price=@Price,PriceOfPRoduct=@PriceOfProduct,ProductName=@Name where ID=@ID
end