using Minimart.Core.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Threading.Tasks;

namespace Minimart.Core.Services
{
    public class SetupService
    {
        string connectionString;

        public SetupService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("SqlConnection");
            
        }

        public async Task<List<string>> CreateObjects()
        {
            var logs = new List<string>();
            try
            {
                logs.Add($"{DateTime.Now:HH:mm:ss} [INFO ] Connecting to the DB...");

                var _context = new DapperContext(connectionString);

                using (var connection = _context.CreateConnection())
                {
					logs.Add($"{DateTime.Now:HH:mm:ss} [INFO ] Connected Ok!");

					logs.Add($"{DateTime.Now:HH:mm:ss} [INFO ] starting tables_01 script");
                    
                    await connection.ExecuteAsync(tables_01);

					logs.Add($"{DateTime.Now:HH:mm:ss} [INFO ] starting tables_02 script");

					await connection.ExecuteAsync(tables_02);

					logs.Add($"{DateTime.Now:HH:mm:ss} [INFO ] starting tables_03 script");

					await connection.ExecuteAsync(tables_03);

					logs.Add($"{DateTime.Now:HH:mm:ss} [INFO ] starting tables_04 script");

					await connection.ExecuteAsync(tables_04);

					logs.Add($"{DateTime.Now:HH:mm:ss} [INFO ] starting procedures_01 script");

					await connection.ExecuteAsync(procedures_01);

					logs.Add($"{DateTime.Now:HH:mm:ss} [INFO ] starting procedures_02 script");

					await connection.ExecuteAsync(procedures_02);

					logs.Add($"{DateTime.Now:HH:mm:ss} [INFO ] done!");
				}


            }
            catch (Exception ex)
            {
                logs.Add($"{DateTime.Now:HH:mm:ss} [ERROR] {ex.Message}");
            }

            return logs;
        }

