-- App version: 1.0.0.0

--Создание юзера-владельца базы данных
--CREATE USER envibad_web_user WITH password 'bqdXgy89Opld';
--CREATE DATABASE envibad_web OWNER envibad_web_user;

--Делать из-под постгреса, встав на созданную БД envibad_web
--CREATE SCHEMA envibad_web_user AUTHORIZATION envibad_web_user;

--Запросы на создание таблиц выполнять от envibad_web_user

CREATE TABLE "LogEntries" (
  "Id" serial,
  "CreationDateTime" timestamptz(6),
  "Level" text,
  "Message" text,
  "Url" text,
  "UserName" text,
  "MachineName" text,
  "Exception" text,
  "UserIp" text,
  CONSTRAINT "PK_LogEntries" PRIMARY KEY ("Id") 
);
COMMENT ON TABLE "LogEntries" is 'Логи приложения';

CREATE TABLE "UpdateInfos" (
	"Id" serial,
	"UpdateTime" timestamp,
	"UpdateVersion" text,
	"AppVersion" text,
	"Description" text,
	CONSTRAINT "PK_UpdateInfos" PRIMARY KEY ("Id") 
);
COMMENT ON TABLE "UpdateInfos" is 'Таблица лога измений SQL';

CREATE TABLE "UserInfos" (
	"Id" serial,
	"CreationDateTime" timestamptz(6),
	"Login" text,
	"PasswordHash" text,
	"RefreshToken" text,
	"IsEnabled" boolean,
	"IsAdmin" boolean,
	CONSTRAINT "PK_UserInfos" PRIMARY KEY ("Id") 
);
COMMENT ON TABLE "UserInfos" is 'Пользователи сервиса';

CREATE TABLE "UserReportRequests" (
	"Id" serial,
	"CreationDateTime" timestamptz(6),
	"UserInfoId" int,
	"ReportName" text,
	"CenterLat" numeric(19,4),
	"CenterLong" numeric(19,4),
	"AreaRadius" numeric(19,4),
	"LastStatus" varchar(64),
	"LastStatusDateTime" timestamptz(6),
	CONSTRAINT "PK_UserReportRequests" PRIMARY KEY ("Id"),
	CONSTRAINT "FK_UserReportRequests_UserInfos" FOREIGN KEY ("UserInfoId") REFERENCES "UserInfos" ("Id") ON DELETE CASCADE
);
COMMENT ON TABLE "UserReportRequests" is 'Запросы пользователей на создание отчетов';

CREATE INDEX "IX_UserReportRequests_UserInfoId" ON "UserReportRequests" ("UserInfoId" ASC);

INSERT INTO "UpdateInfos"("UpdateTime", "UpdateVersion", "AppVersion","Description") 
VALUES ('now', '0.0.0.1', '1.0.0.0', 'Added basic tables'); 
