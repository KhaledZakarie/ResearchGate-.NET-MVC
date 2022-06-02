use QuizApp

create table Users (
Id int identity(1 ,1) primary key,
fullName nvarchar (20) ,
username nvarchar (50) ,
email nvarchar (50) ,
Password nvarchar (50)
)

create table Quiz (
Id int identity( 99000 ,1) primary key,
quizName varchar (50) ,
details varchar (100)
)


create table Question (
Id int identity( 1 ,1) primary key,
IdQuiz int foreign key references Quiz (Id) on delete cascade on update cascade,
question varchar (250) ,
)

create table Choices (
Id int identity( 999000 ,1) primary key,
IdQuestion int foreign key references Question (Id) on delete cascade on update cascade,
choice varchar (100) ,
answer bit default (0)
)

create table DegreeQuizzes (
IdUser int foreign key references Users (Id) on delete cascade on update cascade,
IdQuiz int foreign key references Quiz (Id) on delete cascade on update cascade,
degree varchar (100)
)

create table Favorites (
IdUser int foreign key references Users (Id) on delete cascade on update cascade,
IdQuiz int foreign key references Quiz (Id)  on delete cascade on update cascade,
)

