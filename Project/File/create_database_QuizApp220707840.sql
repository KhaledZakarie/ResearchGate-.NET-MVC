create database QuizApp
on
primary (
name = QuizAppPrimary ,
filename = '####/QuizApp.mdf',
size = 10MB ,
maxsize = 20MB,
filegrowth = 5%
)
log on (
name = QuizApplog ,
filename = '####/QuizApp.ldf',
size = 9MB ,
maxsize = 15MB,
filegrowth = 35%
)
