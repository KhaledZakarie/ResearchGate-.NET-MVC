use AuthorDB



create table Paper
(PaperId int identity(1,1) primary key,
PaperName nvarchar(50),
PublisherId int,
PublishDate date,
PaperFile nvarchar(MAX))

alter table Paper 
add constraint a1 foreign key (PublisherId) references Authors(Id)
on delete cascade on update cascade