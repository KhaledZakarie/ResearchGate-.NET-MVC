use AuthorDB

create table LikeDB
(AId int identity(1,1) primary key,
PaperId int,
AuthorId int,
ActionDate date,
LikeP bit)

alter table LikeDB 
add constraint L0 foreign key (PaperId) references Paper(PaperId)


alter table LikeDB 
add constraint L1 foreign key (AuthorId) references Authors(Id)