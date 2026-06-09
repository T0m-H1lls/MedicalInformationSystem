-- MySQL dump 10.13  Distrib 8.0.19, for Win64 (x86_64)
--
-- Host: 217.150.77.216    Database: hospital
-- ------------------------------------------------------
-- Server version	8.0.46-0ubuntu0.24.04.2

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `appointments`
--

DROP TABLE IF EXISTS `appointments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `appointments` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `PatientId` int DEFAULT NULL,
  `DoctorId` int NOT NULL,
  `AppointmentDate` datetime NOT NULL,
  `StatusId` int DEFAULT NULL,
  `ReferralDoctorId` int DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT '1',
  `DeletedAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `PatientId` (`PatientId`),
  KEY `DoctorId` (`DoctorId`),
  KEY `appointments_status_FK` (`StatusId`),
  KEY `appointments_doctors_FK` (`ReferralDoctorId`),
  CONSTRAINT `appointments_doctors_FK` FOREIGN KEY (`ReferralDoctorId`) REFERENCES `doctors` (`Id`),
  CONSTRAINT `appointments_ibfk_1` FOREIGN KEY (`PatientId`) REFERENCES `patients` (`Id`),
  CONSTRAINT `appointments_ibfk_2` FOREIGN KEY (`DoctorId`) REFERENCES `doctors` (`Id`),
  CONSTRAINT `appointments_status_FK` FOREIGN KEY (`StatusId`) REFERENCES `status` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `appointments`
--

