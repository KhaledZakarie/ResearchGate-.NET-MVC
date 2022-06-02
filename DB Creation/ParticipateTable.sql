use AuthorDB

create table Participate
(PId int identity(1,1) primary key,
PaperId int,
AuthorId int,
StartDate date)

alter table Participate 
add constraint P0 foreign key (PaperId) references Paper(PaperId)
on delete cascade on update cascade

alter table Participate 
add constraint P1 foreign key (AuthorId) references Authors(Id)
