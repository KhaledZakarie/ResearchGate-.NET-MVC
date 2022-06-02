use AuthorDB


create table CommentDB
(AId int identity(1,1) primary key,
PaperId int,
AuthorId int,
ActionDate date,
Comment nvarchar(Max))

alter table CommentDB 
add constraint C0 foreign key (PaperId) references Paper(PaperId)


alter table CommentDB 
add constraint C1 foreign key (AuthorId) references Authors(Id)