LOCK TABLES `appointments` WRITE;
/*!40000 ALTER TABLE `appointments` DISABLE KEYS */;
INSERT INTO `appointments` VALUES (1,1,1,'2025-04-20 10:00:00',1,NULL,1,NULL),(2,2,2,'2025-04-21 12:00:00',2,NULL,0,'2026-05-27 20:50:28'),(3,1,1,'2025-04-22 09:00:00',2,2,1,NULL),(4,2,1,'2025-04-23 11:00:00',2,NULL,1,NULL),(5,13,2,'2025-04-24 14:00:00',1,NULL,0,'2026-05-28 21:58:40'),(6,4,1,'2025-04-25 10:30:00',2,NULL,1,NULL),(7,18,2,'2025-03-25 00:00:00',1,3,0,'2026-06-09 12:13:57'),(18,28,2,'2027-05-27 23:01:00',1,NULL,1,NULL),(19,13,2,'2026-06-07 10:58:13',1,NULL,0,'2026-06-08 21:51:33'),(20,29,2,'2026-06-07 13:59:00',2,2,1,NULL),(21,31,3,'2026-06-07 10:58:55',1,NULL,0,'2026-06-08 22:52:51'),(22,28,2,'2027-06-07 14:00:00',1,3,1,NULL),(23,44,3,'2026-06-08 12:52:33',2,1,1,NULL),(24,28,2,'2026-06-09 08:30:00',1,NULL,1,NULL),(25,28,2,'2026-06-09 13:30:47',1,NULL,0,'2026-06-09 13:31:03'),(26,28,2,'2026-06-09 15:30:00',1,NULL,0,'2026-06-09 13:31:01'),(27,28,2,'2026-06-09 15:17:11',1,NULL,0,'2026-06-09 15:17:33');
/*!40000 ALTER TABLE `appointments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `departments`
--

DROP TABLE IF EXISTS `departments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `departments` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` text NOT NULL,
  `Floor` int DEFAULT NULL,
  `DeletedAt` datetime DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT '1',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `departments`
--

LOCK TABLES `departments` WRITE;
/*!40000 ALTER TABLE `departments` DISABLE KEYS */;
INSERT INTO `departments` VALUES (1,'Терапия',2,NULL,1),(2,'Хирургия',3,NULL,1),(3,'Лабораторная',6,NULL,1),(4,'Анестизиология',4,NULL,1),(5,'Экстренное отделение',5,NULL,1),(6,'Реабилитационная',1,NULL,1),(7,'Административно-управленческая',7,NULL,1);
/*!40000 ALTER TABLE `departments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `doctors`
--

DROP TABLE IF EXISTS `doctors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `doctors` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FullName` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `SpecializationId` int DEFAULT NULL,
  `Phone` text,
  `Room` text,
  `DepartmentId` int DEFAULT NULL,
  `DeletedAt` datetime DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT '1',
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `DepartmentId` (`DepartmentId`),
  KEY `doctors_Specialization_FK` (`SpecializationId`),
  CONSTRAINT `doctors_ibfk_1` FOREIGN KEY (`DepartmentId`) REFERENCES `departments` (`Id`),
  CONSTRAINT `doctors_Specialization_FK` FOREIGN KEY (`SpecializationId`) REFERENCES `specialization` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `doctors`
--

LOCK TABLES `doctors` WRITE;
/*!40000 ALTER TABLE `doctors` DISABLE KEYS */;
INSERT INTO `doctors` VALUES (1,'Иванов И.И.',1,'+7 999 000 1111','201',1,NULL,1,'2026-06-08 10:18:55'),(2,'Петров П.П.',3,'+79990002222','305',2,NULL,1,'2026-06-08 10:18:55'),(3,'Михаилов А.Е.',4,'+79256835751','105',1,NULL,1,'2026-06-08 10:18:55'),(4,'Александров Н.А.',7,'+79946384683','705',7,NULL,1,'2026-06-08 10:18:55'),(9,'Петрова В.И',2,'+7 787 435 8769','203',2,NULL,1,'2026-06-08 10:18:55'),(10,'Павлов К.Ю',3,'+7 996 877 6877','305',1,'2026-06-09 18:20:50',0,'2026-06-09 08:20:13');
/*!40000 ALTER TABLE `doctors` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `medicalrecords`
--

DROP TABLE IF EXISTS `medicalrecords`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `medicalrecords` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DoctorId` int DEFAULT NULL,
  `PatientId` int DEFAULT NULL,
  `Diagnostext` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Description` text,
  `RecordDate` datetime DEFAULT NULL,
  `Medicine` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `DeletedAt` datetime DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT '1',
  PRIMARY KEY (`Id`),
  KEY `medicalrecords_doctors_FK` (`DoctorId`),
  KEY `medicalrecords_patients_FK` (`PatientId`),
  CONSTRAINT `medicalrecords_doctors_FK` FOREIGN KEY (`DoctorId`) REFERENCES `doctors` (`Id`),
  CONSTRAINT `medicalrecords_patients_FK` FOREIGN KEY (`PatientId`) REFERENCES `patients` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `medicalrecords`
--

LOCK TABLES `medicalrecords` WRITE;
/*!40000 ALTER TABLE `medicalrecords` DISABLE KEYS */;
INSERT INTO `medicalrecords` VALUES (1,2,30,'Простуда','Кашель,Хрипы,температура','2026-05-21 10:00:00','Хлоргексидин,Амбробене,',NULL,1);
/*!40000 ALTER TABLE `medicalrecords` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `patients`
--

DROP TABLE IF EXISTS `patients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `patients` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FullName` text NOT NULL,
  `BirthDate` date DEFAULT NULL,
  `Gender` text,
  `Phone` text,
  `Address` text,
  `InsuranceNumber` text,
  `Passport` varchar(100) DEFAULT NULL,
  `SNILS` varchar(100) DEFAULT NULL,
  `doctorId` int DEFAULT NULL,
  `DeletedAt` datetime DEFAULT NULL,
  `IsActive` tinyint DEFAULT NULL,
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `patients_doctors_FK` (`doctorId`),
  CONSTRAINT `patients_doctors_FK` FOREIGN KEY (`doctorId`) REFERENCES `doctors` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `patients`
--

LOCK TABLES `patients` WRITE;
/*!40000 ALTER TABLE `patients` DISABLE KEYS */;
INSERT INTO `patients` VALUES (1,'Сидоров А.А.','1985-05-12','М','+79991112233','Москва','123456','11 62 456524','439-291-521 02',1,NULL,1,'2026-06-08 10:26:13'),(2,'Кузнецова М.В.','1990-07-20','Ж','+79994445566','Москва','654321','95 46 333045','386-016-221 72',2,'2026-06-01 09:42:37',0,'2026-06-08 10:26:13'),(3,'Иванова Е.С.','1975-03-10','Ж','+70000000001','Москва','111111','91 68 121985','314-336-462 45',1,NULL,1,'2026-06-08 10:26:13'),(4,'Смирнов Д.К.','2000-11-25','М','+70000000002','СПб','222222','43 43 095241','122-968-933 90',1,NULL,1,'2026-06-08 10:26:13'),(5,'Попова А.Н.','1995-08-15','Ж','+70000000003','Казань','333333','30 73 624421','503-000-179 92',2,'2026-06-01 09:42:45',0,'2026-06-08 10:26:13'),(13,'Кузнецов В.И','2026-05-17','М','+79874683646','Москва','324236346','3534553453','45435345345',2,'2026-06-08 18:01:32',0,'2026-06-08 10:26:13'),(14,'sdffdsfsdfsd','2026-05-17','М','(+7) 809 709 8888','808080808','808888','80  80  988080','798-678-575- 89',2,'2026-05-29 10:15:42',0,'2026-06-08 10:26:13'),(15,'kgkljhlh','2026-03-17','М','(+7) 908 908 6976','dkhljhkl','645645','89  78  865765','498-798-098- 90',2,'2026-05-29 10:15:40',0,'2026-06-08 10:26:13'),(16,'dklghfsgdjskkkkfldg','2026-05-17','М','(+7) 875 987 4385','kgmfdlkjgdf','890672','66  24  333256','569-456-745- 06',2,'2026-05-29 10:15:38',0,'2026-06-08 10:26:13'),(17,'gdgfgfd','2024-05-18','М','(+7) 878 777 9879','Москва','897649','75  47  645767','676-538-436- 89',2,'2026-05-29 10:15:45',0,'2026-06-08 10:26:13'),(18,'Алексей И.И','2008-05-26','М','+7 952 080 5385','Москва','485784','84  36  896879','896-785-797- 69',2,'2026-06-08 08:42:27',0,'2026-06-08 10:26:13'),(19,'Fkfglkdjg','2026-11-19','М','(+7) 857 487 9945','lkdgjjfdjk','656546','65  84  840983','890-665-406- 75',2,'2026-05-29 10:15:33',0,'2026-06-08 10:26:13'),(20,'fwffs','1926-01-31','М','(+7) 342 342 3432','32432434324','234343','43  24  323243','432-323-242- 34',2,'2026-05-29 10:15:30',0,'2026-06-08 10:26:13'),(21,'авпавп','1969-12-31','М','(+7) 454 354 3543','павп','авп','54  43  534543','543-543-543- 54',2,'2026-05-29 10:15:07',0,'2026-06-08 10:26:13'),(22,'ssdfsdfsf','0001-01-01','Ж','fsdfsdfsfsd','fdsfdfsdf','sdfsdf','fsdfsdf','dsfsf',2,'2026-05-29 10:15:35',0,'2026-06-08 10:26:13'),(23,'111111','2004-12-31','М','+79874675657','111111','111111111','1111111111','11111111111',2,'2026-05-29 10:15:04',0,'2026-06-08 10:26:13'),(24,'111111111111','1988-12-31','М','+7 111 111 1111','1111111111','111111','11 11 111111','111-111-111-11',2,'2026-05-29 10:14:59',0,'2026-06-08 10:26:13'),(25,'Алексей И.И','2008-05-27','М','+7 952 080 5385','Москва','485784','84  36  896879','896-785-797- 69',2,'2026-06-08 08:42:30',0,'2026-06-08 10:26:13'),(26,'2222222222222222222','2026-05-28','М','+7 656 544 5777','765765765765','765765','76 57 657657','765-765-765-76',2,'2026-05-29 10:14:56',0,'2026-05-08 10:26:13'),(27,'Алексей Н.И','2001-05-27','М','+7 986 584 6805','Владивосток','598656','84 76 984576','894-759-867-45',2,'2026-06-08 08:42:33',0,'2026-06-08 10:26:13'),(28,'Поляков В.И','2001-11-15','М','+7 987 648 7686','Владивосток','546546','76 97 546799','754-986-749-57',2,NULL,1,'2026-05-08 10:26:13'),(29,'Светлаков И.М.','1997-05-26','М','+7 985 345 7984','Москва','685467','86 59 476979','434-358-430-58',2,NULL,1,'2026-05-08 10:26:13'),(30,'Васильев А.О','1981-05-26','М','+7 749 875 9355','Липецк','753957','89 76 937957','743-987-987-98',2,NULL,1,'2026-05-08 10:26:13'),(31,'mnnjkhjk','2026-05-28','М','+7 797 667 7686','668','789769','68 76 786687','876-868-767-86',3,'2026-06-08 22:49:23',0,'2026-06-08 10:26:13'),(32,'dfhgghfdgdgdf','2026-05-28','М','+7 345 435 4445','ggfdj','457836','57 84 564873','347-658-456-47',2,'2026-05-29 10:15:24',0,'2026-06-08 10:26:13'),(34,'Нигер Нигеровский Нигерович','2006-03-27','М','+7 952 080 5388','Владивосток','438674','78 96 769867','986-565-769-38',2,'2026-06-08 21:45:36',0,'2026-06-08 08:02:39'),(41,'kkljlkJKJ','2026-06-08','М','+7 880 980 8080','8098098','098908','89 08 098098','098-098-098-09',3,'2026-06-08 22:49:20',0,'2026-06-08 12:34:05'),(42,'nkfjhhkh','2026-06-08','М','+7 798 798 7897','7798789789','797987','97 98 789798','979-879-879-87',3,'2026-06-08 22:49:18',0,'2026-06-08 12:42:05'),(43,'hkhkjh','2026-06-08','М','+7 987 987 9878','979879798','797998','87 98 798789','789-789-797-98',3,'2026-06-08 22:49:15',0,'2026-06-08 12:42:58'),(44,'Гончаров Матвей Игнатьев','2006-04-25','М','+7 877 897 9879','Магадан','887987','97 98 897897','798-798-798-79',3,NULL,1,'2026-06-08 12:52:13'),(45,'Некрасов Виктор Алексеевич','1995-04-25','М','+7 987 354 3809','Самара','579758','74 93 038032','798-573-492-79',2,NULL,1,'2026-06-09 05:20:30');
/*!40000 ALTER TABLE `patients` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `prescriptions`
--

DROP TABLE IF EXISTS `prescriptions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `prescriptions` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DoctorId` int DEFAULT NULL,
  `Medicine` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `PatientId` int DEFAULT NULL,
  `Dosage` text,
  `Duration` text,
  `IsActive` tinyint DEFAULT NULL,
  `DeletedAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `prescriptions_patients_FK` (`PatientId`),
  KEY `prescriptions_doctors_FK` (`DoctorId`),
  CONSTRAINT `prescriptions_doctors_FK` FOREIGN KEY (`DoctorId`) REFERENCES `doctors` (`Id`),
  CONSTRAINT `prescriptions_patients_FK` FOREIGN KEY (`PatientId`) REFERENCES `patients` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `prescriptions`
--

LOCK TABLES `prescriptions` WRITE;
/*!40000 ALTER TABLE `prescriptions` DISABLE KEYS */;
INSERT INTO `prescriptions` VALUES (1,1,'Гастрэнтерал',2,'500 мг 2 раза в день','5 дней',1,NULL),(2,2,'Психотел',13,'200 мг 3 раза в день','3 дня',0,'2026-05-28 22:00:18'),(3,2,'Антибиотик',5,'250 мг 2 раза в день','7 дней',0,'2026-05-28 22:00:15'),(5,2,'Ибупрофен',28,'2 таблетки','7 дней',1,NULL),(18,2,'sdsdadddddddddddddddddddddddddddddddddddddddddddddd',28,'sdsadsadsad','ddddddddddddddddddddddddddddddddddddddd',0,'2026-06-09 15:16:35');
/*!40000 ALTER TABLE `prescriptions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `specialization`
--

DROP TABLE IF EXISTS `specialization`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `specialization` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `specialization`
--

LOCK TABLES `specialization` WRITE;
/*!40000 ALTER TABLE `specialization` DISABLE KEYS */;
INSERT INTO `specialization` VALUES (1,'Хирург'),(2,'Терапевт'),(3,'Гинеколог'),(4,'Травмотолог'),(5,'Психотерапевт'),(6,'Лаборант'),(7,'Главный врач'),(8,'Регистратор');
/*!40000 ALTER TABLE `specialization` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `status`
--

DROP TABLE IF EXISTS `status`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `status` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `status`
--

LOCK TABLES `status` WRITE;
/*!40000 ALTER TABLE `status` DISABLE KEYS */;
INSERT INTO `status` VALUES (1,'Запланирован'),(2,'Завершен'),(3,'Отменен');
/*!40000 ALTER TABLE `status` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Login` text NOT NULL,
  `Password` text NOT NULL,
  `RoleId` int DEFAULT NULL,
  `DoctorId` int DEFAULT NULL,
  `Name` varchar(100) DEFAULT NULL,
  `Surname` varchar(100) DEFAULT NULL,
  `Patronymic` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `users_Specialization_FK` (`RoleId`),
  KEY `users_doctors_FK` (`DoctorId`),
  CONSTRAINT `users_doctors_FK` FOREIGN KEY (`DoctorId`) REFERENCES `doctors` (`Id`),
  CONSTRAINT `users_Specialization_FK` FOREIGN KEY (`RoleId`) REFERENCES `specialization` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'tomHill','1234',1,1,'Иван','Иванов','Иванович'),(2,'sdsd','4321',3,2,'Петр','Петров','Петрович'),(3,'Admin','1234321',7,4,'Александ','Никольский','Андреевич'),(18,'dsds','123',5,3,'Кирилл','Михаилов','Алексеевич');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'hospital'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-06-09 18:22:56
