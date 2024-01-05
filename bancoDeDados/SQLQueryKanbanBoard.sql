create database kanban;

create table Users
(
	id integer primary key identity,
	name varchar(50) not null,
	email varchar(100) not null,
	password varchar(512) not null
);

create table Lists (
    Id integer identity primary key,
    Title varchar(255), 
    CreatedOn DATETIME, 
    ModifiedOn DATETIME 
);

create table KanbanTask (
    Id integer identity primary key,
    Title varchar(255),
    Description varchar(1024),
    Priority int,
    ListId integer,
    CreatedOn DATETIME,
    ModifiedOn DATETIME,
    FOREIGN KEY (ListId) REFERENCES Lists(Id)
);

create login kanban with password='senha123456';
create user kanban from login kanban;
exec sp_addrolemember 'DB_DATAREADER', 'kanban';
exec sp_addrolemember 'DB_DATAWRITER', 'kanban';

select * from KanbanTask
select * from Users