        string tables_01 = @"
drop table if exists  dbo.Store
create table dbo.Store (
	Id int not null identity primary key,
	[Name] nvarchar(100) not null,
	[address]  nvarchar(100) not null,
)


set identity_insert dbo.Store on
insert into dbo.Store (Id, [Name], [address] ) values 
	(1, 'COCO Downtown', 'SE 13 St'),
	(2, 'COCO Bay', 'SW 2nd Ave'),
	(3, 'COCO Mall', 'NW S River Dr')
set identity_insert dbo.Store off


drop table if exists  dbo.StoreOpenDay
create table dbo.StoreOpenDay (
	Id int not null identity primary key,
	StoreId int not null,
	[WeekDay] tinyint not null,
	[From] time not null,
	[To] time not null
)

 
set identity_insert dbo.StoreOpenDay on
insert into dbo.StoreOpenDay (Id, StoreId, [WeekDay], [From] , [To] )  values
(1, 1, 1, '09:00', '13:00'),
(2, 1, 1, '16:00', '20:00'),
(3, 1, 2, '09:00', '13:00'),
(4, 1, 2, '16:00', '20:00'),
(5, 1, 3, '09:00', '13:00'),
(6, 1, 3, '16:00', '20:00'),
(7, 1, 4, '09:00', '13:00'),
(8, 1, 4, '16:00', '20:00'),
(9, 1, 5, '09:00', '13:00'),
(10, 1, 5, '16:00', '20:00'),
(11, 1, 6, '10:00', '14:00'),


(12, 2, 1, '09:00', '18:00'),
(13, 2, 2, '09:00', '18:00'),
(14, 2, 3, '09:00', '18:00'),
(15, 2, 4, '09:00', '18:00'),
(16, 2, 5, '09:00', '18:00'),


(17, 3, 1, '10:00', '22:00'),
(18, 3, 2, '10:00', '22:00'),
(19, 3, 3, '10:00', '22:00'),
(20, 3, 4, '10:00', '22:00'),
(21, 3, 5, '10:00', '22:00') 
set identity_insert dbo.StoreOpenDay off


drop table if exists [dbo].[Category]
create table [dbo].[Category] (
    [Id] int IDENTITY(1,1) NOT NULL primary key,
    [Name] [nvarchar](100) NOT NULL
) 

set identity_insert [dbo].[Category] on
insert into [dbo].[Category] ([Id], [Name] ) values 
(1,'Sodas'),
(2,'Cleaning'),
(3,'Food'),
(4,'Bathroom')
set identity_insert [dbo].[Category] off


drop table if exists [dbo].[Product]
create table [dbo].[Product] (
    [Id] int not null identity primary key,
	[Name] [nvarchar](100) NOT NULL,
	[CategoryId] int not null,
	[Price] decimal (10,2)
)

set identity_insert [dbo].[Product] on
insert into [dbo].[Product] ([Id] ,[Name],[CategoryId], [Price]) values 
(1,'Cold Ice Tea',1, '3.50'),
(2,'Coffee flavoured milk',1, '1.90'),
(3,'Nuke-Cola',1, '2.00'),
(4,'Sprute',1, '1.50'),
(5,'Slurm',1, '2.10'),
(6,' Diet Slurm',1, '2.40'),

(7,'Atlantis detergent',2, '1.80'),
(8,'Virulanita',2, '1.20'),
(9,'Sponge, Bob',2, '1.00'),
(10,'Generic mop',2, '1.50'),

(11,'Salsa Cookies',3, '1.00'),
(12,'Windmill Cookies',3, '0.90'),
(13,'Garlic-o-bread 2000',3, '1.90'),
(14,'LACTEL bread',3, '1.50'),
(15,'Ravioloches x12',3, '1.50'),
(16,'Ravioloches x48',3, '4.50'),
(17,'Milanga ganga',3, '7.50'),
(18,'Milanga ganga napo',3, '8.50'),

(19,'Pure steel toilet paper',4, '2.80'),
(20,'Generic soap',4, '1.50'),
(21,'PANTONE shampoo',4, '3.00'),
(22,'Cabbagegate toothpaste',4, '1.90')
				  
set identity_insert [dbo].[Product] off


drop table if exists [dbo].[Stock]
create table [dbo].[Stock] (
    [Id] int not null identity primary key,
	[ProductId] int not null,
	[StoreId] int not null,
	[Quantity] int not null default(0)
)

            
            ";

