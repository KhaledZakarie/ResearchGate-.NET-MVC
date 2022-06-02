use AuthorDB

create table Authors
(Id int identity(1,1) primary key,
FName nvarchar(50),
LName nvarchar(50),
Mobile bigint,
University nvarchar(50), 
Department nvarchar(50),
Email nvarchar(50),
Passward nvarchar(50),
ImagePath nvarchar(MAX))