--insert Rol
INSERT INTO [dbo].[AspNetRoles]
           ([Id],[Name])
     VALUES
           ('7e65b986-2d63-413e-b446-a65d19fbf698','Director')

INSERT INTO Permisos(NombrePermiso)
VALUES ('Enfermeria'),('Ver Estudiantes'),('Administrar Estudiantes'),('Agregar Nota'),('Editar Notas'),('Administrar Notas'),
('Ver Notas'),('Ver Empleados'),('Administrar Empleados'),('Ver Cursos'),('Administrar Cursos'),('Ver caja'),('Realizar Transaccion'),('Editar Caja'),('Administrar Permisos'),('Administrar Años'),('Ver Años')
,('Ver Historial Diciplinario'),('Crear Falta Disciplinaria'),('Editar Falta Disciplinaria'),('Borrar Falta Diciplinario'),('Ver Cursos'),('Crear Curso'),('Desactivar Curso'),('Administrar Modalidad'),
('Administrar Materias'),('Administrar Secciones'),('Administrar Secciones'),('Administrar Colegio'),('Valorar Evaluaciones'),('Ver Semestre'),('Administrar Semestres'),('Ver Evaluaciones')
,('Administrar Evaluaciones'),('Administrar Periodos');

INSERT INTO PermisosYRoles(Id_Rol,Id_Permiso) VAlUES ('7e65b986-2d63-413e-b446-a65d19fbf698',1),('7e65b986-2d63-413e-b446-a65d19fbf698',2),('7e65b986-2d63-413e-b446-a65d19fbf698',3),('7e65b986-2d63-413e-b446-a65d19fbf698',4),('7e65b986-2d63-413e-b446-a65d19fbf698',5),('7e65b986-2d63-413e-b446-a65d19fbf698',6),('7e65b986-2d63-413e-b446-a65d19fbf698',7),('7e65b986-2d63-413e-b446-a65d19fbf698',8)
,('7e65b986-2d63-413e-b446-a65d19fbf698',9),('7e65b986-2d63-413e-b446-a65d19fbf698',10),('7e65b986-2d63-413e-b446-a65d19fbf698',11),('7e65b986-2d63-413e-b446-a65d19fbf698',12)
,('7e65b986-2d63-413e-b446-a65d19fbf698',13),('7e65b986-2d63-413e-b446-a65d19fbf698',14),('7e65b986-2d63-413e-b446-a65d19fbf698',15),('7e65b986-2d63-413e-b446-a65d19fbf698',16),('7e65b986-2d63-413e-b446-a65d19fbf698',17),('7e65b986-2d63-413e-b446-a65d19fbf698',18),('7e65b986-2d63-413e-b446-a65d19fbf698',19),
('7e65b986-2d63-413e-b446-a65d19fbf698',20),('7e65b986-2d63-413e-b446-a65d19fbf698',21),('7e65b986-2d63-413e-b446-a65d19fbf698',22),('7e65b986-2d63-413e-b446-a65d19fbf698',23),('7e65b986-2d63-413e-b446-a65d19fbf698',24),('7e65b986-2d63-413e-b446-a65d19fbf698',25),
('7e65b986-2d63-413e-b446-a65d19fbf698',26),('7e65b986-2d63-413e-b446-a65d19fbf698',27),('7e65b986-2d63-413e-b446-a65d19fbf698',28),('7e65b986-2d63-413e-b446-a65d19fbf698',29),
('7e65b986-2d63-413e-b446-a65d19fbf698',30),('7e65b986-2d63-413e-b446-a65d19fbf698',31),('7e65b986-2d63-413e-b446-a65d19fbf698',32),('7e65b986-2d63-413e-b446-a65d19fbf698',33)
,('7e65b986-2d63-413e-b446-a65d19fbf698',34),('7e65b986-2d63-413e-b446-a65d19fbf698',35),('7e65b986-2d63-413e-b446-a65d19fbf698',36),('7e65b986-2d63-413e-b446-a65d19fbf698',37)

insert into OpcionesDeColegios(NombreColegio) values ('Colegio')

--Insertar al empleado
INSERT INTO [dbo].[Empleadoes] ([Nombre],[Apellido],[Direccion],[Correo],[FechaNaci],[Domicilio]
	,[Telefono],[Genero],[Identificacion],[Codigo_Empleado],[EstadoCivil],[Dt_Contratacion],[BL_Estado_Empleado],[Bl_ProfesorGuia],[FirstLogin])
     VALUES ('Admin1','AdminApellido','Direccion Administradora','Admin@hotmail.com','1980-09-23 16:35:58.000','Managua','82143371',
	'Masculino','001-230999-0001P','School-1','Soltero','2020-09-23 16:35:58.000','true','false','false')

INSERT INTO [dbo].[AspNetUsers] ([Id],[Email],[EmailConfirmed],[PasswordHash],[SecurityStamp],[PhoneNumber]
           ,[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEndDateUtc],[LockoutEnabled],[AccessFailedCount],[UserName],[Id_Empleado],[Fecha_Conexcion])
     VALUES
           ('eb908a4c-e094-43a2-aa12-d9d17d985de7','miasango2@hotmail.com','False','AKSRaiHzQu7s6Eq712igPyruWnQ/6XyaHRiWo3a/L74SMIsidzJF/gtDiKCIR8A1+Q=='
           ,'a19df023-be79-46a5-b7ce-b1154afc8f98','','0','0','','1','0','Admin@hotmail.com'
           ,'1','1990-03-14 00:00:00.000')

INSERT INTO [dbo].[AspNetUserRoles] ([UserId],[RoleId])
     VALUES
           ('eb908a4c-e094-43a2-aa12-d9d17d985de7','7e65b986-2d63-413e-b446-a65d19fbf698')

--Inser
INSERT INTO [dbo].[Modalidades]
           ([Str_Modalidad])
     VALUES ('Matutino')