        string tables_02 = @"
insert into [dbo].[Stock] (ProductId,  StoreId, Quantity) values 
(1, 1, 50),(2, 1, 50),(3, 1, 50),(4, 1, 0),(5, 1, 0),(6, 1, 50),
(7, 1, 0),(8, 1, 0),(9, 1, 0),(10, 1, 0),(11, 1, 50),(12, 1, 50),
(13, 1, 50),(14, 1, 50),(15, 1, 50),(16, 1, 50),(17, 1, 50),(18, 1, 50),
(19, 1, 0),(20, 1, 50),(21, 1, 50),(22, 1, 50),
( 1, 2, 50),( 2, 2, 50),( 3, 2, 50),( 4, 2, 50),( 5, 2, 50),( 6, 2,  0),
( 7, 2, 50),( 8, 2, 50),( 9, 2, 50),(10, 2, 50),(11, 2, 50),(12, 2, 50),
(13, 2, 50),(14, 2, 50),(15, 2, 50),(16, 2, 50),(17, 2, 50),(18, 2, 50),
(19, 2,  0),(20, 2,  0),(21, 2,  0),(22, 2,  0),( 1, 3, 50),( 2, 3, 50),
( 3, 3, 50),( 4, 3, 50),( 5, 3, 50),( 6, 3, 50),( 7, 3,  0),( 8, 3,  0),
( 9, 3,  0),(10, 3,  0),(11, 3, 50),(12, 3, 50),(13, 3, 50),(14, 3, 50),
(15, 3,  0),(16, 3,  0),(17, 3,  0),(18, 3,  0),(19, 3, 50),(20, 3, 50),
(21, 3, 50),(22, 3, 50)

 
---------------------------------------------------
drop table if exists [dbo].[VoucherDiscountType]
create table [dbo].[VoucherDiscountType] (
    [Id] smallint  NOT NULL primary key,
    [Name] [varchar](100) NOT NULL
) 

insert into [VoucherDiscountType] (Id, Name) values
(1, 'Discount Percent for products'),
(2, 'Discount Percent for categories'),
(3, 'Discount on N order unit'),
(4, 'Discount Pay n take m')


drop table if exists [dbo].[Voucher]
create table [dbo].[Voucher] (
    [Id] varchar(30) not null primary key,	 
	[StoreId] int not null,
	[VoucherDiscountTypeId] smallint not null,
	[ValidFromDay] tinyint null,
	[ValidFromMonth] tinyint null,
	[ValidFromYear] smallint null,
	[ValidToDay] tinyint null,
	[ValidToMonth] tinyint null,
	[ValidToYear] smallint null,

	[Percent] int null,
	[UpToUnit] int null,
	[UnitOrder] int null,
	[TakeUnits] int null,
	[PayUnits] int null
)

drop table if exists [dbo].[VoucherWeekDay]
create table [dbo].[VoucherWeekDay] (
	[VoucherId] varchar(30) not null ,
	[WeekDay] tinyint not null
)


drop table if exists [dbo].[VoucherIncludeCategory]
create table [dbo].[VoucherIncludeCategory] (
	[VoucherId] varchar(30) not null ,
	[CategoryId] int not null
)


drop table if exists [dbo].[VoucherIncludeProduct]
create table [dbo].[VoucherIncludeProduct] (
	[VoucherId] varchar(30) not null ,
	[ProductId] int not null
)

drop table if exists [dbo].[VoucherExcludeProduct]
create table [dbo].[VoucherExcludeProduct] (
	[VoucherId] varchar(30) not null ,
	[ProductId] int not null
)
";

