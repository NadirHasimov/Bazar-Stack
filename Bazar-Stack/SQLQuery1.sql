create procedure uspGetBenefit
as
begin
	Select Products.Price,Products.PriceOfPRoduct,Products.CountOfSoldProduct from Products
end

go
create procedure uspGetBenefitInDate
(
	@date1 date , @date2 date
)
as
begin
select Products.PriceOfPRoduct,Products.Price,Products.CountOfSoldProduct from Products where Tarix between @date1 and @date2
end
select Exam.EmployeeID from Exam where EmployeeID between 1 and 5
select Exam.EmployeeID from Exam