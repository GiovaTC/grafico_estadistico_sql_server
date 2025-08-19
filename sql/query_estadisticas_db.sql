CREATE DATABASE EstadisticasDB;
GO
USE EstadisticasDB;
GO

-- Tabla 1: Ventas
CREATE TABLE Ventas (
    Id INT PRIMARY KEY IDENTITY,
    Monto DECIMAL(10,2),
    Fecha DATE
);

-- Tabla 2: Compras
CREATE TABLE Compras (
    Id INT PRIMARY KEY IDENTITY,
    Monto DECIMAL(10,2),
    Fecha DATE
);

-- Tabla 3: Empleados
CREATE TABLE Empleados (
    Id INT PRIMARY KEY IDENTITY,
    Edad INT,
    FechaIngreso DATE
);

-- Tabla 4: Clientes
CREATE TABLE Clientes (
    Id INT PRIMARY KEY IDENTITY,
    Edad INT,
    FechaRegistro DATE
);

-- Tabla 5: Productos
CREATE TABLE Productos (
    Id INT PRIMARY KEY IDENTITY,
    Precio DECIMAL(10,2),
    FechaAlta DATE
);