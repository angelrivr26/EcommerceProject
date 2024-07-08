CREATE TABLE TIENDAS (
    IdTiendas INT IDENTITY(1,1) NOT NULL,
    Descripcion NVARCHAR(255),
    Activo BIT,
PRIMARY KEY CLUSTERED 
(
	IdTiendas ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];


CREATE PROCEDURE [dbo].[sp_RegistrarTiendas](
@Descripcion varchar(50),
@ACTIVO bit,
@Resultado bit output
)as
begin
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM TIENDAS WHERE Descripcion = @Descripcion)

		insert into TIENDAS(Descripcion, ACTIVO) values (
		@Descripcion, @ACTIVO
		)
	ELSE
		SET @Resultado = 0
	
end


CREATE PROCEDURE [dbo].[sp_ModificarTiendas](
    @IdTiendas int,
    @Descripcion varchar(60),
    @Activo bit,
    @Resultado bit output
)
AS
BEGIN
    SET @Resultado = 1
    IF NOT EXISTS (SELECT * FROM TIENDAS WHERE Descripcion = @Descripcion AND IdTiendas != @IdTiendas)
    BEGIN
        UPDATE TIENDAS SET 
            Descripcion = @Descripcion,
            Activo = @Activo
        WHERE IdTiendas = @IdTiendas;
    END
    ELSE
    BEGIN
        SET @Resultado = 0;
    END
END;