        string tables_03 = @"
--===============================================================================================
--COCO bay
--===============================================================================================
-- COCO1V1F8XOG1MZZ
insert into [dbo].[Voucher] 
	([Id], [StoreId], [VoucherDiscountTypeId], 
	[ValidFromDay], [ValidFromMonth], [ValidFromYear], [ValidToDay], [ValidToMonth], [ValidToYear], 
	[Percent], [UpToUnit], [UnitOrder], [TakeUnits], [PayUnits])
values ('COCO1V1F8XOG1MZZ', 2, 2,  
	27,1,null, 13,2,null,
	20, null, null, null, null) --20%

insert into [VoucherWeekDay] ([VoucherId], [WeekDay])
values ('COCO1V1F8XOG1MZZ', 3), ('COCO1V1F8XOG1MZZ', 4) -- Wednesdays and Thursdays

insert into [VoucherIncludeCategory] ([VoucherId], [CategoryId])
values ('COCO1V1F8XOG1MZZ', 2) -- Cleaning

--===============================================================================================
-- COCOKCUD0Z9LUKBN
insert into [dbo].[Voucher]
	([Id], [StoreId], [VoucherDiscountTypeId], 
	[ValidFromDay], [ValidFromMonth], [ValidFromYear], [ValidToDay], [ValidToMonth], [ValidToYear], 
	[Percent], [UpToUnit], [UnitOrder], [TakeUnits], [PayUnits]) 
values ('COCOKCUD0Z9LUKBN', 2, 4,  
	24,1,null, 6,2,null,
	null, 6, null, 3, 2) --pay 2 take 3

insert into [VoucherIncludeProduct] ([VoucherId], [ProductId])
values ('COCOKCUD0Z9LUKBN', 12) -- prod Windmill Cookies

--===============================================================================================
--COCO Mall
--===============================================================================================
-- COCOG730CNSG8ZVX
insert into [dbo].[Voucher] 
	([Id], [StoreId], [VoucherDiscountTypeId], 
	[ValidFromDay], [ValidFromMonth], [ValidFromYear], [ValidToDay], [ValidToMonth], [ValidToYear], 
	[Percent], [UpToUnit], [UnitOrder], [TakeUnits], [PayUnits])
values ('COCOG730CNSG8ZVX', 3, 2, --desc.x cat  
	31,1,null, 9,2,null,
	10, null, null, null, null) --10%

insert into [VoucherIncludeCategory] ([VoucherId], [CategoryId])
values ('COCOG730CNSG8ZVX', 1) , ('COCOG730CNSG8ZVX', 4) -- Cat. Sodas, Bathroom

--===============================================================================================
-- COCO Downtown
--===============================================================================================
-- COCO2O1USLC6QR22
insert into [dbo].[Voucher] 
	([Id], [StoreId], [VoucherDiscountTypeId], 
	[ValidFromDay], [ValidFromMonth], [ValidFromYear], [ValidToDay], [ValidToMonth], [ValidToYear], 
	[Percent], [UpToUnit], [UnitOrder], [TakeUnits], [PayUnits])
values ('COCO2O1USLC6QR22', 1, 3, --desc.en la 2da unidad 
	null,2,null, null,2,null,
	30, null, 2, null, null) --30% en la 2da unidad de nuka-cola, slurm, Diet Slurm.

insert into [VoucherIncludeProduct] ([VoucherId], [ProductId])
values ('COCO2O1USLC6QR22', 3),('COCO2O1USLC6QR22', 5),('COCO2O1USLC6QR22', 6) -- prod nuka-cola, slurm, Diet Slurm.
--===============================================================================================
--COCO0FLEQ287CC05
insert into [dbo].[Voucher] 
	([Id], [StoreId], [VoucherDiscountTypeId], 
	[ValidFromDay], [ValidFromMonth], [ValidFromYear], [ValidToDay], [ValidToMonth], [ValidToYear], 
	[Percent], [UpToUnit], [UnitOrder], [TakeUnits], [PayUnits])
values ('COCO0FLEQ287CC05', 1, 3, --desc.en la 2da unidad 
	1,2,null, 15,2,null,
	50, null, 2, null, null) --50% en la 2da unidad de nuka-cola, slurm, Diet Slurm.

insert into [VoucherIncludeProduct] ([VoucherId], [ProductId])
values ('COCO0FLEQ287CC05', 22)  -- Cabbagegate toothpaste  (estimo que será hang-yourself toothpaste)

insert into [VoucherWeekDay] ([VoucherId], [WeekDay])
values ('COCO0FLEQ287CC05', 1) --mondays


--============================================================================================
";

