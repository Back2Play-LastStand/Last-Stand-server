-- MySQL dump 10.13  Distrib 9.3.0, for Win64 (x86_64)
--
-- Host: localhost    Database: last_stand_account_data
-- ------------------------------------------------------
-- Server version	9.3.0

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
-- Table structure for table `account_session`
--

DROP TABLE IF EXISTS `account_session`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `account_session` (
  `session_id` varchar(64) NOT NULL,
  `account_id` int NOT NULL,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `expires_at` datetime NOT NULL,
  PRIMARY KEY (`session_id`),
  KEY `account_id` (`account_id`),
  CONSTRAINT `account_session_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `player_account_data` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account_session`
--

LOCK TABLES `account_session` WRITE;
/*!40000 ALTER TABLE `account_session` DISABLE KEYS */;
/*!40000 ALTER TABLE `account_session` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `player_account_data`
--

DROP TABLE IF EXISTS `player_account_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `player_account_data` (
  `id` int NOT NULL AUTO_INCREMENT,
  `player_id` varchar(50) NOT NULL,
  `password` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `is_new_account` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`),
  UNIQUE KEY `player_id` (`player_id`),
  UNIQUE KEY `UQ_Email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `player_account_data`
--

LOCK TABLES `player_account_data` WRITE;
/*!40000 ALTER TABLE `player_account_data` DISABLE KEYS */;
INSERT INTO `player_account_data` VALUES (1,'test1','$2a$11$Te2/LkjIZYLg5n21wQg5cOfUurl2e2F6tTqq8FvZD5cghOn491mau','test1@example.com',0),(2,'test2','$2a$11$fzlH79jPgZq6QrUmbm0UY.0urttgV3XChmRubG7WWL3R5GWAx7.Xe','test2@example.com',0),(3,'test3','$2a$11$aHLWl0AIpcfOQbxSy0aLLu24kcarC4MZbW.yhmCneevDsEtjBpvCW','test3@example.com',0),(4,'test4','$2a$11$5JNnTVWUTI2D4I84IBwuy.b1hvy33HBnAK1xQoQrygEF/MlIZScCe','test4@example.com',0),(5,'Seanyee1227','$2a$11$Po8Eld7EGwG9R7mzOFwsCOjxbXx1X5iN8x0MCFzLBQGSTN37tMj8G','a01080080329@gmail.com',0),(6,'test5','$2a$11$cfis47jo35dXBKAspfScQ.4xL/Fyiea.ZGzAPBmw4dUrym2IdCX1C','b01080080329@gmail.com',0),(7,'test6','$2a$11$Q1Y0ockECK3j/vDsSpjUD.7Uvtb4RzhDudnnOh0q0QtE7eO3YFfQa','s24061@gsm.hs.kr',0),(8,'test7','$2a$11$F7svVfhICQylAd7nbrUWUeecg55qVh0XJ3HUeP76TD36U/WOHWLBC','test7@example.com',1);
/*!40000 ALTER TABLE `player_account_data` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-06-17  7:58:40
