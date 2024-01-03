create database kanbanBoard;

create table Users
(
	id integer primary key identity,
	nome varchar(50) not null,
	email varchar(100) not null,
	senha varchar(512) not null
);

create table Boards (
    Id integer identity primary key,
    Title varchar(255) NOT NULL,
    CreatedBy INT,
    CreatedOn DATETIME NOT NULL,
    ModifiedOn DATETIME NOT NULL,
    FOREIGN KEY (CreatedBy) REFERENCES users(id)
);
create table KanbanLists (
    Id integer identity primary key,
    Title varchar(255) NOT NULL,
    BoardId integer NOT NULL,
    CreatedOn DATETIME NOT NULL,
    ModifiedOn DATETIME NOT NULL,
    FOREIGN KEY (BoardId) REFERENCES Boards(Id)
);

create table KanbanTasks (
    Id integer identity primary key,
    Title varchar(255) NOT NULL,
    Description TEXT,
    Prioridade VARCHAR(50),
    BoardId integer NOT NULL,
    ListId integer NOT NULL,
    CreatedOn DATETIME NOT NULL,
    ModifiedOn DATETIME NOT NULL,
    FOREIGN KEY (BoardId) REFERENCES Boards(Id),
    FOREIGN KEY (ListId) REFERENCES KanbanLists(Id)
);

create login kanbanBoard with password='senha123456';
create user kanbanBoard from login kanbanBoard;
exec sp_addrolemember 'DB_DATAREADER', 'kanbanBoard';
exec sp_addrolemember 'DB_DATAWRITER', 'kanbanBoard';

select * from KanbanTask

------------------------

create database kanban;

create table Users
(
	id integer primary key identity,
	nome varchar(50) not null,
	email varchar(100) not null,
	senha varchar(512) not null
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
