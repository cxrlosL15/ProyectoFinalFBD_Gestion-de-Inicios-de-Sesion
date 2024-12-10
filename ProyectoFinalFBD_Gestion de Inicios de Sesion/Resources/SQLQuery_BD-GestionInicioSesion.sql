-- Crear la base de datos
CREATE DATABASE GestionInicioSesion;
GO

-- Usar la base de datos creada
USE GestionInicioSesion;
GO

-- Crear la tabla Sexo
CREATE TABLE Sexo (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Descripcion VARCHAR(50) NOT NULL
);

-- Crear la tabla SistemaOperativo
CREATE TABLE SistemaOperativo (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Descripcion VARCHAR(50) NOT NULL
);

-- Crear la tabla Usuarios
CREATE TABLE Usuarios (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Usuario VARCHAR(50) NOT NULL UNIQUE,
    Contrasena VARCHAR(255) NOT NULL,
    FechaDeRegistro DATETIME NOT NULL,
    CorreoElectronico VARCHAR(100) NOT NULL UNIQUE,
    Telefono VARCHAR(20) NOT NULL UNIQUE,
    Nombre VARCHAR(50) NOT NULL,
    ApellidoP VARCHAR(50) NOT NULL,
    ApellidoM VARCHAR(50) NOT NULL,
    Edad INT NOT NULL,
    SexoID INT,
    CONSTRAINT FK_Usuarios_Sexo FOREIGN KEY (SexoID) REFERENCES Sexo(ID),
    CONSTRAINT EdadEnRango CHECK (Edad >= 18 AND Edad <= 100)
);

-- Crear la tabla IntentosDeSesion
CREATE TABLE IntentosDeSesion (
    ID INT PRIMARY KEY IDENTITY(1,1),
    UsuarioID INT NOT NULL,
    Estado BIT NOT NULL,
    DireccionIP VARCHAR(45) NOT NULL,
    SistemaOperativoID INT NOT NULL,
    Fecha DATETIME NOT NULL,
    CONSTRAINT FK_IntentosDeSesion_Usuarios FOREIGN KEY (UsuarioID) REFERENCES Usuarios(ID),
    CONSTRAINT FK_IntentosDeSesion_SistemaOperativo FOREIGN KEY (SistemaOperativoID) REFERENCES SistemaOperativo(ID)
);

-- Crear la tabla EstadoDeLaSesion
CREATE TABLE EstadoDeLaSesion (
    ID INT PRIMARY KEY IDENTITY(1,1),
    UsuarioID INT NOT NULL,
    FechaDeEntrada DATETIME NOT NULL,
    FechaDeSalida DATETIME,
    UltimaActividadRealizada DATETIME,
    DireccionIP VARCHAR(45) NOT NULL,
    SistemaOperativoID INT NOT NULL,
    CONSTRAINT FK_EstadoDeLaSesion_Usuarios FOREIGN KEY (UsuarioID) REFERENCES Usuarios(ID),
    CONSTRAINT FK_EstadoDeLaSesion_SistemaOperativo FOREIGN KEY (SistemaOperativoID) REFERENCES SistemaOperativo(ID)
);

insert into Sexo values('Masculino')
insert into Sexo values('Femenino')
insert into Sexo values('Otro')


insert into SistemaOperativo values('Windows')
insert into SistemaOperativo values('Linux')
insert into SistemaOperativo values('Mac')