        string tables_04 = @"
drop table if exists [dbo].[Cart]
create table [dbo].[Cart] (
	[Id] uniqueidentifier not null primary key,    
	[StoreId] int not null,
    [CreatedAt] datetime not null default(getdate()),	 
	[VoucherId] varchar(30) NULL	 
) 

drop table if exists  [dbo].[CartItem]
create table [dbo].[CartItem] (
	[Id] int not null  identity primary key,
	[CartId] uniqueidentifier not null,
	[ProductId] int not null,
	[Quantity] int not null,
	[CreatedAt] datetime not null default(getdate())
)



--============================================================================================
--// TODO! todas las relaciones
--============================================================================================
 
alter table dbo.StoreOpenDay
	add constraint [fk_StoreOpenDay]
	foreign key ([StoreId])
	references [dbo].[Store] ([Id])
	on delete no action on update no action;

alter table dbo.Product
	add constraint [fk_CategoryProduct]
	foreign key ([CategoryId])
	references [dbo].[Category]([Id])
	on delete no action on update no action;


alter table [dbo].[Stock]
	add constraint [fk_StockProduct]
	foreign key ([ProductId])
	references [dbo].[Product] ([Id])
	on delete no action on update no action;

alter table [dbo].[Stock]
	add constraint [fk_StockStore]
	foreign key ([StoreId])
	references [dbo].[Store] ([Id])
	on delete no action on update no action;


alter table [dbo].[Voucher]
	add constraint [fk_VoucheDiscountType]
	foreign key([VoucherDiscountTypeId])
	references [dbo].[VoucherDiscountType] ([Id])
	on delete no action on update no action;

alter table [dbo].[Voucher]
	add constraint [fk_VoucherStore]
	foreign key ([StoreId])
	references [dbo].[Store] ([Id])
	on delete no action on update no action;



alter table [VoucherWeekDay]
	add constraint [fk_VoucherWeekDay]
	foreign key ([VoucherId])
	references [dbo].[Voucher] ([Id])
	on delete no action on update no action;

alter table [dbo].[VoucherIncludeCategory]
	add constraint [fk_VoucherIncCategory]
	foreign key ([VoucherId])
	references [dbo].[Voucher] ([Id])
	on delete no action on update no action;

alter table [dbo].[VoucherIncludeProduct]
	add constraint [fk_VoucherIncProduct]
	foreign key ([VoucherId])
	references [dbo].[Voucher] ([Id])
	on delete no action on update no action;

alter table [dbo].[VoucherExcludeProduct]
	add constraint [fk_VoucherExcProduct]
	foreign key ([VoucherId])
	references [dbo].[Voucher] ([Id])
	on delete no action on update no action;


alter table [dbo].[Cart]
	add constraint [fk_CartStore]
	foreign key ([StoreId])
	references [dbo].[Store] ([Id])
	on delete no action on update no action;


alter table [dbo].[Cart]
	add constraint [fk_CartVoucher]
	foreign key ([VoucherId])
	references [dbo].[Voucher] ([Id])
	on delete no action on update no action;
 

alter table [dbo].[CartItem]
	add constraint [fk_CartItemCart]
	foreign key ([CartId])
	references [dbo].[Cart] ([Id])
	on delete no action on update no action;
 

alter table [dbo].[CartItem]
	add constraint [fk_CartItemProduct]
	foreign key ([ProductId])
	references [dbo].[Product] ([Id])
	on delete no action on update no action;
";

		string procedures_01 = @"

--SELECT GETDATE();
--DROP proc if exists dbo.CartItemAdd;  
 

CREATE proc dbo.CartItemAdd  
 @guid uniqueidentifier,  
 @productId int,  
 @quantity int,  
 @now datetime  
as  
begin  
   
 begin try  
  
  begin tran  
  
  --discount from stock    
  update s   
  set s.Quantity = s.Quantity - @quantity   
  from [dbo].[Stock] s   
  where s.ProductId = @productId   
  and s.StoreId = (select StoreId from [dbo].[Cart] where Id =  @guid )  
  
  --update (if exists)  
  update top(1) [CartItem]  
  set [Quantity] = [Quantity] +  @quantity   
  where [CartId] = @guid   
  and [ProductId] = @productId  
  
  -- insert the product if it doesn't exists  
  if @@ROWCOUNT = 0  
   insert into [CartItem] ([CartId], [ProductId], [Quantity], [CreatedAt])   
   VALUES (@guid, @productId, @quantity, @now)  
  
  commit tran  
 end try  
 begin catch  
  if @@TRANCOUNT > 0  
   rollback tran  
  
   throw;   
 end catch  
    
end 
 
";

		string procedures_02 = @"
--============================================================================================
--DROP proc if exists dbo.CartItemRemove;  
 

create proc dbo.CartItemRemove  
 @guid uniqueidentifier,  
 @productId int   
as  
begin  
   
 begin try  
  
  begin tran  
 
  declare @quantity int = (select [Quantity] from [CartItem] where [CartId] = @guid  and [ProductId] = @productId)  
   
  --restore the stock  
  update s   
  set s.Quantity = s.Quantity + @quantity   
  from [dbo].[Stock] s   
  where s.ProductId = @productId   
  and s.StoreId = (select StoreId from [dbo].[Cart] where Id =  @guid )  
  
  -- remove the item   
  delete [CartItem] where [CartId] = @guid  and [ProductId] = @productId  
   
  commit tran  
 end try  
 begin catch  
  if @@TRANCOUNT > 0  
   rollback tran  
  
   throw;   
 end catch 
  
end 
";


    }
}
