CREATE TABLE `todoitems` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Title` varchar(100) CHARACTER SET utf8mb4,
  `ToDate` varchar(30) NOT NULL,
  `IsComplete` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`Id`));
