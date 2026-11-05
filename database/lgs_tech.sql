-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 05-11-2026 a las 20:40:52
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `lgs_tech`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `archivosorden`
--

CREATE TABLE `archivosorden` (
  `Id` int(11) NOT NULL,
  `OrdenReparacionId` int(11) NOT NULL,
  `NombreArchivo` varchar(255) NOT NULL,
  `RutaArchivo` varchar(500) NOT NULL,
  `TipoArchivo` varchar(100) NOT NULL,
  `FechaSubida` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `archivosorden`
--

INSERT INTO `archivosorden` (`Id`, `OrdenReparacionId`, `NombreArchivo`, `RutaArchivo`, `TipoArchivo`, `FechaSubida`) VALUES
(2, 1, 'Bateria.JPG', 'ordenes/1/d5f23ff7-6846-43ed-8e48-a3b114d460eb.jpg', 'image/jpeg', '2026-10-06 16:30:14.124768'),
(3, 2, 'lenovo.JPG', 'ordenes/2/8a9ed5f7-4d8e-406a-a82a-7a2c54ca39c5.jpg', 'image/jpeg', '2026-11-05 15:41:06.665686');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aspnetroleclaims`
--

CREATE TABLE `aspnetroleclaims` (
  `Id` int(11) NOT NULL,
  `RoleId` varchar(255) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aspnetroles`
--

CREATE TABLE `aspnetroles` (
  `Id` varchar(255) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `NormalizedName` varchar(256) DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `aspnetroles`
--

INSERT INTO `aspnetroles` (`Id`, `Name`, `NormalizedName`, `ConcurrencyStamp`) VALUES
('76cff6c6-4ca1-4ed8-b75e-d77eb795c1e0', 'Administrador', 'ADMINISTRADOR', NULL),
('b8cc71eb-5b1e-430b-8e5d-0496be82b3e1', 'Tecnico', 'TECNICO', NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aspnetuserclaims`
--

CREATE TABLE `aspnetuserclaims` (
  `Id` int(11) NOT NULL,
  `UserId` varchar(255) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aspnetuserlogins`
--

CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(255) NOT NULL,
  `ProviderKey` varchar(255) NOT NULL,
  `ProviderDisplayName` longtext DEFAULT NULL,
  `UserId` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aspnetuserroles`
--

CREATE TABLE `aspnetuserroles` (
  `UserId` varchar(255) NOT NULL,
  `RoleId` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `aspnetuserroles`
--

INSERT INTO `aspnetuserroles` (`UserId`, `RoleId`) VALUES
('2c0cf554-b76f-43a2-a989-29d213f6c287', 'b8cc71eb-5b1e-430b-8e5d-0496be82b3e1'),
('d5cc02ae-000e-4560-bdb8-10441319a401', '76cff6c6-4ca1-4ed8-b75e-d77eb795c1e0'),
('f752528a-9964-4e82-a417-9b8c3e9e149f', 'b8cc71eb-5b1e-430b-8e5d-0496be82b3e1');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aspnetusers`
--

CREATE TABLE `aspnetusers` (
  `Id` varchar(255) NOT NULL,
  `Nombre` varchar(60) NOT NULL,
  `Apellido` varchar(60) NOT NULL,
  `Avatar` varchar(255) DEFAULT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext DEFAULT NULL,
  `SecurityStamp` longtext DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL,
  `PhoneNumber` longtext DEFAULT NULL,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `aspnetusers`
--

INSERT INTO `aspnetusers` (`Id`, `Nombre`, `Apellido`, `Avatar`, `UserName`, `NormalizedUserName`, `Email`, `NormalizedEmail`, `EmailConfirmed`, `PasswordHash`, `SecurityStamp`, `ConcurrencyStamp`, `PhoneNumber`, `PhoneNumberConfirmed`, `TwoFactorEnabled`, `LockoutEnd`, `LockoutEnabled`, `AccessFailedCount`) VALUES
('2c0cf554-b76f-43a2-a989-29d213f6c287', 'Luis', 'Segura', '/uploads/avatars/38a88fb0-72f3-40a9-af23-41ede684df21.png', 'luis.s@gmail.com', 'LUIS.S@GMAIL.COM', 'luis.s@gmail.com', 'LUIS.S@GMAIL.COM', 1, 'AQAAAAIAAYagAAAAEK7emoaJl1DS3YIL7xRMjq5eqRiZdvzfYA7m2ZGNBpHTr4vtNIHhjdm+PaQmGtCeWQ==', 'K2TJ3L35P6PYHTI57FZL6K6N7TJIUF2T', '4dfa3ba4-23fd-443c-9fe7-50d5ac33e0ab', NULL, 0, 0, NULL, 1, 0),
('d5cc02ae-000e-4560-bdb8-10441319a401', 'Administrador', 'LGS', '/uploads/avatars/a11856dd-ee5f-4fe3-88df-2d9b5e74ac1a.jpg', 'admin@lgstech.com', 'ADMIN@LGSTECH.COM', 'admin@lgstech.com', 'ADMIN@LGSTECH.COM', 1, 'AQAAAAIAAYagAAAAEGGZL8149+TDyNx0p3OK1/tktfIwWEIk4mGmc/8l1hj0LHdJBf30EzF/tHErQvfNEg==', 'KCMKAABPPC2VFARSBPRUUDFXCOHOZ7AF', '313613a2-b65a-4a5b-a417-eae55eea73cf', NULL, 0, 0, NULL, 1, 0),
('f752528a-9964-4e82-a417-9b8c3e9e149f', 'Tecnico1', 'Prueba', NULL, 'tecnico@lgstech.com', 'TECNICO@LGSTECH.COM', 'tecnico@lgstech.com', 'TECNICO@LGSTECH.COM', 1, 'AQAAAAIAAYagAAAAECYsLjyFzrOvUZTYq86usWyvNqEQ+Puv9/MvzLnQEGVPRT/V70veldzbOL4GYfHE2A==', 'DDOVASWTDXP2KWDVOOMGGHL5JYAYGUSY', 'cbfd29e8-747c-442d-8ba6-769b647348d6', NULL, 0, 0, NULL, 1, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aspnetusertokens`
--

CREATE TABLE `aspnetusertokens` (
  `UserId` varchar(255) NOT NULL,
  `LoginProvider` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Value` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `clientes`
--

CREATE TABLE `clientes` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(60) NOT NULL,
  `Apellido` varchar(60) NOT NULL,
  `Documento` varchar(20) NOT NULL,
  `Telefono` varchar(30) NOT NULL,
  `Correo` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `clientes`
--

INSERT INTO `clientes` (`Id`, `Nombre`, `Apellido`, `Documento`, `Telefono`, `Correo`) VALUES
(2, 'Brenda Rocio', 'Lopez', '37780282', '2612448650', 'lopezbrenda@gmail.com'),
(3, 'Mirta', 'Gonzalez', '12974149', '2613459635', 'mirta@gmail.com'),
(4, 'Luis', 'Segura', '08369772', '2615242140', 'luis@gmail.com'),
(5, 'Emanuel', 'Garcia', '37123456', '2615113456', 'manu@gmail.com'),
(6, 'asadsa', 'sdasdas', '21312313', '32131231231', 'dasdasd@gmail.com'),
(8, 'aacxcsd', 'zxsadasdasd', '654654564', '98798787897', 'dasdasm@gmail.com'),
(10, 'dcasc', 'xasdad', '6565654545', '6464645456', 'oaoaoa@gmail.com'),
(11, 'adsx', 'xasas', '36363636', '6969696969', 'gmx.asad@gmail.com'),
(12, 'axasax', 'xxasxa', '12554124', '123123123', 'wqxx.l@gmail.com'),
(14, 'Cliente ', 'Demo Editado', '5514132123', '65435432343', 'mmmckask@gmail.com'),
(15, 'Lorena', 'Gonzalez', '2232136564', '02135514412', 'Loren@gmail.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `equipos`
--

CREATE TABLE `equipos` (
  `Id` int(11) NOT NULL,
  `ClienteId` int(11) NOT NULL,
  `Tipo` varchar(30) NOT NULL,
  `Marca` varchar(50) NOT NULL,
  `Modelo` varchar(60) NOT NULL,
  `NumeroSerie` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `equipos`
--

INSERT INTO `equipos` (`Id`, `ClienteId`, `Tipo`, `Marca`, `Modelo`, `NumeroSerie`) VALUES
(2, 2, 'Celular', 'Huawei ', 'P40 pro plus', '00123456789'),
(3, 5, 'Notebook', 'Lenovo', 'Thinpad x840', '00023221332002465321231'),
(4, 15, 'Celular', 'Samsung ', 'Galaxy s24+', '3335544598/7978');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `ordenesreparacion`
--

CREATE TABLE `ordenesreparacion` (
  `Id` int(11) NOT NULL,
  `EquipoId` int(11) NOT NULL,
  `TecnicoId` varchar(255) DEFAULT NULL,
  `Problema` varchar(500) NOT NULL,
  `Diagnostico` varchar(1000) DEFAULT NULL,
  `Estado` int(11) NOT NULL,
  `FechaIngreso` datetime(6) NOT NULL,
  `FechaFinalizacion` datetime(6) DEFAULT NULL,
  `CostoEstimado` decimal(12,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `ordenesreparacion`
--

INSERT INTO `ordenesreparacion` (`Id`, `EquipoId`, `TecnicoId`, `Problema`, `Diagnostico`, `Estado`, `FechaIngreso`, `FechaFinalizacion`, `CostoEstimado`) VALUES
(1, 2, 'f752528a-9964-4e82-a417-9b8c3e9e149f', 'El equipo Enciende, se calienta y a los pocos minutos se apaga, hace eso reiteradas veces', 'Se reemplazó la batería y se realizaron pruebas.', 2, '2026-10-02 17:29:25.466969', '2026-10-02 18:00:31.628997', 50000.00),
(2, 3, '2c0cf554-b76f-43a2-a989-29d213f6c287', 'La notebook se traba y se apaga sin previo aviso', 'Posible problema de memoria ram', 0, '2026-11-05 15:26:08.622864', NULL, 100000.00);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `Id` int(11) NOT NULL,
  `OrdenReparacionId` int(11) NOT NULL,
  `Monto` decimal(12,2) NOT NULL,
  `FechaPago` datetime(6) NOT NULL,
  `MetodoPago` int(11) NOT NULL,
  `TipoPago` int(11) NOT NULL,
  `Observacion` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`Id`, `OrdenReparacionId`, `Monto`, `FechaPago`, `MetodoPago`, `TipoPago`, `Observacion`) VALUES
(1, 1, 40000.00, '2026-10-06 15:32:31.673341', 0, 0, 'Se entrega 40.000, al retirar el equipo se completa el pago ');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20260928012236_InitialCreate', '8.0.13');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `archivosorden`
--
ALTER TABLE `archivosorden`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_ArchivosOrden_OrdenReparacionId` (`OrdenReparacionId`);

--
-- Indices de la tabla `aspnetroleclaims`
--
ALTER TABLE `aspnetroleclaims`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`);

--
-- Indices de la tabla `aspnetroles`
--
ALTER TABLE `aspnetroles`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `RoleNameIndex` (`NormalizedName`);

--
-- Indices de la tabla `aspnetuserclaims`
--
ALTER TABLE `aspnetuserclaims`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_AspNetUserClaims_UserId` (`UserId`);

--
-- Indices de la tabla `aspnetuserlogins`
--
ALTER TABLE `aspnetuserlogins`
  ADD PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  ADD KEY `IX_AspNetUserLogins_UserId` (`UserId`);

--
-- Indices de la tabla `aspnetuserroles`
--
ALTER TABLE `aspnetuserroles`
  ADD PRIMARY KEY (`UserId`,`RoleId`),
  ADD KEY `IX_AspNetUserRoles_RoleId` (`RoleId`);

--
-- Indices de la tabla `aspnetusers`
--
ALTER TABLE `aspnetusers`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  ADD KEY `EmailIndex` (`NormalizedEmail`);

--
-- Indices de la tabla `aspnetusertokens`
--
ALTER TABLE `aspnetusertokens`
  ADD PRIMARY KEY (`UserId`,`LoginProvider`,`Name`);

--
-- Indices de la tabla `clientes`
--
ALTER TABLE `clientes`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `IX_Clientes_Documento` (`Documento`);

--
-- Indices de la tabla `equipos`
--
ALTER TABLE `equipos`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Equipos_ClienteId` (`ClienteId`);

--
-- Indices de la tabla `ordenesreparacion`
--
ALTER TABLE `ordenesreparacion`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_OrdenesReparacion_EquipoId` (`EquipoId`),
  ADD KEY `IX_OrdenesReparacion_TecnicoId` (`TecnicoId`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Pagos_OrdenReparacionId` (`OrdenReparacionId`);

--
-- Indices de la tabla `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `archivosorden`
--
ALTER TABLE `archivosorden`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `aspnetroleclaims`
--
ALTER TABLE `aspnetroleclaims`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `aspnetuserclaims`
--
ALTER TABLE `aspnetuserclaims`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `clientes`
--
ALTER TABLE `clientes`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT de la tabla `equipos`
--
ALTER TABLE `equipos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `ordenesreparacion`
--
ALTER TABLE `ordenesreparacion`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `archivosorden`
--
ALTER TABLE `archivosorden`
  ADD CONSTRAINT `FK_ArchivosOrden_OrdenesReparacion_OrdenReparacionId` FOREIGN KEY (`OrdenReparacionId`) REFERENCES `ordenesreparacion` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `aspnetroleclaims`
--
ALTER TABLE `aspnetroleclaims`
  ADD CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `aspnetuserclaims`
--
ALTER TABLE `aspnetuserclaims`
  ADD CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `aspnetuserlogins`
--
ALTER TABLE `aspnetuserlogins`
  ADD CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `aspnetuserroles`
--
ALTER TABLE `aspnetuserroles`
  ADD CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `aspnetusertokens`
--
ALTER TABLE `aspnetusertokens`
  ADD CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `equipos`
--
ALTER TABLE `equipos`
  ADD CONSTRAINT `FK_Equipos_Clientes_ClienteId` FOREIGN KEY (`ClienteId`) REFERENCES `clientes` (`Id`);

--
-- Filtros para la tabla `ordenesreparacion`
--
ALTER TABLE `ordenesreparacion`
  ADD CONSTRAINT `FK_OrdenesReparacion_AspNetUsers_TecnicoId` FOREIGN KEY (`TecnicoId`) REFERENCES `aspnetusers` (`Id`),
  ADD CONSTRAINT `FK_OrdenesReparacion_Equipos_EquipoId` FOREIGN KEY (`EquipoId`) REFERENCES `equipos` (`Id`);

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `FK_Pagos_OrdenesReparacion_OrdenReparacionId` FOREIGN KEY (`OrdenReparacionId`) REFERENCES `ordenesreparacion` (`Id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
