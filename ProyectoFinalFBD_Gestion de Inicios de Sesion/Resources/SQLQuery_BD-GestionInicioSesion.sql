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

INSERT INTO [GestionInicioSesion].[dbo].[Usuarios] ([Usuario], [Contrasena], [FechaDeRegistro], [CorreoElectronico], [Telefono], [Nombre], [ApellidoP], [ApellidoM], [Edad], [SexoID])
VALUES
('juanperez', 'contraseña123', GETDATE(), 'juanperez@example.com', '1234567890', 'Juan', 'Perez', 'Gomez', 30, 1),
('mariagonzalez', 'contraseña456', GETDATE(), 'mariagonzalez@example.com', '1234567891', 'Maria', 'Gonzalez', 'Lopez', 25, 2),
('pedroalvarez', 'contraseña789', GETDATE(), 'pedroalvarez@example.com', '1234567892', 'Pedro', 'Alvarez', 'Martinez', 35, 1),
('lauracastillo', 'contraseña101', GETDATE(), 'lauracastillo@example.com', '1234567893', 'Laura', 'Castillo', 'Hernandez', 28, 2),
('robertofernandez', 'contraseña102', GETDATE(), 'robertofernandez@example.com', '1234567894', 'Roberto', 'Fernandez', 'Perez', 40, 1),
('luismorales', 'contraseña202', GETDATE(), 'luismorales@example.com', '1234567895', 'Luis', 'Morales', 'Ruiz', 32, 1),
('carolinavazquez', 'contraseña303', GETDATE(), 'carolinavazquez@example.com', '1234567896', 'Carolina', 'Vazquez', 'Gonzalez', 27, 2),
('juanitaorozco', 'contraseña404', GETDATE(), 'juanitaorozco@example.com', '1234567897', 'Juanita', 'Orozco', 'Sanchez', 22, 2),
('pedroprieto', 'contraseña505', GETDATE(), 'pedroprieto@example.com', '1234567898', 'Pedro', 'Prieto', 'Gomez', 45, 1),
('sofiajimenez', 'contraseña606', GETDATE(), 'sofiajimenez@example.com', '1234567899', 'Sofia', 'Jimenez', 'Martinez', 33, 2);
