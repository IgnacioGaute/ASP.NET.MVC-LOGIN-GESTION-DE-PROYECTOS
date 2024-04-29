
CREATE DATABASE DB_ACCESO

USE DB_ACCESO

CREATE TABLE Usuario
(
    Id int primary key identity(1,1),
	Correo varchar(100),
	Contraseña varchar(50),
	Nombre varchar(50),
	Apellido varchar(50)
)


CREATE PROCEDURE sp_RegistrarUsuario
(
@Correo varchar(100),
@Contraseña varchar(50),
@Nombre varchar(50),
@Apellido varchar(50),
@Registrado bit output,
@Mensaje varchar(100) output
)

AS 
BEGIN 

    IF(NOT EXISTS(SELECT * FROM Usuario WHERE Correo = @Correo))
	BEGIN
	    INSERT INTO Usuario(Correo,Contraseña,Nombre,Apellido) values (@Correo, @Contraseña, @Nombre, @Apellido)
		SET @Registrado = 1
		SET @Mensaje = 'Usuario registrado'
	END
	ELSE
	BEGIN
	    SET @Registrado = 0
		SET @Mensaje = 'El correo ya existe'
	END
END







CREATE PROCEDURE sp_ValidarUsuario
(
@Correo varchar(100),
@Contraseña varchar(50)
)
AS
BEGIN
    IF(EXISTS(SELECT * FROM Usuario WHERE Correo = @Correo AND Contraseña = @Contraseña))
	    SELECT Id FROM Usuario WHERE Correo = @Correo AND Contraseña = @Contraseña
	ELSE
	    SELECT '0'
END


CREATE TABLE Tarea
(
    Id int primary key identity(1,1),
	NombreProyecto varchar(50),
	NombreCliente varchar(50),
	Descripcion varchar(100)
)


