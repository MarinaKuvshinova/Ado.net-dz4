create database Purchase
use Purchase

create table Sellers(
	ID_Seller int not null primary key identity,
	FirstName nvarchar(20) not null,
	LastName nvarchar(20) not null,
	[Count] int not null
)
insert into Sellers
values
	('Иванов','Иван', 10)

create table Buyers(
	ID_Buyer int not null primary key identity,
	FirstName nvarchar(20) not null,
	LastName nvarchar(20) not null,
	[Count] int not null
)
insert into Buyers
values
	('Петров','Петр', 0)

create table Checks(
	ID_Check int not null primary key identity,
	ID_Seller int not null foreign key references Sellers(ID_Seller),
	ID_Buyer int not null foreign key references Buyers(ID_Buyer),
	[Date] date not null default getdate()
)
select *from Checks
drop table Checks
insert into Checks
values (1,1,default);

UPDATE Buyers SET Count=0 WHERE ID_Buyer=1
UPDATE Sellers SET Count=10 WHERE ID_Seller=1