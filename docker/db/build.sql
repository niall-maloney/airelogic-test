create database airelogic;
go

use airelogic;

create table people (uuid int Primary Key, first_name nvarchar(50), last_name nvarchar(50), self_link nvarchar(50));
insert into people values (0, 'Unassigned', '', '/api/people/0');

create table issues (uuid int Primary Key, short_description nvarchar(1000), long_description nvarchar(1000), assignee int,  opened datetime, closed datetime, status int, self_link nvarchar(50));
insert into issues values (1, 'first summary', 'first description', 0, '2018-06-24T10:00:00.000', null, 0, '/api/issues/1');