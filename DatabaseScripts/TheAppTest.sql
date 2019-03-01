USE [master]
GO

/****** Object:  Database [TheAppTest]    Script Date: 01.03.2019 12:10:19 ******/
CREATE DATABASE [TheAppTest]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TheAppTest', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\TheAppTest.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'TheAppTest_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\TheAppTest_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
